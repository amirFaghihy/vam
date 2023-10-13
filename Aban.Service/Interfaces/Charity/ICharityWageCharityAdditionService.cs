using Aban.Domain.Entities;
using Aban.Service.IServices.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Aban.Service.Interfaces
{
    public interface ICharityWageCharityAdditionService : IGenericService<CharityWageCharityAddition>
    {
        Tuple<CharityWageCharityAddition, ResultStatusOperation> FillModel(CharityWageCharityAddition charityWageCharityAddition);
        Tuple<IQueryable<CharityWageCharityAddition>, ResultStatusOperation> SpecificationGetData(int charityWageId = 0, int charityAdditionId = 0);
        List<SelectListItem> ReadAll(int selectedValue);
    }
}