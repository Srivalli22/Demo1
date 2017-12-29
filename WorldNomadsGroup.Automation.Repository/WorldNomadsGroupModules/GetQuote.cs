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
using System.Text.RegularExpressions;

namespace WorldNomadsGroup.Automation.Repository
{
    /// <summary>
    /// Get Quote page locators and functions
    /// </summary>

    public class GetQuote : ActionEngine
    {

        #region Constructor
        public GetQuote()
        {

        }
        public GetQuote(RemoteWebDriver driver)
        {
            this.Driver = driver;
        }

        public GetQuote(Dictionary<string, string> testNode)
        {
            this.TestDataNode = testNode;
        }

        public GetQuote(RemoteWebDriver driver, Dictionary<string, string> testNode)
        {
            this.Driver = driver;
            this.TestDataNode = testNode;
        }

        public GetQuote(RemoteWebDriver driver, Dictionary<string, string> testNode, Iteration iteration)
        {
            this.Driver = driver;
            this.TestDataNode = testNode;
            this.Reporter = iteration;
        }
        #endregion

        #region Object definitions
        public static By DestinationCountryError = By.XPath(".//*[@id='quote-destination-errors']/li/span[text()='Please choose or type the countries in which you are spending most of your time.']");
        public static By AgeBlankError = By.XPath(".//*[@id='quote-traveller-ages-errors']//span[text()='Please enter the traveller age.']");
        public static By WhereCountry = By.XPath(".//*[@id='quote-destination']/div");
        public static By WhereCountryClear = By.XPath(".//*[@id='selected-destinations']/li/a");
        public static By WherePlace(string country)
        {
            return By.XPath(".//*[@id='homepage']/div[4]/ul/li[3]/ul/li[text()='" + country + "']");
        }
        public static By DepartureDate = By.XPath(".//div[@class='input-with-icon']");
        public static By DepatureDay = By.XPath(".//*[@id='homepage']/div[4]/div/div[1]/table/tbody/tr[3]/td[4]/a");
        public static By ReturnDate = By.XPath("//input[@id='return-date-input']");
        public static By ReturnDay = By.XPath(".//*[@id='homepage']/div[4]/div/div[1]/table/tbody/tr[4]/td[2]/a");
        public static By Age = By.Id("ActiveQuoteDto_TravellerAges_0__Age");
        public static By GetQuoteButton = By.Name("SubmitQuickQuote");
        public static By DoNotTravel = By.Name("ContinueDoNotTravelPurchase");
        public static By AgeLimitError = By.XPath(".//*[@id='quote-traveller-ages-errors']//span[text()='Sorry, you must be under 111 years of age to be eligible for our policy.']");
        public static By QuotePageImage = By.XPath(".//div[@class='video']/img");


        //pp
        //public static By WhereCountry1 = By.XPath(".//input[@class='Input-field ng-untouched ng-pristine ng-valid']");
        //public static By WhereCountry1 = By.XPath(".//wng-multi-picker[@data-e2e='qqc-destinationCountriesPicker']//input[@class='Input-field ng-untouched ng-pristine ng-valid']");
        public static By WhereCountry1 = By.XPath(".//wng-multi-picker[@data-e2e='qqc-destinationCountriesPicker']//input");
        //public static By WhereCountry2 = By.XPath(".//input[@class='Input-field ng-pristine ng-valid ng-touched']");
        //public static By WhereCountry2 = By.XPath(".//wng-multi-picker[@data-e2e='qqc-homeCountryPicker']//input[@class='Input-field ng-untouched ng-pristine ng-valid']");
        public static By WhereCountry2 = By.XPath(".//wng-multi-picker[@data-e2e='qqc-homeCountryPicker']//input");
        public static By WherePlace1(string country)
        {
            return By.XPath("(.//div[@class='FilterInput-dropbox DropBox']//button[contains(text(),'" + country + "')])[1]");
            //return By.XPath("(.//div[@class='FilterInput-dropbox DropBox']//button[contains(text(),'" + country + "')])[1]/parent::li");

        }
        public static By DestCountry = By.XPath(".//input[@class='Input-field ng-untouched ng-pristine ng-valid']");
        //public static By DestCountry = By.XPath(".//input[@class='Input-field ng-untouched ng-pristine ng-valid']");
        public static By DepartureDate1 = By.XPath(".//label[contains(text(),'Start date')]//input");
        public static By EndDate1 = By.XPath(".//label[contains(text(),'End date')]//input");
        public static By DepatureDay1 = By.XPath(".//*[@id='homepage']/div[4]/div/div[1]/table/tbody/tr[3]/td[4]/a");
        public static By ReturnDate1 = By.XPath("//input[@id='return-date-input']");
        public static By ReturnDay1 = By.XPath(".//*[@id='homepage']/div[4]/div/div[1]/table/tbody/tr[4]/td[2]/a");
        public static By Age1 = By.XPath(".//input[@class='Qqc-age1 Input-field ng-untouched ng-pristine ng-valid']");
        public static By GetQuoteButton1 = By.XPath(".//button[@class='Qqc-submit button expanded primary arrow']");

