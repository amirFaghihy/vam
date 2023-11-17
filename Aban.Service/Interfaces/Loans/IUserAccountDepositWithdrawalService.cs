using Aban.Domain.Entities;
using Aban.Service.IServices.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using static Aban.Domain.Enumerations.Enumeration;

namespace Aban.Service.Interfaces
{
    public interface IUserAccountDepositWithdrawalService : IGenericService<UserAccountDepositWithdrawal>
    {
        Tuple<IQueryable<UserAccountDepositWithdrawal>, ResultStatusOperation> SpecificationGetData(
            int userAccountId = 0,
            double? price = null,
            double? totalPriceAfterTransaction = null,
            TransactionType? accountTransactionType = null,
            TransactionMethod? accountTransactionMethod = null,
            DateTime? transactionDateTimeFrom = null,
            DateTime? transactionDateTimeTo = null,
            DateTime? registerDateFrom = null,
            DateTime? registerDateTo = null);

        Tuple<UserAccountDepositWithdrawal, ResultStatusOperation> FillModel(UserAccountDepositWithdrawal userAccountDepositWithdrawal);

        /// <summary>
        /// آخرین باقیمانده حساب کاربر را باز میگرداند
        /// </summary>
        /// <param name="userAccountId"></param>
        /// <returns></returns>
        double GetLatestTotalPriceAfterTransaction(int userAccountId);

        List<SelectListItem> ReadAll(int selectedValue);
    }
}
