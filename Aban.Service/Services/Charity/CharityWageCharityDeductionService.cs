using Aban.Domain.Entities;
using Aban.Service.Interfaces;
using Aban.DataLayer.Context;
using Microsoft.AspNetCore.Mvc.Rendering;
using Aban.Service.Services.Generic;
using Aban.DataLayer.Interfaces;
using Aban.Domain.Enumerations;

namespace Aban.Service.Services
{
    public class CharityWageCharityDeductionService : GenericService<CharityWageCharityDeduction>, ICharityWageCharityDeductionService
    {
        private readonly ICharityWageCharityDeductionRepository charityWageCharityDeductionRepository;

        public CharityWageCharityDeductionService(AppDbContext _dbContext) : base(_dbContext)
        {
            charityWageCharityDeductionRepository = new DataLayer.Repositories.CharityWageCharityDeductionRepository(_dbContext);
        }


        public Tuple<IQueryable<CharityWageCharityDeduction>, ResultStatusOperation> SpecificationGetData(
            int charityWageId = 0,
            int charityDeducationId = 0)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation();
            resultStatusOperation.IsSuccessed = true;
            resultStatusOperation.Type = Enumeration.MessageTypeResult.Success;
            try
            {
                IQueryable<CharityWageCharityDeduction> query = charityWageCharityDeductionRepository.GetAll()
                    .Where(x => !x.IsDelete);

                if (charityWageId != 0)
                {
                    query = query.Where(x => x.CharityWageId == charityWageId);
                }
                if (charityDeducationId != 0)
                {
                    query = query.Where(x => x.CharityDeducationId == charityDeducationId);
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

        public Tuple<CharityWageCharityDeduction, ResultStatusOperation> FillModel(CharityWageCharityDeduction charityWageCharityDeduction)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation();
            resultStatusOperation.IsSuccessed = true;
            resultStatusOperation.Type = Enumeration.MessageTypeResult.Success;
            try
            {
                charityWageCharityDeduction.IsDelete = false;
                //charityWageCharityDeduction.RegisterDate = DateTime.Now;

                return Tuple.Create(charityWageCharityDeduction, resultStatusOperation);
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
                    List<CharityWageCharityDeduction> query =
                        charityWageCharityDeductionRepository.GetAll()
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
