namespace PhoneManagementSystem.WebApi.BindingModels.Admin
{
    using PhoneManagementSystem.WebApi.BindingModels;

    public class AdminGetUsersBindingModel : Pageable, IPageable
    {
        public string Search { get; set; }

        public bool? IsActive { get; set; }

        public int? DepartmentId { get; set; }

        public bool? IsAdmin { get; set; }
    }
}