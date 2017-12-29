using WorldNomadsGroup.Automation.Accelerators;
using WorldNomadsGroup.Automation.Repository;
using System;
using System.Threading;
using OpenQA.Selenium;

namespace WorldNomadsGroup.Automation.Tests
{
    public class PartnerPurchase_And_Cancel_HappyPath : BaseTest
    {
        protected override void ExecuteTestCase()
        {
            Reporter.Add(new Chapter("Execute test case- 'PartnerPurchase_And_Cancel_HappyPath':  "));

            #region Page References
            var Login = Page<Login>(Driver, TestData, Reporter);
            var GetQuote = Page<GetQuote>(Driver, TestData, Reporter);
            var SelectQuote = Page<SelectQuote>(Driver, TestData, Reporter);
            var CompareQuote = Page<CompareQuotes>(Driver, TestData, Reporter);
            var TermsAndConditions = Page<TermsAndConditions>(Driver, TestData, Reporter);
            var QuoteAndOptions = Page<QuoteAndOptions>(Driver, TestData, Reporter);
            var PersonalDetails = Page<PersonalDetails>(Driver, TestData, Reporter);
            var Payment = Page<Payment>(Driver, TestData, Reporter);
            var Policy = Page<Policy>(Driver, TestData, Reporter);
            var CancelPolicy = Page<CancelPolicy>(Driver, TestData, Reporter);
            #endregion

            #region Variable Declaration
            var strEmailAddress = TestData["EmailAddress"];
            var strPassword = TestData["Password"];
            var strWhereCountry = TestData["WhereCountry"];
            //var strDepartureDate = TestData["DepartureDate"];
            //var strReturnDate = TestData["ReturnDate"];
            var strDuration = TestData["Duration"];
            var strAge = TestData["Age"];
            var strTripLimit = TestData["TripLimit"];
            var strTravellerFirstName = TestData["TravellerFirstName"];
            var strTravellerLastName = TestData["TravellerLastName"];
            var strDOBDay = TestData["DOBDay"];
            var strDOBMonth = TestData["DOBMonth"];
            var strDOBYear = TestData["DOBYear"];
            var strTravellerEmailAddress = TestData["TravellerEmailAddress"];
            var strHomePhone = TestData["HomePhone"];
            var strExistingMedicalConditions = TestData["ExistingMedicalConditions"];
            var strNoOfDependants = TestData["NoOfDependants"];
            var strStreetAddress1 = TestData["StreetAddress1"];
            var strStreetAddress2 = TestData["StreetAddress2"];
            var strSuburb = TestData["Suburb"];
            var strState = TestData["State"];
            var strPostalCode = TestData["PostalCode"];
            var strCreditCardNumber = TestData["CreditCardNumber"];
            var strExpiryMonth = TestData["ExpiryMonth"];
            var strExpiryYear = TestData["ExpiryYear"];
            var strSecurityCode = TestData["SecurityCode"];
            var strCancellationReasonPolicy = TestData["CancellationReasonPolicy"];
            var strCancellationReason = TestData["CancellationReason"];
            #endregion

            var browser = Driver.GetType().ToString();
            //sample description

            try
            {

                #region Launch 'WorldNomadsGroup' application
                Step = "Launch WorldNomadsGroup application";
                Login.HomePageNavigation("");
                Thread.Sleep(5000);
                #endregion

                //#region Login 
                //Step = "Login";
                //Login.EnterLoginDetails(strEmailAddress, strPassword);
                //#endregion

                #region Enter Travel details 
                Step = "Enter Travel details";
                GetQuote.EnterTravelDetailsAngular(strWhereCountry, strDuration, strAge);
                GetQuote.SelectQuote();
                #endregion

                //#region Select quote
                //Step = "Select quote";
                //SelectQuote.SelectQuotes();
                //#endregion

                //#region Select Trip limit and Continue Quote
                //Step = "Select Trip limit and Continue Quote";
                //QuoteAndOptions.SelectExtracancellationoption(strTripLimit);
                //QuoteAndOptions.ClickContinueQuote();
                //#endregion

                //#region Term and Conditions
                //Step = "Term and Conditions";
                //TermsAndConditions.SelectTermandConditions();
                //#endregion;

                //#region Personal Details
                //Step = "Personal Details";
                //PersonalDetails.EnterPersonalDetails(strTravellerFirstName, strTravellerLastName, strDOBDay, strDOBMonth, strDOBYear, strTravellerEmailAddress, strHomePhone, strNoOfDependants, strStreetAddress1, strStreetAddress2, strSuburb, strState, strPostalCode);
                //#endregion

                //#region Payment
                //Step = "Payment";
                //Payment.PaymentDetails(strCreditCardNumber, strExpiryMonth, strExpiryYear, strSecurityCode);
                //#endregion

                //#region Fetch Policy Number
                //Step = "Fetch Policy Number";
                //string policynumber = Policy.FetchPolicyNumber();
                //Policy.ClickPolicyNumber();
                //#endregion

                //#region Search Policy
                //Step = "Search Policy";
                //Policy.SearchPolicy(policynumber);
                //#endregion

                //#region Verify Policy details
                //Step = "Verify Policy details";
                //Policy.VerifyPolicyDetails(strTravellerFirstName, strTravellerLastName, strTravellerEmailAddress);
                //#endregion

                //#region Policy Cancellation
                //Step = "Cancelaltion of Policy";
                //CancelPolicy.PolicyCancel(strCancellationReasonPolicy, strCancellationReason);
                //#endregion

            }

            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                throw new Exception("", e);
            }
        }
    }
}
