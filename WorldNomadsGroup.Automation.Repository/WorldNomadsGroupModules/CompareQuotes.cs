using System;
using System.Linq;
using OpenQA.Selenium;
using WorldNomadsGroup.Automation.Accelerators;
using System.Collections.Generic;
using System.Threading;
using OpenQA.Selenium.Remote;
using System.Collections.ObjectModel;
using Castle.Core.Internal;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using org.bouncycastle.asn1.mozilla;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Appium.Interfaces;
using OpenQA.Selenium.Interactions;
using System.Data;
using System.Xml;
using System.IO;

namespace WorldNomadsGroup.Automation.Repository
{
    /// <summary>
    /// Compare Quote page locators and functions
    /// </summary>

    public class CompareQuotes : ActionEngine
    {

        #region Constructor
        public CompareQuotes()
        {

        }
        public CompareQuotes(RemoteWebDriver driver)
        {
            this.Driver = driver;
        }

        public CompareQuotes(Dictionary<string, string> testNode)
        {
            this.TestDataNode = testNode;
        }

        public CompareQuotes(RemoteWebDriver driver, Dictionary<string, string> testNode)
        {
            this.Driver = driver;
            this.TestDataNode = testNode;
        }

        public CompareQuotes(RemoteWebDriver driver, Dictionary<string, string> testNode, Iteration iteration)
        {
            this.Driver = driver;
            this.TestDataNode = testNode;
            this.Reporter = iteration;
        }
        #endregion

        #region Object definitions
        public static By BasicSelectDurinCompare = By.XPath(".//ul[@class='plans']/li[@data-plancode='budout']//div[@class='button-like buy-button-primary']/a");
        //public static By ComprehensiveSelectDurinCompare = By.XPath(".//ul[@class='plans']/li[@data-plancode='holiday']//div[@class='button-like buy-button-primary']/a");
        public static By ComprehensiveSelectDurinCompare = By.XPath(".//*[@id='container']/div[4]/form/div[4]/div[1]/ul/li[1]/div[2]/div/div");
        #endregion

        #region Page Functions

        /// <summary>
        /// Select Comprehensive quote in Quote Compare page
        /// </summary>
        public void SelectQuoteDuringCompare()
        {
            //Click(BasicSelectDurinCompare, "Basic Quote");
            Click(ComprehensiveSelectDurinCompare, "Comprehensive Quote");
        }

        #endregion

    }
}

