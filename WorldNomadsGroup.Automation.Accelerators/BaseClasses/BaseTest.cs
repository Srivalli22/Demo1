using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Xml;

using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace WorldNomadsGroup.Automation.Accelerators
{
	/// <summary>
	/// Description of BaseTest.
	/// </summary>
	public abstract class BaseTest
	{

        TimeZone _zone = TimeZone.CurrentTimeZone;

        /// <summary>
        /// Gets or Sets Driver
        /// </summary>
        public RemoteWebDriver Driver { get; set; }

		/// <summary>
		/// Gets or Sets Reporter
		/// </summary>
		public Iteration Reporter { get; set; }

		/// <summary>
		/// Gets or Sets Step
		/// </summary>
		protected string Step
		{
			get
			{
				//TODO: Get should go away
				return Reporter.Chapter.Step.Title;
			}
			set
			{
				Reporter.Add(new Step(value));
			}
		}

		/// <summary>
		/// Gets or Sets Identity of Test Case
		/// </summary>
		public string TestCaseId { get; set; }

		/// <summary>
		/// Gets or Sets Identity of Test Data
		/// </summary>
		public string TestDataId { get; set; }

		/// <summary>
		/// Gets or Sets Test Data as XMLNode
		/// </summary>
		//public XmlNode TestDataNode { get; set; }
		
		public Dictionary<string,string> TestDataNode {get;set;}
		
		/// <summary>
		/// Gets or Sets Test Case as XMLNode
		/// </summary>
		public Dictionary<string,string> TestCaseNode { get; set; }

		public Dictionary<string, string> TestData { get; set; }
		
		public string ResultsPath = string.Empty;
		
		#region Instance
		private static BaseTest _instance;
		public static BaseTest Instance
		{
			get
			{
				if (_instance == null)
				{
					if (_instance == null)
					{
						_instance = Activator.CreateInstance<BaseTest>();
					}
				}
				return _instance;
			}
		}
		#endregion
		
		#region Constructor
		public BaseTest()
		{
		}
		
		public BaseTest(RemoteWebDriver driver)
		{
			this.Driver=driver;
		}
		
		public BaseTest(Dictionary<string,string> testNode)
		{
			this.TestCaseNode=testNode;
		}
		#endregion
		
		#region PageInstance
		protected T Page<T>() where T : BasePage
		{
			Type pageType = typeof(T);
			if (pageType != null)
			{
				T ob = Activator.CreateInstance<T>();
				return ob;
			}
			else
			{
				return null;
			}
		}
		
		protected T Page<T>(RemoteWebDriver driver) where T : BasePage, new()
		{
			Type pageType = typeof(T);
			if (pageType != null)
			{
				T ob = (T)Activator.CreateInstance(pageType, new object[] { driver });
				return ob;
			}
			else
			{
				return null;
			}
		}
		
		protected T Page<T>(Dictionary<string,string> testNode) where T : BasePage, new()
		{
			Type pageType = typeof(T);
			if (pageType != null)
			{
				T ob = (T)Activator.CreateInstance(pageType, new object[] { testNode });
				return ob;
			}
			else
			{
				return null;
			}
		}
		
		protected T Page<T>(RemoteWebDriver driver,Dictionary<string,string> testNode) where T : BasePage, new()
		{
			Type pageType = typeof(T);
			if (pageType != null)
			{
				T ob = (T)Activator.CreateInstance(pageType, new object[] { driver,testNode });
				return ob;
			}
			else
			{
				return null;
			}
		}
		
		protected T Page<T>(RemoteWebDriver driver,Dictionary<string,string> testNode,Iteration iteration) where T : BasePage, new()
		{
			Type pageType = typeof(T);
			if (pageType != null)
			{
				T ob = (T)Activator.CreateInstance(pageType, new object[] { driver,testNode,iteration });
				return ob;
			}
			else
			{
				return null;
			}
		}

		#endregion
		

		public void Execute(Dictionary<String, String> browserConfig,
		                    String testCaseId,
		                    String iterationId,
		                    Iteration iteration,
		                    Dictionary<String, String> testData,
		                    Engine reportEngine)
		{
			try
			{

				this.Driver = Util.GetDriver(browserConfig);
				this.Reporter = iteration;
				this.TestCaseId = testCaseId;
				this.TestDataId = iterationId;
				this.TestData = testData;
				this.ResultsPath = reportEngine.ReportPath;


				if (browserConfig["target"] == "local")
				{
					var wmi = new ManagementObjectSearcher("select * from Win32_OperatingSystem").Get().Cast<ManagementObject>().First();

					this.Reporter.Browser.PlatformName = String.Format("{0} {1}", ((string)wmi["Caption"]).Trim(), (string)wmi["OSArchitecture"]);
					this.Reporter.Browser.PlatformVersion = ((string)wmi["Version"]);
					this.Reporter.Browser.BrowserName = Driver.Capabilities.BrowserName;
					this.Reporter.Browser.BrowserVersion = Driver.Capabilities.Version;
				}
				else
				{
					this.Reporter.Browser.PlatformName = browserConfig.ContainsKey("os") ? browserConfig["os"] : browserConfig["device"];
					this.Reporter.Browser.PlatformVersion = browserConfig.ContainsKey("os_version") ? browserConfig["os_version"] : browserConfig.ContainsKey("realMobile") ? "Real" : "Emulator";
					this.Reporter.Browser.BrowserName = browserConfig.ContainsKey("browser") ? browserConfig["browser"] : "Safari";
					this.Reporter.Browser.BrowserVersion = browserConfig.ContainsKey("browser_version") ? browserConfig["browser_version"] : "";
				}

				// Does Seed having anything?
				if (this.Reporter.Chapter.Steps.Count == 0)
					this.Reporter.Chapters.RemoveAt(0);

				ExecuteTestCase();
			}
			catch (OpenQA.Selenium.WebDriverTimeoutException timeex)
			{
				this.Reporter.Chapter.Step.Action.Extra = timeex.Message + "<br/>" + timeex.StackTrace;
				this.Reporter.Chapter.Step.Action.IsSuccess = false;
                Console.WriteLine(timeex.Message + timeex.StackTrace);
			}
			catch (System.IO.IOException sysex)
			{
				this.Reporter.Chapter.Step.Action.Extra = sysex.Message + "<br/>" + sysex.StackTrace;
				this.Reporter.Chapter.Step.Action.IsSuccess = false;
                Console.WriteLine(sysex.Message + sysex.StackTrace);
            }
			catch (SystemException sysex)
			{
				this.Reporter.Chapter.Step.Action.Extra = sysex.Message + "<br/>" + sysex.StackTrace;
				this.Reporter.Chapter.Step.Action.IsSuccess = false;
                Console.WriteLine(sysex.Message + sysex.StackTrace);
            }
			catch (Exception ex)
			{
			    string currentUrl = "";

                try
			    {
			      
			        if (Driver == null)
			        {
			        }
			        else
			        {
			            currentUrl = Driver.Url;
			        }
			    }
			    catch (Exception e)
			    {
			      
			    }

			    this.Reporter.Chapter.Step.Action.Extra = "<font color=blue>URL " + currentUrl + "</font><br/>" + "Exception Message : " + ex.Message + "<br/>" + ex.InnerException + ex.StackTrace;
				this.Reporter.Chapter.Step.Action.IsSuccess = false;
                Console.WriteLine(ex.Message + ex.StackTrace);

            }
			finally
			{
				this.Reporter.IsCompleted = true;

				// If current iteration is a failure, get screenshot
			    try
			    {
			        if (!Reporter.IsSuccess && Driver != null)
			        {
			            ITakesScreenshot iTakeScreenshot = Driver;
			            this.Reporter.Screenshot = iTakeScreenshot.GetScreenshot().AsBase64EncodedString;
			        }
			    }
			    catch (Exception e)
			    {
			        Console.WriteLine(e);
			       
			    }
                //this.Reporter.EndTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
                this.Reporter.EndTime = _zone.ToLocalTime(DateTime.Now);

                lock (reportEngine)
				{
					reportEngine.PublishIteration(this.Reporter);
					reportEngine.Summarize(false);
				}
			    try
			    {
                    Driver.Close();
                    Driver.Quit();
                }
			    catch (Exception e)
			    {
                    Console.WriteLine("Failed to kill driver instance: ");
			    }
             
            }
		}
		/// <summary>	`
		/// Executes Test Case, should be overriden by derived
		/// </summary>
		protected virtual void ExecuteTestCase()
		{
			Reporter.Add(new Chapter("Execute Test Case"));
		}

		/// <summary>
		/// Prepares Seed Data, should be overriden by derived
		/// </summary>
		protected virtual void PrepareSeed()
		{
			Reporter.Add(new Chapter("Prepare Seed Data"));
		}
	}
}
