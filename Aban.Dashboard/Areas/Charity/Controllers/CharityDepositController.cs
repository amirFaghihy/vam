using Aban.Common;
using Aban.Common.Utility;
using Aban.Domain.Entities;
using Aban.Domain.Enumerations;
using Aban.Service.Interfaces;
using Aban.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static Aban.Domain.Enumerations.Enumeration;

namespace Aban.Dashboard.Areas.Charity.Controllers
{

    //رکوردهایی که تایید شدن ، ویرایش نباید بشن
    //هر منشی- رکورد های خودش میبینه
    //سرکاربر- رکوردهای منشی های خودش و خیرهای خودش
    //مدیر اصلی-رکورد های همه رو میبینه


    [Area("Charity")]
    [Authorize]
    public class CharityDepositController : GenericController
    {

        private readonly IUserIdentityService _userIdentityService;
        private readonly ICharityDepositService _charityDepositService;
        private readonly ICharityAccountService _charityAccountService;
        private readonly ICharityUserIdentityCharityHelperService _charityUserIdentityCharityHelperService;
        private readonly ICharityBankRecordService _charityBankRecordService;
        private readonly ICharityWageService _charityWageService;

        public CharityDepositController(ICharityDepositService charityDepositService,
            ICharityAccountService charityAccountService,
            IUserIdentityService userIdentityService,
            ICharityUserIdentityCharityHelperService charityUserIdentityCharityHelperService,
            ICharityBankRecordService charityBankRecordService,
            ICharityWageService charityWageService)
        {
            _charityBankRecordService = charityBankRecordService;
            _charityDepositService = charityDepositService;
            _charityAccountService = charityAccountService;
            _userIdentityService = userIdentityService;
            _charityUserIdentityCharityHelperService = charityUserIdentityCharityHelperService;
            _charityWageService = charityWageService;
        }


        public async Task<IActionResult> Index(
                List<string>? userIdentityId = null,
                List<string>? helper = null,
                List<string>? foreman = null,
                List<string>? clerk = null,
                List<int>? charityAccount = null,
                double amount = 0,
                bool? isConfirm = null,
                string issueTracking = "",
                string registerDateFrom = "",
                string registerDateTo = "",
                string documentRegisterDateTime = "",
                bool isDesc = true,
                int pageNumber = 1,
                int pageSize = 10,
                string sortColumn = "RegisterDate",
                string lastColumn = "",
                string lastFourDigits = "",
                int? id = null,
                ConfirmType? confirmType = null

                )
        {

            #region selectedValue

            ViewBag.pageNumber = pageNumber;
            ViewBag.pageSize = pageSize;
            ViewBag.sortColumn = sortColumn;
            ViewBag.lastColumn = lastColumn;
            ViewBag.isDesc = isDesc;

            ViewBag.userIdentityId = userIdentityId;
            ViewBag.helper = helper;
            ViewBag.foreman = foreman;
            ViewBag.clerk = clerk;
            ViewBag.charityAccount = charityAccount;
            ViewBag.amount = amount;
            ViewBag.isConfirm = isConfirm;
            ViewBag.issueTracking = issueTracking;
            ViewBag.registerDateFrom = registerDateFrom;
            ViewBag.registerDateTo = registerDateTo;
            ViewBag.documentRegisterDateTime = documentRegisterDateTime;
            ViewBag.lastFourDigits = lastFourDigits;
            ViewBag.confirmType = confirmType;


            //ViewBag.BrandId = brandId;

            //ViewBag.headerSort = headerSort;  ViewBag.sortColumn 
            //ViewBag.lastHeader = lastHeader;  ViewBag.lastColumn
            //ViewBag.isDesc = isDesc;          ViewBag.isDesc 
            if (ViewBag.lastColumn == ViewBag.sortColumn)
            {
                ViewBag.isDesc = isDesc == true ? false : true;
                isDesc = ViewBag.isDesc;
            }
            else
            {
                ViewBag.isDesc = isDesc;
            }
            ViewBag.lastColumn = ViewBag.sortColumn;

#pragma warning disable CS8604 // Possible null reference argument.
            FillDropDown("", 0, ActionType.Index, userIdentityId);
#pragma warning restore CS8604 // Possible null reference argument.
            #endregion


            #region DateTime Convertor

            DateTime? _registerDateFrom = null;
            DateTime? _registerDateTo = null;
            DateTime? _documentRegisterDateTime = null;


            if (!string.IsNullOrEmpty(registerDateFrom))
            {
                _registerDateFrom = await registerDateFrom.ToConvertPersianDateToDateTimeAsync(DateTimeFormat.yyyy_mm_dd, DateTimeSpiliter.slash);
            }
            if (!string.IsNullOrEmpty(registerDateTo))
            {
                _registerDateTo = await registerDateTo.ToConvertPersianDateToDateTimeAsync(DateTimeFormat.yyyy_mm_dd, DateTimeSpiliter.slash);
            }
            if (!string.IsNullOrEmpty(documentRegisterDateTime))
            {
                _documentRegisterDateTime = await documentRegisterDateTime.ToConvertPersianDateToDateTimeAsync(DateTimeFormat.yyyy_mm_dd, DateTimeSpiliter.slash);
            }

            #endregion

            return View(
            _charityDepositService.Pagination(_charityDepositService.SpecificationGetData(User, GetUserId(), userIdentityId, helper,
                foreman, clerk, charityAccount ?? new List<int>(), amount, isConfirm, issueTracking, _registerDateFrom, _registerDateTo, _documentRegisterDateTime, lastFourDigits, id, confirmType), false, pageNumber, pageSize, isDesc, sortColumn));
        }

