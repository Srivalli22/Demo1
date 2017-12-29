
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;
using System.Data;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data.OleDb;
using OpenQA.Selenium.Appium.Enums;

namespace WorldNomadsGroup.Automation.Accelerators
{
	/// <summary>
	/// Description of Util.
	/// </summary>
	public class Util
	{
		public static Dictionary<string, string> environmentSettings = new Dictionary<string, string>();
		private static Dictionary<string, string> _commonTestData = new Dictionary<string, string>();
		/// <summary>
		/// Gets settings for current environment
		/// </summary>
		public static Dictionary<string, string> EnvironmentSettings
		{
			get
			{


			    return environmentSettings;


            }

		    
        }
		
		/// <summary>
		/// Prepares RemoteWebDriver basing on configuration supplied
		/// </summary>
		/// <param name="browserConfig"></param>
		/// <returns></returns>
		
		/// <summary>
		/// Prepares RemoteWebDriver basing on configuration supplied
		/// </summary>
		/// <param name="browserConfig"></param>
		/// <returns></returns>
		public static RemoteWebDriver GetDriver(Dictionary<String, String> browserConfig)
		{
			RemoteWebDriver driver = null;
			try
			{
				if (browserConfig["target"] == "local")
				{
					if (browserConfig["browser"].ToString().ToUpper() == "FIREFOX")
					{
                        try
                        {

                            FirefoxProfile Fp = new FirefoxProfile();
                        //    Fp.SetPreference("dom.allow_scripts_to_close_windows", true);
                            driver = new FirefoxDriver();


                        //     driver.Manage().Window.Maximize();
                        }
                        catch (Exception e)
                        {

                            throw new Exception("Unable to create firefox driver object");

                        }
                    }
					else if (browserConfig["browser"] == "IE")
					{
                        string path = Directory.GetCurrentDirectory() + @"\..\..\..\packages\Selenium.WebDriver.IEDriver64.2.53.0.0\driver\";
                        //TODO: Get rid of Framework Path
                        
                        InternetExplorerOptions options = new InternetExplorerOptions();
                        
                        options.EnsureCleanSession = true;
                        
                        driver = new InternetExplorerDriver(options);
                        driver.Manage().Window.Maximize();
                     
                    }
					else if (browserConfig["browser"] == "Chrome")
					{
						DesiredCapabilities capabilities = DesiredCapabilities.Chrome();
						ChromeOptions chrOpts = new ChromeOptions();
						chrOpts.AddArguments("test-type");
						//chrOpts.AddArguments("--disable-extensions");
						//chrOpts.AddArgument("incognito");
					    //chrOpts.AddArgument("enable-automation");
					    //chrOpts.AddArgument("--disable-infobars");
                        //chrOpts.AddUserProfilePreference("download.prompt_for_download", ConfigurationManager.AppSettings["ShowBrowserDownloadPrompt"]);
                        driver = new ChromeDriver(Directory.GetParent(Assembly.GetEntryAssembly().Location).ToString(), chrOpts);
                        driver.Manage().Window.Maximize();
                    } 
                    else if (browserConfig["browser"] == "Mobile")
                    {
                        DesiredCapabilities capabilities = new DesiredCapabilities();
                        capabilities.SetCapability(MobileCapabilityType.BrowserName, "chrome");
                        capabilities.SetCapability(MobileCapabilityType.NewCommandTimeout, "80000");
                        capabilities.SetCapability(MobileCapabilityType.DeviceName, "Samsung");
                        capabilities.SetCapability(MobileCapabilityType.PlatformName, "Android");
                        capabilities.SetCapability(MobileCapabilityType.Orientation, "LANDSCAPE");
                        driver = new RemoteWebDriver(capabilities);
                    }
				    try
				    {
				        driver.Manage().Timeouts().SetPageLoadTimeout(TimeSpan.FromSeconds(300));
				        driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(60));
				    }
				    catch (Exception e)
				    {
				        
				    
				    }
                    
					driver.Manage().Cookies.DeleteAllCookies();
					return driver;
				}
				return null;
			}

			catch (Exception e)
			{
				Console.WriteLine("Get Driver Error: " + e.Message);                
                return null;
			}
		}
        /// <summary>
        /// Gets Browser related configuration data from App.Config
        /// </summary>
        /// <param name="browserId">Identity of Browser</param>
        /// <returns><see cref="Dictionary<String, String>"/></returns>
        /*public static Dictionary<String, String> GetBrowserConfig(String browserId)
		{
			browserId=ConfigurationManager.AppSettings.Get(browserId).ToString();
			Dictionary<String, String> config = new Dictionary<string, string>();
			String[] KeyValue = null;

			foreach (String attribute in browserId.Split(new Char[] { ';' }))
			{
				if (attribute != "")
				{
					KeyValue = attribute.Split(new Char[] { ':' });
					config.Add(KeyValue[0].Trim(), KeyValue[1].Trim());
				}
			}
			return config;
		}*/

