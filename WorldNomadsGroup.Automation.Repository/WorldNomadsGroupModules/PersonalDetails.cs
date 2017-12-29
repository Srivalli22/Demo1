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
    /// Personal details page locators and functions
    /// </summary>

    public class PersonalDetails : ActionEngine
    {
        #region Constructor
        public PersonalDetails()
        {

        }
        public PersonalDetails(RemoteWebDriver driver)
        {
            this.Driver = driver;
        }

        public PersonalDetails(Dictionary<string, string> testNode)
        {
            this.TestDataNode = testNode;
        }

        public PersonalDetails(RemoteWebDriver driver, Dictionary<string, string> testNode)
        {
            this.Driver = driver;
            this.TestDataNode = testNode;
        }

        public PersonalDetails(RemoteWebDriver driver, Dictionary<string, string> testNode, Iteration iteration)
        {
            this.Driver = driver;
            this.TestDataNode = testNode;
            this.Reporter = iteration;
        }
        #endregion

        #region Object definitions
        public static By TravellerFirstName = By.Id("ActiveQuoteDto_Travellers_0__FirstName");
        public static By TravellerLastName = By.Id("ActiveQuoteDto_Travellers_0__Surname");
        public static By TravellerDOB_Day = By.Id("ActiveQuoteDto_Travellers_0__DateOfBirthDay");
        public static By TravellerDOB_Month = By.Id("ActiveQuoteDto_Travellers_0__DateOfBirthMonth");
        public static By TravellerDOB_Year = By.Id("ActiveQuoteDto_Travellers_0__DateOfBirthYear");
        public static By TravellerEmailAddress = By.Id("ActiveQuoteDto_Travellers_0__Email");
        public static By TravellerPhoneNumber = By.Id("ActiveQuoteDto_Travellers_0__LandlinePhoneNumber");
        public static By ExistingMedicalConditionsNo = By.XPath("(.//input[@id='optInForScreening'][@value='false'])[1]");
        public static By ExistingMedicalConditionsYes = By.XPath("(.//input[@id='optInForScreening'][@value='true'])[1]");
        public static By AssseedExistingMedicalConditionsNo = By.XPath("(.//input[@id='optInForScreening'][@value='false'])[2]");
        public static By AssseedExistingMedicalConditionsYes = By.XPath("(.//input[@id='optInForScreening'][@value='true'])[2]");

        public static By NoOfDependants = By.Id("NumberOfDependents");
        public static By AddressLine1 = By.Id("ActiveQuoteDto_AddressLine1");
        public static By AddressLine2 = By.Id("ActiveQuoteDto_AddressLine2");
        public static By Suburb = By.Id("ActiveQuoteDto_AddressLine3");
        public static By State = By.Id("ActiveQuoteDto_ProvinceId");
        public static By PostalCode = By.Id("ActiveQuoteDto_PostalCode");
        public static By Country = By.XPath("//*[@id='container']/div[4]/form/div[7]/fieldset/div[7]/p");
        public static By DetailsContinue = By.Name("SubmitDetailsAsGuest");

        public static By ExistingMedicalConditions = By.XPath(".//a[text()='existing medical condition(s)']");
        public static By CloseInfoPanel = By.Id("closeInfoPanel");
        public static By AutomaticallyCovered = By.XPath(".//a[text()='automatically covered']");
        public static By Assessed = By.XPath(".//a[text()='assessed']");
        public static By FirstNameError = By.XPath(".//ul[@class='input-validation-errors']//label[contains(text(),'first name')]");
        public static By LastNameError = By.XPath(".//ul[@class='input-validation-errors']//label[contains(text(),'surname')]");
        public static By DOBError = By.XPath(".//ul[@class='input-validation-errors']//label[contains(text(),'date of birth (day, month and year) to continue.')]");
        public static By PreExistingMedicalConditionError = By.XPath(".//ul[@class='input-validation-errors']//label[contains(text(),'Please confirm if this traveller has a pre-existing medical condition.')]");
        public static By EnterEmailError = By.XPath(".//ul[@class='input-validation-errors']//label[contains(text(),'Please enter your email address.')]");
        public static By ValidEmailError = By.XPath(".//ul[@class='input-validation-errors']//label[contains(text(),'Please enter a valid email address.')]");
        public static By PhoneError = By.XPath(".//ul[@class='input-validation-errors']//label[contains(text(),'Please enter either the phone number or mobile number to continue.')]");
        public static By ValidPhoneError = By.XPath(".//ul[@class='input-validation-errors']//label[contains(text(),'Please enter a valid telephone number')]");

        public static By AddressError = By.XPath(".//ul[@class='input-validation-errors']//label[contains(text(),'You must enter the address to continue.')]");
        public static By CityError = By.XPath(".//ul[@class='input-validation-errors']//label[contains(text(),'You must enter the city name to continue.')]");
        public static By StateError = By.XPath(".//ul[@class='input-validation-errors']//label[contains(text(),'You must enter the state/region to continue.')]");
        public static By PostcodeError = By.XPath(".//ul[@class='input-validation-errors']//label[contains(text(),'You must enter the postcode to continue.')]");
        #endregion

        #region Page Functions

        /// <summary>
        /// Enter Personal details and verify field level erros
        /// </summary>
        public void VerifyPersonalDetailsFieldErrors(string firstname, string lastname, string Day, string Month, string Year, string Email, string MobileNumber, string NoOfDependant, string Line1, string Line2, string Sub, string States, string PostCode, string invliademail, string invalidphone)
        {
            Thread.Sleep(2000);
            SetValueToObject(TravellerFirstName, "", 60);
            SetValueToObject(TravellerLastName, "", 60);

            JsClick(DetailsContinue, "Continue");

            //First Name, Last Name & DOB error validation
            if (IsElementPresent(FirstNameError, "First Name error"))
            {
                Reporter.Add(new Act("Successfully verified First name error Message <b> Please provide the traveller's first name </b>"));
            }
            else
            {
                throw new Exception("First name error message <b> Please provide the traveller's first name <br />");

            }

            if (IsElementPresent(LastNameError, "Last Name error"))
            {
                Reporter.Add(new Act("Successfully verified Last name error Message <b> Please provide the traveller's surname </b>"));
            }
            else
            {
                throw new Exception("Last name error message <b> Please provide the traveller's surname <br />");

            }

            if (IsElementPresent(DOBError, "DOB error"))
            {
                Reporter.Add(new Act("Successfully verified DOB error Message <b> You must enter the traveller's date of birth (day, month and year) to continue </b>"));
            }
            else
            {
                throw new Exception("DOB error message <b> You must enter the traveller's date of birth (day, month and year) to continue <br />");

            }

            SetValueToObject(TravellerFirstName, firstname, 60);
            SetValueToObject(TravellerLastName, lastname, 60);
            SelectByOptionText(TravellerDOB_Day, Day, "Day");
            SelectByOptionText(TravellerDOB_Month, Month, "Month");
            SelectByOptionText(TravellerDOB_Year, Year, "Year");
            SetValueToObject(TravellerEmailAddress, "", 60);
            SetValueToObject(TravellerPhoneNumber, "", 60);

            JsClick(DetailsContinue, "Continue");

            //Existing Medical condition error
            if (IsElementPresent(PreExistingMedicalConditionError, "Existing Medical Condition error"))
            {
                Reporter.Add(new Act("Successfully verified Existing Medical Condition error Message <b> Please confirm if this traveller has a pre-existing medical condition </b>"));
            }
            else
            {
                throw new Exception("Existing Medical Condition error message <b> Please confirm if this traveller has a pre-existing medical condition <br />");
            }


            JsClick(ExistingMedicalConditionsYes, "MedicalConditions");
            JsClick(AssseedExistingMedicalConditionsYes, "MedicalConditions");
            JsClick(DetailsContinue, "Continue");


            //Email and Phone error validations
            if (IsElementPresent(EnterEmailError, "Email error"))
            {
                Reporter.Add(new Act("Successfully verified Email error Message <b> Please enter your email address </b>"));
            }
            else
            {
                throw new Exception("Email error message <b> Please enter your email address <br />");
            }

            if (IsElementPresent(PhoneError, "Phone error"))
            {
                Reporter.Add(new Act("Successfully verified Phone error Message <b> Please enter either the phone number or mobile number to continue </b>"));
            }
            else
            {
                throw new Exception("Phone error message <b> Please enter either the phone number or mobile number to continue <br />");
            }

            SetValueToObject(TravellerEmailAddress, invliademail, 60);
            SetValueToObject(TravellerPhoneNumber, invalidphone, 60);

            JsClick(DetailsContinue, "Continue");

            if (IsElementPresent(ValidEmailError, "Valid Email error"))
            {
                Reporter.Add(new Act("Successfully verified Valid Email error Message <b> Please enter a valid email address </b>"));
            }
            else
            {
                throw new Exception("Valid Email error message <b> Please enter a valid email address <br />");
            }

            if (IsElementPresent(ValidPhoneError, "Valid Phone error"))
            {
                Reporter.Add(new Act("Successfully verified Phone error Message <b> Please enter a valid telephone number </b>"));
            }
            else
            {
                throw new Exception("Phone error message <b> Please enter a valid telephone number <br />");
            }

            SetValueToObject(TravellerEmailAddress, Email, 60);
            SetValueToObject(TravellerPhoneNumber, MobileNumber, 60);
            JsClick(ExistingMedicalConditionsNo, "MedicalConditions");
            JsClick(DetailsContinue, "Continue");

            //Address section error validations

            //address
            if (IsElementPresent(AddressError, "Address error"))
            {
                Reporter.Add(new Act("Successfully verified Address error Message <b> You must enter the address to continue </b>"));
            }
            else
            {
                throw new Exception("Address error message <b> You must enter the address to continue <br />");
            }
            //city
            if (IsElementPresent(CityError, "City error"))
            {
                Reporter.Add(new Act("Successfully verified City error Message <b> You must enter the city name to continue </b>"));
            }
            else
            {
                throw new Exception("City error message <b> You must enter the city name to continue <br />");
            }
            //state
            if (IsElementPresent(StateError, "State error"))
            {
                Reporter.Add(new Act("Successfully verified State error Message <b> You must enter the state/region to continue </b>"));
            }
            else
            {
                throw new Exception("State error message <b> You must enter the state/region to continue <br />");
            }
            //postal code
            if (IsElementPresent(PostcodeError, "Postal code error"))
            {
                Reporter.Add(new Act("Successfully verified Postal code error Message <b> You must enter the postcode to continue </b>"));
            }
            else
            {
                throw new Exception("Postal code error message <b> You must enter the postcode to continue <br />");
            }


            SelectByOptionText(NoOfDependants, NoOfDependant, "NoOfDependants");
            Thread.Sleep(2000);
            SetValueToObject(AddressLine1, Line1, 60);
            SetValueToObject(AddressLine2, Line2, 60);
            SetValueToObject(Suburb, Sub, 60);
            SelectByOptionText(State, States, "State");
            SetValueToObject(PostalCode, PostCode, 60);
            JsClick(DetailsContinue, "Continue");


        }


        /// <summary>
        /// Enter Personal details
        /// </summary>
        public void EnterPersonalDetails(string firstname, string lastname, string Day, string Month, string Year, string Email, string MobileNumber, string NoOfDependant, string Line1, string Line2, string Sub, string States, string PostCode)
        {
            SetValueToObject(TravellerFirstName, firstname, 60);
            SetValueToObject(TravellerLastName, lastname, 60);
            SelectByOptionText(TravellerDOB_Day, Day, "Day");
            SelectByOptionText(TravellerDOB_Month, Month, "Month");
            SelectByOptionText(TravellerDOB_Year, Year, "Year");

            SetValueToObject(TravellerEmailAddress, Email, 60);
            SetValueToObject(TravellerPhoneNumber, MobileNumber, 60);
            Click(ExistingMedicalConditionsNo, "MedicalConditions");
            SelectByOptionText(NoOfDependants, NoOfDependant, "NoOfDependants");
            SetValueToObject(AddressLine1, Line1, 60);
            SetValueToObject(AddressLine2, Line2, 60);

            SetValueToObject(Suburb, Sub, 60);

            SelectByOptionText(State, States, "State");

            SetValueToObject(PostalCode, PostCode, 60);

            Click(DetailsContinue, "Continue");
        }

        #endregion
    }
}

