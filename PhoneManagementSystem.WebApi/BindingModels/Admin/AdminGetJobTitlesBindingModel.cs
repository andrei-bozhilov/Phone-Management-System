namespace PhoneManagementSystem.WebApi.BindingModels.Admin
{
    public class AdminGetJobTitlesBindingModel : Pageable, IPageable
    {
        public string Search { get; set; }
    }
}