        public static By StartDate = By.XPath("(.//input[@class='Input-field'])[1]");
        public static By EndDate = By.XPath("(.//input[@class='Input-field'])[2]");
        public static By Done = By.XPath(".//button[text()='Done']");
        public static By DispDate = By.XPath("(.//span[@class='Datepicker-head-month'])[1]");
        public static By Standard = By.XPath("(//button[@class='button large arrow secondary'])[1]");
        public static By Explorer = By.XPath("(.//button[@class='button large arrow primary'])[1]");
        public static By AddYourDetails = By.XPath("(//button[contains(text(),'Add your details')])[1]");

        //public static By TC1 = By.XPath(".//span[@data-e2e='termsAndConditions-checkbox']//label[@for='tc0']/span[@class='Checkbox-button']");
        //public static By TC1 = By.XPath(".//label[@for='tc0']/span[@class='Checkbox-button']");
        public static By TC1 = By.XPath("(.//label[@for='tc0']/span)[2]");
        public static By TC2 = By.XPath(".//label[@for='tc1']");
        //public static By TC2 = By.XPath(".//label[@for='tc1']/span[@class='Checkbox-button']");
        //public static By TC2 = By.XPath(".//span[@data-e2e='termsAndConditions-checkbox']//label[@for='tc1']/span[@class='Checkbox-button']");
        public static By Accept = By.XPath(".//button[text()='Accept']");

        public static By FN = By.XPath(".//input[@formcontrolname='firstName']");
        public static By LN = By.XPath(".//input[@formcontrolname='lastName']");
        public static By TravellerDOB_Day = By.XPath("(.//wng-daymonthyear-picker/select)[1]");
        public static By TravellerDOB_Month = By.XPath("(.//wng-daymonthyear-picker/select)[2]");
        public static By TravellerDOB_Year = By.XPath("(.//wng-daymonthyear-picker/select)[3]");
        public static By TravellerEmailAddress = By.XPath(".//div[@data-e2e='details-traveller']//input[@formcontrolname='email']");
        public static By TravellerPhoneNumber = By.XPath(".//input[@formcontrolname='phone']");

        public static By ExistingMedicalConditionsNo = By.XPath("(.//input[@id='optInForScreening'][@value='false'])[1]");
        public static By ExistingMedicalConditionsYes = By.XPath("(.//input[@id='optInForScreening'][@value='true'])[1]");
        public static By AssseedExistingMedicalConditionsNo = By.XPath("(.//input[@id='optInForScreening'][@value='false'])[2]");
        public static By AssseedExistingMedicalConditionsYes = By.XPath("(.//input[@id='optInForScreening'][@value='true'])[2]");

        public static By NoOfDependants = By.Id("NumberOfDependents");
        public static By AddressLine1 = By.XPath(".//input[@formcontrolname='addressLine1']");
        public static By AddressLine2 = By.XPath(".//input[@formcontrolname='addressLine2']");
        public static By Suburb = By.XPath(".//input[@formcontrolname='addressLine3']");
        public static By State = By.XPath("//input[@placeholder='State']");
        public static By PostalCode = By.XPath(".//input[@formcontrolname='postCode']");
        public static By ContinuePayment = By.XPath("//button[contains(text(),'Continue to payment')]");


        public static By ExistingMedicalConditions = By.XPath(".//a[text()='existing medical condition(s)']");


        #endregion

        #region Page Functions

