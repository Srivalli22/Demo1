#region Namespaces
using System;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Reflection;
using ExlInterop = Microsoft.Office.Interop.Excel;
using System.Xml;
#endregion

namespace WorldNomadsGroup.Automation.Accelerators
{
    /// <summary>
    /// Description of ExcelReader.
    /// </summary>
    /// 
    public static class ExcelReader
    {
        public static DataTable ReadSuiteFile(string dataFile, string sheetname, bool isOledb)
        {
            DataTable dt = new DataTable();
            //tblExlData holds only data rows of excel sheet
            DataTable tblExcelData = new DataTable();
            string exlFileName = string.Empty;
            try
            {

                string fileName = sheetname;

                if (isOledb)
                {
                    dt = ReadExcelUsingOledb(dataFile + sheetname);
                }
                else
                {
                    dataFile = dataFile + sheetname;
                    dt = ReadExcelUsingInterop(dataFile, sheetname.Replace(".xlsx", ""));
                }

            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                throw ex;
            }
            return dt;
            //return tblExcelData;
        }
               
        public static DataTable ReadExcelUsingOledb(string exlDataSource)
        {
            OleDbConnection ocon; OleDbDataAdapter oda;
            DataTable tblExcelSchema;
            string sheetName = string.Empty;
            //dt holds both the empty and data rows of excel sheet
            DataTable dt = new DataTable();

            try
            {
                string excelConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + exlDataSource + ";Extended Properties='Excel 12.0 Xml;HDR=Yes'";
                ocon = new OleDbConnection(excelConn);
                ocon.Open();
                tblExcelSchema = ocon.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                sheetName = tblExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                oda = new OleDbDataAdapter("select * from [" + sheetName + "]", ocon);
                dt.TableName = "TestData";
                oda.Fill(dt);
                ocon.Close();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                throw ex;

            }
            return dt;
        }

        public static DataTable ReadExcelUsingInterop(string exlDataSource, string sheetname)
        {
            ExlInterop.Application exlApp = new ExlInterop.Application();
            ExlInterop.Workbook exlWb = null;
            ExlInterop.Worksheet exlSheet;

            int rCnt = 0; int cCnt = 0;
            object cellValue = null;
            string colValue = string.Empty;

            //dt holds both the empty and data rows of excel sheet
            DataTable dt = new DataTable();

            try
            {
                exlWb = exlApp.Workbooks.Open(exlDataSource);

                int numSheets = exlWb.Sheets.Count;
                for (int sheetNum = 1; sheetNum < numSheets + 1; sheetNum++)
                {
                    exlSheet = (ExlInterop.Worksheet)exlWb.Sheets[sheetNum];
                    string strWorksheetName = exlSheet.Name;

                    exlWb.RefreshAll();
                    if (strWorksheetName.Equals(sheetname))
                    {
                        ExlInterop.Range range = exlSheet.UsedRange;
                        //                ExlInterop.Range range = exlSheet.UsedRange.SpecialCells(
                        //                               ExlInterop.XlCellType.xlCellTypeVisible, 
                        //                               Type.Missing);
                        for (rCnt = 1; rCnt <= range.Rows.Count; rCnt++)
                        {
                            DataRow drow = dt.NewRow();
                            for (cCnt = 1; cCnt <= range.Columns.Count; cCnt++)
                            {
                                cellValue = (range.Cells[rCnt, cCnt] as Microsoft.Office.Interop.Excel.Range).Value2;
                                colValue = cellValue == null ? string.Empty : Convert.ToString(cellValue);
                                //Adding Header Row to DataTable
                                if (rCnt == 1)
                                {
                                    dt.Columns.Add(colValue);
                                }
                                else
                                {
                                    drow[cCnt - 1] = colValue;
                                }
                            }
                            if (rCnt > 1)
                            {
                                dt.Rows.Add(drow);
                                dt.AcceptChanges();
                            }
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                throw ex;

            }
            finally
            {

                exlWb.Close(true, Missing.Value, Missing.Value);

                // exlWb.Close();
                exlApp.Quit();
                ReleaseProcessObject(exlWb);
                ReleaseProcessObject(exlApp);
            }
            return dt;
        }


        public static void SaveCsvtoXlxs(string filepath, string newFilePathinXls)
        {
            KillExcel();

            ExlInterop.Application app = new ExlInterop.Application();
            ExlInterop.Workbook wb = app.Workbooks.Open(filepath, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            wb.SaveAs(newFilePathinXls, ExlInterop.XlFileFormat.xlOpenXMLWorkbook, Type.Missing, Type.Missing, Type.Missing, Type.Missing, ExlInterop.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            wb.Close(true, Missing.Value, Missing.Value);
            app.Quit();
            ReleaseProcessObject(wb);
            ReleaseProcessObject(app);


        }

        public static void KillExcel()
        {
            var processes = from p in System.Diagnostics.Process.GetProcessesByName("EXCEL") select p;
            foreach (var process in processes)
            {
                process.Kill();
            }
        }


        private static void ReleaseProcessObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                System.Console.WriteLine(ex.Message);
                throw ex;
            }
            finally
            {
                GC.Collect();
            }
        }
    }
}
