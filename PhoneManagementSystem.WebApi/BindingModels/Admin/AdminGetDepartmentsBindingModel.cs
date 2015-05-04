namespace PhoneManagementSystem.WebApi.BindingModels.Admin
{
    public class AdminGetDepartmentsBindingModel : Pageable, IPageable
    {
        public string Search { get; set; }
    }
}