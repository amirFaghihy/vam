using Aban.Domain.Entities;
using Aban.Service.Interfaces;
using Aban.DataLayer.Context;
using Microsoft.AspNetCore.Mvc.Rendering;
using Aban.Service.Services.Generic;
using Aban.DataLayer.Interfaces;
using static Aban.Domain.Enumerations.Enumeration;
using Microsoft.EntityFrameworkCore;

namespace Aban.Service.Services
{
    public class UserAccountDepositWithdrawalService : GenericService<UserAccountDepositWithdrawal>, IUserAccountDepositWithdrawalService
    {
        private readonly IUserAccountDepositWithdrawalRepository userAccountDepositWithdrawalRepository;

        public UserAccountDepositWithdrawalService(AppDbContext _dbContext) : base(_dbContext)
        {
            userAccountDepositWithdrawalRepository = new DataLayer.Repositories.UserAccountDepositWithdrawalRepository(_dbContext);
        }


        public Tuple<IQueryable<UserAccountDepositWithdrawal>, ResultStatusOperation> SpecificationGetData(
            int userAccountId = 0,
            double? price = null,
            double? totalPriceAfterTransaction = null,
            TransactionType? accountTransactionType = null,
            TransactionMethod? accountTransactionMethod = null,
            DateTime? transactionDateTimeFrom = null,
            DateTime? transactionDateTimeTo = null,
            DateTime? registerDateFrom = null,
            DateTime? registerDateTo = null)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation();
            resultStatusOperation.IsSuccessed = true;
            resultStatusOperation.Type = MessageTypeResult.Success;
            try
            {
                IQueryable<UserAccountDepositWithdrawal> query = userAccountDepositWithdrawalRepository.GetAll()
                    .Where(x => !x.IsDelete);

                if (userAccountId != 0)
                {
                    query = query.Where(x => x.UserAccountId == userAccountId);
                }
                if (price != null)
                {
                    query = query.Where(x => x.Price == price);
                }
                if (totalPriceAfterTransaction != null)
                {
                    query = query.Where(x => x.TotalPriceAfterTransaction == totalPriceAfterTransaction);
                }
                if (accountTransactionType != null)
                {
                    query = query.Where(x => x.AccountTransactionType == accountTransactionType);
                }
                if (accountTransactionMethod != null)
                {
                    query = query.Where(x => x.AccountTransactionMethod == accountTransactionMethod);
                }
                if (transactionDateTimeFrom != null || transactionDateTimeTo != null)
                {
                    DateTime _transactionDateTimeFrom = transactionDateTimeFrom != null ? transactionDateTimeFrom.Value : new DateTime(1930, 1, 1, 1, 0, 0, 0);
                    DateTime _transactionDateTimeTo = transactionDateTimeTo != null ? transactionDateTimeTo.Value : new DateTime(DateTime.Now.AddYears(1).Year, 1, 1, 1, 0, 0, 0);
                    query = query.Where(x => x.TransactionDateTime >= _transactionDateTimeFrom && x.TransactionDateTime <= _transactionDateTimeTo);
                }
                if (registerDateFrom != null || registerDateTo != null)
                {
                    DateTime _registerDateFrom = registerDateFrom != null ? registerDateFrom.Value : new DateTime(1930, 1, 1, 1, 0, 0, 0);
                    DateTime _registerDateTo = registerDateTo != null ? registerDateTo.Value : new DateTime(DateTime.Now.AddYears(1).Year, 1, 1, 1, 0, 0, 0);
                    query = query.Where(x => x.RegisterDate >= _registerDateFrom && x.RegisterDate <= _registerDateTo);
                }

                query = query.Include(x => x.UserAccount!.AccountOwner);

                return Tuple.Create(query, resultStatusOperation);
            }
            catch (Exception exception)
            {
                resultStatusOperation.IsSuccessed = false;
                resultStatusOperation.Message = "خطایی رخ داده است";
                resultStatusOperation.Type = MessageTypeResult.Danger;
                resultStatusOperation.ErrorException = exception;
                throw new Exception("", exception);
            }
        }

