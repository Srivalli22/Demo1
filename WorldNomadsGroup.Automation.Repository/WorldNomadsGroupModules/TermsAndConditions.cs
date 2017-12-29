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
    /// T n C page locators and functions
    /// </summary>

    public class TermsAndConditions : ActionEngine
    {
        #region Constructor
        public TermsAndConditions()
        {

        }
        public TermsAndConditions(RemoteWebDriver driver)
        {
            this.Driver = driver;
        }

        public TermsAndConditions(Dictionary<string, string> testNode)
        {
            this.TestDataNode = testNode;
        }

        public TermsAndConditions(RemoteWebDriver driver, Dictionary<string, string> testNode)
        {
            this.Driver = driver;
            this.TestDataNode = testNode;
        }

        public TermsAndConditions(RemoteWebDriver driver, Dictionary<string, string> testNode, Iteration iteration)
        {
            this.Driver = driver;
            this.TestDataNode = testNode;
            this.Reporter = iteration;
        }
        #endregion

        #region Object definitions
        public static By TC_1 = By.Id("regulatoryDeclaration_0");
        public static By TC_2 = By.Id("regulatoryDeclaration_1");
        public static By TC_3 = By.Id("regulatoryDeclaration_2");
        public static By TC_4 = By.Id("regulatoryDeclaration_3");
        public static By TC_Continue = By.XPath("//*[@id='container']/div[4]/form/div[2]/div/div/div/button");
        public static By TnCError = By.XPath(".//ul[@class='input-validation-errors']//span[text()='Please confirm all regulatory declarations to continue.']");
        public static By PDS = By.XPath(".//a[text()='Product Disclosure Statement (PDS) and SPDS']");
        #endregion

        #region Page Functions

        /// <summary>
        /// Select T n C's
        /// </summary>
        public void SelectTermandConditions()
        {
            Click(TC_1, "emailcopy");
            Click(TC_2, "documents");
            Click(TC_3, "residents of Australia");
            Click(TC_4, "residents of Australia");

            Click(TC_Continue, "Continue");
        }


        /// <summary>
        /// Verify T n C erros
        /// </summary>
        public void VerifyTermandConditionsFieldErrors()
        {
            Click(TC_Continue, "Continue");

            if (IsElementPresent(TnCError, "Terms and Conditions error"))
            {
                Reporter.Add(new Act("Successfully verified Terms and Conditions error Message <b> Please confirm all regulatory declarations to continue </b>"));
            }
            else
            {
                throw new Exception("Terms and Conditions error message <b> Please confirm all regulatory declarations to continue <br />");
            }
            //Click(PDS, "PDS Link");

        }

        #endregion
    }
}
