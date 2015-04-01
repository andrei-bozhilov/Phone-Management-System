namespace TestParseXML
{
    using System;
    using System.Globalization;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using System.Xml.XPath;

    using PhoneManagementSystem.Data;
    using PhoneManagementSystem.Models;

    public class InvoiceXmlParser
    {
        private IPhoneSystemData data;
        private InvoiceInfo invoiceInfo;

        public InvoiceXmlParser()
            : this(new PhoneSystemData())
        {
        }

        public InvoiceXmlParser(IPhoneSystemData data)
        {
            this.data = data;
        }

        public void ParseAndSaveToDb(XElement xmlDoc)
        {
            this.ParseAndAnalizeXmlDoc(xmlDoc);
            //todo save data in database

        }

        public InvoiceReportBindingModel ParseAndAnalizeXmlDoc(XElement xmlDoc)
        {
            this.invoiceInfo = this.ParseInvoiceInfo(xmlDoc);
            this.invoiceInfo.InvoiceData = this.ParseInvoiceData(xmlDoc);

            var invoiceInfoNumber = this.data.InvoiceInfos.All()
                .Where(i => i.InvoiceNumber == this.invoiceInfo.InvoiceNumber)
                .Select(i => i.Id)
                .FirstOrDefault();

            if (invoiceInfoNumber != 0)
            {
                throw new ArgumentException(string.Format("The invoice with number {0} exits in database.", this.invoiceInfo.InvoiceNumber));
            }

            var phoneNumbersDb = this.data.Phones.All()
                .Where(p => p.PhoneStatus != PhoneStatus.Taken)
                .Select(p => p.PhoneId)
                .ToList();
            var phoneNumbersXml = this.invoiceInfo.InvoiceData
                .Select(i => i.PhoneId)
                .ToList();

            var allNumberThatAreNotInDb = phoneNumbersXml.Where(p1 => phoneNumbersDb.Any(p2 => p2 != p1)).ToList();
            var allNumberThatAreNotInXml = phoneNumbersDb.Where(p1 => phoneNumbersXml.Any(p2 => p2 != p1)).ToList();

            return new InvoiceReportBindingModel()
            {
                NumberShouldBeAddToDb = allNumberThatAreNotInDb,
                NumberShouldBeMakeUnactiveInDb = allNumberThatAreNotInXml
            };
        }

        private InvoiceInfo ParseInvoiceInfo(XElement xmlDoc)
        {
            var invoiceInfoElement = xmlDoc.XPathSelectElement("INVOICE");
            var invoiceNumber = invoiceInfoElement.XPathSelectElement("NUMBER").Value;

            var InvoiceInfoId = this.data.InvoiceInfos.All()
                .Where(i => i.InvoiceNumber == invoiceNumber)
                .Select(i => i.Id)
                .FirstOrDefault();
            if (InvoiceInfoId != 0)
            {
                throw new ArgumentException(string.Format(
                    "There is already a invoice with this number {0}, its id in database is {1}",
                    invoiceNumber,
                    InvoiceInfoId));
            }
            var invoiceDate = DateTime.Parse(xmlDoc.XPathSelectElement("INVOICEDATE/TAXEVENT").Value);
            var invoiceStartPeriodDate = DateTime.Parse(xmlDoc.XPathSelectElement("LINESGROUP/ADDLINESGROUP/PERIODSTART").Value);
            var invoiceEndPeriodDate = DateTime.Parse(xmlDoc.XPathSelectElement("LINESGROUP/ADDLINESGROUP/PERIODEND").Value);

            var totalInfos = xmlDoc.XPathSelectElements("MAINLINES/SERVICE/AMOUNT");
            var totalServices = decimal.Parse(totalInfos.First().Value);
            var totalDiscounts = decimal.Parse(totalInfos.Last().Value);

            var totalWithVat = decimal.Parse(xmlDoc.XPathSelectElement("INVOICE/TOTALVAT").Value);
            var totalNoVat = decimal.Parse(xmlDoc.XPathSelectElement("INVOICE/TOTALNOVAT").Value);
            var totalAmountToPay = decimal.Parse(xmlDoc.XPathSelectElement("INVOICE/TOTALAMOUNT").Value);
            var others = decimal.Parse(xmlDoc.XPathSelectElement("INVOICE/OTHERS").Value);

            InvoiceInfo infoModel = new InvoiceInfo()
            {
                InvoiceNumber = invoiceNumber,
                InvoiceDate = invoiceDate,
                InvoiceStartPeriodDate = invoiceStartPeriodDate,
                InvoiceEndPeriodDate = invoiceEndPeriodDate,
                TotalServices = totalServices,
                TotalDiscounts = totalDiscounts,
                TotalNoVat = totalNoVat,
                TotalWithVat = totalWithVat,
                Others = others,
                TotalAmountToPay = totalAmountToPay
            };
            return infoModel;
        }

        private List<InvoiceData> ParseInvoiceData(XElement xmlDoc)
        {
            var invoiceDatas = new List<InvoiceData>();
            var invoiceDataElements = xmlDoc.XPathSelectElements("LINESGROUP/ADDLINESGROUP/PRODUCTGROUP/PRODUCT");

            foreach (var product in invoiceDataElements)
            {
                if (product.Element("PRODUCTID") == null)
                {
                    continue;
                }

                var phoneId = "0" + product.Element("PRODUCTID").Value; //the number come without '0' in front
                var totalMinUsed = new TimeSpan(
                    int.Parse(product.Element("PRODUCTUSAGE").Value.Split(':')[0]),// hours
                    int.Parse(product.Element("PRODUCTUSAGE").Value.Split(':')[1]),// minutes
                           0);
                var totalAmount = decimal.Parse(product.Element("PRODUCTAMOUNT").Value);

                var services = new List<Service>();
                foreach (var item in product.XPathSelectElements("ITEM"))
                {
                    var type = item.Element("TYPE").Value;
                    var label = item.Element("LABEL").Value;

                    DateTime? startDate = null;
                    DateTime? endDate = null;
                    decimal? amount = null;
                    string usage = null;
                    DateTime? month = null;
                    TimeSpan? includedMin = null;
                    TimeSpan? usedMin = null;

                    if (item.Element("SDATE") != null)
                    {
                        startDate = DateTime.Parse(item.Element("SDATE").Value);
                    }
                    if (item.Element("EDATE") != null)
                    {
                        endDate = DateTime.Parse(item.Element("EDATE").Value);
                    }
                    if (item.Element("AMOUNT") != null)
                    {
                        amount = decimal.Parse(item.Element("AMOUNT").Value);
                    }
                    if (item.Element("USAGE") != null)
                    {
                        usage = item.Element("USAGE").Value;
                    }
                    if (item.Element("MONTH") != null)
                    {
                        month = DateTime.Parse(item.Element("MONTH").Value);
                    }
                    if (item.Element("INCLMIN") != null)
                    {
                        includedMin = new TimeSpan(
                            int.Parse(item.Element("INCLMIN").Value.Split(':')[0]),// hours
                            int.Parse(item.Element("INCLMIN").Value.Split(':')[1]),// minutes
                                   0);
                    }
                    if (item.Element("USEDMIN") != null)
                    {
                        usedMin = new TimeSpan(
                            int.Parse(item.Element("USEDMIN").Value.Split(':')[0]),// hours
                            int.Parse(item.Element("USEDMIN").Value.Split(':')[1]),// minutes
                                   0);
                    }

                    var service = new Service()
                    {
                        Type = type,
                        Label = label,
                        StartDate = startDate,
                        EndDate = endDate,
                        Amount = amount,
                        Usage = usage,
                        Month = month,
                        IncludedMin = includedMin,
                        UsedMin = usedMin
                    };

                    services.Add(service);
                }

                var invoiceData = new InvoiceData()
                {
                    PhoneId = phoneId,
                    TotalMinUsed = totalMinUsed,
                    TotalAmount = totalAmount,
                    Services = services,
                };

                invoiceDatas.Add(invoiceData);
            }

            return invoiceDatas;
        }
    }
}
