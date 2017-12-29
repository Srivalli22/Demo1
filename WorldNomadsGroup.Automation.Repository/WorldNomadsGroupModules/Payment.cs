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
    /// Payment page locators and functions
    /// </summary>

    public class Payment : ActionEngine
    {
        #region Constructor
        public Payment()
        {

        }
        public Payment(RemoteWebDriver driver)
        {
            this.Driver = driver;
        }

        public Payment(Dictionary<string, string> testNode)
        {
            this.TestDataNode = testNode;
        }

        public Payment(RemoteWebDriver driver, Dictionary<string, string> testNode)
        {
            this.Driver = driver;
            this.TestDataNode = testNode;
        }

        public Payment(RemoteWebDriver driver, Dictionary<string, string> testNode, Iteration iteration)
        {
            this.Driver = driver;
            this.TestDataNode = testNode;
            this.Reporter = iteration;
        }
        #endregion

        #region Object definitions
        public static By ClickInsertName = By.XPath("//div[@id='payment-details']/fieldset/div[2]/input[2]");
        public static By CreditCardNumber = By.Id("PaymentDto_CreditCardNumber");
        public static By CreditExpiryMonth = By.Id("PaymentDto_CreditCardExpiryDateMonthId");
        public static By CreditExpiryYear = By.Id("PaymentDto_CreditCardExpiryDateYearId");
        public static By SecurityCode = By.Id("PaymentDto_VerificationNumber");
        public static By BuyNowButton = By.Name("SubmitPayment");

        //public static By EditDetails = By.XPath(".//a[text()='Edit details']");
        public static By EditDetails = By.XPath(".//*[@id='policyForm']/div[6]/div/h3/div/a");
        //public static By EditOptionsOrDiscounts = By.XPath(".//a[text()='Edit options or discounts']");
        public static By EditOptionsOrDiscounts = By.XPath(".//*[@id='policyForm']/div[7]/div/h3[2]/div/a");
        //public static By EditTravelClear = By.XPath(".//a[text()='Edit TravelClear']");
        public static By EditTravelClear = By.XPath(".//*[@id='policyForm']/div[7]/div/h3[3]/div/a");
        public static By CardNameError = By.XPath(".//li/span[text()='Please enter the name on the credit card.']");
        public static By CardNumberError = By.XPath(".//li/span[text()='Please enter a valid credit card number.']");
        public static By CardMonthError = By.XPath(".//li/span[text()='You must enter the credit card expiry month .']");
        public static By CardYearError = By.XPath(".//li/span[text()='Please select the credit card expiry year.']");
        public static By CardCVVError = By.XPath(".//li/span[contains(text(),'Please enter a valid verification number')]");

        public static By Enlarge = By.Name("ShowPaymentHint");
        public static By Hide = By.Name("HidePaymentHint");

        public static By EditTripDetails = By.XPath(".//*[@id='policyForm']/div[5]/div/input");
        public static By EditTripCancel = By.Name("CancelEditTrip");
        public static By DetailsContinue = By.Name("SubmitDetailsAsGuest");
        public static By TC_Continue = By.XPath("//*[@id='container']/div[4]/form/div[2]/div/div/div/button");
        public static By ContinueQuote = By.XPath("(//button[contains(text(),'Continue Quote')])[2]");
        public static By ExtraCancellationDropdown = By.Id("ActiveQuoteDto_PolicyOptions_5__PolicyTripCancellationOptionDto_TripCancellationOptionNetPriceId");
        #endregion

        #region Page Functions

        /// <summary>
        /// Enter Payment details and verify field level erros
        /// </summary>
        public void PaymentErrorValidations(string strTripLimit)
        {

            //Edit Trip
            Thread.Sleep(5000);
            JsClick(EditTripDetails, "Edit Trip");
            Thread.Sleep(5000);
            Click(EditTripCancel, "Edit Trip Cancel");

            //Edit details
            Thread.Sleep(3000);
            JsClick(EditDetails, "Edit Details");
            JsClick(DetailsContinue, "Continue");

            //Edit Options and Discounts
            JsClick(EditOptionsOrDiscounts, "Edit Options and Discounts");
            SelectExtracancellationoption(strTripLimit);
            Thread.Sleep(2000);
            JsClick(ContinueQuote, "Continue");
            Thread.Sleep(2000);
            Click(TC_Continue, "Continue");
            Thread.Sleep(2000);
            JsClick(DetailsContinue, "Continue");

            //Edit Travel Clear
            JsClick(EditTravelClear, "Edit Travel Clear");
            Thread.Sleep(2000);

            //Card error validations
            JsClick(BuyNowButton, "BuyNowButton");
            Thread.Sleep(2000);


            if (IsElementPresent(CardNameError, "Card Name error"))
            {
                Reporter.Add(new Act("Successfully verified Card name error Message <b> Please enter the name on the credit card </b>"));
            }
            else
            {
                throw new Exception("Card name error message <b> Please enter the name on the credit card <br />");
            }
            if (IsElementPresent(CardNumberError, "Card Number error"))
            {
                Reporter.Add(new Act("Successfully verified Card Number error Message <b> Please enter a valid credit card number </b>"));
            }
            else
            {
                throw new Exception("Card Number error message <b> Please enter a valid credit card number <br />");
            }
            if (IsElementPresent(CardMonthError, "Card Month error"))
            {
                Reporter.Add(new Act("Successfully verified Card Month error Message <b> You must enter the credit card expiry month </b>"));
            }
            else
            {
                throw new Exception("Card Month error message <b> You must enter the credit card expiry month <br />");
            }
            if (IsElementPresent(CardYearError, "Card Year error"))
            {
                Reporter.Add(new Act("Successfully verified Card Year error Message <b> Please select the credit card expiry year </b>"));
            }
            else
            {
                throw new Exception("Card Year error message <b> Please select the credit card expiry year <br />");
            }
            if (IsElementPresent(CardCVVError, "Card CVV error"))
            {
                Reporter.Add(new Act("Successfully verified Card CVV error Message <b> Please enter a valid verification number (3 digits for all credit cards except American Express, which has 4) </b>"));
            }
            else
            {
                throw new Exception("Card CVV error message <b> Please enter a valid verification number (3 digits for all credit cards except American Express, which has 4) <br />");
            }


            JsClick(Enlarge, "Enlarge");
            Thread.Sleep(2000);
            Click(Hide, "Hide");
            Thread.Sleep(2000);




        }


        /// <summary>
        /// Enter Payment details
        /// </summary>
        public void PaymentDetails(string creditcardnum, string ExpiryMonth, string ExpiryYear, string Code)
        {
            //Click(ClickInsertName, "ClickInsertName");
            JsClick(ClickInsertName, "ClickInsertName");
            Thread.Sleep(2000);

            //string cardnoenc = Encrypt("4444333322221111");
            //string cardnodec = Decrypt(cardnoenc);


            SetValueToObject(CreditCardNumber, Decrypt(creditcardnum), 60);
            SelectByOptionText(CreditExpiryMonth, Decrypt(ExpiryMonth), "CreditExpiryMonth");
            SelectByOptionText(CreditExpiryYear, Decrypt(ExpiryYear), "CreditExpiryYear");
            SetValueToObject(SecurityCode, Decrypt(Code), 120);

            //SetValueToObject(CreditCardNumber, creditcardnum, 60);
            ////Type(CreditCardNumber, creditcardnum, "credit card number");
            //SelectByOptionText(CreditExpiryMonth, CreditExpiryMonth, "CreditExpiryMonth");
            //SelectByOptionText(CreditExpiryYear, CreditExpiryYear, "CreditExpiryYear");
            //SetValueToObject(SecurityCode, SecurityCode, 120);

            Click(BuyNowButton, "BuyNowButton");
        }


        /// <summary>
        /// Select extra cancel option
        /// </summary>
        public void SelectExtracancellationoption(string extracancelvalue)
        {
            SelectByOptionText(ExtraCancellationDropdown, extracancelvalue, "extracancellationoption");
            Thread.Sleep(2000);
        }

        #endregion
    }
}

