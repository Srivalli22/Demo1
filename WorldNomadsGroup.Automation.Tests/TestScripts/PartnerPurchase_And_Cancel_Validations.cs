using WorldNomadsGroup.Automation.Accelerators;
using WorldNomadsGroup.Automation.Repository;
using System;
using System.Threading;
using OpenQA.Selenium;

namespace WorldNomadsGroup.Automation.Tests
{
    public class PartnerPurchase_And_Cancel_Validations : BaseTest
    {
        protected override void ExecuteTestCase()
        {
            Reporter.Add(new Chapter("Execute test case- 'PartnerPurchase_And_Cancel_Validations':  "));
            
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
            var strInvalidEmailAddress = TestData["InvalidEmailAddress"];
            var strInvalidPassword = TestData["InvalidPassword"];
            var strWhereCountry = TestData["WhereCountry"];
            var strSanctionedCountry = TestData["SanctionedCountry"];
            //var strDepartureDate = TestData["DepartureDate"];
            //var strReturnDate = TestData["ReturnDate"];
            var strDuration = TestData["Duration"];
            var strAge = TestData["Age"];
            var strMaxAge = TestData["MaxAge"];
            var strUpdateAge = TestData["UpdateAge"];
            var strTripLimit = TestData["TripLimit"];
            var strTravellerFirstName = TestData["TravellerFirstName"];
            var strTravellerLastName = TestData["TravellerLastName"];
            var strDOBDay = TestData["DOBDay"];
            var strDOBMonth = TestData["DOBMonth"];
            var strDOBYear = TestData["DOBYear"];
            var strTravellerEmailAddress = TestData["TravellerEmailAddress"];
            var strHomePhone = TestData["HomePhone"];
            var strInvalidPhone = TestData["InvalidPhone"];
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
            var strPolicyWithoutCancelOption = TestData["PolicyWithoutCancelOption"];
            #endregion

            var browser = Driver.GetType().ToString();

            try
            {
                #region Launch 'WorldNomadsGroup' application
                Step = "Launch WorldNomadsGroup application";
                Login.HomePageNavigation("");
                #endregion

                #region Login 
                Step = "Login";
                Login.ValidateLoginFieldErrorMessages("", "");

                Login.ValidateLoginFieldErrorMessages(strInvalidEmailAddress, strInvalidPassword);
                Login.EnterLoginDetails(strEmailAddress, strPassword);
                #endregion

                #region Enter Travel details 
                Step = "Enter Travel details";
                GetQuote.EnterInvalidTravelDetailsAndValidateErrors(strWhereCountry, strSanctionedCountry, strMaxAge);
                GetQuote.EnterTravelDetails(strWhereCountry, strDuration, strAge);
                #endregion

                #region Edit Trip details
                Step = "Edit Trip details";
                SelectQuote.EditTripDetails(strUpdateAge);

                #endregion

                #region Compare Quote
                Step = "Compare Quote";
                SelectQuote.CompareQuote(strAge);
                #endregion

                #region Select quote
                Step = "Select quote";
                CompareQuote.SelectQuoteDuringCompare();
                #endregion

                #region Trip cancellation error validation
                Step = "Trip cancellation error validation";
                QuoteAndOptions.VerifyTripCancellationError();
                #endregion

                #region Email quote error validation
                Step = "Email quote error validation";
                QuoteAndOptions.SelectExtracancellationoption(strTripLimit);
                QuoteAndOptions.EmailQuote(strTravellerFirstName, strTravellerLastName, strEmailAddress, strInvalidEmailAddress);
                #endregion

                #region Save and Print quote error validation
                Step = "Save and Print quote error validation";
                QuoteAndOptions.SaveAndPrintQuote("", "", strEmailAddress, strInvalidEmailAddress, strHomePhone, strInvalidPhone);
                #endregion

                #region Select Extra cancellation option
                Step = "Select Extra cancellation option";
                QuoteAndOptions.ClickContinueQuote();
                #endregion

                #region Terms and Conditions error validation
                Step = "Terms and Conditions error validation";
                TermsAndConditions.VerifyTermandConditionsFieldErrors();
                #endregion

                #region Term and Conditions
                Step = "Term and Conditions";
                TermsAndConditions.SelectTermandConditions();
                #endregion;

                #region Personal Details error validation
                Step = "Personal Details error validation";
                PersonalDetails.VerifyPersonalDetailsFieldErrors(strTravellerFirstName, strTravellerLastName, strDOBDay, strDOBMonth, strDOBYear, strTravellerEmailAddress, strHomePhone, strNoOfDependants, strStreetAddress1, strStreetAddress2, strSuburb, strState, strPostalCode, strInvalidEmailAddress, strInvalidPhone);
                #endregion

                #region Payment error validations
                Step = "Payment error validations";
                Payment.PaymentErrorValidations(strTripLimit);
                #endregion

                #region Payment
                Step = "Payment";
                Payment.PaymentDetails(strCreditCardNumber, strExpiryMonth, strExpiryYear, strSecurityCode);
                #endregion

                #region Fetch Policy Number
                Step = "Fetch Policy Number";
                string policynumber = Policy.FetchPolicyNumber();
                Policy.ClickPolicyNumber();
                #endregion

                #region Validate Search Policy error
                Step = "Validate Search Policy error";
                Policy.ValidateSearchPolicyError();
                #endregion

                #region Search Policy
                Step = "Search Policy";
                Policy.SearchPolicy(strPolicyWithoutCancelOption);
                #endregion

                #region Verify Cancel option for which start date has already passed
                Step = "Verify Cancel option for which start date has already passed";
                Policy.VerifyCancelPolicyOption(false);
                #endregion

                #region Search Policy
                Step = "Search Policy";
                Policy.SearchPolicy(policynumber);
                #endregion

                #region Verify Policy details
                Step = "Verify Policy details";
                Policy.VerifyPolicyDetails(strTravellerFirstName, strTravellerLastName, strTravellerEmailAddress);
                #endregion

                #region Validate Policy Cancellation errors
                Step = "Validate Policy Cancellation errors";
                CancelPolicy.ValidatePolicyCancelErrors();
                #endregion

                #region Policy Cancellation
                Step = "Cancelaltion of Policy";
                CancelPolicy.PolicyCancel(strCancellationReasonPolicy, strCancellationReason);
                #endregion

            }

            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                throw new Exception("", e);
            }
        }
    }
    }
