namespace PhoneManagementSystem.Data
{
    using Microsoft.AspNet.Identity.EntityFramework;

    using PhoneManagementSystem.Data.Repository;
    using PhoneManagementSystem.Models;

    public interface IPhoneSystemData
    {
        IRepository<User> Users { get; }

        IRepository<IdentityRole> UserRoles { get; }

        IRepository<Department> Departments { get; }

        IRepository<Phone> Phones { get; }

        IRepository<JobTitle> JobTitles { get; }

        IRepository<PhoneNumberOrder> PhoneNumberOrders { get; }

        int SaveChanges();
    }
}
