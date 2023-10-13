using Aban.Domain.Entities;
using Aban.Service.IServices.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Aban.Service.Interfaces
{
    public interface ICharityBankRecordService : IGenericService<CharityBankRecord>
    {
        /// <summary>
        /// In this case, Issue Number = Document Number
        /// </summary>
        /// <param name="issueNumber"></param>
        /// <returns></returns>
        Task<bool> CheckRecordIsExistByIssueNumberAsync(string issueNumber,double amount,DateTime documentDate, int charityAccountId);

        Task<bool> CheckRecordIsExistByDateAmountCardAsync(double amount, string cardNumber, DateTime date, int charityAccountId);


        Task<CharityBankRecord> UpdateBankRecordToInUse(CharityBankRecord model);
        Task<CharityBankRecord> UpdateBankRecordToInUse(int charityBankRecordId);

        Task<bool> UpdateConfirmByIssueNumberAsync(string issueNumber,double amount,DateTime documentDate, int bankRecordId, int charityAccountId);
        Task<bool> UpdateConfirmByIssueNumberAsync(CharityDeposit deposit, int bankRecordId);
        Task<bool> UpdateConfirmByDateAmountCardNumberAsync(double amount, string cardNumber, DateTime date, int bankRecordId,int charityAccountId, BankName bankName);
        Task<bool> UpdateConfirmByDateAmountCardNumberAsync(CharityDeposit deposit, int bankRecordId);

        Task<CharityBankRecord> GetBankRecordByDateAmountCardNumberAsync(int charityAccountId, double amount, string cardNumber,
            DateTime date);

        Task<CharityBankRecord> GetBankRecordByDateAmountCardNumberForMellatAsync(int charityAccountId,
            double amount, string cardNumber, DateTime date);

        Task<CharityBankRecord> GetBankRecordByIssueNumberAsync(int charityAccountId, string issueNumber, DateTime documentDate, double amount);

        Task<ResultStatusOperation> UpdateDepositWithFindBankRecordAsync(CharityDeposit deposit); 

        Tuple<CharityBankRecord, ResultStatusOperation> FillModel(CharityBankRecord charityBankRecord);
        IQueryable<CharityBankRecord> SpecificationGetData(
            string userIdentityId = "",
            string paperNumber = "",
            string documentNumber = "",
            double creditor = 0,
            double debtor = 0,
            double inventory = 0,
            int? charityAccountId=0,
            string description="",
            DateTime? documentRegisterDateTimeFrom = null,
            DateTime? documentRegisterDateTimeTo = null,
            DateTime? registerDateFrom = null,
            DateTime? registerDateTo = null,
            int? id=null
            );
        List<SelectListItem> ReadAll(int selectedValue);
    }
}