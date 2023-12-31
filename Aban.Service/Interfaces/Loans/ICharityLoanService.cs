using Aban.Domain.Entities;
using Aban.Service.IServices.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using static Aban.Domain.Enumerations.Enumeration;

namespace Aban.Service.Interfaces
{
    public interface ICharityLoanService : IGenericService<CharityLoan>
    {
        Tuple<CharityLoan, ResultStatusOperation> FillModel(CharityLoan charityLoan);
        Tuple<IQueryable<CharityLoan>, ResultStatusOperation> SpecificationGetData(
            int? guaranteeId = null,
            string loanReceiverId = "",
            float? loanAmount = null,
            byte? percentSalary = null,
            string? accountNumber = null,
            TransactionMethod? givingLoanMethod = null,
            DateTime? paymentStartDateFrom = null,
            DateTime? paymentStartDateTo = null,
            byte? numberOfInstallments = null,
            DateTime? registerDateFrom = null,
            DateTime? registerDateTo = null,
            bool? isdone = null,
            List<string>? lstLoanReceiverId = null);
        List<SelectListItem> ReadAll(int selectedValue);
    }
}