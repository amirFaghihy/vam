using Aban.Domain.Entities;
using Aban.Service.Interfaces;
using Aban.DataLayer.Context;
using Microsoft.AspNetCore.Mvc.Rendering;
using Aban.Service.Services.Generic;
using Aban.DataLayer.Interfaces;
using Aban.Domain.Enumerations;
using Microsoft.EntityFrameworkCore;
using System;
using Aban.DataLayer.Repositories;

namespace Aban.Service.Services
{
    public class CharityAccountService : GenericService<CharityAccount>, ICharityAccountService
    {
        private readonly ICharityAccountRepository charityAccountRepository;
        private readonly ICharityDepositRepository _charityDepositRepository;
        private readonly ICharityBankRecordRepository _charityBankRecordRepository;
        public CharityAccountService(AppDbContext _dbContext) : base(_dbContext)
        {
            charityAccountRepository = new DataLayer.Repositories.CharityAccountRepository(_dbContext);
            _charityDepositRepository = new CharityDepositRepository(_dbContext);
            _charityBankRecordRepository = new CharityBankRecordRepository(_dbContext);
        }

        public async Task<ResultStatusOperation> LogicDeleteAllRelatedData(int id)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation();
            resultStatusOperation.IsSuccessed = true;
            resultStatusOperation.Type = Enumeration.MessageTypeResult.Success;
            resultStatusOperation.Message = "عملیات با موفقیت انجام شد";
            try
            {
                await this.DeleteLogic(id);
                await _charityDepositRepository.GetAll().Where(x => x.CharityAccountId == id).ForEachAsync(x =>
               {
                   x.IsDelete = true;
               });

                await _charityBankRecordRepository.GetAll().Where(x => x.CharityAccountId == id).ForEachAsync(x =>
                {
                    x.IsDelete = true;
                });
                this.SaveChanges();

            }
            catch (Exception exception)
            {
                resultStatusOperation.IsSuccessed = false;
                resultStatusOperation.Message = "خطایی رخ داده است";
                resultStatusOperation.Type = Enumeration.MessageTypeResult.Danger;
                resultStatusOperation.ErrorException = exception;
            }
            return resultStatusOperation;
        }

        public Tuple<IQueryable<CharityAccount>, ResultStatusOperation> SpecificationGetData(
            string userIdentityId = "",
            BankName? bankName = null,
            string title = "",
            string accountNumber = "",
            bool? isvisible = null,
            DateTime? registerDateFrom = null,
            DateTime? registerDateTo = null)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation();
            resultStatusOperation.IsSuccessed = true;
            resultStatusOperation.Message = "عملیات با موفقیت انجام شد.";
            resultStatusOperation.Type = Enumeration.MessageTypeResult.Success;
            try
            {
                IQueryable<CharityAccount> query = charityAccountRepository.GetAll().Where(x => !x.IsDelete);

                if (!string.IsNullOrEmpty(userIdentityId))
                {
                    query = query.Where(x => x.UserIdentityId == userIdentityId);
                }
                if (bankName != null)
                {
                    query = query.Where(x => x.BankName == bankName);
                }
                if (!string.IsNullOrEmpty(title))
                {
                    query = query.Where(x => x.Title.Contains(title));
                }
                if (!string.IsNullOrEmpty(accountNumber))
                {
                    query = query.Where(x => x.AccountNumber.Contains(accountNumber));
                }
                if (isvisible != null)
                {
                    query = query.Where(x => x.IsVisible == isvisible);
                }
                if (registerDateFrom != null || registerDateTo != null)
                {
                    DateTime _registerDateFrom = registerDateFrom != null ? registerDateFrom.Value : new DateTime(1930, 1, 1, 1, 0, 0, 0);
                    DateTime _registerDateTo = registerDateTo != null ? registerDateTo.Value : new DateTime(DateTime.Now.AddYears(1).Year, 1, 1, 1, 0, 0, 0);
                    query = query.Where(x => x.RegisterDate >= _registerDateFrom && x.RegisterDate <= _registerDateTo);
                }

                query = query.Include(x => x.UserIdentity);

                return Tuple.Create(query, resultStatusOperation);
            }
            catch (Exception exception)
            {
                resultStatusOperation.IsSuccessed = false;
                resultStatusOperation.Message = "خطایی رخ داده است";
                resultStatusOperation.Type = Enumeration.MessageTypeResult.Danger;
                resultStatusOperation.ErrorException = exception;
                throw new Exception("", exception);
            }
        }

        public Tuple<CharityAccount, ResultStatusOperation> FillModel(CharityAccount charityAccount)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation();
            resultStatusOperation.IsSuccessed = true;
            resultStatusOperation.Type = Enumeration.MessageTypeResult.Success;
            try
            {
                charityAccount.IsDelete = false;
                charityAccount.RegisterDate = charityAccount.RegisterDate == new DateTime() ? DateTime.Now : charityAccount.RegisterDate;
                //charityAccount.UserIdentityId = "";


                return Tuple.Create(charityAccount, resultStatusOperation);
            }
            catch (Exception exception)
            {
                resultStatusOperation.IsSuccessed = false;
                resultStatusOperation.Message = "خطایی رخ داده است";
                resultStatusOperation.Type = Enumeration.MessageTypeResult.Danger;
                resultStatusOperation.ErrorException = exception;
                throw new Exception("", exception);
            }
        }

        public List<SelectListItem> ReadAll(int selectedValue)
        {

            {
                ResultStatusOperation resultStatusOperation = new ResultStatusOperation();
                resultStatusOperation.IsSuccessed = true;
                resultStatusOperation.Type = Enumeration.MessageTypeResult.Success;

                try
                {
                    List<CharityAccount> query =
                        charityAccountRepository.GetAll().Where(x => !x.IsDelete).OrderBy(x => x.Id).ToList();

                    List<SelectListItem> item = query.ConvertAll(x =>
                    {
                        return new SelectListItem()
                        {
                            Text = x.Title,
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
                    resultStatusOperation.Type = Enumeration.MessageTypeResult.Danger;
                    resultStatusOperation.ErrorException = exception;
                    throw new Exception("", exception);
                }
            }
        }

    }
}
