using Aban.Domain.Entities;
using Aban.Service.IServices.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Aban.Service.Interfaces
{
    public interface IBlogArticleService : IGenericService<BlogArticle>
    {
        Tuple<BlogArticle, ResultStatusOperation> FillModel(BlogArticle blogArticle);
        Tuple<IQueryable<BlogArticle>, ResultStatusOperation> SpecificationGetData(string userIdentityId = "", string writerId = "", string title = "", DateTime? registerDateFrom = null, DateTime? registerDateTo = null, bool? isvisible = null);
        List<SelectListItem> ReadAll(Guid selectedValue);
    }
}