        /// <summary>
        /// Enter travel details and Verify field level errors in Get quote page
        /// </summary>
        public void EnterInvalidTravelDetailsAndValidateErrors(string strCountry, string strSanctionedCountry, string strMaxAge)
        {
            //Destination country mandatory 
            Thread.Sleep(9000);
            //WaitForPageLoad(6);
            //CheckIfObjectExists(WherePlace(strCountry), 5);

            JsClick(GetQuoteButton, "SubmitQuote");

            if (IsElementPresent(DestinationCountryError, "Destination country error"))
            {
                Reporter.Add(new Act("Successfully verified the blank destination error Message <b> Please choose or type the countries in which you are spending most of your time </b>"));
            }
            else
            {
                throw new Exception("Destination field error message <b> Please choose or type the countries in which you are spending most of your time </b> not present  <br />");
            }

            //Sanctioned country 
            Click(WhereCountry, "Country");
            SetValueToObject(By.XPath(".//input[@class='pickerInput instructions-open']"), strSanctionedCountry, 60);
            //Driver.FindElement(By.XPath(".//input[@class='pickerInput instructions-open']")).SendKeys(Keys.Tab
            Driver.FindElement(By.XPath(".//em[text()='" + strSanctionedCountry + "']")).Click();


            Thread.Sleep(2000);
            //WaitForPageLoad(6);
            //CheckIfObjectExists(GetQuoteButton, 5);
            JsClick(GetQuoteButton, "SubmitQuote");
            if (IsElementPresent(DoNotTravel, "Do Not Travel Error"))
            {
                Reporter.Add(new Act("Successfully verified the Do Not Travel error Message "));
            }
            else
            {
                throw new Exception("Do Not Travel error message not present");
            }
            Click(DoNotTravel, "Confirm");
            //WaitForPageLoad(6);
            //CheckIfObjectExists(WhereCountry, 5);

            //Blank Age error
            Click(WhereCountry, "Country");

            Thread.Sleep(2000);
            //WaitForPageLoad(6);
            //CheckIfObjectExists(WherePlace(strCountry), 5);
            JsClick(WherePlace(strCountry), "Country");
            Driver.FindElement(By.XPath(".//input[@class='pickerInput instructions-open']")).SendKeys(Keys.Tab);
            Thread.Sleep(2000);
            //WaitForPageLoad(10);
            //CheckIfObjectExists(GetQuoteButton, 5);
            JsClick(GetQuoteButton, "SubmitQuote");

            if (IsElementPresent(AgeBlankError, "Age BlankError"))
            {
                Reporter.Add(new Act("Successfully verified the blank age error Message <b> Please enter the traveller age </b>"));
            }
            else
            {
                throw new Exception("Blank Age error message <b> Please enter the traveller age </b> not present");
            }

            //Age limit error
            SetValueToObject(Age, strMaxAge, 60);
            JsClick(GetQuoteButton, "SubmitQuote");

            if (IsElementPresent(AgeLimitError, "Age Limit Error"))
            {
                Reporter.Add(new Act("Successfully verified the Age Limit error Message <b> Sorry, you must be under 111 years of age to be eligible for our policy </b>"));
            }
            else
            {
                throw new Exception("Age Limit error message <b> Sorry, you must be under 111 years of age to be eligible for our policy </b> not present");
            }


            Click(WhereCountryClear, "Clear Country");
        }


        /// <summary>
        /// Enter travel details
        /// </summary>
        public void EnterTravelDetails(string strCountry, string strDuration, string strAge)
        {
            Thread.Sleep(6000);
            //WaitForPageLoad(6);
            //CheckIfObjectExists(WherePlace(strCountry), 5);
            Click(WhereCountry, "Country");


            Thread.Sleep(2000);
            //WaitForPageLoad(6);
            //CheckIfObjectExists(WherePlace(strCountry), 5);
            JsClick(WherePlace(strCountry), strCountry);
            Driver.FindElement(By.XPath(".//input[@class='pickerInput instructions-open']")).SendKeys(Keys.Tab);
            Thread.Sleep(2000);

            //
            DateTime strDepartureDate1 = DateTime.Now.AddDays(1);
            string strDepartureDate = strDepartureDate1.ToString("dd MMM yyyy");
            DateTime strReturnDate1 = DateTime.Now.AddDays(Convert.ToDouble(strDuration));
            string strReturnDate = strReturnDate1.ToString("dd MMM yyyy");
            //

            SelectDateFromCalendar(strDepartureDate);
            Thread.Sleep(3000);
            SelectDateFromCalendar(strReturnDate);
            Thread.Sleep(3000);
            //WaitForPageLoad(10);
            //CheckIfObjectExists(GetQuote, 5);            
            SetValueToObject(Age, strAge, 60);
            Thread.Sleep(3000);
            //WaitForPageLoad(10);
            //CheckIfObjectExists(GetQuoteButton, 5);
            Click(GetQuoteButton, "SubmitQuote");

        }


