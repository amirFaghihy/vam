using Aban.Domain.Entities;
using Aban.Service.IServices.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Aban.Service.Interfaces
{
    public interface ICharityWageCharityLoanInstallmentService : IGenericService<CharityWageCharityLoanInstallment>
    {
        Tuple<CharityWageCharityLoanInstallment, ResultStatusOperation> FillModel(CharityWageCharityLoanInstallment charityWageCharityLoanInstallment);
        Tuple<IQueryable<CharityWageCharityLoanInstallment>, ResultStatusOperation> SpecificationGetData(int charityWageId = 0, int charityLoanInstallmentId = 0);
        List<SelectListItem> ReadAll(int selectedValue);
    }
}