using Aban.Domain.Entities;
using Aban.Service.IServices.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using static Aban.Domain.Enumerations.Enumeration;

namespace Aban.Service.Interfaces
{
    public interface IUserAccountDepositWithdrawalService : IGenericService<UserAccountDepositWithdrawal>
    {
        Tuple<UserAccountDepositWithdrawal, ResultStatusOperation> FillModel(UserAccountDepositWithdrawal userAccountDepositWithdrawal);
        Tuple<IQueryable<UserAccountDepositWithdrawal>, ResultStatusOperation> SpecificationGetData(
            int? userAccountId = 0,
            double? price = null,
            double? totalPriceAfterTransaction = null,
            TransactionType? accountTransactionType = null,
            TransactionMethod? accountTransactionMethod = null,
            DateTime? transactionDateTimeFrom = null,
            DateTime? transactionDateTimeTo = null,
            DateTime? registerDateFrom = null,
            DateTime? registerDateTo = null);
        List<SelectListItem> ReadAll(int selectedValue);
    }
}
