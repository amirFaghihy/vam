using Aban.Domain.Entities;
using Aban.Service.IServices.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Aban.Service.Interfaces
{
    public interface ICharityLoanService : IGenericService<CharityLoan>
    {
        Tuple<CharityLoan, ResultStatusOperation> FillModel(CharityLoan charityLoan);
        Tuple<IQueryable<CharityLoan>, ResultStatusOperation> SpecificationGetData(string? userIdentityId = "", string loanReceiverId = "", float? loanAmount = null, byte? percentSalary = null, string? accountNumber = null, DateTime? paymentStartDateFrom = null, DateTime? paymentStartDateTo = null, byte? numberOfInstallments = null, DateTime? registerDateFrom = null, DateTime? registerDateTo = null, bool? isDone = null);
        List<SelectListItem> ReadAll(int selectedValue);
    }
}