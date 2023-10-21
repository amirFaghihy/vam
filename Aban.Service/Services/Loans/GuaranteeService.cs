using Aban.Domain.Entities;
using Aban.Service.Interfaces;
using Aban.DataLayer.Context;
using Microsoft.AspNetCore.Mvc.Rendering;
using Aban.Service.Services.Generic;
using Aban.DataLayer.Interfaces;
using static Aban.Domain.Enumerations.Enumeration;
using Microsoft.EntityFrameworkCore;

namespace Aban.Service.Services
{
    public class GuaranteeService : GenericService<Guarantee>, IGuaranteeService
    {
        private readonly IGuaranteeRepository guaranteeRepository;

        public GuaranteeService(AppDbContext _dbContext) : base(_dbContext)
        {
            guaranteeRepository = new DataLayer.Repositories.GuaranteeRepository(_dbContext);
        }


        public Tuple<IQueryable<Guarantee>, ResultStatusOperation> SpecificationGetData(
            string guaranteeId = "",
            string? chequeNumber = null,
            BankName? bankName = null,
            double? chequePrice = null,
            string? bankDraftNumber = null,
            double? bankDraftPrice = null,
            string? goldGuarantee = null,
            string? paySlip = null,
            DateTime? registerDateFrom = null,
            DateTime? registerDateTo = null)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation();
            resultStatusOperation.IsSuccessed = true;
            resultStatusOperation.Type = MessageTypeResult.Success;
            try
            {
                IQueryable<Guarantee> query = guaranteeRepository.GetAll()
                    .Where(x => !x.IsDelete);

                if (!string.IsNullOrEmpty(guaranteeId))
                {
                    query = query.Where(x => x.GuaranteeId.Contains(guaranteeId));
                }
                if (chequeNumber != null)
                {
                    query = query.Where(x => x.ChequeNumber == chequeNumber);
                }
                if (bankName != null)
                {
                    query = query.Where(x => x.BankName == bankName);
                }
                if (chequePrice != null)
                {
                    query = query.Where(x => x.ChequePrice == chequePrice);
                }
                if (bankDraftNumber != null)
                {
                    query = query.Where(x => x.BankDraftNumber == bankDraftNumber);
                }
                if (bankDraftPrice != null)
                {
                    query = query.Where(x => x.BankDraftPrice == bankDraftPrice);
                }
                if (goldGuarantee != null)
                {
                    query = query.Where(x => x.GoldGuarantee == goldGuarantee);
                }
                if (paySlip != null)
                {
                    query = query.Where(x => x.PaySlip == paySlip);
                }
                if (registerDateFrom != null || registerDateTo != null)
                {
                    DateTime _registerDateFrom = registerDateFrom != null ? registerDateFrom.Value : new DateTime(1930, 1, 1, 1, 0, 0, 0);
                    DateTime _registerDateTo = registerDateTo != null ? registerDateTo.Value : new DateTime(DateTime.Now.AddYears(1).Year, 1, 1, 1, 0, 0, 0);
                    query = query.Where(x => x.RegisterDate >= _registerDateFrom && x.RegisterDate <= _registerDateTo);
                }

                return Tuple.Create(query, resultStatusOperation);
            }
            catch (Exception exception)
            {
                resultStatusOperation.IsSuccessed = false;
                resultStatusOperation.Message = "خطایی رخ داده است";
                resultStatusOperation.Type = MessageTypeResult.Danger;
                resultStatusOperation.ErrorException = exception;
                throw new Exception("", exception);
            }
        }

        public Tuple<Guarantee, ResultStatusOperation> FillModel(Guarantee guarantee)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation();
            resultStatusOperation.IsSuccessed = true;
            resultStatusOperation.Type = MessageTypeResult.Success;
            try
            {
                guarantee.IsDelete = false;
                guarantee.RegisterDate = DateTime.Now;

                return Tuple.Create(guarantee, resultStatusOperation);
            }
            catch (Exception exception)
            {
                resultStatusOperation.IsSuccessed = false;
                resultStatusOperation.Message = "خطایی رخ داده است";
                resultStatusOperation.Type = MessageTypeResult.Danger;
                resultStatusOperation.ErrorException = exception;
                throw new Exception("", exception);
            }
        }

        public List<SelectListItem> ReadAll(int selectedValue)
        {

            {
                ResultStatusOperation resultStatusOperation = new ResultStatusOperation();
                resultStatusOperation.IsSuccessed = true;
                resultStatusOperation.Type = MessageTypeResult.Success;

                try
                {
                    var query = guaranteeRepository.GetAll()
                        .Where(x => !x.IsDelete)
                        .Include(x => x.GuaranteeUser)
                        .OrderBy(x => x.Id).ToList();

                    List<SelectListItem> item = query.ConvertAll(x =>
                    {
                        return new SelectListItem()
                        {
                            Text =
                            string.IsNullOrEmpty(x.GuaranteeUser!.FatherName) ?
                            $"{x.Id} | {x.GuaranteeUser!.FirstName} {x.GuaranteeUser.LastName}" :
                            $"{x.Id} | {x.GuaranteeUser!.FirstName} {x.GuaranteeUser.LastName} فرزند: {x.GuaranteeUser!.FatherName}"
                            ,
                            Value = x.Id.ToString(),
                            Selected = (x.Id == selectedValue) ? true : false
                        };
                    });


                    return item;
                }
                catch (Exception exception)
                {
                    resultStatusOperation.IsSuccessed = false;
                    resultStatusOperation.Message = "خطایی رخ داده است";
                    resultStatusOperation.Type = MessageTypeResult.Danger;
                    resultStatusOperation.ErrorException = exception;
                    throw new Exception("", exception);
                }
            }
        }

    }
}