        [HttpGet]
        public IActionResult Create()
        {
            FillDropDown();

            return View(new CharityDeposit() { UserIdentityId = GetUserId() });
        }

        private async Task<ResultStatusOperation> CheckConfirm(CharityDeposit model)
        => await _charityBankRecordService.UpdateDepositWithFindBankRecordAsync(model);

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CharityDeposit model, string DocumentRegisterDate, string DocumentRegisterTime)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation();
            resultStatusOperation.IsSuccessed = true;
            resultStatusOperation.Message = "عملیات با موفقیت انجام شد.";
            resultStatusOperation.Type = MessageTypeResult.Success;

            try
            {
                Tuple<CharityDeposit, ResultStatusOperation> result =
                    _charityDepositService.FillModel(model, DocumentRegisterDate, DocumentRegisterTime);


                // اگر رکورد تکراری نبود،رکورد جدید را ذخیره میکند
                if (_charityDepositService.SpecificationGetData(GetUserId(), result.Item1.HelperId, result.Item1.CharityAccountId, result.Item1.Amount, null, result.Item1.IssueTracking, "", result.Item1.DocumentRegisterDateTime, result.Item1.DocumentRegisterDateTime, null, null, (result.Item1.LastFourDigits.IsNullOrEmpty() ? "" : result.Item1.LastFourDigits!)).Item1.FirstOrDefault() != null)
                {
                    resultStatusOperation.Message = ("واریزی تکراری میباشد !");
                    resultStatusOperation.Type = MessageTypeResult.Warning;

                    SetMessage(resultStatusOperation);
                    return RedirectToAction(nameof(Index));
                }

                Tuple<CharityDeposit, ResultStatusOperation> resultAdd = 
                    resultAdd = await _charityDepositService.Insert(fillControllerInfo(new List<string> { "UserIdentity", "Helper", "CharityAccount" }), result.Item1);


                SetMessage(resultAdd.Item2);
                switch (resultAdd.Item2.Type)
                {
                    case MessageTypeResult.Success:
                        break;


                    case MessageTypeResult.Danger:
                    case MessageTypeResult.Warning:
                        FillDropDown(resultAdd.Item1.HelperId, resultAdd.Item1.CharityAccountId);
                        return View(resultAdd.Item1);

                    default:
                        return RedirectToAction(nameof(Index));
                }

                var resultConfirm = await this.CheckConfirm(resultAdd.Item1);
                SetMessage(resultConfirm.Type == MessageTypeResult.Info ? resultConfirm : resultAdd.Item2);
                return RedirectToAction(nameof(Index));

            }
            catch
            {
                return RedirectToAction(nameof(Index));

            }
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {

            Tuple<CharityDeposit, ResultStatusOperation> resultFind = await _charityDepositService.Find(id);
            switch (resultFind.Item2.Type)
            {
                case MessageTypeResult.Success:

                    //ثبت کننده یا ادمین
                    if (resultFind.Item1.UserIdentityId == GetUserId() || User.IsInRole(RoleName.Admin.ToString()) ||
                        (
                        User.IsInRole(RoleName.Foreman.ToString()) &&
                        await _charityUserIdentityCharityHelperService.IsUserSavedByUserId(GetUserId(), resultFind.Item1.UserIdentityId)
                        )
                        )

                    {
                        FillDropDown(resultFind.Item1.HelperId, resultFind.Item1.CharityAccountId);
                        TempData["IsConfirm"] = resultFind.Item1.IsConfirm;
                        return View(resultFind.Item1);
                    }
                    else
                    {
                        SetMessage(new ResultStatusOperation() { IsSuccessed = false, Type = MessageTypeResult.Warning, Message = "به این رکورد دسترسی ندارید" });
                        return RedirectToAction(nameof(Index));
                    }


                case MessageTypeResult.Danger:
                case MessageTypeResult.Warning:
                    SetMessage(resultFind.Item2);
                    return RedirectToAction(nameof(Index));

                default:
                    SetMessage(resultFind.Item2);
                    return RedirectToAction(nameof(Index));
            }

        }


