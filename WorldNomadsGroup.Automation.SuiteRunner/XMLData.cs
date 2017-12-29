using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Data;

using System.IO;


namespace WorldNomadsGroup.Automation.SuiteRunner
{
    class XMLData
    {
        

        public void XMLDataaaa()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("C:\\Users\\e001207\\Desktop\\TestDataNew.xml");
            XmlNodeList TCID = xmlDoc.GetElementsByTagName("TestCaseID");

        }

    }


    
}