        public static Dictionary<String, String> GetBrowserConfig(String browserType)
        {

            Dictionary<String, String> config = new Dictionary<string, string>();
            config.Add("browser", browserType);
            config.Add("target", "local");


            return config;
        }


        public static DataTable ReadCsvContent(String fileDirectory, String filename)
        {
            DataTable table = new DataTable();
            int temp = 0;

            foreach (String fName in filename.Split(','))
            {
                string[] lines = File.ReadAllLines(Path.Combine(fileDirectory, fName));

                if (temp == 0)
                {
                    temp = 1;
                    // identify columns
                    foreach (String columnName in lines[0].Split(new char[] { ',' }))
                    {
                        table.Columns.Add(columnName, typeof(String));
                    }
                }
                foreach (String data in lines.Where((val, index) => index != 0))
                {
                    table.Rows.Add(data.Split(new Char[] { ',' }));
                }
            }
            return table;
        }

        

        public  static void ReleaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Unable to release the Object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }

        public static DataTable GetIterationsTestDataCsv(String location, String testCaseName)
        {
            //			lock (commonTestData)
            //			{
            //				if (commonTestData.Count == 0) LoadCommonTestData(location);
            //			}


            String[] foundFiles = Directory.GetFiles(location, String.Format("{0}.csv", testCaseName), SearchOption.AllDirectories);

            if (foundFiles.Length == 0)
                throw new FileNotFoundException(String.Format("Test Data file not found at {0}", location), String.Format("{0}.csv", testCaseName));

            //DataTable tableTestData = ReadExcelFile("", foundFiles[0]);
            DataTable tableTestData = ReadCsvContent(location, foundFiles[0]);

            //			foreach (DataRow eachRow in tableTestData.Rows)
            //			{
            //				foreach (DataColumn eachColumn in tableTestData.Columns)
            //				{
            //					if (eachRow[eachColumn].ToString().StartsWith("#"))
            //					{
            //						eachRow[eachColumn] = commonTestData[eachRow[eachColumn].ToString().Replace("#", "")];
            //					}
            //				}
            //			}
            return tableTestData;
        }


        #region GetTestData

        public static DataTable GetIterationsTestData(String location, String filename, String testCaseName)
        {

            try
            {
                DataTable dt1 = new DataTable();
                string path = location + "\\" + filename;
                string connStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties=Excel 12.0;";
                if (File.Exists(path))
                {
                    DataTable sheets = GetSchemaTable(connStr);
                    string sql = string.Empty;
                    DataSet ds = new DataSet();
                    bool flag = false;
                    foreach (DataRow dr in sheets.Rows)
                    {  //Print_Area
                        string workSheetName = dr["TABLE_NAME"].ToString().Trim();
                        string sheetName = workSheetName.Replace("$", "");

                        if (sheetName == testCaseName)
                        {
                            sql = "SELECT * FROM [" + workSheetName + "]";
                            ds.Clear();
                            OleDbDataAdapter data = new OleDbDataAdapter(sql, connStr);
                            data.Fill(ds);
                            dt1 = ds.Tables[0];
                            flag = true;
                            break;
                        }
                    }
                    if (flag == false)
                        throw new Exception("'"+testCaseName + "' sheet is not found in following excel workbook:" + filename);
                }

                else
                {
                    throw new FileNotFoundException(String.Format("Test Data file not found at ", location), String.Format(filename, testCaseName));
                }
                return dt1;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
              
        }


        #region readsuitedetails
        public static DataTable GetSchemaTable(string connectionString)
        {
            using (OleDbConnection connection = new
                   OleDbConnection(connectionString))
            {
                connection.Open();
                DataTable schemaTable = connection.GetOleDbSchemaTable(
                    OleDbSchemaGuid.Tables,
                    new object[] { null, null, null, "TABLE" });
                return schemaTable;
            }
        }

        public static DataTable ReadExcelContent(String fileDirectory, String filename)
        {
            string path = fileDirectory + "" + filename;
            string connStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties=Excel 12.0;";

            DataTable sheets = GetSchemaTable(connStr);
            string sql = string.Empty;
            DataTable dt1 = new DataTable();
            DataSet ds = new DataSet();
            foreach (DataRow dr in sheets.Rows)
            {  //Print_Area
                string workSheetName = dr["TABLE_NAME"].ToString().Trim();
                sql = "SELECT * FROM [" + workSheetName + "]";
                ds.Clear();
                OleDbDataAdapter data = new OleDbDataAdapter(sql, connStr);
                data.Fill(ds);
                dt1 = ds.Tables[0];
            }
            return dt1;

        }
        #endregion
        #endregion
        public static void LoadCommonTestData(String location)
		{
			String columnValue = String.Empty;
			DataTable tableCommonData = ReadExcelContent(location, EnvironmentSettings["CommonData"]);

			foreach (DataRow eachRow in tableCommonData.Rows)
			{
				_commonTestData.Add(eachRow["Key"].ToString(), eachRow["Value"].ToString());
			}
		}

        
    }
}
