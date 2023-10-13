using Aban.Domain.Entities;
using Aban.Service.Interfaces;
using Aban.DataLayer.Context;
using Microsoft.AspNetCore.Mvc.Rendering;
using Aban.Service.Services.Generic;
using Aban.DataLayer.Interfaces;
using Aban.Domain.Enumerations;

namespace Aban.Service.Services
{
    public class CharityWageCharityAdditionService : GenericService<CharityWageCharityAddition>, ICharityWageCharityAdditionService
    {
        private readonly ICharityWageCharityAdditionRepository charityWageCharityAdditionRepository;

        public CharityWageCharityAdditionService(AppDbContext _dbContext) : base(_dbContext)
        {
            charityWageCharityAdditionRepository = new DataLayer.Repositories.CharityWageCharityAdditionRepository(_dbContext);
        }


        public Tuple<IQueryable<CharityWageCharityAddition>, ResultStatusOperation> SpecificationGetData(
            int charityWageId = 0,
            int charityAdditionId = 0)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation();
            resultStatusOperation.IsSuccessed = true;
            resultStatusOperation.Type = Enumeration.MessageTypeResult.Success;
            try
            {
                IQueryable<CharityWageCharityAddition> query = charityWageCharityAdditionRepository.GetAll();

                if (charityWageId != 0)
                {
                    query = query.Where(x => x.CharityWageId == charityWageId);
                }
                if (charityAdditionId != 0)
                {
                    query = query.Where(x => x.CharityAdditionId == charityAdditionId);
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

        public Tuple<CharityWageCharityAddition, ResultStatusOperation> FillModel(CharityWageCharityAddition charityWageCharityAddition)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation();
            resultStatusOperation.IsSuccessed = true;
            resultStatusOperation.Type = Enumeration.MessageTypeResult.Success;
            try
            {
                charityWageCharityAddition.IsDelete = false;
                //charityWageCharityAddition.RegisterDate = DateTime.Now;

                return Tuple.Create(charityWageCharityAddition, resultStatusOperation);
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

        public List<SelectListItem> ReadAll(int selectedValue)
        {

            {
                ResultStatusOperation resultStatusOperation = new ResultStatusOperation();
                resultStatusOperation.IsSuccessed = true;
                resultStatusOperation.Type = Enumeration.MessageTypeResult.Success;

                try
                {
                    List<CharityWageCharityAddition> query =
                        charityWageCharityAdditionRepository.GetAll()
                        .Where(x => !x.IsDelete)
                        .OrderBy(x => x.Id).ToList();

                    List<SelectListItem> item = query.ConvertAll(x =>
                    {
                        return new SelectListItem()
                        {
                            Text = x.ToString(),// تصمیم گرفته شود که چه فیلدی را نمایش دهد
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
