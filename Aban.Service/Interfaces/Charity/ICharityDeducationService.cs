using Aban.Domain.Entities;
using Aban.Service.IServices.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Aban.Service.Interfaces
{
    public interface ICharityDeducationService : IGenericService<CharityDeducation>
    {
        Tuple<CharityDeducation, ResultStatusOperation> FillModel(CharityDeducation charityDeducation);
        Tuple<IQueryable<CharityDeducation>, ResultStatusOperation> SpecificationGetData(string userIdentityId = "", string userIdentityReciverId = "", string title = "", double amount = 0, string accountNumber = "", string description = "", DateTime? timeForActionFrom = null, DateTime? timeForActionTo = null, DateTime? registerDateFrom = null, DateTime? registerDateTo = null, bool? isDone = null);
        List<SelectListItem> ReadAll(int selectedValue);
    }
}