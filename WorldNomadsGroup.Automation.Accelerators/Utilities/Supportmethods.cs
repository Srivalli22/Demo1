using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Actions = OpenQA.Selenium.Interactions.Actions;
using Alert = OpenQA.Selenium.IAlert;
using By = OpenQA.Selenium.By;
using ExpectedConditions = OpenQA.Selenium.Support.UI.ExpectedConditions;
using JavascriptExecutor = OpenQA.Selenium.IJavaScriptExecutor;
using NoAlertPresentException = OpenQA.Selenium.NoAlertPresentException;
using System.Configuration;
using Select = OpenQA.Selenium.Support.UI.SelectElement;
using WebDriverWait = OpenQA.Selenium.Support.UI.WebDriverWait;
//using OutputType = OpenQA.Selenium.OutputType;
using WebElement = OpenQA.Selenium.IWebElement;
using System.IO;
using OpenQA.Selenium;
using System.Drawing;
using System.Data;
using Excel = Microsoft.Office.Interop.Excel;

namespace WorldNomadsGroup.Automation.Accelerators 
{
   public static class Supportmethods 
    {
        /// <summary>
        /// Same as FindElement only returns null when not found instead of an exception.
        /// </summary>
        /// usage -IWebElement element = driver.FindElmentSafe(By.Id("the id"));
        /// <param name="driver">current browser instance</param>
        /// <param name="by">The search string for finding element</param>
        /// <returns>Returns element or null if not found</returns>
        public static IWebElement FindElementSafe(this IWebDriver driver, By by)
        {
            try
            {
                return driver.FindElement(by);
            }
            catch (NoSuchElementException)
            {
                return null;
            }
        }

        /// <summary>
        /// Requires finding element by FindElementSafe(By).
        /// Returns T/F depending on if element is defined or null.
        /// </summary> 
        /// usage - bool exists = element.Exists();
        /// <param name="element">Current element</param>
        /// <returns>Returns T/F depending on if element is defined or null.</returns>
        public static bool Exists(this IWebElement element)
        {
            if (element == null)
            { return false; }
            return true;
        }

        public static string GetTestDataFilePath(string filename)
        {
            string testDataFolder = Environment.ExpandEnvironmentVariables(ConfigurationManager.AppSettings["TestDataFolder"]);
            string loginTestDataFileName = System.IO.Path.Combine(testDataFolder, filename);
            return loginTestDataFileName;
        }

        public static string ConvertRgbToHexCode(int codeFirst,int codeSecond,int codeThird)
        {
            string hex;


            Color myColor = Color.FromArgb(codeFirst, codeSecond, codeThird);
            return    hex = myColor.R.ToString("X2") + myColor.G.ToString("X2") + myColor.B.ToString("X2");


        }

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static DataTable ReadExcelFile(string path, string sheetname)
        {
            Excel.Application xlApp;
            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            Excel.Range range;
            var datatable = new DataTable();
            int rCnt = 0;
            int cCnt = 0;
            object cellValue = null;
            string colValue = string.Empty;

            xlApp = new Excel.Application();
            xlWorkBook = xlApp.Workbooks.Open(path, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            int count = xlWorkBook.Sheets.Count;

            int i = 1;
            while (i < count + 1)
            {
                xlWorkSheet = (Excel.Worksheet)xlWorkBook.Sheets[i];
                xlWorkBook.RefreshAll();
                if (sheetname.Equals(xlWorkSheet.Name))
                {
                    range = xlWorkSheet.UsedRange;
                    if (range.Rows.Count > 1)
                    {
                        for (rCnt = 1; rCnt <= range.Rows.Count; rCnt++)
                        {
                            DataRow drow = datatable.NewRow();
                            for (cCnt = 1; cCnt <= range.Columns.Count; cCnt++)
                            {
                                cellValue = (range.Cells[rCnt, cCnt] as Excel.Range).Value2;
                                colValue = cellValue == null ? string.Empty : Convert.ToString(cellValue);
                                //Adding Header Row to DataTable
                                if (rCnt == 1)
                                {
                                    datatable.Columns.Add(colValue);
                                }
                                else
                                {
                                    drow[cCnt - 1] = colValue;
                                }
                            }
                            if (rCnt > 1)
                            {
                                datatable.Rows.Add(drow);
                                datatable.AcceptChanges();
                            }
                        }

                    }
                    break;
                }
                i++;
            }
            xlWorkBook.Close(true, null, null);
            xlApp.Quit();

            //releaseObject(xlWorkSheet);
            ReleaseObject(xlWorkBook);
            ReleaseObject(xlApp);
            return datatable;
        }
        public static void ReleaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                //MessageBox.Show("Unable to release the Object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }

    }
}
