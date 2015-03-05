namespace PhoneManagementSystem.Models
{
    using System.Collections.Generic;

    public class JobTitle
    {
        private ICollection<User> users;

        public JobTitle()
        {
            this.Users = new HashSet<User>();           
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<User> Users
        {
            get { return this.users; }
            set { this.users = value; }
        }
    }
}
