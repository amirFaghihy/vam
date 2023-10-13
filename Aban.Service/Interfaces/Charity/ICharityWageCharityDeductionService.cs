using Aban.Domain.Entities;
using Aban.Service.IServices.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Aban.Service.Interfaces
{
    public interface ICharityWageCharityDeductionService : IGenericService<CharityWageCharityDeduction>
    {
        Tuple<CharityWageCharityDeduction, ResultStatusOperation> FillModel(CharityWageCharityDeduction charityWageCharityDeduction);
        Tuple<IQueryable<CharityWageCharityDeduction>, ResultStatusOperation> SpecificationGetData(int charityWageId = 0, int charityDeducationId = 0);
        List<SelectListItem> ReadAll(int selectedValue);
    }
}