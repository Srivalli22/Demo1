using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Text;
using Castle.Core.Internal;
using WorldNomadsGroup.Automation.Accelerators;
using System.Security.Cryptography;


namespace WorldNomadsGroup.Automation.SuiteRunner
{
    class Program
    {
       
        public static Dictionary<string, string> QualifiedNames = new Dictionary<string, string>();
        private static List<object[]> TestCaseToExecute = new List<object[]>();
        public static bool ReportSummary = true;
        public static bool Detailedreportflag = true;
        public static string BrowserTypeValue = null;
        
        public static int Main(string[] args)
        {
            try
            {                
                Arguments CommandLine = new Arguments(args);

                Addconfig(CommandLine);

                Console.WriteLine("Execution started at " + DateTime.Now);

                var workingDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\";
                var strModuleName = string.Empty;
                var strTestCaseId = string.Empty;
                var strTestCaseName = string.Empty;

                var reportEngine = new Engine(Util.EnvironmentSettings["ReportsPath"], Util.EnvironmentSettings["Server"]);

                // Get all Test Cases (Derived from BaseTestCase)
                var typeBaseCase = typeof(BaseTest);
                foreach (var type in Assembly.LoadFrom(ConfigurationManager.AppSettings.Get("TestsDLLName").ToString()).GetTypes().Where(x => x.IsSubclassOf(typeBaseCase)))
                {
                    QualifiedNames.Add(type.Name, type.AssemblyQualifiedName);
                }

                Testdriver(CommandLine, workingDirectory, reportEngine, Util.EnvironmentSettings["TestSuite"]);

                Processor(int.Parse(ConfigurationManager.AppSettings.Get("MaxDegreeOfParallelism")));

                reportEngine.Summarize();

                Console.WriteLine("Execution completed at " + DateTime.Now);

                // generate re-run suite
                var suiteContent = new StringBuilder();
                suiteContent.AppendLine("TestCaseID,Run,Browser,TestCaseTitle,RequirementFeature");
                foreach (var eachCase in reportEngine.Reporter.TestCases)
                {
                    if (eachCase.IsSuccess) continue;
                    var browsers = eachCase.Browsers.Where(eachBrowser => !eachBrowser.IsSuccess).Aggregate(string.Empty, (current, eachBrowser) => current + $"{eachBrowser.Title};");
                    browsers = browsers.TrimEnd(new char[] { ';' });
                    suiteContent.AppendLine(
                        $"{eachCase.Title},Yes,{browsers},{eachCase.Name},{eachCase.RequirementFeature.Replace(',', ' ')}");
                }
                var fileName = Path.Combine(reportEngine.ReportPath, "FailedSuite.csv");
                using (var output = new StreamWriter(fileName))
                {
                    output.Write(suiteContent.ToString());
                }

                #region RerunFailedTest

                Console.WriteLine("Rerun of test is " + Util.EnvironmentSettings["RerunTest"]);

                if (Util.EnvironmentSettings["RerunTest"]
                    .Equals("YES", StringComparison.InvariantCultureIgnoreCase))
                {
                    ExcelReader.SaveCsvtoXlxs(fileName, Path.Combine(reportEngine.ReportPath, "FailedSuite.xlsx"));

                    TestCaseToExecute.Clear();

                    var reportRerunEngine = new Engine(reportEngine.ReportPath, Util.EnvironmentSettings["Server"],
                        "YES");

                    Testdriver(CommandLine, reportEngine.ReportPath + "\\", reportRerunEngine, "FailedSuite.xlsx");

                    Processor(int.Parse(ConfigurationManager.AppSettings.Get("MaxDegreeOfParallelism")));

                    reportRerunEngine.Summarize();
                }

                reportEngine.Summary.TestCases.ForEach(testcase=> testcase.IsSuccess.Equals(true));


                TearDown();

                #endregion


                if (reportEngine.Summary.FailedCount > 0)
                {

                    Console.WriteLine("Execution completed at " + DateTime.Now);
                    Console.WriteLine("Execution completed . " + reportEngine.Summary.FailedCount+ "Test  have failed");
                    return -1;
                }
               
            }
            catch (Exception ex)
            {
                Console.WriteLine("Execution terminated.. " + ex.Message);
                Console.WriteLine("Execution completed at " + DateTime.Now);

                return -1;
            }

            return 0;
            
        }

        private static void TearDown()
        {
            ExcelReader.KillExcel();

            var latestfile = new DirectoryInfo(Util.EnvironmentSettings["ReportsPath"]).GetDirectories().OrderByDescending(d => d.LastWriteTimeUtc).First();

            var reportsdir = new DirectoryInfo(Directory.GetCurrentDirectory() + "\\..\\..\\..\\WorldNomadsGroup.Automation.SuiteRunner\\" + "Reports");

            if (!Directory.Exists(Directory.GetCurrentDirectory() + "\\..\\..\\..\\WorldNomadsGroup.Automation.SuiteRunner\\" + "Reports"))
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\..\\..\\..\\WorldNomadsGroup.Automation.SuiteRunner\\" + "Reports");
            }

            DeleteDirectoryContent(reportsdir);

            CopyFilesRecursively(latestfile, reportsdir);
        }

        private static void DeleteDirectoryContent(DirectoryInfo reportsdir)
        {
            foreach (FileInfo file in reportsdir.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in reportsdir.GetDirectories())
            {
                dir.Delete(true);
            }
        }

