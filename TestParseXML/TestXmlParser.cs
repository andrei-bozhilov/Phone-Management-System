namespace TestParseXML
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using System.Xml.XPath;

    using PhoneManagementSystem.Data;

    class TestXmlParser
    {
        static void Main(string[] args)
        {
            var data = new PhoneSystemData();

            XElement elemnet = XElement.Load("../../test.xml");

            var parser = new InvoiceXmlParser();
            parser.ParseAndSaveToDb(elemnet);
        }
    }
}
