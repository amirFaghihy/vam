using Aban.Domain.Entities;
using Aban.Service.Interfaces;
using Aban.DataLayer.Context;
using Microsoft.AspNetCore.Mvc.Rendering;
using Aban.Service.Services.Generic;
using Aban.DataLayer.Interfaces;
using Aban.Domain.Enumerations;
using static Aban.Domain.Enumerations.Enumeration;
using Microsoft.EntityFrameworkCore;

namespace Aban.Service.Services
{
    public class CharityLoanInstallmentsService : GenericService<CharityLoanInstallments>, ICharityLoanInstallmentsService
    {
        private readonly ICharityLoanInstallmentsRepository charityLoanInstallmentsRepository;

        public CharityLoanInstallmentsService(AppDbContext _dbContext) : base(_dbContext)
        {
            charityLoanInstallmentsRepository = new DataLayer.Repositories.CharityLoanInstallmentsRepository(_dbContext);
        }


        public Tuple<IQueryable<CharityLoanInstallments>, ResultStatusOperation> SpecificationGetData(
            int? charityLoanId = null,
            double? installmentAmount = null,
            DateTime? paymentDueFrom = null,
            DateTime? paymentDueTo = null,
            DateTime? paymentDate = null,
            TransactionMethod? paymentMethod = null,
            DateTime? registerDateFrom = null,
            DateTime? registerDateTo = null,
            bool? isdone = null)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation();
            resultStatusOperation.IsSuccessed = true;
            resultStatusOperation.Type = MessageTypeResult.Success;
            try
            {
                IQueryable<CharityLoanInstallments> query = charityLoanInstallmentsRepository.GetAll()
                    .Where(x => !x.IsDelete);

                if (charityLoanId != null)
                {
                    query = query.Where(x => x.CharityLoanId == charityLoanId);
                }
                if (installmentAmount != null)
                {
                    query = query.Where(x => x.InstallmentAmount == installmentAmount);
                }
                if (paymentDueFrom != null || paymentDueTo != null)
                {
                    DateTime _paymentDueFrom = paymentDueFrom != null ? paymentDueFrom.Value : new DateTime(1930, 1, 1, 1, 0, 0, 0);
                    DateTime _paymentDueTo = paymentDueTo != null ? paymentDueTo.Value : new DateTime(DateTime.Now.AddYears(1).Year, 1, 1, 1, 0, 0, 0);
                    query = query.Where(x => x.PaymentDue >= _paymentDueFrom && x.PaymentDue <= _paymentDueTo);
                }
                if (paymentDate != null)
                {
                    query = query.Where(x => x.PaymentDate == paymentDate);
                }
                if (paymentMethod != null)
                {
                    query = query.Where(x => x.PaymentMethod == paymentMethod);
                }
                if (registerDateFrom != null || registerDateTo != null)
                {
                    DateTime _registerDateFrom = registerDateFrom != null ? registerDateFrom.Value : new DateTime(1930, 1, 1, 1, 0, 0, 0);
                    DateTime _registerDateTo = registerDateTo != null ? registerDateTo.Value : new DateTime(DateTime.Now.AddYears(1).Year, 1, 1, 1, 0, 0, 0);
                    query = query.Where(x => x.RegisterDate >= _registerDateFrom && x.RegisterDate <= _registerDateTo);
                }
                if (isdone != null)
                {
                    query = query.Where(x => x.IsDone == isdone);
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

        /// <summary>
        /// بر اساس اطلاعات اولیه وام یک لیست از مدل اقساط وام میسازد
        /// </summary>
        /// <param name="charityLoan"></param>
        /// <returns></returns>
        public List<CharityLoanInstallments> CreateListOfModel(CharityLoan charityLoan)
        {
            List<CharityLoanInstallments> charityLoanInstallments = new List<CharityLoanInstallments>();

            // مبلغ هر قسط
            double installmentAmount = (((charityLoan.LoanAmount * charityLoan.PercentSalary) / 100) + charityLoan.LoanAmount) / charityLoan.NumberOfInstallments;


            for (int i = 0; i < charityLoan.NumberOfInstallments; i++)
            {
                charityLoanInstallments.Add(new CharityLoanInstallments()
                {
                    CharityLoanId = charityLoan.Id,
                    InstallmentAmount = installmentAmount,
                    IsDelete = false,
                    IsDone = false,
                    PaymentDue = charityLoan.PaymentStartDate.AddMonths(i),
                    RegisterDate = DateTime.Now
                });
            }
            return charityLoanInstallments;
        }


        public Tuple<CharityLoanInstallments, ResultStatusOperation> FillModel(CharityLoanInstallments charityLoanInstallments)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation();
            resultStatusOperation.IsSuccessed = true;
            resultStatusOperation.Type = MessageTypeResult.Success;
            try
            {
                charityLoanInstallments.IsDelete = false;
                charityLoanInstallments.RegisterDate = DateTime.Now;

                return Tuple.Create(charityLoanInstallments, resultStatusOperation);
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
                    List<CharityLoanInstallments> query = charityLoanInstallmentsRepository.GetAll()
                        .Where(x => !x.IsDelete)
                        .Include(x => x.CharityLoan!.LoanReceiver)
                        .OrderBy(x => x.Id).ToList();

                    List<SelectListItem> item = query.ConvertAll(x =>
                    {
                        return new SelectListItem()
                        {
                            Text =
                            string.IsNullOrEmpty(x.CharityLoan!.LoanReceiver!.FatherName) ?
                            $"{x.Id} | {x.CharityLoan!.LoanReceiver!.FirstName} {x.CharityLoan.LoanReceiver.LastName}" :
                            $"{x.Id} | {x.CharityLoan!.LoanReceiver!.FirstName} {x.CharityLoan.LoanReceiver.LastName} فرزند: {x.CharityLoan!.LoanReceiver!.FatherName}"
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
