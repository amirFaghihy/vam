using Aban.Domain.Entities;
using Aban.Service.IServices.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Aban.Service.Interfaces
{
    public interface ICharityWageCharityDepositService : IGenericService<CharityWageCharityDeposit>
    {
        Tuple<CharityWageCharityDeposit, ResultStatusOperation> FillModel(CharityWageCharityDeposit charityWageCharityDeposit);
        Tuple<IQueryable<CharityWageCharityDeposit>, ResultStatusOperation> SpecificationGetData(int charityWageId = 0, int charityDepositId = 0);
        List<SelectListItem> ReadAll(Guid selectedValue);
    }
}