        public Tuple<IQueryable<UserAccountDepositWithdrawal>, ResultStatusOperation> SpecificationGetData(
            List<int> userAccountIds)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation();
            resultStatusOperation.IsSuccessed = true;
            resultStatusOperation.Type = MessageTypeResult.Success;
            try
            {
                IQueryable<UserAccountDepositWithdrawal> query = userAccountDepositWithdrawalRepository.GetAll()
                    .Where(x => !x.IsDelete && userAccountIds.Contains(x.UserAccountId))
                    .OrderByDescending(x => x.RegisterDate)
                    ;

                return Tuple.Create(query, resultStatusOperation);
            }
            catch (Exception exception)
            {
                resultStatusOperation.IsSuccessed = false;
                resultStatusOperation.Message = "خطایی رخ داده است";
                resultStatusOperation.Type = MessageTypeResult.Danger;
                resultStatusOperation.ErrorException = exception;
                throw new Exception("", exception);
            }
        }


        public Tuple<UserAccountDepositWithdrawal, ResultStatusOperation> FillModel(UserAccountDepositWithdrawal userAccountDepositWithdrawal)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation();
            resultStatusOperation.Message = "عملیات با موفقیت انجام شد.";
            resultStatusOperation.IsSuccessed = true;
            resultStatusOperation.Type = MessageTypeResult.Success;
            try
            {
                userAccountDepositWithdrawal.IsDelete = false;
                userAccountDepositWithdrawal.RegisterDate = DateTime.Now;

                return Tuple.Create(userAccountDepositWithdrawal, resultStatusOperation);
            }
            catch (Exception exception)
            {
                resultStatusOperation.IsSuccessed = false;
                resultStatusOperation.Message = "خطایی رخ داده است";
                resultStatusOperation.Type = MessageTypeResult.Danger;
                resultStatusOperation.ErrorException = exception;
                throw new Exception("", exception);
            }
        }

        public double GetLatestTotalPriceAfterTransaction(int userAccountId)
        {
            UserAccountDepositWithdrawal? userAccountDepositWithdrawal = userAccountDepositWithdrawalRepository.GetAll().Where(x => !x.IsDelete &&
            x.UserAccountId == userAccountId).OrderByDescending(x => x.Id).FirstOrDefault();

            if (userAccountDepositWithdrawal == null)
            {
                return 0;
            }

            return userAccountDepositWithdrawal.TotalPriceAfterTransaction;
        }

        public List<SelectListItem> ReadAll(int selectedValue)
        {

            {
                ResultStatusOperation resultStatusOperation = new ResultStatusOperation();
                resultStatusOperation.IsSuccessed = true;
                resultStatusOperation.Type = MessageTypeResult.Success;

                try
                {
                    var query = userAccountDepositWithdrawalRepository.GetAll()
                        .Where(x => !x.IsDelete)
                        .Include(x => x.UserAccount!.AccountOwner)
                        .OrderBy(x => x.Id).ToList();

                    List<SelectListItem> item = query.ConvertAll(x =>
                    {
                        return new SelectListItem()
                        {
                            Text =
                            string.IsNullOrEmpty(x.UserAccount!.AccountOwner!.FatherName) ?
                            $"{x.Id} | {x.UserAccount!.AccountOwner!.FirstName} {x.UserAccount!.AccountOwner.LastName}" :
                            $"{x.Id} | {x.UserAccount!.AccountOwner!.FirstName} {x.UserAccount!.AccountOwner.LastName} فرزند: {x.UserAccount!.AccountOwner.FatherName}"
                            ,
                            Value = x.Id.ToString(),
                            Selected = (x.Id == selectedValue) ? true : false
                        };
                    });


                    return item;
                }
                catch (Exception exception)
                {
                    resultStatusOperation.IsSuccessed = false;
                    resultStatusOperation.Message = "خطایی رخ داده است";
                    resultStatusOperation.Type = MessageTypeResult.Danger;
                    resultStatusOperation.ErrorException = exception;
                    throw new Exception("", exception);
                }
            }
        }

    }
}
