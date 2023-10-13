using Aban.Domain.Entities;
using Aban.Service.IServices.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Aban.Service.Interfaces
{
    public interface ICharityLoanInstallmentsService : IGenericService<CharityLoanInstallments>
    {
        Tuple<CharityLoanInstallments, ResultStatusOperation> FillModel(CharityLoanInstallments charityLoanInstallments);
        Tuple<IQueryable<CharityLoanInstallments>, ResultStatusOperation> SpecificationGetData(int? charityLoanId = null, double? installmentAmount = null, DateTime? paymentDateFrom = null, DateTime? paymentDateTo = null, DateTime? registerDateFrom = null, DateTime? registerDateTo = null, bool? isDone = null);
        List<CharityLoanInstallments> CreateListOfModel(CharityLoan charityLoan);
        List<SelectListItem> ReadAll(int selectedValue);
    }
}