        /// <summary>
        /// Select date from Calendar
        /// </summary>
        public void SelectDateFromCalendar(string dateToSelect)
        {
            try
            {
                string givenDate = DateTime.Parse(dateToSelect).ToString("MMMM yyyy");


                // By CalendarMonthYear = By.XPath("//div[@data-role='calendar']/div[@class='k-header']/a[@class='k-link k-nav-fast'][not(ancestor::div[contains(@style,'display: none')])]");
                By CalendarMonthYearNext = By.XPath(".//*[@id='homepage']//a[@class='datepicker-show-next-month']");
                By CalendarMonthYearPrev = By.XPath(".//*[@id='homepage']//a[@class='datepicker-show-past-month']");
                bool boolFlag = true;
                //WaitForPageLoad(10);
                //CheckIfObjectExists(CalendarMonthYearNext, 5);

                while (boolFlag)
                {
                    string month = Driver.FindElement(By.XPath("(.//*[@id='homepage']//div[@class='datepicker-calendar-title']/span[1])[1]")).Text;
                    string year = Driver.FindElement(By.XPath("(.//*[@id='homepage']//div[@class='datepicker-calendar-title']/span[2])[1]")).Text;
                    string displayedDate = DateTime.Parse(month + year).ToString("MMMM yyyy");

                    Console.WriteLine(displayedDate);
                    if (DateTime.Parse(givenDate) == DateTime.Parse(displayedDate))
                    {
                        boolFlag = false;
                    }
                    else if (DateTime.Parse(givenDate) > DateTime.Parse(displayedDate))
                    {
                        Click(CalendarMonthYearNext, "");
                        Thread.Sleep(2000);
                    }
                    else if (DateTime.Parse(givenDate) < DateTime.Parse(displayedDate))
                    {
                        Click(CalendarMonthYearPrev, "");
                        Thread.Sleep(2000);
                    }
                }
                string selectedDate = DateTime.Parse(dateToSelect).ToString("dd");

                Click(By.XPath("(.//*[@id='homepage']//td/a[contains(text(),'" + selectedDate + "')])[1]"), "Date");
                WaitForPageLoad(10);


                Reporter.Add(new Act("Click was successfully performed on the date  " + dateToSelect));

            }

            catch (Exception e)
            {
                throw new Exception("Error in Function SelectDateFromCalendar </br>" + e.Message);
            }
        }


        //Angular
        public void EnterTravelDetailsAngular(string strCountry, string strDuration, string strAge)
        {
          
            Click(WhereCountry1, "Country");
            SetValueToObject(WhereCountry1, strCountry, 10);
            JsClick(WherePlace1(strCountry), strCountry);
            SetValueToObject(WhereCountry2, "Australia", 10);
            JsClick(WherePlace1("Australia"), "Australia");

            DateTime strDepartureDate1 = DateTime.Now.AddDays(1);
            string strDepartureDate = strDepartureDate1.ToString("dd MMM yyyy");
            DateTime strReturnDate1 = DateTime.Now.AddDays(Convert.ToDouble(strDuration));
            string strReturnDate = strReturnDate1.ToString("dd MMM yyyy");

            Click(StartDate, "Start Date");

            SelectDateFromCalendar1(strDepartureDate);
            SelectDateFromCalendar1(strReturnDate);
            Click(Done, "Done");
            SetValueToObject(Age1, strAge, 60);
            Click(GetQuoteButton1, "SubmitQuote");

        }

