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
using System.Security.Cryptography;

namespace WorldNomadsGroup.Automation.Repository
{
    /// <summary>
    /// Login page locators and functions
    /// </summary>

    public class Login : ActionEngine
    {

        #region Constructor
        public Login()
        {

        }
        public Login(RemoteWebDriver driver)
        {
            this.Driver = driver;
        }

        public Login(Dictionary<string, string> testNode)
        {
            this.TestDataNode = testNode;
        }

        public Login(RemoteWebDriver driver, Dictionary<string, string> testNode)
        {
            this.Driver = driver;
            this.TestDataNode = testNode;
        }

        public Login(RemoteWebDriver driver, Dictionary<string, string> testNode, Iteration iteration)
        {
            this.Driver = driver;
            this.TestDataNode = testNode;
            this.Reporter = iteration;
        }
        #endregion

        #region Object definitions
        public static By EmailAddress = By.Id("AuthenticationRequestDto_EmailAddress");
        public static By Password = By.Id("AuthenticationRequestDto_Password");
        public static By SignIn = By.XPath("//button[contains(text(),'Log In')]");
        public static By InvalidLogin = By.XPath(".//div[@class='messages error']/ul/li/span[text()='Either your login or password is incorrect.']");
        public static By EmailError = By.XPath(".//*[@id='signInEditor']//label[text()='Please enter your email address.']");
        public static By PasswordError = By.XPath(".//*[@id='signInEditor']//label[text()='Please enter a password.']");
        #endregion

        #region Page Functions


        /// <summary>
        /// Login to Application
        /// </summary>
        public void EnterLoginDetails(string strEmailAddress, string strPassword)
        {
            SetValueToObject(EmailAddress, strEmailAddress, 60);
            SetValueToObject(Password, strPassword, 60);
            Click(SignIn, "Login");
        }

        /// <summary>
        /// Launches WNG Sure Save application
        /// </summary>
        public void HomePageNavigation(string urlPrefix)
        {
            try
            {                
                string url = Util.EnvironmentSettings["Server"];
                url = url.TrimEnd();
                NavigateToUrl(url + urlPrefix);
                Reporter.Add(new Act("Successfully navigated to " + url + urlPrefix));
                
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Validate field level error messages in Login screen
        /// </summary>
        public void ValidateLoginFieldErrorMessages(string strEmail, string strPassword)
        {
            SetValueToObject(EmailAddress, strEmail, 60);
            SetValueToObject(Password, strPassword, 60);
            Click(SignIn, "Login");

            //Validate error messages for invalid email/password
            if ((strEmail != "") && (strPassword != ""))
            {
                if (IsElementPresent(InvalidLogin, "Invalid login message"))
                {
                    Reporter.Add(new Act("Successfully verified the login error Message <b> Either your login or password is incorrect </b>"));
                }
                else
                {
                    throw new Exception(" Login error message <b> Either your login or password is incorrect </b> not present  <br />");
                }
            }
            //Validate error messages for blank email/password
            else
            {
                if (IsElementPresent(EmailError, "Email blank error"))
                {
                    Reporter.Add(new Act("Successfully verified the Email field error message <b> Please enter your email address </b>"));
                }
                else
                {
                    throw new Exception("Email field error message <b> Please enter your email address </b> not present  <br />");
                }

                if (IsElementPresent(PasswordError, "Password blank error"))
                {
                    Reporter.Add(new Act("Successfully verified the Password field error message <b> Please enter a password </b>"));

                }
                else
                {
                    throw new Exception("Password field error message <b> Please enter a password </b> not present  <br />");
                }
            }
        }


        #endregion

    }
}

