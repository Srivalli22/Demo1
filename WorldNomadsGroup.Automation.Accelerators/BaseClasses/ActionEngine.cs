using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System.Globalization;
using System.Security.Cryptography;
using System.IO;

namespace WorldNomadsGroup.Automation.Accelerators
{
    public class ActionEngine : BasePage
    {
        

        public ActionEngine()
        {

        }
        public ActionEngine(RemoteWebDriver driver)
        {
            this.Driver = driver;
        }
        public ActionEngine(Dictionary<string, string> testNode)
        {
            this.TestDataNode = testNode;
        }
        public ActionEngine(RemoteWebDriver driver, Dictionary<string, string> testNode)
        {
            this.Driver = driver;
            this.TestDataNode = testNode;
        }
        public ActionEngine(RemoteWebDriver driver, Dictionary<string, string> testNode, Iteration iteration)
        {
            this.Driver = driver;
            this.TestDataNode = testNode;
            this.Reporter = iteration;
        }
        
        #region ObjectRetrievalFunctions
        internal IWebElement GetNativeElement(By lookupBy, int maxWaitTime = 60)
        {
            IWebElement element = null;
            try
            {
                for (int i = 0; i < maxWaitTime; i++)
                {
                    try
                    {
                        element = Driver.FindElement(lookupBy);
                    }
                    catch
                    {
                        Thread.Sleep(1000);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            if (element != null)
            {
                try
                {
                    string script = String.Format(@"arguments[0].style.cssText = ""border-width: 4px; border-style: solid; border-color: {0}"";", "orange");
                    IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)Driver;
                    jsExecutor.ExecuteScript(script, new object[] { element });
                    jsExecutor.ExecuteScript(String.Format(@"$(arguments[0].scrollIntoView(true));"), new object[] { element });
                }
                catch { }
            }
            return element;
        }
        internal IWebElement GetNativeElementInElement(IWebElement parentElement, By lookupBy, int maxWaitTime = 60)
        {
            IWebElement element = null;
            try
            {
                for (int i = 0; i < maxWaitTime; i++)
                {
                    try
                    {
                        element = parentElement.FindElement(lookupBy);
                    }
                    catch
                    {
                        Thread.Sleep(1000);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            if (element != null)
            {
                try
                {
                    string script = String.Format(@"arguments[0].style.cssText = ""border-width: 4px; border-style: solid; border-color: {0}"";", "orange");
                    IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)Driver;
                    jsExecutor.ExecuteScript(script, new object[] { element });
                    jsExecutor.ExecuteScript(String.Format(@"$(arguments[0].scrollIntoView(true));"), new object[] { element });
                }
                catch { }
            }
            return element;
        }
        internal IWebElement WaitForElementVisible(By lookupBy, int maxWaitTime = 60)
        {
            IWebElement element = null;
            try
            {
                element = new WebDriverWait(Driver, TimeSpan.FromSeconds(maxWaitTime)).Until(ExpectedConditions.ElementIsVisible(lookupBy));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            if (element != null)
            {
                try
                {
                    string script = String.Format(@"arguments[0].style.cssText = ""border-width: 4px; border-style: solid; border-color: {0}"";", "orange");
                    IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)Driver;
                    //jsExecutor.ExecuteScript(script, new object[] { element });
                    //jsExecutor.ExecuteScript(String.Format(@"$(arguments[0].scrollIntoView(true));"), new object[] { element });
                }
                catch { }
            }
            return element;
        }
        internal IWebElement WaitForElementIsPresent(By lookupBy, int maxWaitTime = 60)
        {
            IWebElement element = null;
            try
            {
                element = new WebDriverWait(Driver, TimeSpan.FromSeconds(maxWaitTime)).Until(ExpectedConditions.ElementExists(lookupBy));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            if (element != null)
            {
                try
                {
                    string script = String.Format(@"arguments[0].style.cssText = ""border-width: 4px; border-style: solid; border-color: {0}"";", "orange");
                    IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)Driver;
                    jsExecutor.ExecuteScript(script, new object[] { element });
                    jsExecutor.ExecuteScript(String.Format(@"$(arguments[0].scrollIntoView(true));"), new object[] { element });
                }
                catch { }
            }
            return element;
        }
        public bool SearchTextInElement(By lookup, string text)
        {
            IReadOnlyCollection<IWebElement> elements = Driver.FindElements(lookup);
            foreach (IWebElement ele in elements)
            {
                string eleText = ele.Text;
                if (eleText.Equals(text))
                    return true;
            }
            return false;
        }

        public bool SwitchWindowByTitle(string windowTitle)
        {
            bool flag = false;
            try
            {
                IReadOnlyCollection<string> availableWindows = Driver.WindowHandles;
                String currentWindow = Driver.CurrentWindowHandle;
                if (availableWindows.Count > 0)
                {
                    foreach (var windowId in availableWindows)
                    {
                        string switchedWindowTitle = Driver.SwitchTo().Window(windowId).Title;
                        System.Console.WriteLine(switchedWindowTitle + " window title");
                        if (switchedWindowTitle.Equals(windowTitle))
                        {
                            Driver.SwitchTo().Window(windowId);
                            System.Console.WriteLine(switchedWindowTitle + " window title");
                            System.Console.WriteLine(Driver.Url);
                        }
                    }
                }
                Reporter.Add(new Act("Focus navigated to the window with title "
                                     + windowTitle));
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        #region imported
        public bool Type(By lookupBy, string testdata, string locatorName)
        {

            bool flag = false;
            try
            {
                WaitForVisibilityOfElement(lookupBy, 30);
                Driver.FindElement(lookupBy).Clear();
                Driver.FindElement(lookupBy).SendKeys(testdata);
                flag = true;
                Reporter.Add(new Act("Data typing action is performed on <b>" + locatorName + "</b> with data <b>" + testdata + "</b>"));

            }
            catch (Exception ex)
            {
                throw new Exception("Data typing action is not perform on <b>" + locatorName
                                    + "</b> with data <b>" + testdata + "</b><br/>" + ex.Message);
            }
            return flag;
        }
        public bool WaitForElementPresent(By locatorName, String locator)
        {
            bool flag = false;
            try
            {
                WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(30));
                IWebElement element = null;
                element = wait.Until(ExpectedConditions.ElementExists(locatorName));
                Size size = element.Size;
                bool enabled = size.Height > 0;
                if (enabled)
                {
                    flag = true;
                }
                else
                {
                    Thread.Sleep(50);
                }
            }
            catch (Exception e)
            {
                System.Console.WriteLine("waitForElementPresent : Falied to locate element " + locator);
                throw new Exception("waitForElementPresent : Falied to locate element " + locator);
            }
            Reporter.Add(new Act("Successfully located element " + locator));
            return flag;

        }
        public bool Click(By lookupBy, string locatorName)
        {
            bool flag = false;
            try
            {
               
                Driver.FindElement(lookupBy).Click();                
                Reporter.Add(new Act("Successfully clicked on <b>" + locatorName + "</b>"));
                flag = true;
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to click on <b>" + locatorName + "</b> <br />" + ex.Message);
            }
            return flag;
        }

        public bool CloseWindow(String windowTitle)
        {

            bool flag = false;
            try
            {
                String CurrentWindowHandle = Driver.CurrentWindowHandle;
                IReadOnlyCollection<string> availableWindows = Driver.WindowHandles;
                String currentWindow = Driver.CurrentWindowHandle;
                if (availableWindows.Count > 0)
                {
                    foreach (var windowId in availableWindows)
                    {
                        string switchedWindowTitle = Driver.SwitchTo().Window(windowId).Title;
                        System.Console.WriteLine(switchedWindowTitle + " window title");
                        if (switchedWindowTitle.Equals(windowTitle))
                        {
                            Driver.SwitchTo().Window(windowId);
                            System.Console.WriteLine(switchedWindowTitle + " window title");
                            Driver.Close();
                            Driver.SwitchTo().Window(CurrentWindowHandle);
                            break;
                        }
                    }
                }
                Reporter.Add(new Act("Focus navigated to the window with title "
                                     + windowTitle));
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public void WaitSeconds(int seconds)
        {

            Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(seconds));
        }
        public void Sync()
        {
            int timer = 5;
            bool isLoadingDisplayed = false;
            do
            {
                timer--;
                isLoadingDisplayed = WaitForElementIsInvisible(By.XPath("//div[@class='loader']"), 10);
                Thread.Sleep(2000);
            } while (!isLoadingDisplayed & timer < 0);

        }
        public void SelectDateFromCalendar(string dateToSelect)
        {
            try
            {
                string givenDate = DateTime.Parse(dateToSelect).ToString("MMMM yyyy");
                By calendarMonthYear = By.XPath("//div[@data-role='calendar']/div[@class='k-header']/a[@class='k-link k-nav-fast'][not(ancestor::div[contains(@style,'display: none')])]");
                By calendarMonthYearNext = By.XPath("//div[@data-role='calendar']/div[@class='k-header']/a[@class='k-link k-nav-next']/span[not(ancestor::div[contains(@style,'display: none')])]");
                By calendarMonthYearPrev = By.XPath("//div[@data-role='calendar']/div[@class='k-header']/a[@class='k-link k-nav-prev']/span[not(ancestor::div[contains(@style,'display: none')])]");
                bool boolFlag = true;

                while (boolFlag)
                {
                    string displayedDate = DateTime.Parse(Driver.FindElement(calendarMonthYear).Text).ToString("MMMM yyyy");
                    Console.WriteLine(displayedDate);
                    if (DateTime.Parse(givenDate) == DateTime.Parse(displayedDate))
                    {
                        boolFlag = false;
                    }
                    else if (DateTime.Parse(givenDate) > DateTime.Parse(displayedDate))
                    {
                        JsClick(calendarMonthYearNext, "");
                        Sync();
                        Thread.Sleep(2000);
                    }
                    else if (DateTime.Parse(givenDate) < DateTime.Parse(displayedDate))
                    {
                        JsClick(calendarMonthYearPrev, "");
                        Sync();
                        Thread.Sleep(2000);
                    }
                }
                string selectedDate = DateTime.Parse(dateToSelect).ToString("dd MMM yyyy");
                ObjectClick(By.XPath("//div[@data-role='calendar']/table//a[@title='" + selectedDate + "'][not(ancestor::div[contains(@style,'display: none')])]"), selectedDate, 10);
                Sync();
                Reporter.Add(new Act("Click was successfully performed on the date  " + selectedDate));
                Sync();
            }

            catch (Exception e)
            {
                throw new Exception("Error in Function SelectDateFromCalendar </br>" + e.Message);
            }
        }
        public void Scroll(double value)
        {
            try
            {
                IJavaScriptExecutor executor = (IJavaScriptExecutor)Driver;
                string script = String.Format("window.scrollBy(0,{0})", value);
                executor.ExecuteScript(script, "");
                //executor.ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }
        public bool WaitForElementIsInvisible(By lookupBy, double timeout)
        {
            bool flag = false;
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeout));
            try
            {
                flag = wait.Until(ExpectedConditions.InvisibilityOfElementLocated(lookupBy));
                //flag = true;
                //return true;
            }
            catch (Exception ex)
            {
                return false;
                //throw new Exception(ex.Message);
            }
            return flag;
        }
        public bool WaitForVisibilityOfElement(By lookupBy, double timeout)
        {
            bool flag = false;
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeout));
            try
            {
                wait.Until((d) =>
                {
                    IWebElement element = Driver.FindElements(lookupBy).FirstOrDefault();
                    try
                    {
                        if (element != null && element.Displayed)
                        {
                            return true;
                        }
                    }
                    catch (Exception)
                    {


                    }


                    return false;
                });

                flag = true;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            return flag;
        }

        public bool WaitForVisibilityOfElement(IWebElement element, double timeout)
        {
            bool flag = false;
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeout));
            try
            {
                wait.Until((d) =>
                {

                    try
                    {
                        if (element != null && element.Displayed)
                        {
                            return true;
                        }
                    }
                    catch (Exception)
                    {


                    }


                    return false;
                });

                flag = true;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            return flag;
        }

        public string GetText(By lookupBy, string locatorName)
        {
            string text = "";
            bool flag = false;
            try
            {
                if (IsElementPresent(lookupBy, locatorName))
                {
                    text = Driver.FindElement(lookupBy).Text;
                    flag = true;
                    Reporter.Add(new Act("Successfully retrived text from <b>" + locatorName + "</b>" + ". The displayed value is  <b>" + text + "</b>"));
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to get Text from " + locatorName + "<br />" + ex.Message);
            }
            return text;
        }
        public bool AssertElementPresent(By lookupBy, string locatorName)
        {

            bool flag = false;
            try
            {
                if (IsElementPresent(lookupBy, locatorName))
                    flag = true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return flag;

        }
        public bool JsClick(By lookupBy, string locatorName)
        {
            bool flag = false;
            try
            {
                Thread.Sleep(500);
                IWebElement element = Driver.FindElement(lookupBy);

                IJavaScriptExecutor executor = (IJavaScriptExecutor)Driver;
                executor.ExecuteScript("arguments[0].click();", element);
                WaitForPageLoad(60);
                flag = true;
                Reporter.Add(new Act("Successfully clicked on <b>" + locatorName + "</b>"));

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return flag;
        }
        public bool IsElementPresent(By lookupBy, string locatorName)
        {
            bool flag = false;
            try
            {
                Driver.FindElement(lookupBy);
                flag = true;
                Reporter.Add(new Act("<b>" + locatorName + "</b> is present"));
            }
            catch (Exception e)
            {

                Console.WriteLine("<b>" + locatorName + "</b> is not present <br />" + e.Message);
                throw new Exception("<b>" + locatorName + "</b> is not present <br />" + e.Message);
            }
            return flag;

        }
        public bool IsChecked(By lookupBy, string locatorName)
        {
            bool bvalue = false;
            bool flag = false;
            try
            {
                if (Driver.FindElement(lookupBy).Selected)
                {
                    flag = true;
                    bvalue = true;
                }

            }
            catch (Exception e)
            {

                bvalue = false;
            }
            return bvalue;
        }
        public bool IsElementDisabled(By lookupby)
        {
            bool flag = false;
            try
            {
                if (IsElementPresent(lookupby, ""))
                {
                    flag = Driver.FindElement(lookupby).Enabled;
                    if (flag)
                        flag = false;
                    else
                        flag = true;
                }

            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
            return flag;
        }

        public bool IsElementDisplayedTemp(IWebElement we)
        {
            bool flag = false;
            try
            {
                flag = we.Displayed;
                if (flag)
                {
                    System.Console.WriteLine("found the element ");
                }
            }
            catch (Exception e)
            {
                return false;
            }
            return flag;
        }
        public bool IsLocatorDisplayed(By locator)
        {
            bool flag = false;
            try
            {

                flag = Driver.FindElement(locator).Displayed;
                if (flag)
                {
                    System.Console.WriteLine("found the element ");
                }
            }
            catch (Exception e)
            {
                return false;
            }
            return flag;
        }

        #endregion

        public bool VerifyTextPresentinActual(String actualtext, String expectedText, String locatorname)
        {
            bool flag = false;

            if (actualtext.Contains(expectedText))

            {
                Reporter.Add(new Act("<b>" + expectedText + " </b>is present in " + locatorname));
                flag = true;
            }
            else
            {
                throw new Exception("<b>" + expectedText + " </b>is not present in " + locatorname);
            }
            return flag;
        }

        #endregion

        #region ObjectOperationFunctions
        public void ObjectClick(By lookupBy, String locatorName, int maxWaitTime = 60)
        {
            IWebElement element = null;
            try
            {
                element = WaitForElementVisible(lookupBy, maxWaitTime);
                if (element != null)
                {

                    element.Click();
                    Reporter.Add(new Act("Successfully click on <b>" + locatorName + "</b>"));
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Unable to click on <b>" + locatorName + "</b> <br />" + ex.Message);
            }
        }
        public void WaitForPageLoad(int maxWaitTimeInSeconds)
        {
            string state = string.Empty;
            try
            {
                WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(maxWaitTimeInSeconds));

                //Checks every 500 ms whether predicate returns true if returns exit otherwise keep trying till it returns ture
                wait.Until(d =>
                {
                    try
                    {
                        state = ((IJavaScriptExecutor)Driver).ExecuteScript(@"return document.readyState").ToString();
                    }
                    catch (InvalidOperationException)
                    {
                        //Ignore
                    }
                    catch (NoSuchWindowException)
                    {
                        //when popup is closed, switch to last windows
                        Driver.SwitchTo().Window(Driver.WindowHandles.Last());
                    }
                    //In IE7 there are chances we may get state as loaded instead of complete
                    return (state.Equals("complete", StringComparison.InvariantCultureIgnoreCase) || state.Equals("loaded", StringComparison.InvariantCultureIgnoreCase));

                });
            }
            catch (TimeoutException)
            {
                //sometimes Page remains in Interactive mode and never becomes Complete, then we can still try to access the controls
                if (!state.Equals("interactive", StringComparison.InvariantCultureIgnoreCase))
                    throw;
            }
            catch (NullReferenceException)
            {
                //sometimes Page remains in Interactive mode and never becomes Complete, then we can still try to access the controls
                if (!state.Equals("interactive", StringComparison.InvariantCultureIgnoreCase))
                    throw;
            }
            catch (WebDriverException)
            {
                if (Driver.WindowHandles.Count == 1)
                {
                    Driver.SwitchTo().Window(Driver.WindowHandles[0]);
                }
                state = ((IJavaScriptExecutor)Driver).ExecuteScript(@"return document.readyState").ToString();
                if (!(state.Equals("complete", StringComparison.InvariantCultureIgnoreCase) || state.Equals("loaded", StringComparison.InvariantCultureIgnoreCase)))
                    throw;
            }
        }
        public void SetValueToObject(By lookupBy, string strInputValue, int maxWaitTime = 60)
        {
            try
            {
                var element = WaitForElementVisible(lookupBy, maxWaitTime);
                if (element != null)
                {
                    element.Clear();
                    element.SendKeys(strInputValue);
                    Reporter.Add(new Act("Successfully entered value :  <b>" + strInputValue + "</b>"));
                    Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "Failed to enter value :  <b>" + strInputValue + "</b>");
            }
        }
        public void ObjectselectValue(By lookupBy, string strInputValue, string selectBy = "text", int maxWaitTime = 60)
        {
            IWebElement element = null;
            try
            {
                element = WaitForElementVisible(lookupBy, maxWaitTime);
                if (element != null)
                {
                    SelectElement dropDownElement = new SelectElement(element);
                    switch (selectBy.ToLower())
                    {
                        case "text": dropDownElement.SelectByText(strInputValue); break;
                        case "index": dropDownElement.SelectByIndex(Int32.Parse(strInputValue)); break;
                        case "value": dropDownElement.SelectByValue(strInputValue); break;
                        default: dropDownElement.SelectByText(strInputValue); break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void SetValueByJs(By lookupBy, string inputValue)
        {
            try
            {
                IWebElement element = Driver.FindElement(lookupBy);
                IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)Driver;
                jsExecutor.ExecuteScript(String.Format(@"$(arguments[0]).val('{0}');", inputValue), new object[] { element });
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        public void ClearObjectValue(By lookupBy, int maxWaitTime = 60)
        {
            IWebElement element = null;
            try
            {
                element = WaitForElementVisible(lookupBy, maxWaitTime);
                if (element != null)
                {
                    element.Clear();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public bool ValidateElementAttributeValue(By lookupBy, string strAttributeName, string strExpectedValue, int maxWaitTime = 60)
        {
            bool valueEquals = false;
            IWebElement element = null;
            try
            {
                element = WaitForElementVisible(lookupBy, maxWaitTime);
                if (element != null)
                {
                    valueEquals = string.Equals(element.GetAttribute(strAttributeName).Trim().ToLower(), strExpectedValue.ToLower());
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return valueEquals;
        }
        public string RetrieveElementAttributeValue(By lookupBy, string strAttributeName, int maxWaitTime = 60)
        {
            string returnValue = string.Empty;
            IWebElement element = null;
            try
            {
                element = WaitForElementVisible(lookupBy, maxWaitTime);
                if (element != null)
                {
                    if (strAttributeName.ToLower().Equals("text"))
                    {
                        return element.Text;
                    }
                    else
                    {
                        returnValue = element.GetAttribute(strAttributeName);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return returnValue;
        }

        public bool IsElementSelected(By lookupBy, int maxWaitTime = 60)
        {
            bool valueEquals = false;
            IWebElement element = null;
            try
            {
                element = WaitForElementVisible(lookupBy, maxWaitTime);
                if (element != null)
                {
                    valueEquals = element.Selected;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return valueEquals;
        }

        public void MouseOverOnObject(By lookupBy, int maxWaitTime = 60)
        {
            try
            {
                var element = WaitForElementVisible(lookupBy, maxWaitTime);
                if (element != null)
                {
                    //new Actions(Driver).moveToElement(element).Perform();
                    Actions build = new Actions(Driver);
                    build.MoveToElement(element, element.Location.X, element.Location.Y).Build();
                    build.Perform();
                    Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void MoveToElement(By lookupBy, int intOffSetX, int intOffSetY, int maxWaitTime = 60)
        {
            IWebElement element = null;
            try
            {
                element = WaitForElementVisible(lookupBy, maxWaitTime);
                if (element != null)
                {
                    new Actions(Driver).MoveToElement(element).Build().Perform();
                    Thread.Sleep(500);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void MoveToElementAndClick(By lookupBy, int intOffSetX, int intOffSetY, int maxWaitTime = 60)
        {
            IWebElement element = null;
            try
            {
                element = WaitForElementVisible(lookupBy, maxWaitTime);
                if (element != null)
                {
                    MoveToElement(lookupBy, intOffSetX, intOffSetY);
                    new Actions(Driver).Click().Perform();
                    Thread.Sleep(500);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool CheckIfObjectNotExists(By lookupBy, int maxWaitTime = 5)
        {
            IWebElement element = null;
            try
            {
                element = WaitForElementVisible(lookupBy, maxWaitTime);
                if (element != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch
            {
                return false;
            }
        }

        public bool CheckIfObjectExists(By lookupBy, int maxWaitTime = 5)
        {
            IWebElement element = null;
            try
            {
                element = WaitForElementVisible(lookupBy, maxWaitTime);
                if (element != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch
            {
                return false;
            }
        }


        public Size RetrieveElementSize(By lookupBy, int maxWaitTime = 60)
        {
            Size elementSize = new Size();
            IWebElement element = null;
            try
            {
                element = WaitForElementVisible(lookupBy, maxWaitTime);
                if (element != null)
                {
                    elementSize = element.Size;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return elementSize;
        }

        public void MouseOverElement(By lookupBy, string locatorName)
        {
            bool flag = false;
            try
            {
                IWebElement element = Driver.FindElement(lookupBy);
                new Actions(Driver).MoveToElement(element).Build().Perform();
                flag = true;
                Reporter.Add(new Act(" MouserOver Action is Done on " + locatorName));
            }
            catch (Exception e)
            {
                throw new Exception(" MouseOver action is not perform on" + locatorName + "<br/>" + e.Message);
            }
        }

        public string GetTextUsingJavaScript(By locator, string locatorName)
        {
            bool flag = false;
            String text = "";
            try
            {
                string brower = Driver.GetType().ToString();
                if (brower.Contains("Firefox"))
                {
                    text = GetText(locator, locatorName);
                }
                else
                {
                    IWebElement element = Driver.FindElement(locator);
                    IJavaScriptExecutor executor = (IJavaScriptExecutor)Driver;
                    text = (String)executor.ExecuteScript("return arguments[0].innerText", element);
                }
                flag = true;
                System.Console.WriteLine("Get text of " + locatorName + " is " + text);
            }
            catch (Exception e)
            {
                throw new Exception("Unable to read text from the " + locatorName);
            }

            return text;
        }



        public bool SelectByVisibleText(By locator, string visibletext, string locatorName)
        {
            bool flag = false;
            try
            {
                IWebElement element = Driver.FindElement(locator);

                SelectElement s = new SelectElement(element);
                s.SelectByText(visibletext);
                flag = true;
                Reporter.Add(new Act("<b>" + visibletext + "</b>  is Selected from the DropDown " + locatorName));
                return true;
            }
            catch (Exception e)
            {
                throw new Exception("<b>" + visibletext
                                    + "</b> is Not Select from the DropDown " + locatorName);
            }
        }

        public bool SelectByOptionText(By locator, string value, string locatorName)
        {
            bool flag = false;
            try
            {
                IWebElement listBox = Driver.FindElement(locator);
                IReadOnlyCollection<IWebElement> options = listBox.FindElements(By.TagName("option"));
                foreach (var option in options)
                {
                    string opt = option.Text.Trim();
                    if (opt.ToLower().Equals(value.ToLower().Trim()))
                    {
                        option.Click();
                        Thread.Sleep(1000);
                        flag = true;
                        Reporter.Add(new Act("Item with value attribute <b>'" + value + "' </b> is  Selected from the DropDown " + locatorName));
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Item with value attribute " + value + " is Not Selected from the DropDown " + locatorName + " " + e.Message);
            }
            return flag;
        }

        public bool SelectByOptionByValue(By locator, string value, string locatorName)
        {

            bool flag = false;
            try
            {
                SelectElement select = new SelectElement(Driver.FindElement(locator));
                select.SelectByValue(value);
                Reporter.Add(new Act("Option with value attribute <b>'" + value + "' </b> is  Selected from the DropDown " + locatorName));
            }
            catch (Exception e)
            {
                throw new Exception("Option with value attribute " + value + " is Not Select from the DropDown " + locatorName + " " + e.Message);
            }
            return flag;

        }

        public bool SelectOptionsByText(By locator, string value, string locatorName)
        {
            bool flag = false;
            try
            {
                IWebElement listBox = Driver.FindElement(locator);
                IReadOnlyCollection<IWebElement> options = listBox.FindElements(By.TagName("option"));
                foreach (var option in options)
                {
                    string opt = option.Text.Trim();
                    if (opt.ToLower().Equals(value.ToLower().Trim()))
                    {
                        option.Click();
                        flag = true;
                        Reporter.Add(new Act("Option with value attribute " + value + " is  selected from the dropDown " + locatorName));
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Option with value attribute <b> '" + value + "' </b> is not selected from the dropDown " + locatorName + " " + e.Message);
            }
            return flag;
        }

        public bool SelectDropdwonByText(By locator, string value, string locatorName)
        {
            bool flag = false;
            try
            {
                SelectElement select = new SelectElement(Driver.FindElement(locator));
                select.SelectByText(value);
            }
            catch (Exception e)
            {
                throw new Exception("Option with value attribute " + value + " is Not Select from the DropDown " + locatorName + " " + e.Message);
            }
            return flag;
        }

        #endregion

        #region GenericFunctions
        public void NavigateToUrl(string strUrl)
        {
            try
            {
                //Driver.Manage().Timeouts().SetPageLoadTimeout(TimeSpan.FromSeconds(30));
                Driver.Navigate().GoToUrl(strUrl);
                Thread.Sleep(2000);

            }
            catch (Exception ex)
            {
                //throw new Exception(ex.Message);
                Driver.Manage().Timeouts().SetPageLoadTimeout(TimeSpan.FromSeconds(60));
            }
        }

        public string RetrieveCurrentBrowserUrl(int maxWaitTime = 0)
        {
            try
            {
                Thread.Sleep(maxWaitTime * 1000);
                return Driver.Url;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void DoubleClickOnObject(By lookupBy, int maxWaitTime = 60)
        {
            IWebElement element = null;
            try
            {
                element = WaitForElementVisible(lookupBy, maxWaitTime);
                if (element != null)
                {
                    Actions action = new Actions(Driver);
                    action.DoubleClick(element).Build().Perform();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void SwitchToElement(By lookupBy, int maxWaitTime)
        {
            IWebElement element = null;
            try
            {
                element = WaitForElementVisible(lookupBy, maxWaitTime);
                if (element != null)
                {
                    Driver.SwitchTo().Frame(element);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void SwitchToDefaultContent()
        {
            try
            {
                Driver.SwitchTo().DefaultContent();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void waitForPageLoad(int maxWaitTime = 60)
        {
            bool objAvailable = false;
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(maxWaitTime));
            IJavaScriptExecutor javascript = Driver as IJavaScriptExecutor;
            if (javascript == null)
                throw new ArgumentException("Driver", "Driver not supports javascript execution");
            objAvailable = wait.Until((d) =>
            {
                try
                {
                    string readyState = javascript.ExecuteScript(
                        "if (document.readyState) return document.readyState;").ToString();
                    return readyState.ToLower() == "complete";
                }

                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            });
        }

        public void ZooomInZoomOut(string zoomlevelPercent)
        {
            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)Driver;
            jsExecutor.ExecuteScript("document.body.style.zoom='" + zoomlevelPercent + "'");


        }
        public string GetCssValue(By locator, string cssattribute, string locatorName)
        {
            String value = "";
            bool flag = false;
            try
            {
                if (IsElementPresent(locator, locatorName))
                {
                    value = Driver.FindElement(locator).GetCssValue(cssattribute);
                    flag = true;
                }
            }
            catch (Exception e)
            {

            }
            finally
            {
                if (!flag)
                {
                    throw new Exception(" Was not able to get Css value from " + locatorName);

                }
                else if (flag)
                {
                    Reporter.Add(new Act(" Was able to get Css value from " + locatorName));
                }
            }
            return value;
        }
        public bool VerifyText(By by, string text, string locatorName)
        {
            bool flag = false;

            try
            {

                String vtxt = GetText(by, locatorName).Trim();
                vtxt.Equals(text.Trim());
                flag = true;
                Reporter.Add(new Act("<b> \"" + text + " \" </b>is present in the location <b>" + locatorName + "</b>"));

            }
            catch (Exception e)
            {

                throw new Exception(" <b> \" " + text + " \" </b> is not present in the location <b>" + locatorName + "</b>" + e.Message);
            }
            return true;
        }

        #endregion

        #region AlertPopUpFunctions
        internal IAlert GetAlertHandle(int maxWaitTime = 10)
        {
            IAlert alertHandle = null;
            try
            {
                for (int i = 0; i < maxWaitTime; i++)
                {
                    try
                    {
                        alertHandle = Driver.SwitchTo().Alert();
                        break;
                    }
                    catch
                    {
                        Thread.Sleep(1000);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return alertHandle;
        }

        public void AcceptAlert(int maxWaitTime = 10)
        {
            IAlert alertHandle = null;
            try
            {
                alertHandle = GetAlertHandle(maxWaitTime);
                if (alertHandle != null)
                {
                    alertHandle.Accept();
                }
                else
                {
                    throw new Exception("Alert handle not available");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void DismissAlert(int maxWaitTime = 10)
        {
            IAlert alertHandle = null;
            try
            {
                alertHandle = GetAlertHandle(maxWaitTime);
                if (alertHandle != null)
                {
                    alertHandle.Dismiss();
                }
                else
                {
                    throw new Exception("Alert handle not available");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public string GetAlertText(int maxWaitTime = 10)
        {
            IAlert alertHandle = null;
            string strAlertText = string.Empty;
            try
            {
                alertHandle = GetAlertHandle(maxWaitTime);
                if (alertHandle != null)
                {
                    strAlertText = alertHandle.Text;
                }
                else
                {
                    throw new Exception("Alert handle not available");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return strAlertText;
        }
        #endregion

        #region AssertionFunctions
        public void Equal(Iteration reporter, String actual, String expected)
        {
            reporter.Chapter.Step.Actions.Add(new Act(String.Format("Verify '{0}' Equals '{1}'", expected, actual)));

            if (!String.Equals(actual, expected))
            {
                throw new Exception(String.Format("Not Equal {0} : {1}", actual, expected));
            }
        }
        public void CheckStringContains(Iteration reporter, String strText, String token)
        {
            try
            {
                reporter.Chapter.Step.Actions.Add(new Act(String.Format("Verify '{0}' contains '{1}'", strText, token)));
                if (!strText.Contains(token))
                {
                    throw new Exception(String.Format("Does not Contan {0} : {1}", strText, token));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public void Equal(Iteration reporter, DateTime actual, DateTime expected, String name = "")
        {
            reporter.Chapter.Step.Actions.Add(new Act(String.Format("Verify '{0}' Equals '{1}'", expected, actual)));

            if (!String.Equals(actual, expected))
            {
                throw new Exception(String.Format("Not Equal {0} : {1}", actual, expected));
            }
        }

        public void NullOrEmpty(Iteration reporter, String data)
        {
            reporter.Chapter.Step.Actions.Add(new Act(String.Format("Verify Null or Empty '{0}'", data)));

            if (!String.IsNullOrEmpty(data) || !String.IsNullOrWhiteSpace(data))
            {
                throw new Exception(String.Format("Data is not Null or Empty"));
            }
        }
        public void NotNullOrEmpty(Iteration reporter, String data)
        {
            reporter.Chapter.Step.Actions.Add(new Act(String.Format("Verify Null or Empty '{0}'", data)));

            if (String.IsNullOrEmpty(data) || String.IsNullOrWhiteSpace(data))
            {
                throw new Exception(String.Format("Data is Null or Empty"));
            }
        }

        public void Equal(Iteration reporter, Int64 first, Int64 second)
        {
            if (!(first == second))
            {
                throw new Exception(String.Format("Not Equal {0} : {1}", first, second));
            }
        }

        public void ScrollIntoMiddle(IWebElement locator)
        {

            var js = (IJavaScriptExecutor)Driver;


            int height = Driver.Manage().Window.Size.Height;

            var hoverItem = (ILocatable)locator;
            var locationY = hoverItem.LocationOnScreenOnceScrolledIntoView.Y;
            js.ExecuteScript(string.Format(CultureInfo.InvariantCulture, "javascript:window.scrollBy(0,{0})", locationY - (height / 2)));

        }

        public void CheckEqualityOfObjects(Iteration reporter, bool first, bool second, string strStepDesc = "")
        {
            if (string.IsNullOrEmpty(strStepDesc))
            {
                reporter.Chapter.Step.Actions.Add(new Act(String.Format("Verify '{0}' Equals '{1}'", first, second)));
            }
            else
            {
                reporter.Chapter.Step.Actions.Add(new Act(strStepDesc));
            }

            if (!(first == second))
            {
                throw new Exception(String.Format("Not Equal {0} : {1}", first, second));
            }
        }

        public void Equal(Iteration reporter, Decimal first, Decimal second)
        {
            if (!first.Equals(second))
            {
                throw new Exception(String.Format("Not Equal {0} : {1}", first, second));
            }
        }

        public void AlphabeticalOrder(IList<String> items)
        {
            String lastItem = String.Empty;
            foreach (String item in items)
            {
                if (lastItem == String.Empty)
                {
                    lastItem = item;
                }

                if (lastItem.CompareTo(item) > 0)
                {
                    throw new Exception(String.Format("Item {0} not in alphabetical order", item));
                }
            }
        }

        public void NullOrEmpty(String data)
        {
            if (String.IsNullOrEmpty(data) || String.IsNullOrWhiteSpace(data))
            {
                throw new Exception(String.Format("Data is Null or Empty"));
            }
        }

        public void NotEqual(Iteration reporter, String first, String second)
        {
            reporter.Chapter.Step.Actions.Add(new Act(String.Format("Verify '{0}' Not Equals '{1}'", first, second)));

            if (String.Equals(first, second))
            {
                throw new Exception(String.Format("Equal {0} : {1}", first, second));
            }
        }

        public void NotEqual(Iteration reporter, Int64 first, Int64 second)
        {
            reporter.Chapter.Step.Actions.Add(new Act(String.Format("Verify '{0}' Not Equals '{1}'", first, second)));

            if (first == second)
            {
                throw new Exception(String.Format("Not Equal {0} : {1}", first, second));
            }
        }
        #endregion

        public void SwitchToWindowUsingTitle(String title, double timeout)
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeout));
            wait.Until(
                driver =>
                {
                    foreach (var handle in Driver.WindowHandles)
                    {
                        Driver.SwitchTo().Window(handle);
                        if (driver.Title.Equals(title))
                        {
                            return true;
                        }
                    }

                    return false;
                });
        }


        #region Encrypt Functions
        private byte[] _keyByte = { };
        //Default Key
        private static string _key = "Pass@123";
        //Default initial vector
        private byte[] _ivByte = { 0x01, 0x12, 0x23, 0x34, 0x45, 0x56, 0x67, 0x78 };


        //Encrypt 
        public string Encrypt(string value)
        {
            return Encrypt(value, string.Empty);
        }
               
        public string Encrypt(string value, string key)
        {
            return Encrypt(value, key, string.Empty);
        }
                
        public string Encrypt(string value, string key, string iv)
        {
            string encryptValue = string.Empty;
            MemoryStream ms = null;
            CryptoStream cs = null;
            if (!string.IsNullOrEmpty(value))
            {
                try
                {
                    if (!string.IsNullOrEmpty(key))
                    {
                        _keyByte = Encoding.UTF8.GetBytes
                                (key.Substring(0, 8));
                        if (!string.IsNullOrEmpty(iv))
                        {
                            _ivByte = Encoding.UTF8.GetBytes
                                (iv.Substring(0, 8));
                        }
                    }
                    else
                    {
                        _keyByte = Encoding.UTF8.GetBytes(_key);
                    }
                    using (DESCryptoServiceProvider des =
                            new DESCryptoServiceProvider())
                    {
                        byte[] inputByteArray =
                            Encoding.UTF8.GetBytes(value);
                        ms = new MemoryStream();
                        cs = new CryptoStream(ms, des.CreateEncryptor
                        (_keyByte, _ivByte), CryptoStreamMode.Write);
                        cs.Write(inputByteArray, 0, inputByteArray.Length);
                        cs.FlushFinalBlock();
                        encryptValue = Convert.ToBase64String(ms.ToArray());
                    }
                }
                catch
                {
                    //TODO: write log 
                }
                finally
                {
                    cs.Dispose();
                    ms.Dispose();
                }
            }
            return encryptValue;
        }
               
        public string Decrypt(string value)
        {
            return Decrypt(value, string.Empty);
        }
                
        public string Decrypt(string value, string key)
        {
            return Decrypt(value, key, string.Empty);
        }
                
        public string Decrypt(string value, string key, string iv)
        {
            string decrptValue = string.Empty;
            if (!string.IsNullOrEmpty(value))
            {
                MemoryStream ms = null;
                CryptoStream cs = null;
                value = value.Replace(" ", "+");
                byte[] inputByteArray = new byte[value.Length];
                try
                {
                    if (!string.IsNullOrEmpty(key))
                    {
                        _keyByte = Encoding.UTF8.GetBytes
                                (key.Substring(0, 8));
                        if (!string.IsNullOrEmpty(iv))
                        {
                            _ivByte = Encoding.UTF8.GetBytes
                                (iv.Substring(0, 8));
                        }
                    }
                    else
                    {
                        _keyByte = Encoding.UTF8.GetBytes(_key);
                    }
                    using (DESCryptoServiceProvider des =
                            new DESCryptoServiceProvider())
                    {
                        inputByteArray = Convert.FromBase64String(value);
                        ms = new MemoryStream();
                        cs = new CryptoStream(ms, des.CreateDecryptor
                        (_keyByte, _ivByte), CryptoStreamMode.Write);
                        cs.Write(inputByteArray, 0, inputByteArray.Length);
                        cs.FlushFinalBlock();
                        Encoding encoding = Encoding.UTF8;
                        decrptValue = encoding.GetString(ms.ToArray());
                    }
                }
                catch
                {
                    //TODO: write log 
                }
                finally
                {
                    cs.Dispose();
                    ms.Dispose();
                }
            }
            return decrptValue;
        }
        #endregion

    }
}
