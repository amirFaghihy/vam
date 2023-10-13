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
    public class CharityDeducationService : GenericService<CharityDeducation>, ICharityDeducationService
    {
        private readonly ICharityDeducationRepository charityDeducationRepository;

        public CharityDeducationService(AppDbContext _dbContext) : base(_dbContext)
        {
            charityDeducationRepository = new DataLayer.Repositories.CharityDeducationRepository(_dbContext);
        }


        public Tuple<IQueryable<CharityDeducation>, ResultStatusOperation> SpecificationGetData(
            string userIdentityId = "",
            string userIdentityReciverId = "",
            string title = "",
            double amount = 0,
            string accountNumber = "",
            string description = "",
            DateTime? timeForActionFrom = null,
            DateTime? timeForActionTo = null,
            DateTime? registerDateFrom = null,
            DateTime? registerDateTo = null,
            bool? isDone = null)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation();
            resultStatusOperation.IsSuccessed = true;
            resultStatusOperation.Type = Enumeration.MessageTypeResult.Success;
            try
            {
                IQueryable<CharityDeducation> query = charityDeducationRepository.GetAll()
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
                if (timeForActionFrom != null || timeForActionTo != null)
                {
                    DateTime _timeForActionFrom = timeForActionFrom != null ? timeForActionFrom.Value : new DateTime(1930, 1, 1, 1, 0, 0, 0);
                    DateTime _timeForActionTo = timeForActionTo != null ? timeForActionTo.Value : new DateTime(DateTime.Now.AddYears(1).Year, 1, 1, 1, 0, 0, 0);
                    query = query.Where(x => x.TimeForAction >= _timeForActionFrom && x.TimeForAction <= _timeForActionTo);
                }
                if (registerDateFrom != null || registerDateTo != null)
                {
                    DateTime _registerDateFrom = registerDateFrom != null ? registerDateFrom.Value : new DateTime(1930, 1, 1, 1, 0, 0, 0);
                    DateTime _registerDateTo = registerDateTo != null ? registerDateTo.Value : new DateTime(DateTime.Now.AddYears(1).Year, 1, 1, 1, 0, 0, 0);
                    query = query.Where(x => x.RegisterDate >= _registerDateFrom && x.RegisterDate <= _registerDateTo);
                }

                query = query
                    .Include(x => x.UserIdentity)
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

        public Tuple<CharityDeducation, ResultStatusOperation> FillModel(CharityDeducation charityDeducation)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation();
            resultStatusOperation.IsSuccessed = true;
            resultStatusOperation.Type = Enumeration.MessageTypeResult.Success;
            try
            {
                charityDeducation.IsDelete = false;
                charityDeducation.RegisterDate = DateTime.Now;

                return Tuple.Create(charityDeducation, resultStatusOperation);
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
                    var query = charityDeducationRepository.GetAll()
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
