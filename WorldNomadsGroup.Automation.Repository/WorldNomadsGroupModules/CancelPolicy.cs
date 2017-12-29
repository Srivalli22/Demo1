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
    /// Cancel policy page locators and functions
    /// </summary>

    public class CancelPolicy : ActionEngine
    {

        #region Constructor
        public CancelPolicy()
        {

        }
        public CancelPolicy(RemoteWebDriver driver)
        {
            this.Driver = driver;
        }

        public CancelPolicy(Dictionary<string, string> testNode)
        {
            this.TestDataNode = testNode;
        }

        public CancelPolicy(RemoteWebDriver driver, Dictionary<string, string> testNode)
        {
            this.Driver = driver;
            this.TestDataNode = testNode;
        }

        public CancelPolicy(RemoteWebDriver driver, Dictionary<string, string> testNode, Iteration iteration)
        {
            this.Driver = driver;
            this.TestDataNode = testNode;
            this.Reporter = iteration;
        }
        #endregion

        #region Object definitions        
        public static By PolicyCancell = By.XPath("//div[@class='button-like button-negative cancel-policy']");
        public static By CancellPolicy = By.XPath("//a[text()='Cancel Policy']");
        public static By PolicyCancelError = By.XPath(".//ul[@class='input-validation-errors']//span[text()='Please confirm all regulatory declarations to continue.']");
        public static By NoteCancel = By.Name("Undo");
        public static By TC1_CancelPolicy = By.Id("regulatoryDeclaration_0");
        public static By TC2_CancelPolicy = By.Id("regulatoryDeclaration_1");
        public static By CancellationReasonPolicy = By.Id("PolicyNoteDto_PolicyNoteReasonId");
        public static By CancellationReasonText = By.Id("PolicyNoteDto_NoteText");
        public static By ConfirmCancellationPolicy = By.XPath("//button[@value='Confirm Cancellation']");
        public static By NoteError = By.XPath(".//ul[@class='input-validation-errors']//label[text()='Please enter a policy note.']");
        public static By YesCancel = By.Name("ConfirmCancelPolicy");
        public static By NoCancel = By.Name("CloseConfirmCancelPolicy");
        public static By AbortPolicyCancel = By.XPath(".//a[text()='Abort Cancellation']");
        #endregion

        #region Page Functions

        /// <summary>
        /// Verify field level erros on Cancel Policy page
        /// </summary>
        public void ValidatePolicyCancelErrors()
        {
            Click(CancellPolicy, "CancelPolicy");
            Thread.Sleep(2000);
            Click(NoCancel, "No Cancel");
            Thread.Sleep(2000);
            Click(CancellPolicy, "CancelPolicy");
            Thread.Sleep(2000);
            Click(YesCancel, "YesCancel");

            if (IsElementPresent(PolicyCancelError, "T n C error"))
            {
                Reporter.Add(new Act("Successfully verified T n C error Message <b> PleasePlease confirm all regulatory declarations to continue </b>"));
            }
            else
            {
                throw new Exception("T n C error message <b> Please confirm all regulatory declarations to continue <br />");
            }

            Click(TC1_CancelPolicy, "TC1CancelPolicy");
            Click(TC2_CancelPolicy, "TC2_CancelPolicy");
            Click(YesCancel, "YesCancel");
            Thread.Sleep(2000);

            Click(ConfirmCancellationPolicy, "ConfirmCancellationPolicy");
            if (IsElementPresent(NoteError, "Note error"))
            {
                Reporter.Add(new Act("Successfully verified Note error Message <b> Please enter a policy note </b>"));
            }
            else
            {
                throw new Exception("Note error message <b> Please enter a policy note <br />");
            }

            Click(NoteCancel, "Note Cancel");
            Thread.Sleep(2000);

        }


        /// <summary>
        /// Cancel a policy
        /// </summary>
        public void PolicyCancel(string CancellationReasonPolicy, string CancellationReason)
        {
            Click(CancellPolicy, "CancelPolicy");
            Thread.Sleep(2000);
            Click(TC1_CancelPolicy, "TC1CancelPolicy");
            Click(TC2_CancelPolicy, "TC2_CancelPolicy");
            Click(YesCancel, "YesCancel");
            //SelectByOptionText(CancellationReasonPolicy, CancellationReasonPolicy, "CancellationReasonPolicy");
            Type(CancellationReasonText, CancellationReason, "CancellationReasonText");
            Click(ConfirmCancellationPolicy, "ConfirmCancellationPolicy");



            if (IsElementPresent(AbortPolicyCancel, "Abort Cancellation"))
            {
                Reporter.Add(new Act("Successfully verified <b> Abort Cancellation </b>"));
            }
            else
            {
                throw new Exception("<b> Abort Cancellation </b> verification failed");
            }



        }

        #endregion
    }
}