        [HttpPost]
        public async Task<IActionResult> Edit(
            CharityDeposit model,
            string DocumentRegisterDate,
            string DocumentRegisterTime)
        {
            try
            {
                var isConfirm = TempData.Peek("IsConfirm") as bool?;
                //اگر رکورد تایید شده بود قابل ویرایش نیست دیگر
                if (isConfirm.HasValue && isConfirm.Value)
                {
                    FillDropDown(model.HelperId, model.CharityAccountId);
                    SetMessage(new ResultStatusOperation() { IsSuccessed = false, Type = MessageTypeResult.Warning, Message = "رکورد قابل ویرایش نیست" });
                    return View(model);
                }

                //اگر ادمین یا خود ثبت کننده ویرایش کند
                if (User.IsInRole(RoleName.Admin.ToString()) || model.UserIdentityId == GetUserId())
                {

                    Tuple<CharityDeposit, ResultStatusOperation> result =
                        _charityDepositService.FillModel(model, DocumentRegisterDate, DocumentRegisterTime);

                    Tuple<CharityDeposit, ResultStatusOperation> resultUpdate =
                        await _charityDepositService.Update(fillControllerInfo(new List<string> { "UserIdentity", "Helper", "CharityAccount" }), result.Item1);

                    SetMessage(resultUpdate.Item2);
                    switch (resultUpdate.Item2.Type)
                    {
                        case MessageTypeResult.Success:
                            var resultConfirm = await this.CheckConfirm(resultUpdate.Item1);
                            SetMessage(resultConfirm.Type == MessageTypeResult.Info ? resultConfirm : resultUpdate.Item2);
                            return RedirectToAction(nameof(Index));

                        case MessageTypeResult.Danger:
                        case MessageTypeResult.Warning:
                            FillDropDown(resultUpdate.Item1.HelperId, resultUpdate.Item1.CharityAccountId);
                            return View(resultUpdate.Item1);

                        default:
                            return RedirectToAction(nameof(Index));
                    }
                }

                FillDropDown(model.HelperId, model.CharityAccountId);
                SetMessage(new ResultStatusOperation() { IsSuccessed = false, Type = MessageTypeResult.Warning, Message = "رکورد قابل ویرایش نیست" });
                return View(model);
                //model.UserIdentityId = GetUserId();


            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ConfirmDeposit(int? deposithiddenid, int? bankRecordId, bool isForceToConfirm = false)
        {
            ResultStatusOperation _resultStatusOperation = new()
            {
                IsSuccessed = false,
                Title = "خطا",
                Message = "خطا در تایید رکورد",
                Type = MessageTypeResult.Danger,
                ErrorException = null
            };
            if (deposithiddenid is null)
            {
                _resultStatusOperation.Message = "شناسه واریزی اجباری است";

                return Json(new { data = _resultStatusOperation });
            }

            if (bankRecordId is null)
            {
                _resultStatusOperation.Message = "شناسه سوابق بانکی اجباری است";

                return Json(new { data = _resultStatusOperation });
            }

            var resultFind = await _charityDepositService.Find(deposithiddenid.Value);
            var resultFindBank = await _charityBankRecordService.Find(bankRecordId.Value);
            switch (resultFindBank.Item2.Type)
            {
                case MessageTypeResult.Success:
                    if (resultFindBank.Item1.IsInUse)
                    {
                        _resultStatusOperation.Message = "شناسه سوابق بانکی قبلا تایید شده";
                        return Json(new { data = _resultStatusOperation });
                    }
                    break;
                case MessageTypeResult.Danger:
                case MessageTypeResult.Warning:
                case MessageTypeResult.Info:
                default:
                    SetMessage(resultFindBank.Item2);
                    _resultStatusOperation.Message = "شناسه سوابق بانکی یاقت نشد";
                    return Json(new { data = _resultStatusOperation });
            }
            switch (resultFind.Item2.Type)
            {
                case MessageTypeResult.Success:

                    if ((resultFindBank.Item1.CharityAccountId == resultFind.Item1.CharityAccountId && resultFindBank.Item1.Creditor == resultFind.Item1.Amount &&
                        resultFindBank.Item1.DocumentRegisterDateTime.Date == resultFind.Item1.DocumentRegisterDateTime.Date) || isForceToConfirm)
                    {


                        resultFind.Item1.IsConfirm = true;
                        resultFind.Item1.CharityBankRecordId = resultFindBank.Item1.Id;
                        resultFind.Item1.ConfirmType = ConfirmType.By_Admin;
                        resultFind.Item1.ConfirmDate = DateTime.Now;
                        resultFindBank.Item1.IsInUse = true;

                        await _charityDepositService.Update(true, resultFind.Item1, false);
                        await _charityBankRecordService.Update(true, resultFindBank.Item1);


                        _resultStatusOperation.Title = "عملیات با موفقیت انجام شد";
                        _resultStatusOperation.Message = "عملیات با موفقیت انجام شد";
                        _resultStatusOperation.Type = MessageTypeResult.Success;
                        _resultStatusOperation.IsSuccessed = true;

                        return Json(new { data = _resultStatusOperation });
                    }
                    else
                    {
                        _resultStatusOperation.Message = "حساب، وجه یا تاریخ تطابق ندارد";
                        _resultStatusOperation.Type = MessageTypeResult.Warning;
                        return Json(new { data = _resultStatusOperation });
                    }


                case MessageTypeResult.Danger:
                case MessageTypeResult.Warning:
                case MessageTypeResult.Info:
                default:
                    _resultStatusOperation.Message = "شناسه واریزی یاقت نشد";
                    return Json(new { data = _resultStatusOperation });
            }
        }
        //public async Task<IActionResult> Delete()
        //{
        //    return null;
        //}

        [HttpGet("/find-foreman")]
        public async Task<IActionResult> FindForeman(int depositId)
        {
            var resultDetail = await _charityDepositService.GetDetailByDepositId(depositId);

            string name = $"سرکاربر: {resultDetail.foremanName}";
            return Json(new { data = name });
        }

        /// <summary>
        /// اطلاعات مربوط به منشی و خیر را باز میگرداند
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns></returns>
        [HttpPost("/find-user-data")]
        public IActionResult FindUserData(string stringUserIds)
        {

            if (stringUserIds.IsNullOrEmpty())
            {
                return Json(null);
            }

#pragma warning disable CS8604
            List<string> userIds = new List<string>(JsonConvert.DeserializeObject<string[]>(stringUserIds));
#pragma warning restore CS8604

            List<UserIdentity> userData = _userIdentityService.SpecificationGetData(userIds).Result.Item1.ToList();

            List<UserIdentityViewModel> resultViewModel = userData.Select(x => new UserIdentityViewModel
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                FatherName = x.FatherName
            }).ToList();


            return Json(new { data = resultViewModel });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            Tuple<CharityDeposit, ResultStatusOperation> resultFind = await _charityDepositService.Find(id);
            switch (resultFind.Item2.Type)
            {
                case MessageTypeResult.Success:

                    //ثبت کننده یا ادمین
                    if (resultFind.Item1.UserIdentityId == GetUserId() || User.IsInRole(RoleName.Admin.ToString())
                       )

                    {
                        var resultDelete = await _charityDepositService.DeleteLogic(resultFind.Item1);
                        switch (resultDelete.Type)
                        {
                            case MessageTypeResult.Success:
                            case MessageTypeResult.Danger:
                            case MessageTypeResult.Warning:
                            case MessageTypeResult.Info:
                            default:
                                SetMessage(resultDelete);
                                return RedirectToAction(nameof(Index));

                        }

                    }
                    else
                    {
                        SetMessage(new ResultStatusOperation() { IsSuccessed = false, Type = MessageTypeResult.Warning, Message = "به این رکورد دسترسی ندارید" });
                        return RedirectToAction(nameof(Index));
                    }


                case MessageTypeResult.Danger:
                case MessageTypeResult.Warning:
                    SetMessage(resultFind.Item2);
                    return RedirectToAction(nameof(Index));

                default:
                    SetMessage(resultFind.Item2);
                    return RedirectToAction(nameof(Index));
            }
        }


        /// <summary>
        /// دریافت واریزیهایی که سود آنها برای حقوق محاسبه شده است
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult CharityWageDeposits(int wageId)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation();
            resultStatusOperation.IsSuccessed = true;
            resultStatusOperation.Message = "عملیات با موفقیت انجام شد.";
            resultStatusOperation.Type = Enumeration.MessageTypeResult.Success;
            try
            {
                if (!User.IsInRole("Admin"))
                {
                    CharityWage? charityWage = _charityWageService.GetAll()
                        .Where(x => !x.IsDelete && x.Id == wageId && x.WageReceiverId == GetUserId()).FirstOrDefault();
                    if (charityWage == null)
                    {
                        resultStatusOperation.Message = "شما به این صفحه دسترسی ندارید !!!";
                        resultStatusOperation.Type = Enumeration.MessageTypeResult.Warning;
                        return RedirectToAction("Index", "Home", new { area = "" });
                    }
                }

                Tuple<IQueryable<CharityDeposit>, ResultStatusOperation> result =
                    _charityDepositService.GetDepositsByCharityWageId(wageId);

                return View("Index", _charityDepositService.Pagination(result.Item1, true, 1, int.MaxValue, true, "RegisterDate"));
            }
            catch (Exception exception)
            {
                SetMessageException(new ResultStatusOperation("", "", MessageTypeResult.Danger, false, exception), MessageTypeActionMethod.Index);
                return RedirectToAction("ShowException", "Error", new { area = "" });
            }
        }


