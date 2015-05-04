namespace PhoneManagementSystem.WebApi.BindingModels
{
    public interface IPageable
    {
        int? StartPage { get; set; }

        int? PageSize { get; set; }

        string OrderBy { get; set; }
    }
}