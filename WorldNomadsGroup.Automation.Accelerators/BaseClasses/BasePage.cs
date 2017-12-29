#region namespaces
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using JavascriptExecutor = OpenQA.Selenium.IJavaScriptExecutor;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using iTextSharp.text.pdf;
using WebElement = OpenQA.Selenium.IWebElement;
using System.Linq;
using System.Text;
#endregion

namespace WorldNomadsGroup.Automation.Accelerators
{
    /// <summary>
    /// This is the Super class for all pages
    /// </summary>
    /// 
    public abstract class BasePage
    {
        /// <summary>
        /// Gets or Sets Driver
        /// </summary>
        public RemoteWebDriver Driver { get; set; }

        /// <summary>
        /// Gets or Sets Test Data as XMLNode
        /// </summary>
        public Dictionary<string, string> TestDataNode { get; set; }

        /// <summary>
        /// Gets or Sets Reporter
        /// </summary>
        public Iteration Reporter { get; set; }

        #region Instance
        private BasePage _instance;
        public BasePage Instance
        {
            get
            {
                if (_instance == null)
                {
                    if (_instance == null)
                    {
                        _instance = Activator.CreateInstance<BasePage>();
                    }
                }
                return _instance;
            }
        }
        #endregion

        #region Constructor
        public BasePage()
        {

        }
        public BasePage(RemoteWebDriver driver)
        {
            this.Driver = driver;
        }

        public BasePage(Dictionary<string, string> testNode)
        {
            this.TestDataNode = testNode;
        }

        public BasePage(RemoteWebDriver driver, Dictionary<string, string> testNode)
        {
            this.Driver = driver;
            this.TestDataNode = testNode;
        }
        #endregion
        
        public void Report(string message, bool isSuccess)
        {
            if (isSuccess)
            {
                Reporter.Add(new Act("<b> <font color='green' >" + message + " </b></font>"));
            }
            else
            {

                throw new Exception(" <font color='red' > <b>" + message + " </b> </font> ");
            }

        }
        
    }
}