        public static void CopyFilesRecursively(DirectoryInfo source, DirectoryInfo target)
        {
            foreach (DirectoryInfo dir in source.GetDirectories())
                CopyFilesRecursively(dir, target.CreateSubdirectory(dir.Name));
            foreach (FileInfo file in source.GetFiles())
                file.CopyTo(Path.Combine(target.FullName, file.Name));
        }

        private static void Testdriver(Arguments commandLine, string workingDirectory, Engine reportEngine, string environmentSetting)
        {
            //foreach (DataRow eachRow in ExcelReader.ReadSuiteFile(workingDirectory, environmentSetting,false).Rows) //to read from oledb
            foreach (DataRow eachRow in XMLReader.ReadXMLFile(workingDirectory, environmentSetting).Rows) //to read from oledb
            {
                try
                {
                    

                    if (eachRow["Run"].ToString().ToUpper() != "YES")
                        continue;
                    var testCaseId = eachRow["TestCaseID"].ToString().Trim();
                    var testCaseName = eachRow["TestCaseTitle"].ToString().Trim();
                    var testCaseRequirementFeature = eachRow["RequirementFeature"].ToString().Trim();
                    var testCaseReporter = new TestCase(testCaseId, testCaseName, testCaseRequirementFeature);
                    testCaseReporter.Summary = reportEngine.Reporter;
                    reportEngine.Reporter.TestCases.Add(testCaseReporter);
                    if (!commandLine["Browser"].IsNullOrEmpty())
                    {
                        eachRow["Browser"] = commandLine["Browser"];
                    }
                    foreach (var browserId in eachRow["Browser"].ToString().Split(new char[] { ';' }))
                    {
                        var overRideBrowserId = string.Empty;
                        if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings.Get("DefaultBrowser").ToString()))
                            overRideBrowserId = ConfigurationManager.AppSettings.Get("DefaultBrowser").ToString();
                        var browserReporter =
                            new Browser(browserId != string.Empty ? browserId : overRideBrowserId)
                            {
                                TestCase = testCaseReporter
                            };
                        testCaseReporter.Browsers.Add(browserReporter);
                        var TempworkingDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\";
                       
                        //foreach (DataRow iterationTestData in ExcelReader.ReadXMLFile(Path.Combine(TempworkingDirectory, "TestData\\TestData.xml"), testCaseName).Rows)
                            foreach (DataRow iterationTestData in XMLReader.ReadXMLFile(TempworkingDirectory,"TestData\\TestData.xml",testCaseName).Rows)
                            {
                            var testData = iterationTestData.Table.Columns.Cast<DataColumn>().ToDictionary(col => col.ColumnName, col => Convert.ToString((iterationTestData.Field<string>(col.ColumnName))));
                       
                            var browserConfig = Util.GetBrowserConfig(browserId != string.Empty ? browserId : overRideBrowserId);
                            var iterationId = iterationTestData["TestCaseID"].ToString();
                            //String defectID = iterationTestData["DefectID"].ToString();
                            var iterationReporter = new Iteration(iterationId);
                            iterationReporter.Browser = browserReporter;
                            browserReporter.Iterations.Add(iterationReporter);
                            Console.WriteLine("Browser name :{0},Testcase id:{1},iteration id:{2},iteration report id:{3},test data :{4}, report engine :{5}",browserConfig, testCaseId, iterationId, iterationReporter, testData, reportEngine);
                            TestCaseToExecute.Add(new object[] { browserConfig, testCaseId, iterationId, iterationReporter, testData, reportEngine });
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Execution terminated.. " + ex.Message);
                    Console.WriteLine("Execution completed at " + DateTime.Now);
                }
            }
        }
        private static void Addconfig(Arguments CommandLine)
        {

            foreach (var configKey in ConfigurationManager.AppSettings.AllKeys)
            {
                var temp = string.Empty;

                if (CommandLine[configKey] != null)
                {
                    temp = CommandLine[configKey];
                    Util.environmentSettings.Add(configKey, temp);
                }

            }
            try
            {
                foreach (var configKey in ConfigurationManager.AppSettings.AllKeys)
                {
                    if (!Util.environmentSettings.ContainsKey(configKey))
                    {
                        Util.environmentSettings.Add(configKey, ConfigurationManager.AppSettings[configKey]);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        static void Processor(int maxDegree)
        {
            try
            {
                
                if (ConfigurationManager.AppSettings.Get("ExecutionMode").ToLower().Equals("s"))
                {
                    //Use this loop for sequential running of the test cases
                    foreach (var work in TestCaseToExecute)
                    {
                        ProcessEachWork(work);
                        
                    }
                }
                else if (ConfigurationManager.AppSettings.Get("ExecutionMode").ToLower().Equals("p"))
                {
                    /*Use this loop for parellel running of the test cases*/
                    //   testCaseToExecute = testCaseToExecute.OrderBy(Object => Object[1]).ToList();
                    Console.WriteLine("Test case count is " + TestCaseToExecute.Count);
                    Parallel.ForEach(TestCaseToExecute,
                        new ParallelOptions { MaxDegreeOfParallelism = maxDegree },
                        ProcessEachWork);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        private static void ProcessEachWork(object[] data)
        {
            try
            {
                //Console.WriteLine("Test case count is " + data.Length);
                var dictionary = (Dictionary<string, string>)data[4];
                var testName = data[1].ToString();
                Console.WriteLine("Test Name " + testName);
                var typeTestCase = Type.GetType(QualifiedNames[dictionary["TestCaseName"]]);
                Console.WriteLine("Started: " + typeTestCase);
                var baseCase = Activator.CreateInstance(typeTestCase) as BaseTest;
                typeTestCase.GetMethod("Execute").Invoke(
                    baseCase, data);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }


        

    }
}