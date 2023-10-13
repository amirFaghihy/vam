using Aban.Domain.Entities;
using Aban.Service.IServices.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Aban.Service.Interfaces
{
    public interface ICharityAdditionService : IGenericService<CharityAddition>
    {
        Tuple<CharityAddition, ResultStatusOperation> FillModel(CharityAddition charityAddition);
        Tuple<IQueryable<CharityAddition>, ResultStatusOperation> SpecificationGetData(string userIdentityId = "", string userIdentityReciverId = "", string title = "", double amount = 0, string accountNumber = "", string description = "", DateTime? documentRegisterDateTimeFrom = null, DateTime? documentRegisterDateTimeTo = null, DateTime? registerDateFrom = null, DateTime? registerDateTo = null, bool? isDone = null);
        List<SelectListItem> ReadAll(int selectedValue);
    }
}