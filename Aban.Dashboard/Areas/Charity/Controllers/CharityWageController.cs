using Aban.Common;
using Aban.Common.HtmlHelper;
using Aban.Common.Utility;
using Aban.Domain.Entities;
using Aban.Domain.Enumerations;
using Aban.Service.Interfaces;
using Aban.Service.Services;
using Aban.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.Collections.Generic;
using static Aban.Domain.Enumerations.Enumeration;

namespace Aban.Dashboard.Areas.Charity.Controllers
{
    [Area("Charity")]
    public class CharityWageController : GenericController
    {
        #region constructor

        private readonly ICharityWageService charityWageService;
        private readonly IUserIdentityService userIdentityService;
        private readonly ICharityDepositService charityDepositService;
        private readonly ICharityAdditionService charityAdditionService;
        private readonly ICharityDeducationService charityDeducationService;
        private readonly ICharityLoanService charityLoanService;
        private readonly ICharityLoanInstallmentsService charityLoanInstallmentsService;


        public CharityWageController(
            ICharityWageService charityWageService,
            ICharityDepositService charityDepositService,
            IUserIdentityService userIdentityService,
            ICharityLoanService charityLoanService,
            ICharityAdditionService charityAdditionService,
            ICharityDeducationService charityDeducationService,
            ICharityLoanInstallmentsService charityLoanInstallmentsService
            )
        {
            this.userIdentityService = userIdentityService;

            this.charityWageService = charityWageService;
            this.charityDepositService = charityDepositService;
            this.charityAdditionService = charityAdditionService;
            this.charityDeducationService = charityDeducationService;
            this.charityLoanService = charityLoanService;
            this.charityLoanInstallmentsService = charityLoanInstallmentsService;
        }