        private void FillDropDown(
            string helperId = "",
            int accountId = 0,
            ActionType? actionType = null,
            List<string>? userIdentityId = null)
        {

            if (actionType.HasValue && actionType.Value == ActionType.Index)
            {
                if (User.IsInRole("Admin"))
                {

                    ViewBag.listUserIdentity = _charityDepositService.SelectListItems(GetUserId(),
                        ViewBag.userIdentityId as List<string> ?? new List<string>(),
                        new List<IQueryable<UserIdentity>>()
                        {
                            //گرفتن تمام سرکاربرهای یک ادمین
                            _charityUserIdentityCharityHelperService.GetAdminAllForeman(GetUserId()),

                            //گرفتن منشی های تمام سرکاربرهای مربوط به یک ادمین
                            _charityUserIdentityCharityHelperService.GetForemanAllClerk(
                                _charityUserIdentityCharityHelperService.GetAdminAllForeman(GetUserId()))
                        }, "---انتخاب---"
                    );



                    ViewBag.listCharityHelper = _charityDepositService.SelectListItems("",
                        ViewBag.helper as List<string> ?? new List<string>(),
                        new List<IQueryable<UserIdentity>>()
                        {
                            //گرفتن تمام مددکار های یک ادمین
                            _charityUserIdentityCharityHelperService.GetAdminAllHelper(GetUserId()),

                            //گرفتن مددکارهای مربوط به سرکاربرهای یک ادمین
                            _charityUserIdentityCharityHelperService.GetForemanAllHelper(
                                _charityUserIdentityCharityHelperService.GetAdminAllForeman(GetUserId())),

                            //گرفتن مددکارهای مربوط به یک ادمین در ارتباط با منشی های سرکاربرهای یک ادمین
                            _charityUserIdentityCharityHelperService.GetClerkAllHelper(
                                _charityUserIdentityCharityHelperService.GetForemanAllHelper(
                                    _charityUserIdentityCharityHelperService.GetAdminAllForeman(GetUserId())))


                        }, "---انتخاب---"
                    );

                    //ViewBag.listUserIdentity = _charityUserIdentityCharityHelperService.ReadAll(new List<IQueryable<UserIdentity>>()
                    //{
                    ////گرفتن تمام سرکاربرهای یک ادمین
                    //_charityUserIdentityCharityHelperService.GetAdminAllForeman(GetUserId()),

                    ////گرفتن منشی های تمام سرکاربرهای مربوط به یک ادمین
                    //_charityUserIdentityCharityHelperService.GetForemanAllClerk(_charityUserIdentityCharityHelperService.GetAdminAllForeman(GetUserId()))
                    //}, GetUserId(), x => x.FirstName + x.LastName, x => x.Id, ViewBag.userIdentityId as List<string> ?? new List<string>(), "--- انتخاب ---");



                    //ViewBag.listCharityHelper = _charityUserIdentityCharityHelperService.ReadAll(new List<IQueryable<UserIdentity>>()
                    //{
                    //    //گرفتن تمام مددکار های یک ادمین
                    //    _charityUserIdentityCharityHelperService.GetAdminAllHelper(GetUserId()),

                    //    //گرفتن مددکارهای مربوط به سرکاربرهای یک ادمین
                    //    _charityUserIdentityCharityHelperService.GetForemanAllHelper(_charityUserIdentityCharityHelperService.GetAdminAllForeman(GetUserId())),

                    //    //گرفتن مددکارهای مربوط به یک ادمین در ارتباط با منشی های سرکاربرهای یک ادمین
                    //    _charityUserIdentityCharityHelperService.GetClerkAllHelper(_charityUserIdentityCharityHelperService.GetForemanAllHelper(_charityUserIdentityCharityHelperService.GetAdminAllForeman(GetUserId())))


                    //}, null, x => x.FirstName + x.LastName + x.FatherName, x => x.Id, ViewBag.helper as List<string> ?? new List<string>(), "--- انتخاب ---");

                }


                else if (User.IsInRole(RoleName.Foreman.ToString()))
                {
                    ViewBag.listUserIdentity = _charityDepositService.SelectListItems(GetUserId(),
                        ViewBag.userIdentityId as List<string> ?? new List<string>(),
                        new List<IQueryable<UserIdentity>>()
                        {
                            _charityUserIdentityCharityHelperService.GetForemanAllClerk(GetUserId())
                        }, "---انتخاب---"
                    );

                    //ViewBag.listUserIdentity = _charityUserIdentityCharityHelperService.ReadAll(new List<IQueryable<UserIdentity>>()
                    //{
                    //    _charityUserIdentityCharityHelperService.GetForemanAllClerk(GetUserId())
                    //}, GetUserId(), x => x.FirstName + x.LastName + x.FatherName , x => x.Id, ViewBag.userIdentityId as List<string> ?? new List<string>(), "--- انتخاب ---");

                    ViewBag.listCharityHelper = _charityDepositService.SelectListItems("",
                        ViewBag.helper as List<string> ?? new List<string>(),
                        new List<IQueryable<UserIdentity>>()
                        {

                            //گرفتن مددکار های مربوط به سرکاربرهای یک ادمین
                            _charityUserIdentityCharityHelperService.GetForemanAllHelper(GetUserId()),

                            //گرفتن مددکارهای مربوط به یک ادمین در ارتباط با منشی های سرکاربرهای یک ادمین
                            _charityUserIdentityCharityHelperService.GetClerkAllHelper(
                                _charityUserIdentityCharityHelperService.GetForemanAllHelper(GetUserId()))


                        }, "انتخاب---"
                    );

                    //ViewBag.listCharityHelper = _charityUserIdentityCharityHelperService.ReadAll(new List<IQueryable<UserIdentity>>()
                    //{

                    //    //گرفتن مددکار های مربوط به سرکاربرهای یک ادمین
                    //    _charityUserIdentityCharityHelperService.GetForemanAllHelper(GetUserId()),

                    //    //گرفتن مددکارهای مربوط به یک ادمین در ارتباط با منشی های سرکاربرهای یک ادمین
                    //    _charityUserIdentityCharityHelperService.GetClerkAllHelper(_charityUserIdentityCharityHelperService.GetForemanAllHelper(GetUserId()))


                    //}, null, x => x.FirstName + x.LastName + x.FatherName, x => x.Id, ViewBag.helper as List<string> ?? new List<string>(), "--- انتخاب ---");

                }

                else if (User.IsInRole(RoleName.Clerk.ToString()))
                {
                    ViewBag.listCharityHelper = _charityDepositService.SelectListItems("",
                        ViewBag.userIdentityId as List<string> ?? new List<string>(),
                        new List<IQueryable<UserIdentity>>()
                        {
                            _charityUserIdentityCharityHelperService.GetClerkAllHelper(GetUserId())
                        }, "---انتخاب---"
                    );

                    //ViewBag.listCharityHelper = _charityUserIdentityCharityHelperService.ReadAll(new List<IQueryable<UserIdentity>>()
                    //{
                    //    _charityUserIdentityCharityHelperService.GetClerkAllHelper(GetUserId())
                    //}, null, x => x.FirstName + x.LastName, x => x.Id, ViewBag.userIdentityId as List<string> ?? new List<string>() { helperId }, "--- انتخاب ---");

                }

            }
            else
            {
                //if(User.IsInRole(RoleName.Admin.ToString()))
                //{

                //}   
                //else if(User.IsInRole(RoleName.Foreman.ToString()))
                //{

                //}
                //else if (User.IsInRole(RoleName.Clerk.ToString()))
                //{

                //}

                ViewBag.listCharityHelper = _charityDepositService.SelectListItems("", new List<string>() { helperId },
                    new List<IQueryable<UserIdentity>>()
                    {
                        _charityUserIdentityCharityHelperService.GetClerkAllHelper(GetUserId())
                    }, "---انتخاب---");

                //ViewBag.listCharityHelper = _charityUserIdentityCharityHelperService.ReadAll(new List<IQueryable<UserIdentity>>()
                //    {
                //        _charityUserIdentityCharityHelperService.GetClerkAllHelper(GetUserId())
                //    }, null, x => x.FirstName + x.LastName + x.FatherName, x => x.Id, new List<string>() { helperId }, "--- انتخاب ---");


            }

            //TODO: فقط باید لیست اکانت هایی که فعال هستند را بیاورد
            ViewBag.listCharityAccount = _charityAccountService.SelectListItem(
                _charityAccountService.GetAll().Where(x => x.IsVisible && !x.IsDelete), x => x.Title, x => x.Id, selectedValue: accountId.ToString(), "--- انتخاب ---"
                );


            ConfirmType? confirmType = ViewBag.confirmType ?? null;
            ViewBag.listConfirmType = GenericEnumList.GetSelectValueEnum<ConfirmType>(confirmType.HasValue ? confirmType.Value.ToString() : "");
        }
    }
}