namespace PhoneManagementSystem.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class PhoneNumberOrder
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual User User { get; set; }

        [Required]
        public string PhoneId { get; set; }

        public virtual Phone Phone { get; set; }

        public DateTime ActionDate { get; set; }

        public PhoneAction PhoneAction { get; set; }

        [Required]
        public string AdminId { get; set; }

        public virtual User Admin { get; set; }
    }
}
