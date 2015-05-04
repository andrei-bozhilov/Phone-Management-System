namespace PhoneManagementSystem.WebApi.BindingModels
{
    using System.ComponentModel.DataAnnotations;

    public class DepartmentBindingModel
    {
        public int Id { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}