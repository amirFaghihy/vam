using Aban.Common;
using Aban.Domain.Entities;
using Aban.Service.Interfaces;
using Aban.DataLayer.Context;
using Microsoft.AspNetCore.Mvc.Rendering;
using Aban.Service.Services.Generic;
using Aban.DataLayer.Interfaces;
using Aban.DataLayer.Repositories;
using Aban.Domain.Enumerations;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace Aban.Service.Services
{
    public class CharityBankRecordService : GenericService<CharityBankRecord>, ICharityBankRecordService
    {
        private readonly ICharityBankRecordRepository _charityBankRecordRepository;
        private readonly ICharityDepositRepository _charityDepositRepository;
        private readonly ICharityAccountRepository _charityAccountRepository;
        public CharityBankRecordService(AppDbContext _dbContext) : base(_dbContext)
        {
            _charityAccountRepository = new CharityAccountRepository(_dbContext);
            _charityBankRecordRepository = new DataLayer.Repositories.CharityBankRecordRepository(_dbContext);
            _charityDepositRepository = new CharityDepositRepository(_dbContext);
        }


        private string GetProperCardNumber(string cardNumber, BankName bankName)
        {
            string lastFourDigits = cardNumber.Length > 4 ? cardNumber.Substring((cardNumber.Length - 4), 4) : cardNumber;


            if (lastFourDigits.Last() == '0' && bankName.Equals(BankName.بانک_ملت))
            {
                ///remove 0 fro last charcter
                lastFourDigits = lastFourDigits.Remove(lastFourDigits.Length - 1, 1);

            }

            return lastFourDigits;

        }


        public async Task<CharityBankRecord> UpdateBankRecordToInUse(CharityBankRecord model)
        {
            if (model != default)
            {
                model.IsInUse = true;
                await this.Update(true, model);
                return model;
            }

            return new();
        }

        public async Task<CharityBankRecord> UpdateBankRecordToInUse(int charityBankRecordId)
        {
            var resultFind = await this.Find(charityBankRecordId);
            if (resultFind.Item1 == default || resultFind.Item1 == null)
            {
                return new();
            }

            resultFind.Item1.IsInUse = true;
            await this.Update(true, resultFind.Item1);
            return resultFind.Item1;
        }

        public async Task<bool> UpdateConfirmByDateAmountCardNumberAsync(double amount, string cardNumber,
            DateTime date, int bankRecordId, int charityAccountId, BankName bankName)
        {
            //Bank Mellat
            string lastFourDigits = this.GetProperCardNumber(cardNumber, bankName);

            CharityDeposit? resultFindCharityDeposit =
                lastFourDigits.Length.Equals(3) ?
                    await _charityDepositRepository.GetAll().FirstOrDefaultAsync(x =>
                        !x.IsConfirm && !x.IsDelete && x.CharityAccountId == charityAccountId && x.Amount == amount &&
                        x.DocumentRegisterDateTime.Date == date.Date && x.LastFourDigits!.Substring(0, 3).Equals(lastFourDigits))
                    :
            await _charityDepositRepository.GetAll().FirstOrDefaultAsync(x => !x.IsConfirm && !x.IsDelete && x.CharityAccountId == charityAccountId && x.Amount == amount && x.LastFourDigits!.Equals(lastFourDigits) && x.DocumentRegisterDateTime.Date == date.Date);

            if (resultFindCharityDeposit == default) return false;

            resultFindCharityDeposit.CharityBankRecordId = bankRecordId;
            resultFindCharityDeposit.IsConfirm = true;
            resultFindCharityDeposit.ConfirmType = ConfirmType.By_Card_Number;
            resultFindCharityDeposit.ConfirmDate = DateTime.Now;
            await _charityDepositRepository.UpdateAsync(resultFindCharityDeposit, false);

            if (await this.UpdateBankRecordToInUse(bankRecordId) == default)
            {
                resultFindCharityDeposit.ConfirmDate = null;
                resultFindCharityDeposit.IsConfirm = false;
                resultFindCharityDeposit.ConfirmType = ConfirmType.None;
                resultFindCharityDeposit.CharityBankRecordId = null;
                return false;
            }

            await _charityDepositRepository.SaveChangesAsync();
            return true;

        }

        public async Task<bool> UpdateConfirmByDateAmountCardNumberAsync(CharityDeposit deposit, int bankRecordId)
        {
            deposit.CharityBankRecordId = bankRecordId;
            deposit.IsConfirm = true;
            deposit.ConfirmType = ConfirmType.By_Card_Number;
            deposit.ConfirmDate = DateTime.Now;
            await _charityDepositRepository.UpdateAsync(deposit, false);
            if (await this.UpdateBankRecordToInUse(bankRecordId) == default)
            {
                deposit.ConfirmDate = null;
                deposit.IsConfirm = false;
                deposit.ConfirmType = ConfirmType.None;
                deposit.CharityBankRecordId = null;
                return false;
            }

            await _charityDepositRepository.SaveChangesAsync();
            return true;

        }


        public async Task<bool> CheckRecordIsExistByDateAmountCardAsync(double amount, string cardNumber, DateTime date, int charityAccountId)
            => await _charityBankRecordRepository.GetAll()
                .AnyAsync(x =>
                    !x.IsDelete && x.Creditor == amount && x.DocumentRegisterDateTime.Date == date.Date &&
                    x.Accountant == cardNumber && x.CharityAccountId == charityAccountId);

        public async Task<bool> CheckRecordIsExistByIssueNumberAsync(string issueNumber, double amount, DateTime documentDate, int charityAccountId)
        => await _charityBankRecordRepository.GetAll()
            .AnyAsync(x => !x.IsDelete && x.DocumentNumber == (issueNumber) && x.Creditor == amount && x.CharityAccountId == charityAccountId
            && x.DocumentRegisterDateTime.Date == documentDate.Date);


        public async Task<CharityBankRecord> GetBankRecordByDateAmountCardNumberAsync(int charityAccountId,
            double amount, string cardNumber,
            DateTime date)
        => await _charityBankRecordRepository.GetAll().AsNoTracking().FirstOrDefaultAsync(x => !x.IsInUse &&
            !x.IsDelete && x.CharityAccountId == charityAccountId && x.Creditor == amount && x.DocumentRegisterDateTime.Date == date.Date &&
            x.Accountant!.Substring(x.Accountant.Length - 4).Equals(cardNumber));

        public async Task<CharityBankRecord> GetBankRecordByDateAmountCardNumberForMellatAsync(int charityAccountId,
            double amount, string cardNumber,
            DateTime date)
        {
            //extract the three first letter of card number
            string newCardNumber = cardNumber.Substring(0, 3);


           return  await _charityBankRecordRepository.GetAll().AsNoTracking().FirstOrDefaultAsync(x => !x.IsInUse &&
                !x.IsDelete && x.CharityAccountId == charityAccountId && x.Creditor == amount &&
                x.DocumentRegisterDateTime.Date == date.Date &&
                //Extract the 12,13,14 index of card number (from 0,15) because in the Bankrecord the most cardnumbers are end with 0 and I compare just 3 letter
                x.Accountant!.Length.Equals(16) ? x.Accountant!.Substring(12,3).Equals(newCardNumber) 
                : x.Accountant!.Substring(x.Accountant.Length - 4).Equals(cardNumber)
            );
        }

        public async Task<CharityBankRecord> GetBankRecordByIssueNumberAsync(int charityAccountId, string issueNumber, DateTime documentDate, double amount)
            => await _charityBankRecordRepository.GetAll().AsNoTracking().FirstOrDefaultAsync(x =>
                !x.IsInUse && !x.IsDelete && x.DocumentNumber == (issueNumber) && x.DocumentRegisterDateTime.Date == documentDate.Date && x.Creditor == amount &&
                x.CharityAccountId == charityAccountId);

        public async Task<ResultStatusOperation> UpdateDepositWithFindBankRecordAsync(CharityDeposit deposit)
        {
            CharityAccount? charityAccount = await _charityAccountRepository.FindAsync(deposit.CharityAccountId);


            CharityBankRecord? resultFindBankRecord =
                await this.GetBankRecordByIssueNumberAsync(deposit.CharityAccountId, deposit.IssueTracking, deposit.DocumentRegisterDateTime, deposit.Amount);

            switch (charityAccount.BankName)
            {
                case BankName.بانک_سپه:
                case BankName.بانک_ملی_ایران:

                    if (resultFindBankRecord != null)
                    {
                        await this.UpdateConfirmByIssueNumberAsync(deposit, resultFindBankRecord.Id);

                        //await this.UpdateBankRecordToInUse(resultFind);
                        return new()
                        {
                            Title = "اطلاعات تایید شد",
                            Type = Enumeration.MessageTypeResult.Info,
                            IsSuccessed = true,
                            Message = "اطلاعات تایید شد"
                        };
                    }
                    resultFindBankRecord = await this.GetBankRecordByDateAmountCardNumberAsync(deposit.CharityAccountId, deposit.Amount,
                        deposit.LastFourDigits!.ReplacePersianNumberToEnglishNumber(), deposit.DocumentRegisterDateTime.Date);


                    if (resultFindBankRecord != null)
                    {
                        await this.UpdateConfirmByDateAmountCardNumberAsync(deposit, resultFindBankRecord.Id);

                        //await this.UpdateBankRecordToInUse(resultFind);
                        return new()
                        {
                            Title = "اطلاعات تایید شد",
                            Type = Enumeration.MessageTypeResult.Info,
                            IsSuccessed = true,
                            Message = "اطلاعات تایید شد"
                        };
                    }

                    break;


                //کد پیگیری ندارد
                case BankName.بانک_ملت:

                    resultFindBankRecord = await this.GetBankRecordByDateAmountCardNumberForMellatAsync(deposit.CharityAccountId, deposit.Amount,
                        deposit.LastFourDigits!.ReplacePersianNumberToEnglishNumber(), deposit.DocumentRegisterDateTime.Date);

                    if (resultFindBankRecord != null)
                    {
                        await this.UpdateConfirmByDateAmountCardNumberAsync(deposit, resultFindBankRecord.Id);

                        //await this.UpdateBankRecordToInUse(resultFind);
                        return new()
                        {
                            Title = "اطلاعات تایید شد",
                            Type = Enumeration.MessageTypeResult.Info,
                            IsSuccessed = true,
                            Message = "اطلاعات تایید شد"
                        };
                    }

                    break;


                case BankName.بانک_صنعت_و_معدن:
                case BankName.بانک_کشاورزی:
                case BankName.بانک_مسکن:
                case BankName.بانک_توسعه_صادرات_ایران:
                case BankName.بانک_توسعه_تعاون:
                case BankName.پست_بانک_ایران:
                case BankName.بانک_اقتصاد_نوین:
                case BankName.بانک_پارسیان:
                case BankName.بانک_کارآفرین:
                case BankName.بانک_سامان:
                case BankName.بانک_سینا:
                case BankName.بانک_خاورمیانه:
                case BankName.بانک_شهر:
                case BankName.بانک_دی:
                case BankName.بانک_صادرات:
                case BankName.بانک_تجارت:
                case BankName.بانک_رفاه:
                case BankName.بانک_حکمت_ایرانیان:
                case BankName.بانک_گردشگردی:
                case BankName.بانک_ایران_زمین:
                case BankName.بانک_قوامین:
                case BankName.بانک_انصار:
                case BankName.بانک_سرمایه:
                case BankName.بانک_پاسارگاد:
                case BankName.بانک_قرض_الحسنه_مهر_ایران:
                case BankName.بانک_قرض_الحسنه_مهر_رسالت:
                default:
                    return new()
                    {
                        Type = Enumeration.MessageTypeResult.Warning,
                        IsSuccessed = false,
                    };
            }


            return new()
            {
                Type = Enumeration.MessageTypeResult.Warning,
                IsSuccessed = false,
            };

        }

        public async Task<bool> UpdateConfirmByIssueNumberAsync(string issueNumber, double amount, DateTime documentDate, int bankRecordId, int charityAccountId)
        {
            var resultFInd = await _charityDepositRepository.GetAll().FirstOrDefaultAsync(x => !x.IsConfirm && !x.IsDelete && x.IssueTracking == issueNumber && x.CharityAccountId == charityAccountId && x.Amount == amount && x.DocumentRegisterDateTime.Date == documentDate.Date);

            if (resultFInd != default)
            {
                resultFInd.CharityBankRecordId = bankRecordId;
                resultFInd.IsConfirm = true;
                resultFInd.ConfirmType = ConfirmType.By_Issue_number;
                resultFInd.ConfirmDate = DateTime.Now;
                await _charityDepositRepository.UpdateAsync(resultFInd, false);

                if (await this.UpdateBankRecordToInUse(bankRecordId) == default)
                {
                    resultFInd.ConfirmDate = null;
                    resultFInd.IsConfirm = false;
                    resultFInd.ConfirmType = ConfirmType.None;
                    resultFInd.CharityBankRecordId = null;
                    return false;
                }


                return true;
            }

            return false;
        }
        public async Task<bool> UpdateConfirmByIssueNumberAsync(CharityDeposit deposit, int bankRecordId)
        {
            var resultFInd = await _charityDepositRepository.GetAll().FirstOrDefaultAsync(x => !x.IsConfirm && !x.IsDelete && x.Amount == deposit.Amount && x.DocumentRegisterDateTime.Date == deposit.DocumentRegisterDateTime.Date && x.IssueTracking == deposit.IssueTracking && x.CharityAccountId == deposit.CharityAccountId);

            if (resultFInd != default)
            {
                resultFInd.CharityBankRecordId = bankRecordId;
                resultFInd.IsConfirm = true;
                resultFInd.ConfirmType = ConfirmType.By_Issue_number;
                resultFInd.ConfirmDate = DateTime.Now;
                await _charityDepositRepository.UpdateAsync(resultFInd);
                if (await this.UpdateBankRecordToInUse(bankRecordId) == default)
                {
                    resultFInd.ConfirmDate = null;
                    resultFInd.IsConfirm = false;
                    resultFInd.ConfirmType = ConfirmType.None;
                    resultFInd.CharityBankRecordId = null;
                    return false;
                }
                return true;
            }

            return false;
        }

        public IQueryable<CharityBankRecord> SpecificationGetData(
            string userIdentityId = "",
            string paperNumber = "",
            string documentNumber = "",
            double creditor = 0,
            double debtor = 0,
            double inventory = 0,
            int? charityAccountId = 0,
            string description = "",
            DateTime? documentRegisterDateTimeFrom = null,
            DateTime? documentRegisterDateTimeTo = null,
            DateTime? registerDateFrom = null,
            DateTime? registerDateTo = null,
            int? id = null)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation();
            resultStatusOperation.IsSuccessed = true;
            resultStatusOperation.Type = Enumeration.MessageTypeResult.Success;
            try
            {
                IQueryable<CharityBankRecord> query = _charityBankRecordRepository.GetAll()
                    .Where(x => !x.IsDelete);

                //if (!string.IsNullOrEmpty(userIdentityId))
                //{
                //    query = query.Where(x => x.UserIdentityId == userIdentityId);
                //}
                if (!string.IsNullOrEmpty(paperNumber))
                {
                    query = query.Where(x => x.PaperNumber.Contains(paperNumber));
                }
                if (!string.IsNullOrEmpty(documentNumber))
                {
                    query = query.Where(x => x.DocumentNumber.Contains(documentNumber));
                }
                if (creditor != 0)
                {
                    query = query.Where(x => x.Creditor == creditor);
                }
                if (debtor != 0)
                {
                    query = query.Where(x => x.Debtor == debtor);
                }
                if (inventory != 0)
                {
                    query = query.Where(x => x.Inventory == inventory);
                }
                if (charityAccountId != null && charityAccountId != 0)
                {
                    query = query.Where(x => x.CharityAccountId == charityAccountId);
                }

                if (!description.IsNullOrEmpty())
                {
                    query = query.Where(x => x.Description.Contains(description));
                }

                if (id is not null)
                {
                    query = query.Where(x => x.Id == id);
                }
                if (documentRegisterDateTimeFrom != null || documentRegisterDateTimeTo != null)
                {
                    DateTime _documentRegisterDateTimeFrom = documentRegisterDateTimeFrom != null ? documentRegisterDateTimeFrom.Value : new DateTime(1930, 1, 1, 1, 0, 0, 0);
                    DateTime _documentRegisterDateTimeTo = documentRegisterDateTimeTo != null ? documentRegisterDateTimeTo.Value : new DateTime(DateTime.Now.AddYears(1).Year, 1, 1, 1, 0, 0, 0);
                    query = query.Where(x => x.DocumentRegisterDateTime.Date >= _documentRegisterDateTimeFrom.Date && x.DocumentRegisterDateTime.Date <= _documentRegisterDateTimeTo.Date);
                }
                if (registerDateFrom != null || registerDateTo != null)
                {
                    DateTime _registerDateFrom = registerDateFrom != null ? registerDateFrom.Value : new DateTime(1930, 1, 1, 1, 0, 0, 0);
                    DateTime _registerDateTo = registerDateTo != null ? registerDateTo.Value : new DateTime(DateTime.Now.AddYears(1).Year, 1, 1, 1, 0, 0, 0);
                    query = query.Where(x => x.RegisterDate.Date >= _registerDateFrom.Date && x.RegisterDate.Date <= _registerDateTo.Date);
                }

                query = query.Include(x => x.CharityAccount);

                return query;
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

        public Tuple<CharityBankRecord, ResultStatusOperation> FillModel(CharityBankRecord charityBankRecord)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation();
            resultStatusOperation.IsSuccessed = true;
            resultStatusOperation.Type = Enumeration.MessageTypeResult.Success;
            try
            {
                charityBankRecord.IsDelete = false;
                charityBankRecord.RegisterDate = DateTime.Now;

                return Tuple.Create(charityBankRecord, resultStatusOperation);
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
                    var query = _charityBankRecordRepository.GetAll()
                        .Where(x => !x.IsDelete).OrderBy(x => x.Id).ToList();

                    List<SelectListItem> item = query.ConvertAll(x =>
                    {
                        return new SelectListItem()
                        {
                            Text = $"برگه: {x.PaperNumber} - سند: {x.DocumentNumber}",
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
