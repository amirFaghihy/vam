using Aban.Domain.Entities;
using Aban.Service.IServices.Generic;
using Aban.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using static Aban.Domain.Enumerations.Enumeration;

namespace Aban.Service.Interfaces
{
    public interface ICharityWageService : IGenericService<CharityWage>
    {
        List<CharityWage> ConvertViewModelToModel(string userIdentityId, List<CharityUserIdentityDepositViewModel> viewModel);

        List<PaySlipViewModel> ConvertModelToViewModelPaySlip(List<CharityWage> charityWages);
        Tuple<CharityWage, ResultStatusOperation> FillModel(CharityWage charityWage);
        Tuple<IQueryable<CharityWage>, ResultStatusOperation> SpecificationGetData(string userIdentityId = "", string wageReceiverId = "", string accountNumber = "", string description = "", float? fixedSalary = null, byte? percentSalary = null, DateTime? registerDateFrom = null, DateTime? registerDateTo = null, DateTime? wageDateFrom = null, DateTime? wageDateTo = null);
        Tuple<List<CharityUserIdentityDepositViewModel>, ResultStatusOperation> ConvertModelToViewModel(
            List<UserIdentity> users, List<RoleName> roleName, List<CharityDeposit>? deposits = null, List<CharityWage>? wages = null, DateTime? startDate = null, DateTime? endDate = null, string roleId = "");
        Tuple<List<CharityUserIdentityDepositViewModel>, ResultStatusOperation> ConvertModelToViewModelForSet(
            string userIdentityId, List<UserIdentity> users, List<RoleName> roleName, List<CharityDeposit>? deposits = null, List<CharityWage>? wages = null, DateTime? startDate = null, DateTime? endDate = null);

        List<SelectListItem> ReadAll(int selectedValue);
    }
}