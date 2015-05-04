namespace PhoneManagementSystem.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.ComponentModel.DataAnnotations;

    public class Department
    {
        [Key]
        public int Id { get; set; }

        [Index(IsUnique = true)]
        [StringLength(50)]
        [Required]
        public string Name { get; set; }
    }
}
