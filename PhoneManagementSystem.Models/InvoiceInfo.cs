namespace PhoneManagementSystem.Models
{
    using System;
    using System.Collections.Generic;

    public class InvoiceInfo
    {
        private ICollection<InvoiceData> invoiceData;

        public InvoiceInfo()
        {
            this.invoiceData = new HashSet<InvoiceData>();
        }

        public int Id { get; set; }

        public string InvoiceNumber { get; set; }

        public DateTime InvoiceDate { get; set; }

        public DateTime InvoiceStartPeriodDate { get; set; }

        public DateTime InvoiceEndPeriodDate { get; set; }
        //MAINLINES/INVOICE/AMOUNT the first item
        public decimal TotalServices { get; set; }
        //MAINLINES/INVOICE/AMOUNT the second item
        public decimal TotalDiscounts { get; set; }
        //INVOICE/TOTALNOVAT
        public decimal TotalNoVat { get; set; }
        //INVOICE/TOTALVAT
        public decimal TotalWithVat { get; set; }
        //INVOICE/OTHERS
        public decimal Others { get; set; }
        //INVOICE/TOTALAMOUNT
        public decimal TotalAmountToPay { get; set; }

        public virtual ICollection<InvoiceData> InvoiceData
        {
            get { return this.invoiceData; }
            set { this.invoiceData = value; }
        }

    }
}
