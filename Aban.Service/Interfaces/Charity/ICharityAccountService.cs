using Aban.Domain.Entities;
using Aban.Service.IServices.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Aban.Service.Interfaces
{
    public interface ICharityAccountService : IGenericService<CharityAccount>
    {
        Tuple<CharityAccount, ResultStatusOperation> FillModel(CharityAccount charityAccount);
        Tuple<IQueryable<CharityAccount>, ResultStatusOperation> SpecificationGetData(string userIdentityId = "", BankName? bankName = null, string title = "", string accountNumber = "", bool? isvisible = null, DateTime? registerDateFrom = null, DateTime? registerDateTo = null);
        List<SelectListItem> ReadAll(int selectedValue);
        Task<ResultStatusOperation> LogicDeleteAllRelatedData(int id);
    }
}