        public void SelectDateFromCalendar1(string dateToSelect)
        {
            try
            {
                string givenDate = DateTime.Parse(dateToSelect).ToString("MMMM yyyy");


                // By CalendarMonthYear = By.XPath("//div[@data-role='calendar']/div[@class='k-header']/a[@class='k-link k-nav-fast'][not(ancestor::div[contains(@style,'display: none')])]");
                //By CalendarMonthYearNext = By.XPath(".//*[@id='homepage']//a[@class='datepicker-show-next-month']");
                By CalendarMonthYearNext = By.XPath("(.//button[@class='Icon--md Icon--circle Icon--nav icon-wn-arrow-right'])[2]");
                By CalendarMonthYearPrev = By.XPath(".//*[@id='homepage']//a[@class='datepicker-show-past-month']");
                bool boolFlag = true;
                //WaitForPageLoad(10);
                //CheckIfObjectExists(CalendarMonthYearNext, 5);

                while (boolFlag)
                {
                    //string month = Driver.FindElement(By.XPath("(.//*[@id='homepage']//div[@class='datepicker-calendar-title']/span[1])[1]")).Text;
                    //string year = Driver.FindElement(By.XPath("(.//*[@id='homepage']//div[@class='datepicker-calendar-title']/span[2])[1]")).Text;
                    string appdisplaydate = Driver.FindElement(By.XPath("(.//span[@class='Datepicker-month-label'])[1]")).Text;

                    string[] words = Regex.Split(appdisplaydate, "\r\n");

                    string displayedDate = DateTime.Parse(words[0]).ToString("MMMM yyyy");

                    Console.WriteLine(displayedDate);
                    if (DateTime.Parse(givenDate) == DateTime.Parse(displayedDate))
                    {
                        boolFlag = false;
                    }
                    else if (DateTime.Parse(givenDate) > DateTime.Parse(displayedDate))
                    {
                        Click(CalendarMonthYearNext, "");
                        Thread.Sleep(1000);
                    }
                    else if (DateTime.Parse(givenDate) < DateTime.Parse(displayedDate))
                    {
                        Click(CalendarMonthYearPrev, "");
                        Thread.Sleep(1000);
                    }
                }
                string selectedDate = DateTime.Parse(dateToSelect).Day.ToString("d");

                //Click(By.XPath("(.//*[@id='homepage']//td/a[contains(text(),'" + selectedDate + "')])[1]"), "Date");
                Click(By.XPath("(.//div[@class='Datepicker-column Datepicker-primary']//div[@class='Datepicker-calendar']//td/button/span[text()='" + selectedDate + "'])[1]"), "Date");



                WaitForPageLoad(10);


                Reporter.Add(new Act("Click was successfully performed on the date  " + dateToSelect));

            }


            catch (Exception e)
            {
                throw new Exception("Error in Function SelectDateFromCalendar </br>" + e.Message);
            }
        }

        public void SelectQuote()
        {
            try
            {

                Thread.Sleep(5000);
                JsClick(Standard, "Standard");
                Thread.Sleep(3000);
                JsClick(AddYourDetails, "AddYourDetails");
                Thread.Sleep(3000);

                Click(TC1, "TC1");
                Click(TC2, "TC2");
                Click(Accept, "Accept");

                SetValueToObject(FN, "FN", 60);
                SetValueToObject(LN, "LN", 60);
                SelectByOptionText(TravellerDOB_Day, "10", "Day");
                SelectByOptionText(TravellerDOB_Month, "Apr", "Month");
                SelectByOptionText(TravellerDOB_Year, "1984", "Year");
                SetValueToObject(TravellerEmailAddress, "abc@def.com", 60);
                SetValueToObject(TravellerPhoneNumber, "123456789", 60);
                SetValueToObject(AddressLine1, "line1", 60);
                SetValueToObject(AddressLine2, "line2", 60);
                SetValueToObject(Suburb, "sub", 60);
                //SelectByOptionText(State, "New South Wales", "State");
                //SelectByOptionByValue(State, "New South Wales", "State");
                //SelectDropdwonByText(State, "New South Wales", "State");
                SetValueToObject(State, "New South Wales", 10);
                SetValueToObject(PostalCode, "1234", 60);
                JsClick(ContinuePayment, "ContinuePayment");
            }
            catch (Exception e)
            {
                throw new Exception("Error in Function Get Quote </br>" + e.Message);
            }

        }

        public void TestFn()
        {


        }

        #endregion




    }
}

