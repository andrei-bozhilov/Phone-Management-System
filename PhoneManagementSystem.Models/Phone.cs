namespace PhoneManagementSystem.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Phone
    {
        private ICollection<PhoneNumberOrder> phoneNumberOrders;
        private ICollection<InvoiceData> invoiceData;

        public Phone()
        {
            this.phoneNumberOrders = new HashSet<PhoneNumberOrder>();
            this.invoiceData = new HashSet<InvoiceData>();
        }

        [Key]
        public string PhoneId { get; set; }

        public PhoneStatus PhoneStatus { get; set; }

        public bool HasRouming { get; set; }

        public int? CreditLimit { get; set; }

        public CardType CardType { get; set; }

        public DateTime? AvailableAfter { get; set; }

        public virtual ICollection<PhoneNumberOrder> PhoneNumberOrders
        {
            get { return this.phoneNumberOrders; }
            set { this.phoneNumberOrders = value; }
        }

        public virtual ICollection<InvoiceData> InvoiceData
        {
            get { return this.invoiceData; }
            set { this.invoiceData = value; }
        }
    }
}
