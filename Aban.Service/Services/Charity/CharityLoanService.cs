using Aban.Domain.Entities;
using Aban.Service.Interfaces;
using Aban.DataLayer.Context;
using Microsoft.AspNetCore.Mvc.Rendering;
using Aban.Service.Services.Generic;
using Aban.DataLayer.Interfaces;
using Aban.Domain.Enumerations;
using Microsoft.EntityFrameworkCore;

namespace Aban.Service.Services
{
    public class CharityLoanService : GenericService<CharityLoan>, ICharityLoanService
    {
        private readonly ICharityLoanRepository charityLoanRepository;

        public CharityLoanService(AppDbContext _dbContext) : base(_dbContext)
        {
            charityLoanRepository = new DataLayer.Repositories.CharityLoanRepository(_dbContext);
        }


        public Tuple<IQueryable<CharityLoan>, ResultStatusOperation> SpecificationGetData(
            string? userIdentityId = "",
            string loanReceiverId = "",
            float? loanAmount = null,
            byte? percentSalary = null,
            string? accountNumber = null,
            DateTime? paymentStartDateFrom = null,
            DateTime? paymentStartDateTo = null,
            byte? numberOfInstallments = null,
            DateTime? registerDateFrom = null,
            DateTime? registerDateTo = null,
            bool? isDone = null)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation();
            resultStatusOperation.IsSuccessed = true;
            resultStatusOperation.Type = Enumeration.MessageTypeResult.Success;
            try
            {
                IQueryable<CharityLoan> query = charityLoanRepository.GetAll()
                    .Where(x => !x.IsDelete);

                if (isDone != null)
                {
                    query = query.Where(x => x.IsDone == isDone);
                }
                if (!string.IsNullOrEmpty(userIdentityId))
                {
                    query = query.Where(x => x.UserIdentityId == userIdentityId);
                }
                if (!string.IsNullOrEmpty(loanReceiverId))
                {
                    query = query.Where(x => x.LoanReceiverId.Contains(loanReceiverId));
                }
                if (loanAmount != null)
                {
                    query = query.Where(x => x.LoanAmount == loanAmount);
                }
                if (percentSalary != null)
                {
                    query = query.Where(x => x.PercentSalary == percentSalary);
                }
                if (accountNumber != null)
                {
                    query = query.Where(x => x.AccountNumber == accountNumber);
                }
                if (paymentStartDateFrom != null || paymentStartDateTo != null)
                {
                    DateTime _paymentStartDateFrom = paymentStartDateFrom != null ? paymentStartDateFrom.Value : new DateTime(1930, 1, 1, 1, 0, 0, 0);
                    DateTime _paymentStartDateTo = paymentStartDateTo != null ? paymentStartDateTo.Value : new DateTime(DateTime.Now.AddYears(1).Year, 1, 1, 1, 0, 0, 0);
                    query = query.Where(x => x.PaymentStartDate >= _paymentStartDateFrom && x.PaymentStartDate <= _paymentStartDateTo);
                }
                if (numberOfInstallments != null)
                {
                    query = query.Where(x => x.NumberOfInstallments == numberOfInstallments);
                }
                if (registerDateFrom != null || registerDateTo != null)
                {
                    DateTime _registerDateFrom = registerDateFrom != null ? registerDateFrom.Value : new DateTime(1930, 1, 1, 1, 0, 0, 0);
                    DateTime _registerDateTo = registerDateTo != null ? registerDateTo.Value : new DateTime(DateTime.Now.AddYears(1).Year, 1, 1, 1, 0, 0, 0);
                    query = query.Where(x => x.RegisterDate >= _registerDateFrom && x.RegisterDate <= _registerDateTo);
                }

                query = query.Include(x => x.UserIdentity);

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


        public Tuple<CharityLoan, ResultStatusOperation> FillModel(CharityLoan charityLoan)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation();
            resultStatusOperation.IsSuccessed = true;
            resultStatusOperation.Type = Enumeration.MessageTypeResult.Success;
            try
            {
                charityLoan.IsDelete = false;
                charityLoan.RegisterDate = DateTime.Now;

                return Tuple.Create(charityLoan, resultStatusOperation);
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
                    var query = charityLoanRepository.GetAll()
                        .Where(x => !x.IsDelete)
                        .Include(x => x.LoanReceiver)
                        .OrderBy(x => x.Id).ToList();

                    List<SelectListItem> item = query.ConvertAll(x =>
                    {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                        return new SelectListItem()
                        {
                            Text = $"{x.LoanReceiver.FirstName} {x.LoanReceiver.LastName} | کد: {x.Id}",
                            Value = x.Id.ToString(),
                            Selected = (x.Id == selectedValue) ? true : false
                        };
#pragma warning restore CS8602 // Dereference of a possibly null reference.
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