        #endregion

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index(
            string userIdentityId = "",
            string wageReceiverId = "",
            string accountNumber = "",
            string description = "",
            float? fixedSalary = null,
            byte? percentSalary = null,
            string registerDateFrom = "",
            string registerDateTo = "",
            int pageNumber = 1,
            int pageSize = 10,
            string sortColumn = "RegisterDate",
            string lastColumn = "",
            bool isDesc = true)
        {
            try
            {


                #region selectedValue

                ViewBag.pageNumber = pageNumber;
                ViewBag.pageSize = pageSize;
                ViewBag.sortColumn = sortColumn;
                ViewBag.lastColumn = lastColumn;
                ViewBag.isDesc = isDesc;
                ViewBag.userIdentityId = userIdentityId;
                ViewBag.wageReceiverId = wageReceiverId;
                ViewBag.accountNumber = accountNumber;
                ViewBag.description = description;
                ViewBag.registerDateFrom = registerDateFrom;
                ViewBag.registerDateTo = registerDateTo;
                ViewBag.fixedSalary = fixedSalary;
                ViewBag.percentSalary = percentSalary;


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

                FillDropDown();
                #endregion


                #region DateTime Convertor

                DateTime? _registerDateFrom = null;
                DateTime? _registerDateTo = null;


                if (!string.IsNullOrEmpty(registerDateFrom))
                {
                    _registerDateFrom = await registerDateFrom.ToConvertPersianDateToDateTimeAsync(DateTimeFormat.yyyy_mm_dd, DateTimeSpiliter.slash);
                }
                if (!string.IsNullOrEmpty(registerDateTo))
                {
                    _registerDateTo = await registerDateTo.ToConvertPersianDateToDateTimeAsync(DateTimeFormat.yyyy_mm_dd, DateTimeSpiliter.slash);
                }

                #endregion

                Tuple<IQueryable<CharityWage>, ResultStatusOperation> result =
                    charityWageService.SpecificationGetData(userIdentityId, wageReceiverId, accountNumber, description, fixedSalary, percentSalary, _registerDateFrom, _registerDateTo);

                return View(charityWageService.Pagination(result.Item1, true, pageNumber, pageSize, isDesc, sortColumn));
                //return View(result.Item1.ToPagedList(pageNumber, pageSize));
            }
            catch (Exception exception)
            {
                SetMessageException(new ResultStatusOperation("", "", MessageTypeResult.Danger, false, exception), MessageTypeActionMethod.Index);
                return RedirectToAction("ShowException", "Error", new { area = "" });
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            try
            {
                FillDropDown();
                return View(new CharityWage());
            }
            catch (Exception exception)
            {
                SetMessageException(new ResultStatusOperation("", "", MessageTypeResult.Danger, false, exception), MessageTypeActionMethod.Index);
                return RedirectToAction("ShowException", "Error", new { area = "" });
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(
            CharityWage model,
            string wageDateFromString = "",
            string wageDateToString = "")
        {
            try
            {

                #region DateTime Convertor

                if (!string.IsNullOrEmpty(wageDateFromString) && !string.IsNullOrEmpty(wageDateToString))
                {
                    DateTime _wageDateFromString = wageDateFromString.ToConvertPersianDateToDateTime(DateTimeFormat.yyyy_mm_dd, DateTimeSpiliter.slash);
                    TimeSpan _wageDateFromTime = TimeSpan.Zero;
                    model.WageDateFrom = _wageDateFromString.MergeDateAndTime(_wageDateFromTime);

                    DateTime _wageDateToString = wageDateToString.ToConvertPersianDateToDateTime(DateTimeFormat.yyyy_mm_dd, DateTimeSpiliter.slash);
                    TimeSpan _wageDateToTime = TimeSpan.Zero;
                    model.WageDateTo = _wageDateToString.MergeDateAndTime(_wageDateToTime);
                }
                else
                {
                    return RedirectToAction(nameof(Index));
                }

                #endregion


                Tuple<CharityWage, ResultStatusOperation> resultFillModel =
                    charityWageService.FillModel(model);

                resultFillModel.Item1.UserIdentityId = GetUserId();

                Tuple<CharityWage, ResultStatusOperation> resultAdd =
                    await charityWageService.Insert(fillControllerInfo("UserIdentityId"), resultFillModel.Item1);

                SetMessage(resultAdd.Item2);
                switch (resultAdd.Item2.Type)
                {
                    case MessageTypeResult.Success:
                        return RedirectToAction(nameof(Index));

                    case MessageTypeResult.Danger:
                    case MessageTypeResult.Warning:
                        FillDropDown();
                        return View(resultAdd.Item1);

                    default:
                        return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception exception)
            {
                SetMessageException(new ResultStatusOperation("", "", MessageTypeResult.Danger, false, exception), MessageTypeActionMethod.Index);
                return RedirectToAction("ShowException", "Error");
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    SetMessageEditnotFound();
                    return RedirectToAction("Index");
                }
                Tuple<CharityWage, ResultStatusOperation> resultFindModel = await charityWageService.Find(id.Value);

                SetMessage(resultFindModel.Item2);
                switch (resultFindModel.Item2.Type)
                {
                    case MessageTypeResult.Success:
                        ViewBag.wageReceiverId = resultFindModel.Item1.WageReceiverId;
                        //Pass Parameter like ViewBag.cityId = resultFindModel.Item1.CityId;
                        FillDropDown();
                        return View(resultFindModel.Item1);

                    case MessageTypeResult.Danger:
                        SetMessageException(resultFindModel.Item2, MessageTypeActionMethod.EditGet);
                        return RedirectToAction("ShowException", "Error");

                    case MessageTypeResult.Warning:
                        return RedirectToAction(nameof(Index));

                    default:
                        return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception exception)
            {
                SetMessageException(new ResultStatusOperation("", "", MessageTypeResult.Danger, false, exception), MessageTypeActionMethod.Index);
                return RedirectToAction("ShowException", "Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(
            CharityWage model,
            string wageDateFromString = "",
            string wageDateToString = "")
        {
            try
            {

                #region DateTime Convertor

                if (!string.IsNullOrEmpty(wageDateFromString) && !string.IsNullOrEmpty(wageDateToString))
                {
                    DateTime _wageDateFromString = wageDateFromString.ToConvertPersianDateToDateTime(DateTimeFormat.yyyy_mm_dd, DateTimeSpiliter.slash);
                    TimeSpan _wageDateFromTime = TimeSpan.Zero;
                    model.WageDateFrom = _wageDateFromString.MergeDateAndTime(_wageDateFromTime);

                    DateTime _wageDateToString = wageDateToString.ToConvertPersianDateToDateTime(DateTimeFormat.yyyy_mm_dd, DateTimeSpiliter.slash);
                    TimeSpan _wageDateToTime = TimeSpan.Zero;
                    model.WageDateTo = _wageDateToString.MergeDateAndTime(_wageDateToTime);
                }
                else
                {
                    return RedirectToAction(nameof(Index));
                }

                #endregion


                model.UserIdentityId = GetUserId();
                model.ModifiedDate = DateTime.Now;

                Tuple<CharityWage, ResultStatusOperation> resultEdit = await charityWageService.Update(fillControllerInfo("UserIdentityId"), model);

                SetMessage(resultEdit.Item2);
                switch (resultEdit.Item2.Type)
                {
                    case MessageTypeResult.Success:
                        return RedirectToAction(nameof(Index));

                    case MessageTypeResult.Danger:
                        SetMessageException(resultEdit.Item2, MessageTypeActionMethod.EditPost);
                        return RedirectToAction("ShowException", "Error");

                    case MessageTypeResult.Warning:
                        FillDropDown();
                        return View(resultEdit.Item1);

                    default:
                        FillDropDown();
                        return View(resultEdit.Item1);

                }
            }
            catch (Exception exception)
            {
                SetMessageException(new ResultStatusOperation("", "", MessageTypeResult.Danger, false, exception), MessageTypeActionMethod.Index);
                return RedirectToAction("ShowException", "Error");
            }
        }

        #region محاسبه حقوق ماهانه کاربران سیستم

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult GetDataForSalaryCalculation()
        {
            FillDropDownRoles();
            return View();
        }

        /// <summary>
        /// لیست کاربران بر اساس رول انتخاب شده
        /// و مقدار حقوق ثابت و درصد و جمع واریزیهای تأیید شده را باز میگرداند
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="startDateString"></param>
        /// <param name="endDateString"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        [ActionName("GetSalaryCalculation")]
        public IActionResult SalaryCalculation(
            string roleId = "",
            string startDateString = "",
            string endDateString = "")
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation()
            {
                IsSuccessed = true,
                Type = MessageTypeResult.Success,
                Message = "عملیات با موفقیت انجام شد."
            };

            try
            {
                if (roleId.IsNullOrEmpty() || startDateString.IsNullOrEmpty() || endDateString.IsNullOrEmpty())
                {
                    resultStatusOperation.IsSuccessed = false;
                    resultStatusOperation.Type = MessageTypeResult.Warning;
                    resultStatusOperation.Message = "اطلاعات کامل نیست !!!";

                    SetMessage(resultStatusOperation);
                    return RedirectToAction(nameof(GetDataForSalaryCalculation));
                }

                #region DateTime Convertor

                DateTime? _startDate = null;
                DateTime? _endDate = null;


                if (!string.IsNullOrEmpty(startDateString))
                {
                    DateTime _timeForAction = startDateString.ToConvertPersianDateToDateTime(DateTimeFormat.yyyy_mm_dd, DateTimeSpiliter.slash);
                    TimeSpan _timeForActionTime = TimeSpan.Zero; //startDateString.ToConvertStringToTime()
                    _startDate = _timeForAction.MergeDateAndTime(_timeForActionTime);
                }
                if (!string.IsNullOrEmpty(endDateString))
                {
                    DateTime _timeForAction = endDateString.ToConvertPersianDateToDateTime(DateTimeFormat.yyyy_mm_dd, DateTimeSpiliter.slash);
                    TimeSpan _timeForActionTime = new TimeSpan(23, 59, 59); //endDateString.ToConvertStringToTime()
                    _endDate = _timeForAction.MergeDateAndTime(_timeForActionTime);
                }

                #endregion

                List<RoleName> roleName = userIdentityService.FindRoleName(new List<string>() { roleId });

                List<UserIdentity> users = userIdentityService.GetAllUserInRole(roleName).ToList();


                Tuple<List<CharityUserIdentityDepositViewModel>, ResultStatusOperation> charityUserIdentityDepositViewModel;

                // دریافت واریزیهای منشی ها
                if (roleName.FirstOrDefault() == RoleName.Clerk)
                {
                    List<CharityDeposit> deposits = charityDepositService.GetAllDepositsByUserIdentity(users).ToList();
                    charityUserIdentityDepositViewModel = charityWageService.ConvertModelToViewModel(users, roleName, deposits, null, _startDate, _endDate, roleId);
                }

                // دریافت واریزیهای منشی های سرکاربر
                else
                {
                    List<CharityWage> wages = charityDepositService.GetAllClerkWagesByFormenUserIdentity(users).ToList();
                    charityUserIdentityDepositViewModel = charityWageService.ConvertModelToViewModel(users, roleName, null, wages, _startDate, _endDate, roleId);

                }


                SetMessage(resultStatusOperation);
                if (charityUserIdentityDepositViewModel.Item1.Count() != 0)
                    return View("SalaryCalculation", charityUserIdentityDepositViewModel.Item1);

                resultStatusOperation = new ResultStatusOperation()
                {
                    IsSuccessed = false,
                    Type = MessageTypeResult.Warning,
                    Message = "هیچ رکوردی در این بازه زمانی یافت نشد !!!"
                };

                SetMessage(resultStatusOperation);
                return RedirectToAction(nameof(GetDataForSalaryCalculation));
            }
            catch (Exception exception)
            {
                SetMessageException(new ResultStatusOperation("", "", MessageTypeResult.Danger, false, exception), MessageTypeActionMethod.Index);
                return RedirectToAction("ShowException", "Error");
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        [ActionName("SetSalaryCalculation")]
        public IActionResult SalaryCalculation(
            List<CharityUserIdentityDepositViewModel> charityUserIdentityDepositViewModel,
            DateTime startDate,
            DateTime endDate,
            string roleId = "")
        {

            ResultStatusOperation resultStatusOperation = new ResultStatusOperation()
            {
                IsSuccessed = true,
                Type = MessageTypeResult.Success,
                Message = "عملیات با موفقیت انجام شد."
            };

            try
            {
                List<string> userIds = new List<string>();
                if (charityUserIdentityDepositViewModel.Count() != 0)
                {
                    userIds = charityUserIdentityDepositViewModel.Select(x => x.UserIdentityId).ToList();
                }
                else
                {
                    resultStatusOperation.IsSuccessed = false;
                    resultStatusOperation.Type = MessageTypeResult.Warning;
                    resultStatusOperation.Message = "هیچ رکوردی یافت نشد !!!";

                    SetMessage(resultStatusOperation);
                    return RedirectToAction(nameof(GetDataForSalaryCalculation));
                }

                List<RoleName> roleName = userIdentityService.FindRoleName(new List<string>() { roleId });


                List<UserIdentity> users = userIdentityService.SpecificationGetData(userIds).Result.Item1.ToList();
                // ابتدا مقادیر مربوط حقوق ثابت و درصد کاربران آپدیت شود
                foreach (var item in users)
                {
#pragma warning disable CS8600
                    CharityUserIdentityDepositViewModel singleItem = charityUserIdentityDepositViewModel.FirstOrDefault(x => x.UserIdentityId == item.Id);
#pragma warning restore CS8600

#pragma warning disable CS8602
                    item.FixedSalary = singleItem.FixedSalary;
                    item.PercentSalary = singleItem.PercentSalary;
#pragma warning restore CS8602
                }
                _ = userIdentityService.UpdateRange(true, users);

                List<CharityDeposit>? deposits = new List<CharityDeposit>();
                List<CharityWage>? wages = new List<CharityWage>();

                if (roleName.FirstOrDefault() == RoleName.Clerk)
                {
                    deposits = charityDepositService.GetAllDepositsByUserIdentity(users).ToList();// محاسبه جمع واریزی منشیها
                }
                else if (roleName.FirstOrDefault() == RoleName.Foreman)
                {
                    wages = charityDepositService.GetAllClerkWagesByFormenUserIdentity(users).ToList();// دریافت حمع واریزی سرکاربران


                    wages.ForEach(x => x.IsUsedForForeman = true);
                    _ = charityWageService.UpdateRange(true, wages, true);
                }

                // محاسبه حقوق و اضافات و کسوراتو درصد سود و کم کردن قسط وام
                Tuple<List<CharityUserIdentityDepositViewModel>, ResultStatusOperation> result =
                    charityWageService.ConvertModelToViewModelForSet(GetUserId(), users, roleName, deposits, wages, startDate, endDate);


                SetMessage(result.Item2);
                switch (result.Item2.Type)
                {
                    case MessageTypeResult.Success:
                        return RedirectToAction(nameof(Index));

                    case MessageTypeResult.Danger:
                    case MessageTypeResult.Warning:
                    case MessageTypeResult.Info:
                        return RedirectToAction(nameof(GetDataForSalaryCalculation));

                    default:
                        return RedirectToAction(nameof(GetDataForSalaryCalculation));
                }
            }
            catch (Exception exception)
            {
                SetMessageException(new ResultStatusOperation("", "", MessageTypeResult.Danger, false, exception), MessageTypeActionMethod.Index);
                return RedirectToAction("ShowException", "Error");
            }
        }

        #endregion


        #region لیست اکسل حقوقهای

        [HttpGet]
        [ActionName("GetExportUsersCharity")]
        [Authorize(Roles = "Admin")]
        public IActionResult ExportUsersCharity()
        {
            return View("ExportUsersCharity");
        }


        /// <summary>
        /// تبدیل لیست به فایل اکسل
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("SettExportUsersCharity")]
        [Authorize(Roles = "Admin")]
        public ActionResult ExportUsersCharity(string startDateString, string endDateString)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation()
            {
                IsSuccessed = true,
                Message = "عملیات با موفقیت انجام شد.",
                Type = MessageTypeResult.Success
            };
            try
            {

                #region DateTime Convertor

                DateTime _startDateString;
                DateTime _endDateString;

                if (!string.IsNullOrEmpty(startDateString) && !string.IsNullOrEmpty(endDateString))
                {
                    DateTime _wageDateFromString = startDateString.ToConvertPersianDateToDateTime(DateTimeFormat.yyyy_mm_dd, DateTimeSpiliter.slash);
                    TimeSpan _wageDateFromTime = TimeSpan.Zero;
                    _startDateString = _wageDateFromString.MergeDateAndTime(_wageDateFromTime);

                    DateTime _wageDateToString = endDateString.ToConvertPersianDateToDateTime(DateTimeFormat.yyyy_mm_dd, DateTimeSpiliter.slash);
                    TimeSpan _wageDateToTime = TimeSpan.Zero;
                    _endDateString = _wageDateToString.MergeDateAndTime(_wageDateToTime);
                }
                else
                {
                    resultStatusOperation.IsSuccessed = false;
                    resultStatusOperation.Message = "تاریخ را به درستی انتخاب کنید.";
                    resultStatusOperation.Type = MessageTypeResult.Warning;

                    return RedirectToAction("GetExportUsersCharity");
                }

                #endregion



#pragma warning disable CS8602
                List<CharityWage> listCharityWage = charityWageService.SpecificationGetData(wageDateFrom: _startDateString, wageDateTo: _endDateString)
                    .Item1.AsQueryable().Include(x => x.WageReceiver.UserRegistrar).ToList();
#pragma warning restore CS8602

                List<PaySlipViewModel> resultViewModel = charityWageService.ConvertModelToViewModelPaySlip(listCharityWage);

                // Create a new Excel package
                using (var excelPackage = new ExcelPackage())
                {
                    // Create a worksheet
                    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Users");

                    // Set column names
                    worksheet.Cells[1, 1].Value = "نام";
                    worksheet.Cells[1, 2].Value = "نام خانوادگی";
                    worksheet.Cells[1, 3].Value = "نام پدر";
                    worksheet.Cells[1, 4].Value = "ثبت کننده";
                    worksheet.Cells[1, 5].Value = "کد ملی";
                    worksheet.Cells[1, 6].Value = "تاریخ تولد";
                    worksheet.Cells[1, 7].Value = "شماره حساب/شماره کارت";
                    worksheet.Cells[1, 8].Value = "درصد";
                    worksheet.Cells[1, 9].Value = "ثابت";
                    worksheet.Cells[1, 10].Value = "جمع اضافات";
                    worksheet.Cells[1, 11].Value = "جمع کسورات";
                    worksheet.Cells[1, 12].Value = "جمع وام";
                    worksheet.Cells[1, 13].Value = "جمع واریزیها";
                    worksheet.Cells[1, 14].Value = "حقوق خالص";

                    // Fill in user data
                    int row = 2;
                    foreach (var item in resultViewModel)
                    {
                        double netReceipts = (item.TotalOfDeposits * item.PercentSalary / 100) + item.FixedSalary;
                        double charityAdditions = 0;
                        double charityDeducations = 0;
                        double charityLoanInstallments = 0;

                        if (item.CharityAdditions != null && item.CharityAdditions.Count() != 0)
                        {
                            charityAdditions = item.CharityAdditions.Select(x => x.Amount).Sum();
                            netReceipts += charityAdditions;
                        }


                        if (item.CharityDeducations != null && item.CharityDeducations.Count() != 0)
                        {
                            charityDeducations = item.CharityDeducations.Select(x => x.Amount).Sum();
                            netReceipts -= charityDeducations;
                        }

                        if (item.CharityLoanInstallments != null && item.CharityLoanInstallments.Count() != 0)
                        {
                            charityLoanInstallments = item.CharityLoanInstallments.Select(x => x.InstallmentAmount).Sum(); ;
                            netReceipts -= charityLoanInstallments;
                        }

                        worksheet.Cells[row, 1].Value = item.FirstName;
                        worksheet.Cells[row, 2].Value = item.LastName;
                        worksheet.Cells[row, 3].Value = item.FatherName;
                        worksheet.Cells[row, 4].Value = $"{item.UserRegistrar?.FirstName} {item.UserRegistrar?.LastName}";
                        worksheet.Cells[row, 5].Value = item.NationalCode;
                        worksheet.Cells[row, 6].Value = item.BirthDate?.ToConvertDateTimeToPersianDate();
                        worksheet.Cells[row, 7].Value = item.AccountNumber;
                        worksheet.Cells[row, 8].Value = item.PercentSalary;
                        worksheet.Cells[row, 9].Value = item.FixedSalary;
                        worksheet.Cells[row, 10].Value = charityAdditions;
                        worksheet.Cells[row, 11].Value = charityDeducations;
                        worksheet.Cells[row, 12].Value = charityLoanInstallments;
                        worksheet.Cells[row, 13].Value = item.TotalOfDeposits;
                        worksheet.Cells[row, 14].Value = netReceipts;
                        row++;
                    }

                    // Create a memory stream to store the Excel package
                    using (var stream = new MemoryStream())
                    {
                        excelPackage.SaveAs(stream);

                        // Define the content type and file name
                        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        string fileName = "filename.xlsx";

                        // Create a file content result and set headers
                        FileContentResult fileContentResult = new FileContentResult(stream.ToArray(), contentType)
                        {
                            FileDownloadName = fileName
                        };

                        // Return the file content result
                        return fileContentResult;

                    }
                }
            }
            catch (Exception exception)
            {
                SetMessageException(new ResultStatusOperation("", "", MessageTypeResult.Danger, false, exception), MessageTypeActionMethod.Index);
                return RedirectToAction("ShowException", "Error");
            }
        }


        #endregion

        #region Ajax Actions

        /// <summary>
        /// آخرین حقوق دریافتی کاربر
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult GetWageByUserId(string userId, DateTime? paymentDateFrom = null, DateTime? paymentDateTo = null)
        {
            List<string> items = new List<string>();

            CharityWage? charityWage = charityWageService.SpecificationGetData("", userId).Item1
                .OrderByDescending(x => x.RegisterDate).FirstOrDefault();

            if (charityWage != null)
            {
                return Json(new { result = new List<string>() { $"{charityWage.FixedSalary.ToString("N0")} ریال" } });

            }

            return Json(new { result = new List<string>() { "موردی یافت نشد" } });
        }

        /// <summary>
        /// لیسن اضافات
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAdditionsByUserId(string userId, DateTime? paymentDateFrom = null, DateTime? paymentDateTo = null)
        {
            List<string> items = new List<string>();

            List<CharityAddition> charityAddition =
                charityAdditionService.SpecificationGetData(userIdentityReciverId: userId, isDone: false).Item1.ToList();

            if (charityAddition != null && charityAddition.Count() != 0)
            {
                foreach (var item in charityAddition)
                {

                    items.Add($"{item.Title} - {item.Amount.ToString("N0")}");
                }

                return Json(new { result = items });
            }

            return Json(new { result = new List<string>() { "موردی یافت نشد" } });
        }

        /// <summary>
        /// لیست کسورات
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult GetDeducationsByUserId(string userId, DateTime? paymentDateFrom = null, DateTime? paymentDateTo = null)
        {
            List<string> items = new List<string>();

            List<CharityDeducation> charityDeducation =
                charityDeducationService.SpecificationGetData(userIdentityReciverId: userId, isDone: false).Item1.ToList();

            if (charityDeducation != null && charityDeducation.Count() != 0)
            {
                foreach (var item in charityDeducation)
                {

                    items.Add($"{item.Title} - {item.Amount.ToString("N0")}");
                }

                return Json(new { result = items });
            }

            return Json(new { result = new List<string>() { "موردی یافت نشد" } });
        }

        /// <summary>
        /// لیست اقساط سررسید شده
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult GetLoanInstallmentsByUserId(string userId, DateTime paymentDateFrom, DateTime paymentDateTo)
        {
            List<string> items = new List<string>();

            CharityLoan? charityLoan = charityLoanService.SpecificationGetData(loanReceiverId: userId, isDone: false).Item1.FirstOrDefault();

            if (charityLoan != null)
            {
                List<CharityLoanInstallments> charityLoanInstallments =
                    charityLoanInstallmentsService
                    .SpecificationGetData(charityLoanId: charityLoan.Id, isDone: false, paymentDateFrom: paymentDateFrom, paymentDateTo: paymentDateTo)
                    .Item1.ToList();

                if (charityLoanInstallments != null && charityLoanInstallments.Count() != 0)
                {
                    foreach (var item in charityLoanInstallments)
                    {

                        items.Add($"مبلغ: {item.InstallmentAmount.ToString("N0")} - موعد پرداخت: {DateTimeUtility.ToConvertDateTimeToPersianDate(item.PaymentDue)}");
                    }

                    return Json(new { result = items });
                }
            }

            return Json(new { result = new List<string>() { "موردی یافت نشد" } });
        }

        #endregion

        //[HttpGet]
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    try
        //    {
        //        if (id == null)
        //        {
        //            SetMessageEditnotFound();
        //            return RedirectToAction("Index");
        //        }
        //        Tuple<CharityWage, ResultStatusOperation> resultFindModel = await charityWageService.Find(id.Value);
        //        SetMessage(charityWageService.LogicDelete(resultFindModel.Item1));
        //        return RedirectToAction("Index");
        //    }
        //    catch (Exception exception)
        //    {
        //        SetMessageException(new ResultStatusOperation("", "", MessageTypeResult.Danger, false, exception), MessageTypeActionMethod.Index);
        //        return RedirectToAction("ShowException", "Error");
        //    }
        //}


        /// <summary>
        /// فیش حقوقی
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public IActionResult PaySlip()
        {
            List<CharityWage> charityWage = charityWageService.SpecificationGetData("", GetUserId()).Item1.Take(10).ToList();

            List<PaySlipViewModel> viewModelPaySlip = charityWageService.ConvertModelToViewModelPaySlip(charityWage).OrderByDescending(x => x.CharityWageId).ToList();

            if (viewModelPaySlip != null && viewModelPaySlip.Count() != 0)
            {
                return View(viewModelPaySlip);
            }

            return Redirect("/");
        }


        private void FillDropDown()
        {
            try
            {
                string wageReceiverId = ViewBag.wageReceiverId;
                ViewBag.listWageReceiver = userIdentityService.ReadAllWithFatherName(wageReceiverId, true, false, new List<RoleName>() { RoleName.Clerk, RoleName.Foreman });

            }
            catch (Exception exception)
            {
                SetMessageException(new ResultStatusOperation("", "", MessageTypeResult.Danger, false, exception), MessageTypeActionMethod.Index);
            }
        }

        private void FillDropDownRoles()
        {
            try
            {
                ViewBag.rolesList = userIdentityService.GetAllRoles(new List<string>(), RoleName.Admin);
            }
            catch (Exception exception)
            {
                SetMessageException(new ResultStatusOperation("", "", MessageTypeResult.Danger, false, exception), MessageTypeActionMethod.Index);
            }
        }

    }
}
