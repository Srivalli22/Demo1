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
    /// Description of XMLReader.
    /// </summary>
    /// 
    public static class XMLReader
    {
        public static DataTable ReadXMLFile(string dataFile, string sheetname)
        {
            DataTable dt = new DataTable();

            DataTable tblExcelData = new DataTable();
            string exlFileName = string.Empty;
            try
            {
                string fileName = sheetname;
                dt = ReadXMLSuite(dataFile + sheetname);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                throw ex;
            }
            return dt;
        }

        public static DataTable ReadXMLFile(string dataFile, string sheetname, string TestCaseName)
        {
            DataTable dt = new DataTable();

            DataTable tblExcelData = new DataTable();
            string exlFileName = string.Empty;
            try
            {
                string fileName = sheetname;
                dt = ReadXMLTestData(dataFile + sheetname, TestCaseName);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                throw ex;
            }
            return dt;
        }

        public static DataTable ReadXMLSuite(string DataFilename)
        {
            try
            {
                DataTable dtsuite = new DataTable("xmlsuite");
                DataSet ds = new DataSet();
                ds.ReadXml(DataFilename, XmlReadMode.Auto);
                dtsuite = ds.Tables[0];
                return dtsuite;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                throw new Exception("", e);
            }
        }

        public static DataTable ReadXMLTestData(string DataFilename, string TestcaseName)
        {
            try
            {
                DataTable dtsuite = new DataTable("xmlsuite");
                DataSet ds = new DataSet();
                ds.ReadXml(DataFilename, XmlReadMode.Auto);
                dtsuite = ds.Tables[TestcaseName];
                return dtsuite;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                throw new Exception("", e);
            }
        }


    }
}
