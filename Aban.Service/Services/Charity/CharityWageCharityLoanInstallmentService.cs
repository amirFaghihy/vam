using Aban.Domain.Entities;
using Aban.Service.Interfaces;
using Aban.DataLayer.Context;
using Microsoft.AspNetCore.Mvc.Rendering;
using Aban.Service.Services.Generic;
using Aban.DataLayer.Interfaces;
using Aban.Domain.Enumerations;

namespace Aban.Service.Services
{
    public class CharityWageCharityLoanInstallmentService : GenericService<CharityWageCharityLoanInstallment>, ICharityWageCharityLoanInstallmentService
    {
        private readonly ICharityWageCharityLoanInstallmentRepository charityWageCharityLoanInstallmentRepository;

        public CharityWageCharityLoanInstallmentService(AppDbContext _dbContext) : base(_dbContext)
        {
            charityWageCharityLoanInstallmentRepository = new DataLayer.Repositories.CharityWageCharityLoanInstallmentRepository(_dbContext);
        }


        public Tuple<IQueryable<CharityWageCharityLoanInstallment>, ResultStatusOperation> SpecificationGetData(int charityWageId = 0, int charityLoanInstallmentId = 0)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation();
            resultStatusOperation.IsSuccessed = true;
            resultStatusOperation.Type = Enumeration.MessageTypeResult.Success;
            try
            {
                IQueryable<CharityWageCharityLoanInstallment> query = charityWageCharityLoanInstallmentRepository.GetAll();

                if (charityWageId != 0)
                {
                    query = query.Where(x => x.CharityWageId == charityWageId);
                }
                if (charityLoanInstallmentId != 0)
                {
                    query = query.Where(x => x.CharityLoanInstallmentId == charityLoanInstallmentId);
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

        public Tuple<CharityWageCharityLoanInstallment, ResultStatusOperation> FillModel(CharityWageCharityLoanInstallment charityWageCharityLoanInstallment)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation();
            resultStatusOperation.IsSuccessed = true;
            resultStatusOperation.Type = Enumeration.MessageTypeResult.Success;
            try
            {
                charityWageCharityLoanInstallment.IsDelete = false;
                //charityWageCharityLoanInstallment.RegisterDate = DateTime.Now;

                return Tuple.Create(charityWageCharityLoanInstallment, resultStatusOperation);
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
                    var query = charityWageCharityLoanInstallmentRepository.GetAll().OrderBy(x => x.Id).ToList();

                    List<SelectListItem> item = query.ConvertAll(x =>
                    {
                        return new SelectListItem()
                        {
                            Text = x.ToString(), // فیلد مناسب قرار بگیرد
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
