using Aban.Domain.Entities;
using Aban.Service.IServices.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using static Aban.Domain.Enumerations.Enumeration;

namespace Aban.Service.Interfaces
{
    public interface IGuaranteeService : IGenericService<Guarantee>
    {
        Tuple<Guarantee, ResultStatusOperation> FillModel(Guarantee guarantee);
        Tuple<IQueryable<Guarantee>, ResultStatusOperation> SpecificationGetData(string guaranteeId = ""  , string? chequeNumber = null  , BankName? bankName = null  , double? chequePrice = null  , string? bankDraftNumber = null  , double? bankDraftPrice = null  , string? goldGuarantee = null  , string? paySlip = null  , DateTime? registerDateFrom =null,DateTime? registerDateTo = null  );
        List<SelectListItem> ReadAll(int selectedValue);
    }
}
