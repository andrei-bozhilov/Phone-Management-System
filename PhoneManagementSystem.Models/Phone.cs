namespace PhoneManagementSystem.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Phone
    {
        private ICollection<PhoneNumberOrder> phoneNumberOrders;

        public Phone()
        {
            this.PhoneNumberOrders = new HashSet<PhoneNumberOrder>();
        }

        [Key]
        public int Id { get; set; }

        [Index(IsUnique = true)]
        [StringLength(20)] 
        [Required]
        public string Number { get; set; }

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
    }
}
