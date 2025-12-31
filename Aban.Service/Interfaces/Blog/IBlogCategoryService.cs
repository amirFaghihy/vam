using Aban.Domain.Entities;
using Aban.Service.IServices.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Aban.Service.Interfaces
{
    public interface IBlogCategoryService : IGenericService<BlogCategory>
    {
        Task<Tuple<BlogCategory, ResultStatusOperation>> FillModel(BlogCategory blogCategory);
        Task<Tuple<IQueryable<BlogCategory>, ResultStatusOperation>> SpecificationGetData(string? userIdentityId = "", int? parentId = null, string? title = null, bool? isvisible = null, string? blogCategoryURL = null);
        List<SelectListItem> ReadAll(int selectedValue);
        List<SelectListItem> ReadAll(List<int> selectedValue);
    }
}
