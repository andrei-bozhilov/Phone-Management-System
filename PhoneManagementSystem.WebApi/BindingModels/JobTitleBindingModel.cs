using System.ComponentModel.DataAnnotations;
namespace PhoneManagementSystem.WebApi.BindingModels
{
    public class JobTitleBindingModel
    {
        public int Id { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}