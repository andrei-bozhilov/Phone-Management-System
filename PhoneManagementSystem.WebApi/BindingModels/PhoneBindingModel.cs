namespace PhoneManagementSystem.WebApi.BindingModels
{
    using System.ComponentModel.DataAnnotations;

    using PhoneManagementSystem.Models;

    public class PhoneBindingModel
    {
        [Required]
        [MinLength(10)]
        [MaxLength(10)]
        public string PhoneId { get; set; }

        public bool? HasRouming { get; set; }

        public int? CreditLimit { get; set; }

        public string CardType { get; set; }
    }
}