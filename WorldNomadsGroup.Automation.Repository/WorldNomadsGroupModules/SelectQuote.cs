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
    /// Select quote page locators and functions
    /// </summary>

    public class SelectQuote : ActionEngine
    {
        #region Constructor
        public SelectQuote()
        {

        }
        public SelectQuote(RemoteWebDriver driver)
        {
            this.Driver = driver;
        }

        public SelectQuote(Dictionary<string, string> testNode)
        {
            this.TestDataNode = testNode;
        }

        public SelectQuote(RemoteWebDriver driver, Dictionary<string, string> testNode)
        {
            this.Driver = driver;
            this.TestDataNode = testNode;
        }

        public SelectQuote(RemoteWebDriver driver, Dictionary<string, string> testNode, Iteration iteration)
        {
            this.Driver = driver;
            this.TestDataNode = testNode;
            this.Reporter = iteration;
        }
        #endregion

        #region Object definitions
        public static By BasicSelect = By.XPath("//*[@id='container']/div[4]/form/div[4]/ul/li[2]/div[1]/div/div/div/a");
        public static By ComprehensiveSelect = By.XPath("//*[@id='container']/div[4]/form/div[4]/ul/li[1]/div[1]/div/div/div/a");
        //public static By ComprehensiveCheckbox = By.XPath("(.//input[@name='quotesToCompare'])[1]");
        public static By ComprehensiveCheckbox = By.XPath(".//*[@id='container']/div[4]/form/div[4]/ul/li[1]/div[2]/div/input");
        //public static By BasicCheckbox = By.XPath("(.//input[@name='quotesToCompare'])[2]");
        public static By BasicCheckbox = By.XPath(".//*[@id='container']/div[4]/form/div[4]/ul/li[2]/div[2]/div/input");
        public static By Compare = By.XPath("(.//*[@name='SubmitQuoteSelector'])[1]");
        //public static By EditTripDetails = By.Name("EditTrip");
        //public static By EditTripDetails = By.XPath(".//input[@value='Edit Trip Details']");
        public static By EditTripDetail = By.XPath(".//*[@id='policyForm']/div[5]/div/input");
        public static By EditTripDetails_SelectQuote = By.XPath(".//*[@id='container']/div[4]/form/div[2]/div/input");

        //Edit your trip
        public static By EditTripWhereCountryClear = By.XPath(".//a[text()='×']");
        public static By EditTripAge = By.Id("ActiveQuoteDto_TravellerAges_0__Age");
        public static By EditTripSubmit = By.Name("SubmitQuickQuote");
        public static By EditTripCancel = By.Name("CancelEditTrip");
        #endregion

        #region Page Functions

        /// <summary>
        /// Edit Trip details
        /// </summary>
        public void EditTripDetails(string strAge)
        {
            Thread.Sleep(3000);
            Click(EditTripDetails_SelectQuote, "Edit Trip Details");
            Thread.Sleep(1000);
            Click(EditTripCancel, "Cancel Edit Trip");

            Thread.Sleep(3000);
            Click(EditTripDetails_SelectQuote, "Edit Trip Details");
            Thread.Sleep(1000);
            SetValueToObject(EditTripAge, strAge, 60);
            Click(EditTripSubmit, "Submit");

        }


        /// <summary>
        /// Select a quote
        /// </summary>
        public void SelectQuotes()
        {
            //Click(BasicSelect, "Basic Quote");
            Click(ComprehensiveSelect, "Comprehensive Quote");
        }


        /// <summary>
        /// Compare quotes
        /// </summary>
        public void CompareQuote(string strAge)
        {
            Thread.Sleep(3000);
            JsClick(ComprehensiveCheckbox, " Comprehensive Checkbox");
            Thread.Sleep(1000);
            JsClick(BasicCheckbox, " Basic Checkbox");
            Thread.Sleep(3000);
            JsClick(Compare, " Compare");
            Thread.Sleep(3000);

            //EditTripDetails(strAge);

        }

        #endregion

    }
}

