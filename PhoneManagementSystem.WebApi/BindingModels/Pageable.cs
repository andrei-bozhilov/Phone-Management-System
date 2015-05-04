namespace PhoneManagementSystem.WebApi.BindingModels
{
    using System.ComponentModel.DataAnnotations;

    public class Pageable : IPageable
    {
        [Range(1, 100000, ErrorMessage = "Page number should be in range [1...100000].")]
        public int? StartPage { get; set; }

        [Range(1, 1000, ErrorMessage = "Page size be in range [1...1000].")]
        public int? PageSize { get; set; }

        /// <summary>
        /// Order by property.
        /// <example>Id|Desc or Name|Asc</example>
        /// </summary>
        /// <exception cref="HttpResponceException"></exception>
        public string OrderBy { get; set; }
    }
}