namespace PhoneManagementSystem.WebApi.BindingModels.Admin
{
    using PhoneManagementSystem.Models;
    using System.ComponentModel.DataAnnotations;

    public class AdminPhonesFilterBindinModel : Pageable, IPageable
    {
        public AdminPhonesFilterBindinModel()
        {
            this.StartPage = 1;
        }

        public string Search { get; set; }

        /// <summary>
        /// Search for status. Example: Free|Taken
        /// </summary>      
        public string PhoneStatus { get; set; }

        public bool? HasRouming { get; set; }

        /// <summary>
        /// Search for credit limit. Supported operators are &lt;, &gt; and -. Example: For range 20-50; for smaller than &lt; 50; for bigger than &gt; 30.
        /// </summary>    
        public string CreditLimit { get; set; }

        public string CardType { get; set; }

        public bool? IsAvailable { get; set; }

    }
}