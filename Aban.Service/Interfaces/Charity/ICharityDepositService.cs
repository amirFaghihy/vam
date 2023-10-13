using Aban.Domain.Entities;
using Aban.Service.IServices.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace Aban.Service.Interfaces
{
    public interface ICharityDepositService : IGenericService<CharityDeposit>
    {
        List<SelectListItem> SelectListItems(string userId, List<string?>? selectedValue,
            List<IQueryable<UserIdentity>> query, string nullText = "");
        Task<bool> CheckDepositUniqueAsync(CharityDeposit model);
        Task<(string clreckName, string foremanName, int? depositId)> GetDetailByBankRecordId(int bankRecordId);
        Task<(string clreckName, string foremanName, int? bankRecordId)> GetDetailByDepositId(int depositId);
        IQueryable<CharityDeposit> GetAllDepositsByUserIdentity(List<UserIdentity> userIdentities);
        IQueryable<CharityWage> GetAllClerkWagesByFormenUserIdentity(List<UserIdentity> userIdentities);
        Tuple<IQueryable<CharityDeposit>, ResultStatusOperation> GetDepositsByCharityWageId(int charityWageId = 0);
        Tuple<CharityDeposit, ResultStatusOperation> FillModel(CharityDeposit charityDeposit, string date, string time);
        Tuple<IQueryable<CharityDeposit>, ResultStatusOperation> SpecificationGetData(string userIdentityId = "", string helperId = "", int charityAccountId = 0, double amount = 0, bool? isconfirm = null, string issueTracking = "", string description = "", DateTime? documentRegisterDateTimeFrom = null, DateTime? documentRegisterDateTimeTo = null, DateTime? registerDateFrom = null, DateTime? registerDateTo = null, string lastFourDigits = "");
        IQueryable<CharityDeposit> SpecificationGetData(
            ClaimsPrincipal USER,
            string currentUserId,
             List<string>? userIdentityId = null,
             List<string>? helper = null,
            List<string>? foreman = null,
            List<string>? clerk = null,
            List<int>? charityAccount = null,
            double amount = 0,
            bool? isConfirm = null,
            string issueTracking = "",
            DateTime? registerDateFrom = null,
            DateTime? registerDateTo = null,
            DateTime? documentRegisterDateTime = null,
            string lastFourDigits = "",
            int? id = null,
            ConfirmType? confirmType = null
            );
        List<SelectListItem> ReadAll(int selectedValue);
    }
}