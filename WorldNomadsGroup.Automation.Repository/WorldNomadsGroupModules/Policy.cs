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
    /// Policy page locators and functions
    /// </summary>

    public class Policy : ActionEngine
    {
        string policynumber = "";

        #region Constructor
        public Policy()
        {

        }
        public Policy(RemoteWebDriver driver)
        {
            this.Driver = driver;
        }

        public Policy(Dictionary<string, string> testNode)
        {
            this.TestDataNode = testNode;
        }

        public Policy(RemoteWebDriver driver, Dictionary<string, string> testNode)
        {
            this.Driver = driver;
            this.TestDataNode = testNode;
        }

        public Policy(RemoteWebDriver driver, Dictionary<string, string> testNode, Iteration iteration)
        {
            this.Driver = driver;
            this.TestDataNode = testNode;
            this.Reporter = iteration;
        }
        #endregion

        #region Object definitions
        public static By PolicyNumber = By.XPath(".//*[@id='container']//div[@class='policy-number']/h2/a");
        public static By PoliciesTab = By.XPath("//a[text()='Policies']");
        public static By QuoteSearchText = By.Id("QuoteSearchCriteriaDto_QuoteReferenceNumber");
        public static By PolicySearchText = By.Id("PolicySearchCriteriaDto_PolicyNumber");
        public static By SearchButton = By.XPath("//button[text()='Search']");
        public static By PolicyLink(string policynum)
        {
            return By.XPath(".//a[text()='" + policynum + "']");
        }
        public static By SearchError = By.XPath(".//span[text()='Please enter at least one search criteria.']");

        public static By ViewCOI = By.XPath("//a[text()='View COI']");
        public static By CancelPolicy = By.XPath("//a[text()='Cancel Policy']");
        public static By TC1_CancelPolicy = By.Id("regulatoryDeclaration_0");
        public static By TC2_CancelPolicy = By.Id("regulatoryDeclaration_1");
        public static By YesCancel = By.Name("ConfirmCancelPolicy");
        public static By NoCancel = By.Name("CloseConfirmCancelPolicy");

        public static By CancellationReasonPolicy = By.Id("PolicyNoteDto_PolicyNoteReasonId");
        public static By CancellationReasonText = By.Id("PolicyNoteDto_NoteText");
        public static By ConfirmCancellationPolicy = By.XPath("//button[@value='Confirm Cancellation']");

        // Verification
        public static By PolicyHolderFirstName = By.XPath(".//*[@id='container']//div[@class='panel light traveller-details']/div//li[2]/dl/dd");
        public static By PolicyHolderLastName = By.XPath(".//*[@id='container']//div[@class='panel light traveller-details']/div//li[3]/dl/dd");
        public static By PolicyHolderEmail = By.XPath(".//*[@id='container']//div[@class='panel light traveller-details']/div//li[4]/dl/dd");

        public static By ViewPDS = By.XPath(".//a[text()='View PDS']");
        public static By AbortPolicyCancel = By.XPath(".//a[text()='Abort Cancellation']");
        #endregion

        #region Functions

        /// <summary>
        /// Fetch Policy number
        /// </summary>
        public string FetchPolicyNumber()
        {
            policynumber = GetText(PolicyNumber, "policynumber");
            return policynumber;
        }


        /// <summary>
        /// Click Policy number
        /// </summary>
        public void ClickPolicyNumber()
        {
            Click(PolicyNumber, "PolicyNumber");
        }


        /// <summary>
        /// Search Policy
        /// </summary>
        public void SearchPolicy(string strPolicy)
        {
            Click(PoliciesTab, "PoliciesTab");
            SetValueToObject(PolicySearchText, strPolicy, 60);
            Click(SearchButton, "SearchButton");

            JsClick(PolicyLink(strPolicy), "PolicyLink");
        }


        /// <summary>
        /// Verify blank Policy search error
        /// </summary>
        public void ValidateSearchPolicyError()
        {
            Click(PoliciesTab, "PoliciesTab");
            Click(SearchButton, "SearchButton");

            if (IsElementPresent(SearchError, "Policy Search error"))
            {
                Reporter.Add(new Act("Successfully verified Policy Search error Message <b> Please enter at least one search criteria </b>"));
            }
            else
            {
                throw new Exception("Policy Search error message <b> Please enter at least one search criteria <br />");
            }
        }


        /// <summary>
        /// Verify Policy details
        /// </summary>
        public void VerifyPolicyDetails(string FirstName, string LastName, string email)
        {
            string fname = GetText(PolicyHolderFirstName, "PolicyHolderFirstName");
            string surname = GetText(PolicyHolderLastName, "PolicyHolderLastName");
            string Holderemail = GetText(PolicyHolderEmail, "PolicyHolderEmail");

            if (fname == FirstName)
            {
                Reporter.Add(new Act("Successfully Verified first name. Actual is <b>" + fname + "</b> Expected value is <b>" + FirstName + "</b>"));
            }
            else
            {
                Reporter.Add(new Act("Failed to Verify first name. Actual is <b>" + fname + "</b> Expected value is <b>" + FirstName + "</b>"));
            }

            if (surname == LastName)
            {
                Reporter.Add(new Act("Successfully Verified sur name. Actual is <b>" + surname + "</b> Expected value is <b>" + LastName + "</b>"));
            }
            else
            {
                Reporter.Add(new Act("Failed to Verify sur name. Actual is <b>" + surname + "</b> Expected value is <b>" + LastName + "</b>"));
            }

            if (Holderemail == email)
            {
                Reporter.Add(new Act("Successfully Verified email. Actual is <b>" + Holderemail + "</b> Expected value is <b>" + email + "</b>"));
            }
            else
            {
                Reporter.Add(new Act("Failed to Verify email. Actual is <b>" + Holderemail + "</b> Expected value is <b>" + email + "</b>"));
            }


            //COI
            if (IsElementPresent(ViewCOI, "View COI"))
            {
                Reporter.Add(new Act("Successfully verified <b> View COI </b>link"));
            }
            else
            {
                throw new Exception("<b> View COI </b> verification failed");
            }

            //PDS
            if (IsElementPresent(ViewPDS, "View PDS"))
            {
                Reporter.Add(new Act("Successfully verified <b> View PDS </b> link"));
            }
            else
            {
                throw new Exception("<b> View PDS </b> verification failed ");
            }

        }

        /// <summary>
        /// Verify Cancel Policy option
        /// </summary>
        public void VerifyCancelPolicyOption(bool blnPresence)
        {
            if (blnPresence)
            {
                if (IsElementPresent(CancelPolicy, "Cancel Policy"))
                {
                    Reporter.Add(new Act("<b> Cancel Policy </b> option available"));
                }
                else
                {
                    throw new Exception("<b> Cancel Policy </b> option not available");
                }
            }
            else
            {
                if (CheckIfObjectExists(CancelPolicy, 5))
                {
                    throw new Exception("<b> Cancel Policy </b> option is available");
                }
                else
                {
                    Reporter.Add(new Act("<b> Cancel Policy </b> option not available")); ;
                }
            }



        }
        #endregion

    }
}

