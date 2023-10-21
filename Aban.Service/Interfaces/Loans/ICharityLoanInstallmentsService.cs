using Aban.Domain.Entities;
using Aban.Service.IServices.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using static Aban.Domain.Enumerations.Enumeration;

namespace Aban.Service.Interfaces
{
    public interface ICharityLoanInstallmentsService : IGenericService<CharityLoanInstallments>
    {
        Tuple<CharityLoanInstallments, ResultStatusOperation> FillModel(CharityLoanInstallments charityLoanInstallments);
        Tuple<IQueryable<CharityLoanInstallments>, ResultStatusOperation> SpecificationGetData(int? charityLoanId = null, double? installmentAmount = null, DateTime? paymentDueFrom = null, DateTime? paymentDueTo = null, DateTime? paymentDate = null, TransactionMethod? paymentMethod = null, DateTime? registerDateFrom = null, DateTime? registerDateTo = null, bool? isdone = null);
        List<CharityLoanInstallments> CreateListOfModel(CharityLoan charityLoan);
        List<SelectListItem> ReadAll(int selectedValue);
    }
}
