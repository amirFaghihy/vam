using Aban.Domain.Entities;
using Aban.Service.IServices.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using static Aban.Domain.Enumerations.Enumeration;

namespace Aban.Service.Interfaces
{
    public interface IUserAccountService : IGenericService<UserAccount>
    {
        Tuple<UserAccount, ResultStatusOperation> FillModel(UserAccount userAccount);
        Tuple<IQueryable<UserAccount>, ResultStatusOperation> SpecificationGetData(string accountOwnerId = "", BankName? bankName = null, string title = "", double? accountNumber = null, DateTime? registerDateFrom = null, DateTime? registerDateTo = null);
        List<SelectListItem> ReadAll(int selectedValue);
    }
}
