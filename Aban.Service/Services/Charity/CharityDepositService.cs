using Aban.Common;
using Aban.Common.Utility;
using Aban.DataLayer.Context;
using Aban.DataLayer.Interfaces;
using Aban.DataLayer.Repositories;
using Aban.Domain.Entities;
using Aban.Domain.Enumerations;
using Aban.Service.Interfaces;
using Aban.Service.Services.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;
using static Aban.Domain.Enumerations.Enumeration;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Aban.Service.Services
{
    public class CharityDepositService : GenericService<CharityDeposit>, ICharityDepositService
    {
        private readonly IUserIdentityRepository _userIdentityRepository;
        private readonly ICharityBankRecordRepository _charityBankRecordRepository;
        private readonly ICharityDepositRepository _charityDepositRepository;
        private readonly ICharityWageRepository _charityWageRepository;
        private readonly ICharityUserIdentityCharityHelperService _charityUserIdentityCharityHelperService;
        private readonly IUserIdentityService _userIdentityService;
        private readonly ICharityWageCharityDepositRepository _charityWageCharityDepositRepository;
        public CharityDepositService(AppDbContext _dbContext, ICharityUserIdentityCharityHelperService charityUserIdentityCharityHelperService, IUserIdentityService userIdentityService) : base(_dbContext)
        {
            _userIdentityService = userIdentityService;
            _userIdentityRepository = new UserIdentityRepository(_dbContext);
            _charityBankRecordRepository = new CharityBankRecordRepository(_dbContext);
            _charityDepositRepository = new CharityDepositRepository(_dbContext);
            _charityWageRepository = new CharityWageRepository(_dbContext);
            _charityWageCharityDepositRepository = new CharityWageCharityDepositRepository(_dbContext);
            _charityUserIdentityCharityHelperService = charityUserIdentityCharityHelperService;
        }

        public List<SelectListItem> SelectListItems(string userId, List<string?>? selectedValue, List<IQueryable<UserIdentity>> query, string nullText = "")
        {

            List<SelectListItem> listItem = new List<SelectListItem>();


            query.ForEach(x =>
            {
                x.ToList().ForEach(y =>
                {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8604 // Possible null reference argument.
                    SelectListItem selectListItem = new SelectListItem()
                    {
                        Text = !y.FatherName.IsNullOrEmpty() ? y.FirstName + " " + y.LastName + " |نام پدر: " + y.FatherName : y.FirstName + " " + y.LastName,
                        Value = y.Id,
                        Selected = selectedValue.Contains(y.Id)
                    };
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                    listItem.Add(selectListItem);
                });

            });
            if (!String.IsNullOrEmpty(nullText))
            {
                listItem.Insert(0, new SelectListItem(nullText, ""));
            }
            if (!userId.IsNullOrEmpty())
            {
#pragma warning disable CS8604 // Possible null reference argument.
                Task<Tuple<UserIdentity, ResultStatusOperation>> userIdentity = _userIdentityService.Find(userId);
#pragma warning restore CS8604 // Possible null reference argument.
                listItem.Insert(1, new SelectListItem(userIdentity.Result.Item1.FirstName + " " + userIdentity.Result.Item1.LastName, userIdentity.Result.Item1.Id, false));
            }
            return listItem;
        }

        public async Task<bool> CheckDepositUniqueAsync(CharityDeposit model)
        => await _charityDepositRepository.GetAll().AnyAsync(x =>
                !x.IsDelete && x.CharityAccountId == model.CharityAccountId && x.Amount == model.Amount &&
                x.HelperId == model.HelperId && x.DocumentRegisterDateTime.Date == model.DocumentRegisterDateTime.Date && x.UserIdentityId == model.UserIdentityId);

        public async Task<(string clreckName, string foremanName, int? depositId)> GetDetailByBankRecordId(int bankRecordId)
        {
            var resultFindBankRecord = await _charityBankRecordRepository.FindAsync(bankRecordId);

            var resultDeposit =
                await this.GetAll().FirstOrDefaultAsync(x => x.CharityBankRecordId == resultFindBankRecord.Id);

            if (resultDeposit != null
                )
            {
                var resultFindClreck = await _userIdentityRepository.FindAsync(resultDeposit.UserIdentityId);
                var findForemanId = _charityUserIdentityCharityHelperService
                    .GetAll().FirstOrDefault(x => x.HelperId == resultFindClreck.Id);
                var resultFindForeman = await _userIdentityRepository.FindAsync(findForemanId!.UserIdentityId);

                return (resultFindClreck.FirstName + " " + resultFindClreck.LastName,
                    resultFindForeman.FirstName + " " + resultFindForeman.LastName, resultDeposit.Id);
            }

            return ("تایید نشده",
                "تایید نشده"
                , null);
        }

        public async Task<(string clreckName, string foremanName, int? bankRecordId)> GetDetailByDepositId(int depositId)
        {
            var resultFindDeposit = await _charityDepositRepository.FindAsync(depositId);
            var resultFindClreck = await _userIdentityRepository.FindAsync(resultFindDeposit.UserIdentityId);
            var findForemanId = _charityUserIdentityCharityHelperService
                .GetAll().FirstOrDefault(x => x.HelperId == resultFindClreck.Id);
            var resultFindForeman = await _userIdentityRepository.FindAsync(findForemanId!.UserIdentityId);

            return (resultFindClreck.FirstName + " " + resultFindClreck.LastName,
                resultFindForeman.FirstName + " " + resultFindForeman.LastName, resultFindDeposit.CharityBankRecordId);
        }

        public Tuple<IQueryable<CharityDeposit>, ResultStatusOperation> SpecificationGetData(
            string userIdentityId = "",
            string helperId = "",
            int charityAccountId = 0,
            double amount = 0,
            bool? isconfirm = null,
            string issueTracking = "",
            string description = "",
            DateTime? documentRegisterDateTimeFrom = null,
            DateTime? documentRegisterDateTimeTo = null,
            DateTime? registerDateFrom = null,
            DateTime? registerDateTo = null,
            string lastFourDigits = "")
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation();
            resultStatusOperation.IsSuccessed = true;
            resultStatusOperation.Message = "عملیات با موفقیت انجام شد.";
            resultStatusOperation.Type = MessageTypeResult.Success;
            try
            {
                IQueryable<CharityDeposit> query = _charityDepositRepository.GetAll()
                    .Where(x => !x.IsDelete);

                if (!string.IsNullOrEmpty(userIdentityId))
                {
                    query = query.Where(x => x.UserIdentityId == userIdentityId);
                }
                if (!string.IsNullOrEmpty(helperId))
                {
                    query = query.Where(x => x.HelperId.Contains(helperId));
                }
                if (charityAccountId != 0)
                {
                    query = query.Where(x => x.CharityAccountId == charityAccountId);
                }
                if (amount != 0)
                {
                    query = query.Where(x => x.Amount == amount);
                }
                if (isconfirm != null)
                {
                    query = query.Where(x => x.IsConfirm == isconfirm);
                }
                if (!string.IsNullOrEmpty(issueTracking))
                {
                    query = query.Where(x => x.IssueTracking.Contains(issueTracking));
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
                if (!lastFourDigits.IsNullOrEmpty())
                {
#pragma warning disable CS8602
                    query = query.Where(x => x.LastFourDigits.Contains(lastFourDigits));
#pragma warning restore CS8602
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


        public IQueryable<CharityDeposit> SpecificationGetData(
            ClaimsPrincipal USER,
            string currentUserId,
            List<string>? userIdentityId = null,
            List<string>? helper = null,
            List<string>? foreman = null,
            List<string>? clerk = null,
            List<int>? charityAccount = null,
            double amount = 0,
            bool? isConfirm = null,
            string issueTracking = "",
            DateTime? registerDateFrom = null,
            DateTime? registerDateTo = null,
            DateTime? documentRegisterDateTime = null,
            string lastFourDigits = "",
            int? id = null,
            ConfirmType? confirmType = null
            )
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation();
            resultStatusOperation.IsSuccessed = true;
            resultStatusOperation.Message = "عملیات با موفقیت انجام شد.";
            resultStatusOperation.Type = MessageTypeResult.Success;
            try
            {

                IQueryable<CharityDeposit> query = _charityDepositRepository.GetAll().Include(x => x.CharityAccount)
                    .Where(x => !x.IsDelete );

                if (USER.IsInRole(RoleName.Admin.ToString()))
                {
                    query = query.Where(x =>
                        x.UserIdentityId == currentUserId ||
                        _charityUserIdentityCharityHelperService.GetAdminAllForeman(currentUserId).Select(y => y.Id).ToList().Contains(x.UserIdentityId) ||
                        _charityUserIdentityCharityHelperService.GetForemanAllClerk(_charityUserIdentityCharityHelperService.GetAdminAllForeman(currentUserId)).Select(y => y.Id).ToList().Contains(x.UserIdentityId)
                    );
                }
                else if (USER.IsInRole(RoleName.Foreman.ToString()))
                {
                    List<string> allClerk = _charityUserIdentityCharityHelperService.GetForemanAllClerk(currentUserId).Select(x => x.Id).ToList();

                    //query = query.Where(x =>x.UserIdentityId==currentUserId
                    //);

                    query = query.Where(x =>
                        allClerk.Contains(x.UserIdentityId) || x.UserIdentityId == currentUserId
                    );

                }
                else if (USER.IsInRole(RoleName.Clerk.ToString()))
                {
                    query = query.Where(x => x.UserIdentityId == currentUserId);
                }

#pragma warning disable CS8604 // Possible null reference argument.
                if (userIdentityId.Any(x => !x.IsNullOrEmpty()))
                {
                    query = query.Where(x => userIdentityId.Contains(x.UserIdentityId));
                }
                if (helper.Any(x => !x.IsNullOrEmpty()))
                {
                    query = query.Where(x => helper.Contains(x.HelperId));
                }

                if (clerk.Any(x => !x.IsNullOrEmpty()))
                {

                }

                if (foreman.Any(x => !x.IsNullOrEmpty()))
                {

                }

                if (charityAccount.Count() != 0)
                {
                    query = query.Where(x => charityAccount.Contains(x.CharityAccountId));
                }
#pragma warning restore CS8604 // Possible null reference argument.
                if (amount != 0)
                {
                    query = query.Where(x => x.Amount == amount);
                }
                if (isConfirm != null)
                {
                    query = query.Where(x => x.IsConfirm == isConfirm.Value);
                }
                if (!string.IsNullOrEmpty(issueTracking))
                {
                    query = query.Where(x => x.IssueTracking.Contains(issueTracking));
                }

                if (!lastFourDigits.IsNullOrEmpty())
                {
                    query = query.Where(x => x.LastFourDigits == lastFourDigits);
                }

                //id != null
                if (id is not null)
                {
                    query = query.Where(x => x.Id == id);
                }
                else
                {
                    query = query.Where(x => !x.IsDone);
                }


                if (confirmType is not null)
                {
                    query = query.Where(x => x.ConfirmType == confirmType.Value);
                }

                if (registerDateFrom != null || registerDateTo != null)
                {
                    DateTime _documentRegisterDateTimeFrom = registerDateFrom != null ? registerDateFrom.Value : new DateTime(1930, 1, 1, 1, 0, 0, 0);
                    DateTime _documentRegisterDateTimeTo = registerDateTo != null ? registerDateTo.Value : new DateTime(DateTime.Now.AddYears(1).Year, 1, 1, 1, 0, 0, 0);
                    query = query.Where(x => x.DocumentRegisterDateTime >= _documentRegisterDateTimeFrom && x.DocumentRegisterDateTime <= _documentRegisterDateTimeTo);
                }

                if (documentRegisterDateTime != null)
                {
                    DateTime documentDateTime = documentRegisterDateTime != null ? documentRegisterDateTime.Value : new DateTime(1930, 1, 1, 1, 0, 0, 0);
                    query = query.Where(x => x.DocumentRegisterDateTime.Date == documentDateTime.Date);
                }


                //query = query.Include(x => x.Helper).Include(x => x.UserIdentity);
                
                return query;
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

        /// <summary>
        /// دریافت واریزی های منشی ها
        /// </summary>
        /// <param name="userIdentities"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public IQueryable<CharityDeposit> GetAllDepositsByUserIdentity(List<UserIdentity> userIdentities)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation();
            resultStatusOperation.IsSuccessed = true;
            resultStatusOperation.Message = "عملیات با موفقیت انجام شد.";
            resultStatusOperation.Type = Enumeration.MessageTypeResult.Success;
            try
            {
                IQueryable<CharityDeposit> query = _charityDepositRepository.GetAll()
                    .Where(x => userIdentities.Select(u => u.Id).Contains(x.UserIdentityId)
                    && !x.IsDelete && x.IsConfirm && !x.IsDone);

                return query;
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

        /// <summary>
        /// دریافت حقوق های منشی های سرکاربران
        /// </summary>
        /// <param name="userIdentities"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public IQueryable<CharityWage> GetAllClerkWagesByFormenUserIdentity(List<UserIdentity> userIdentities)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation();
            resultStatusOperation.IsSuccessed = true;
            resultStatusOperation.Message = "عملیات با موفقیت انجام شد.";
            resultStatusOperation.Type = Enumeration.MessageTypeResult.Success;
            try
            {

                //IQueryable<UserIdentity> clerkUsers = _userIdentityRepository.GetAll()
                //.Where(uClerck => userIdentities.Select(uId => uId.Id).Contains(uClerck.UserRegistrarId));

                IQueryable<CharityWage> query = _charityWageRepository.GetAll().Where(cWage => !cWage.IsUsedForForeman &&

                _userIdentityRepository.GetAll()
                .Where(uClerck => userIdentities.Select(uId => uId.Id)
                .Contains(uClerck.UserRegistrarId))

                .Select(cUser => cUser.Id).Contains(cWage.WageReceiverId));



                return query;
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

        /// <summary>
        /// دریافت واریزیهایی که سود آنها برای حقوق محاسبه شده است
        /// </summary>
        /// <returns></returns>
        public Tuple<IQueryable<CharityDeposit>, ResultStatusOperation> GetDepositsByCharityWageId(int charityWageId = 0)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation();
            resultStatusOperation.IsSuccessed = true;
            resultStatusOperation.Message = "عملیات با موفقیت انجام شد.";
            resultStatusOperation.Type = Enumeration.MessageTypeResult.Success;
            try
            {

                IQueryable<CharityDeposit> query =
                    _charityDepositRepository.GetAll().Where(d =>
                    _charityWageCharityDepositRepository.GetAll().Where(cwcd => !cwcd.IsDelete && cwcd.CharityWageId == charityWageId)
                    .Select(cd => cd.CharityDepositId).Contains(d.Id)
                    ).Include(d=>d.UserIdentity).Include(x => x.Helper).Include(x => x.CharityAccount);

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

        public Tuple<CharityDeposit, ResultStatusOperation> FillModel(CharityDeposit charityDeposit, string date, string time)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation();
            resultStatusOperation.IsSuccessed = true;
            resultStatusOperation.Type = Enumeration.MessageTypeResult.Success;
            try
            {
                charityDeposit.RegisterDate = charityDeposit.RegisterDate == new DateTime() ? DateTime.Now : charityDeposit.RegisterDate;
                charityDeposit.IsDelete = false;
                charityDeposit.LastFourDigits = charityDeposit.LastFourDigits!.ReplacePersianNumberToEnglishNumber();
                charityDeposit.IssueTracking = charityDeposit.IssueTracking.ReplacePersianNumberToEnglishNumber();

                DateTime testDate = date.ToConvertPersianDateToDateTime();
                TimeSpan testTime = TimeSpan.Parse(time);

                charityDeposit.DocumentRegisterDateTime = new DateTime(testDate.Year, testDate.Month, testDate.Day, testTime.Hours, testTime.Minutes, 0);
                charityDeposit.IsConfirm = charityDeposit.IsConfirm != false ? charityDeposit.IsConfirm : false;

                return Tuple.Create(charityDeposit, resultStatusOperation);
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
                    var query = _charityDepositRepository.GetAll()
                        .Include(x => x.Helper)
                        .Include(x => x.UserIdentity)
                        .OrderBy(x => x.Id).ToList();

                    List<SelectListItem> item = query.ConvertAll(x =>
                    {
                        return new SelectListItem()
                        {
                            Text = $"ثبت کننده: {x.UserIdentity.FirstName} {x.UserIdentity.LastName}" +
                            $"مددکار: {x.Helper.FirstName} {x.Helper.LastName}",
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
