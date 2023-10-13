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
    public class CharityAdditionService : GenericService<CharityAddition>, ICharityAdditionService
    {
        private readonly ICharityAdditionRepository charityAdditionRepository;

        public CharityAdditionService(AppDbContext _dbContext) : base(_dbContext)
        {
            charityAdditionRepository = new DataLayer.Repositories.CharityAdditionRepository(_dbContext);
        }


        public Tuple<IQueryable<CharityAddition>, ResultStatusOperation> SpecificationGetData(
            string userIdentityId = "",
            string userIdentityReciverId = "",
            string title = "",
            double amount = 0,
            string accountNumber = "",
            string description = "",
            DateTime? documentRegisterDateTimeFrom = null,
            DateTime? documentRegisterDateTimeTo = null,
            DateTime? registerDateFrom = null,
            DateTime? registerDateTo = null,
            bool? isDone = null)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation();
            resultStatusOperation.IsSuccessed = true;
            resultStatusOperation.Type = Enumeration.MessageTypeResult.Success;
            try
            {
                IQueryable<CharityAddition> query = charityAdditionRepository.GetAll()
                    .Where(x => !x.IsDelete);

                if (isDone != null)
                {
                    query = query.Where(x => x.IsDone == isDone);
                }

                if (!string.IsNullOrEmpty(userIdentityId))
                {
                    query = query.Where(x => x.UserIdentityId == userIdentityId);
                }
                if (!string.IsNullOrEmpty(userIdentityReciverId))
                {
                    query = query.Where(x => x.UserIdentityReciverId == userIdentityReciverId);
                }
                if (!string.IsNullOrEmpty(title))
                {
                    query = query.Where(x => x.Title.Contains(title));
                }
                if (amount != 0)
                {
                    query = query.Where(x => x.Amount == amount);
                }
                if (!string.IsNullOrEmpty(accountNumber))
                {
                    query = query.Where(x => x.AccountNumber.Contains(accountNumber));
                }
                if (!string.IsNullOrEmpty(description))
                {
                    query = query.Where(x => x.Description.Contains(description));
                }
                if (documentRegisterDateTimeFrom != null || documentRegisterDateTimeTo != null)
                {
                    DateTime _documentRegisterDateTimeFrom = documentRegisterDateTimeFrom != null ? documentRegisterDateTimeFrom.Value : new DateTime(1930, 1, 1, 1, 0, 0, 0);
                    DateTime _documentRegisterDateTimeTo = documentRegisterDateTimeTo != null ? documentRegisterDateTimeTo.Value : new DateTime(DateTime.Now.AddYears(1).Year, 1, 1, 1, 0, 0, 0);
                    query = query.Where(x => x.DocumentRegisterDateTime >= _documentRegisterDateTimeFrom && x.DocumentRegisterDateTime <= _documentRegisterDateTimeTo);
                }
                if (registerDateFrom != null || registerDateTo != null)
                {
                    DateTime _registerDateFrom = registerDateFrom != null ? registerDateFrom.Value : new DateTime(1930, 1, 1, 1, 0, 0, 0);
                    DateTime _registerDateTo = registerDateTo != null ? registerDateTo.Value : new DateTime(DateTime.Now.AddYears(1).Year, 1, 1, 1, 0, 0, 0);
                    query = query.Where(x => x.RegisterDate >= _registerDateFrom && x.RegisterDate <= _registerDateTo);
                }

                query = query.Include(x => x.UserIdentity)
                        .Include(x => x.UserIdentityReciver);

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


        public Tuple<CharityAddition, ResultStatusOperation> FillModel(CharityAddition charityAddition)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation();
            resultStatusOperation.IsSuccessed = true;
            resultStatusOperation.Type = Enumeration.MessageTypeResult.Success;
            try
            {
                charityAddition.IsDelete = false;
                charityAddition.RegisterDate = DateTime.Now;

                return Tuple.Create(charityAddition, resultStatusOperation);
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
                    var query = charityAdditionRepository.GetAll()
                        .Where(x => !x.IsDelete).OrderBy(x => x.Id).ToList();

                    List<SelectListItem> item = query.ConvertAll(x =>
                    {
                        return new SelectListItem()
                        {
                            Text = x.Title,
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
