using Aban.Domain.Entities;
using Aban.Service.Interfaces;
using Aban.DataLayer.Context;
using Microsoft.AspNetCore.Mvc.Rendering;
using Aban.Service.Services.Generic;
using Aban.DataLayer.Interfaces;
using Aban.Domain.Enumerations;

namespace Aban.Service.Services
{
    public class CharityWageCharityDepositService : GenericService<CharityWageCharityDeposit>, ICharityWageCharityDepositService
    {
        private readonly ICharityWageCharityDepositRepository charityWageCharityDepositRepository;

        public CharityWageCharityDepositService(AppDbContext _dbContext) : base(_dbContext)
        {
            charityWageCharityDepositRepository = new DataLayer.Repositories.CharityWageCharityDepositRepository(_dbContext);
        }


        public Tuple<IQueryable<CharityWageCharityDeposit>, ResultStatusOperation> SpecificationGetData(
            int charityWageId = 0,
            int charityDepositId = 0)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation();
            resultStatusOperation.IsSuccessed = true;
            resultStatusOperation.Type = Enumeration.MessageTypeResult.Success;
            try
            {
                IQueryable<CharityWageCharityDeposit> query = charityWageCharityDepositRepository.GetAll();

                if (charityWageId != 0)
                {
                    query = query.Where(x => x.CharityWageId == charityWageId);
                }
                if (charityDepositId != 0)
                {
                    query = query.Where(x => x.CharityDepositId == charityDepositId);
                }


                return Tuple.Create(query, resultStatusOperation);
            }
            catch (Exception exception)
            {
                resultStatusOperation.IsSuccessed = false;
                resultStatusOperation.Message = "خطایی رخ داده است";
                resultStatusOperation.Type = Enumeration.MessageTypeResult.Danger;
                resultStatusOperation.ErrorException = exception;
                throw new Exception("", exception);
            }
        }

        public Tuple<CharityWageCharityDeposit, ResultStatusOperation> FillModel(CharityWageCharityDeposit charityWageCharityDeposit)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation();
            resultStatusOperation.IsSuccessed = true;
            resultStatusOperation.Type = Enumeration.MessageTypeResult.Success;
            try
            {
                charityWageCharityDeposit.IsDelete = false;
                //charityWageCharityDeposit.RegisterDate = DateTime.Now;

                return Tuple.Create(charityWageCharityDeposit, resultStatusOperation);
            }
            catch (Exception exception)
            {
                resultStatusOperation.IsSuccessed = false;
                resultStatusOperation.Message = "خطایی رخ داده است";
                resultStatusOperation.Type = Enumeration.MessageTypeResult.Danger;
                resultStatusOperation.ErrorException = exception;
                throw new Exception("", exception);
            }
        }

        public List<SelectListItem> ReadAll(Guid selectedValue)
        {

            {
                ResultStatusOperation resultStatusOperation = new ResultStatusOperation();
                resultStatusOperation.IsSuccessed = true;
                resultStatusOperation.Type = Enumeration.MessageTypeResult.Success;

                try
                {
                    List<CharityWageCharityDeposit> query = charityWageCharityDepositRepository.GetAll()
                        .OrderBy(x => x.Id).ToList();

                    List<SelectListItem> item = query.ConvertAll(x =>
                    {
                        return new SelectListItem()
                        {
                            Text = "",//x.Title,
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
                    resultStatusOperation.Type = Enumeration.MessageTypeResult.Danger;
                    resultStatusOperation.ErrorException = exception;
                    throw new Exception("", exception);
                }
            }
        }

    }
}
