namespace PhoneManagementSystem.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class JobTitle
    {
        private ICollection<User> users;

        public JobTitle()
        {
            this.Users = new HashSet<User>();
        }

        public int Id { get; set; }

        [Index(IsUnique = true)]
        [StringLength(50)]
        [Required]
        public string Name { get; set; }

        public virtual ICollection<User> Users
        {
            get { return this.users; }
            set { this.users = value; }
        }
    }
}
