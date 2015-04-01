namespace PhoneManagementSystem.Data
{
    using System.Data.Entity;
    using Microsoft.AspNet.Identity.EntityFramework;

    using PhoneManagementSystem.Models;
    using PhoneManagementSystem.Data.Migrations;


    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext()
            : base("DefaultConnectionString", throwIfV1Schema: false)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, Configuration>());
        }

        public IDbSet<Department> Departments { get; set; }

        public IDbSet<Phone> Phones { get; set; }

        public IDbSet<PhoneNumberOrder> PhoneNumberOrders { get; set; }

        public IDbSet<JobTitle> JobTitles { get; set; }

        public IDbSet<InvoiceData> InvoiceDatas { get; set; }

        public IDbSet<InvoiceInfo> InvoiceInfos { get; set; }

        public IDbSet<Service> Services { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PhoneNumberOrder>()
                    .HasRequired(m => m.Admin)
                    .WithMany(t => t.UserPhoneNumberOrders)
                    .HasForeignKey(m => m.AdminId)
                    .WillCascadeOnDelete(false);

            modelBuilder.Entity<PhoneNumberOrder>()
                        .HasRequired(m => m.User)
                        .WithMany(t => t.AdminPhoneNumberOrders)
                        .HasForeignKey(m => m.UserId)
                        .WillCascadeOnDelete(false);
        }
    }
}
