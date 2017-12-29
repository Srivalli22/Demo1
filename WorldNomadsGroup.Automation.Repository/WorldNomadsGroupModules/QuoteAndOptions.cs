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
    /// Quote and Options page locators and functions
    /// </summary>

    public class QuoteAndOptions : ActionEngine
    {
        #region Constructor
        public QuoteAndOptions()
        {

        }
        public QuoteAndOptions(RemoteWebDriver driver)
        {
            this.Driver = driver;
        }

        public QuoteAndOptions(Dictionary<string, string> testNode)
        {
            this.TestDataNode = testNode;
        }

        public QuoteAndOptions(RemoteWebDriver driver, Dictionary<string, string> testNode)
        {
            this.Driver = driver;
            this.TestDataNode = testNode;
        }

        public QuoteAndOptions(RemoteWebDriver driver, Dictionary<string, string> testNode, Iteration iteration)
        {
            this.Driver = driver;
            this.TestDataNode = testNode;
            this.Reporter = iteration;
        }
        #endregion

        #region Object definitions
        public static By ExtraCancellationOption = By.XPath(".//*[@id='ActiveQuoteDto_PolicyOptions_3__PolicyTripCancellationOptionDto_TripCancellationOptionNetPriceId']/option");
        //public static By ExtraCancellationDropdown = By.Id("ActiveQuoteDto_PolicyOptions_3__PolicyTripCancellationOptionDto_TripCancellationOptionNetPriceId");
        public static By ExtraCancellationDropdown = By.Id("ActiveQuoteDto_PolicyOptions_5__PolicyTripCancellationOptionDto_TripCancellationOptionNetPriceId");
        public static By ContinueQuote = By.XPath("(//button[contains(text(),'Continue Quote')])[2]");
        public static By TripCancellationError = By.XPath(".//label[text()='Please select a trip cancellation value.']");
        public static By EmailQuotes = By.Name("OpenEmailQuote");
        public static By SaveAndPrintQuotes = By.Name("ShowQuoteReferenceModal");

        //Email a quote
        public static By EmailFirstName = By.Id("QuoteReferenceDto_FirstName");
        public static By EmailLastName = By.Id("QuoteReferenceDto_Surname");
        public static By EmailQuoteEmailAddress = By.Id("QuoteReferenceDto_EmailAddress");
        public static By SendEmail = By.Name("SubmitEmailQuote");
        public static By NameAndEmailError = By.XPath(".//ul[@class='input-validation-errors']/span[text()='Name and email address are both required for emailing a quote.']");
        public static By EmailQuoteEmailError = By.XPath(".//ul[@class='input-validation-errors']/span[text()='Please enter a valid email address.']");
        public static By EmailQuoteCancel = By.Name("CloseEmailQuote");

        //Save a quote        
        public static By SaveQuoteTelephone = By.Id("QuoteReferenceDto_PhoneNumber");
        public static By SaveThisQuote = By.Name("UpdateQuoteReference");
        public static By SaveQuoteName_Phone_EmailError = By.XPath(".//ul[@class='input-validation-errors']/span[text()='At least one of these fields is required - name, email, phone number or full address.']");
        public static By SaveQuoteEmailError = By.XPath(".//ul[@class='input-validation-errors']/span[text()='Please enter a valid email address.']");
        public static By SaveQuoteCancel = By.Name("CloseQuoteReferenceModal");
        #endregion

        #region Page Functions

        /// <summary>
        /// Verify field level erros during policy cancellation
        /// </summary>
        public void VerifyTripCancellationError()
        {
            ClickContinueQuote();

            if (IsElementPresent(TripCancellationError, "Trip cancellation message"))
            {
                Reporter.Add(new Act("Successfully verified the Trip cancellation error message <b> Please select a trip cancellation value </b>"));

            }
            else
            {
                throw new Exception("Trip cancellation error message <b> Please select a trip cancellation value </b> not present  <br />");
            }

        }


        /// <summary>
        /// Verify Email quote dialog and field level errors
        /// </summary>
        public void EmailQuote(string strFirstName, string strLastName, string strValidEmail, string strInvalidEmail)
        {
            JsClick(EmailQuotes, "Email Quote");

            Click(SendEmail, "Send Email");
            if (IsElementPresent(NameAndEmailError, "Name and Email error"))
            {
                Reporter.Add(new Act("Successfully verified the blank name and email error Message <b> Name and email address are both required for emailing a quote</b>"));
            }
            else
            {
                throw new Exception("Name and email field error message <b> Name and email address are both required for emailing a quote </b> not present  <br />");
            }

            SetValueToObject(EmailFirstName, strFirstName, 60);
            SetValueToObject(EmailLastName, strLastName, 60);
            Driver.FindElement(By.Id("QuoteReferenceDto_Surname")).SendKeys(Keys.Tab);
            SetValueToObject(EmailQuoteEmailAddress, strInvalidEmail, 60);
            Click(SendEmail, "Send Email");
            if (IsElementPresent(EmailQuoteEmailError, "Email error"))
            {
                Reporter.Add(new Act("Successfully verified the invalid email error Message <b> Please enter a valid email address </b>"));
            }
            else
            {
                throw new Exception("Invalid email error message <b> Please enter a valid email address </b> not present  <br />");
            }

            SetValueToObject(EmailFirstName, strFirstName, 60);
            SetValueToObject(EmailLastName, strLastName, 60);
            Driver.FindElement(By.Id("QuoteReferenceDto_Surname")).SendKeys(Keys.Tab);
            SetValueToObject(EmailQuoteEmailAddress, strValidEmail, 60);
            Click(SendEmail, "Send Email");
            Click(EmailQuoteCancel, "Email Quote Close");

        }


        /// <summary>
        /// Verify Save and Print dialog and field level errors
        /// </summary>
        public void SaveAndPrintQuote(string strFirstName, string strLastName, string strValidEmail, string strInvalidEmail, string strValidMobile, string strInvalidMobile)
        {
            Thread.Sleep(2000);
            JsClick(SaveAndPrintQuotes, "Save and Print Quote");

            SetValueToObject(EmailFirstName, "", 60);
            SetValueToObject(EmailLastName, "", 60);
            SetValueToObject(EmailQuoteEmailAddress, "", 60);
            SetValueToObject(SaveQuoteTelephone, "", 60);

            Click(SaveThisQuote, "Save this Quote");

            if (IsElementPresent(SaveQuoteName_Phone_EmailError, "Name Phone and Email error"))
            {
                Reporter.Add(new Act("Successfully verified the blank name mobile and email error Message <b> At least one of these fields is required - name, email, phone number or full address </b>"));
            }
            else
            {
                throw new Exception("Name Mobile and email field error message <b> At least one of these fields is required - name, email, phone number or full address </b> not present  <br />");
            }

            SetValueToObject(EmailQuoteEmailAddress, strInvalidEmail, 60);
            Click(SaveThisQuote, "Save this Quote");

            if (IsElementPresent(SaveQuoteEmailError, "Email error"))
            {
                Reporter.Add(new Act("Successfully verified the invalid email error Message <b> Please enter a valid email address </b>"));
            }
            else
            {
                throw new Exception("Email field error message <b> Please enter a valid email address <br />");
            }

            SetValueToObject(SaveQuoteTelephone, strInvalidMobile, 60);
            Click(SaveThisQuote, "Save this Quote");

            if (IsElementPresent(SaveQuoteEmailError, "Telephone error"))
            {
                Reporter.Add(new Act("Successfully verified the invalid Telephone error Message <b> Please enter a valid email address </b>"));
            }
            else
            {
                throw new Exception("Telephone field error message <b> Please enter a valid email address <br />");

            }

            SetValueToObject(EmailQuoteEmailAddress, strValidEmail, 60);
            SetValueToObject(SaveQuoteTelephone, strValidMobile, 60);
            Click(SaveThisQuote, "Save this Quote");
            Click(SaveQuoteCancel, "Cancel Save Quote");



        }


        /// <summary>
        /// Select extra cancel option
        /// </summary>
        public void SelectExtracancellationoption(string extracancelvalue)
        {
            SelectByOptionText(ExtraCancellationDropdown, extracancelvalue, "extracancellationoption");
            Thread.Sleep(2000);
        }


        /// <summary>
        /// Select continue quote option
        /// </summary>
        public void ClickContinueQuote()
        {
            Thread.Sleep(2000);
            JsClick(ContinueQuote, "ContinueQuote");
        }

        #endregion

    }
}

