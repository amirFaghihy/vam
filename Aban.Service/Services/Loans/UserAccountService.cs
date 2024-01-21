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
    public class UserAccountService : GenericService<UserAccount>, IUserAccountService
    {
        private readonly IUserAccountRepository userAccountRepository;

        public UserAccountService(AppDbContext _dbContext) : base(_dbContext)
        {
            userAccountRepository = new DataLayer.Repositories.UserAccountRepository(_dbContext);
        }


        public Tuple<IQueryable<UserAccount>, ResultStatusOperation> SpecificationGetData(
            string accountOwnerId = "",
            BankName? bankName = null,
            string title = "",
            double? accountNumber = null,
            DateTime? registerDateFrom = null,
            DateTime? registerDateTo = null)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation();
            resultStatusOperation.IsSuccessed = true;
            resultStatusOperation.Type = MessageTypeResult.Success;
            try
            {
                IQueryable<UserAccount> query = userAccountRepository.GetAll().Where(x => !x.IsDelete);

                if (!string.IsNullOrEmpty(accountOwnerId))
                {
                    query = query.Where(x => x.AccountOwnerId.Contains(accountOwnerId));
                }
                if (bankName != null)
                {
                    query = query.Where(x => x.BankName == bankName);
                }
                if (!string.IsNullOrEmpty(title))
                {
                    query = query.Where(x => x.Title.Contains(title));
                }
                if (accountNumber != null)
                {
                    query = query.Where(x => x.AccountNumber == accountNumber);
                }
                if (registerDateFrom != null || registerDateTo != null)
                {
                    DateTime _registerDateFrom = registerDateFrom != null ? registerDateFrom.Value : new DateTime(1930, 1, 1, 1, 0, 0, 0);
                    DateTime _registerDateTo = registerDateTo != null ? registerDateTo.Value : new DateTime(DateTime.Now.AddYears(1).Year, 1, 1, 1, 0, 0, 0);
                    query = query.Where(x => x.RegisterDate >= _registerDateFrom && x.RegisterDate <= _registerDateTo);
                }

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


        public Tuple<UserAccount, ResultStatusOperation> FillModel(UserAccount userAccount)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation();
            resultStatusOperation.IsSuccessed = true;
            resultStatusOperation.Type = MessageTypeResult.Success;
            try
            {
                userAccount.IsDelete = false;
                userAccount.RegisterDate = DateTime.Now;

                return Tuple.Create(userAccount, resultStatusOperation);
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

        public List<SelectListItem> ReadAll(int selectedValue)
        {

            {
                ResultStatusOperation resultStatusOperation = new ResultStatusOperation();
                resultStatusOperation.IsSuccessed = true;
                resultStatusOperation.Type = MessageTypeResult.Success;

                try
                {
                    var query = userAccountRepository.GetAll()
                        .Where(x => !x.IsDelete)
                        .Include(x => x.AccountOwner)
                        .OrderBy(x => x.Id).ToList();

                    List<SelectListItem> item = query.ConvertAll(x =>
                    {
                        return new SelectListItem()
                        {
                            Text =
                            string.IsNullOrEmpty(x.AccountOwner!.FatherName) ?
                            $"{x.Id} | {x.AccountOwner!.FirstName} {x.AccountOwner.LastName}" :
                            $"{x.Id} | {x.AccountOwner!.FirstName} {x.AccountOwner.LastName} فرزند: {x.AccountOwner!.FatherName}"
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
