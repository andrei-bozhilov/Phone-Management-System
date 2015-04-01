namespace TestParseXML
{
    using System.Collections.Generic;
    public class InvoiceReportBindingModel
    {
        public List<string> NumberShouldBeAddToDb { get; set; }

        public List<string> NumberShouldBeMakeUnactiveInDb { get; set; }
    }
}
