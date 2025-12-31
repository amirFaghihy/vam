using Aban.Common;
using Aban.Common.Utility;
using Aban.Domain.Entities.Loans;
using Aban.Domain.Entities.Messages; // اضافه شده برای مدیریت پیام‌ها
using Aban.Service.Interfaces.Generic;
using Aban.Service.Interfaces.Loans;
using Aban.Service.Interfaces.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Aban.Domain.Enumerations.Enumeration;

namespace Aban.Dashboard.Areas.Loans.Controllers
{
    [Area("Loans")]
    [Authorize(Roles = "Admin")]
    public class CharityLoanController : Controller // تغییر مهم: ارث‌بری مستقیم از Controller
    {
        #region constructor

        private readonly IGenericService<CharityLoan> _charityLoanGenericService;
        private readonly ICharityLoanService _charityLoanService;
        private readonly IUserIdentityService _userIdentityService;
        private readonly ICharityLoanInstallmentsService _installmentsService;
        private readonly IGuaranteeService _guaranteeService;

        public CharityLoanController(
            IGenericService<CharityLoan> charityLoanGenericService,
            ICharityLoanService charityLoanService,
            IUserIdentityService userIdentityService,
            ICharityLoanInstallmentsService installmentsService,
            IGuaranteeService guaranteeService
            )
        {
            _charityLoanGenericService = charityLoanGenericService;
            _charityLoanService = charityLoanService;
            _userIdentityService = userIdentityService;
            _installmentsService = installmentsService;
            _guaranteeService = guaranteeService;
        }

        #endregion

        // متد کمکی برای ارسال پیام به ویو
        protected void SetMessage(ResultStatusOperation result)
        {
            if (result == null) return;
            TempData["Message"] = result.Message;
            TempData["Type"] = result.Type.ToString();
        }

        protected void SetMessageException(ResultStatusOperation result, MessageTypeActionMethod method)
        {
             TempData["Message"] = result.Message + " | " + result.Exception?.Message;
             TempData["Type"] = "Danger";
        }

        protected ControllerInfo fillControllerInfo(string actionName)
        {
             return new ControllerInfo { ControllerName = "CharityLoan", ActionName = actionName };
        }

        [HttpGet]
        public async Task<IActionResult> Index(
            int? guaranteeId = null,
            string loanReceiverId = "",
            float? loanAmount = null,
            byte? percentSalary = null,
            string? accountNumber = null,
            TransactionMethod? givingLoanMethod = null,
            string paymentStartDateFrom = "",
            string paymentStartDateTo = "",
            byte? numberOfInstallments = null,
            string registerDateFrom = "",
            string registerDateTo = "",
            bool? isdone = null,
            int pageNumber = 1,
            int pageSize = 10,
            string sortColumn = "RegisterDate",
            string lastColumn = "",
            bool isDesc = true)
        {
            try
            {
                ViewBag.pageNumber = pageNumber;
                ViewBag.pageSize = pageSize;
                ViewBag.sortColumn = sortColumn;
                ViewBag.lastColumn = lastColumn;
                ViewBag.isDesc = isDesc;
                ViewBag.guaranteeId = guaranteeId;
                ViewBag.loanReceiverId = loanReceiverId;
                
                if (ViewBag.lastColumn == ViewBag.sortColumn)
                {
                    ViewBag.isDesc = !isDesc;
                    isDesc = ViewBag.isDesc;
                }
                else
                {
                    ViewBag.isDesc = isDesc;
                }
                ViewBag.lastColumn = ViewBag.sortColumn;

                await FillDropDown();

                DateTime? _paymentStartDateFrom = string.IsNullOrEmpty(paymentStartDateFrom) ? null : await paymentStartDateFrom.ToConvertPersianDateToDateTimeAsync(DateTimeFormat.yyyy_mm_dd, DateTimeSpiliter.slash);
                DateTime? _paymentStartDateTo = string.IsNullOrEmpty(paymentStartDateTo) ? null : await paymentStartDateTo.ToConvertPersianDateToDateTimeAsync(DateTimeFormat.yyyy_mm_dd, DateTimeSpiliter.slash);
                DateTime? _registerDateFrom = string.IsNullOrEmpty(registerDateFrom) ? null : await registerDateFrom.ToConvertPersianDateToDateTimeAsync(DateTimeFormat.yyyy_mm_dd, DateTimeSpiliter.slash);
                DateTime? _registerDateTo = string.IsNullOrEmpty(registerDateTo) ? null : await registerDateTo.ToConvertPersianDateToDateTimeAsync(DateTimeFormat.yyyy_mm_dd, DateTimeSpiliter.slash);

                var result = _charityLoanService.SpecificationGetData(
                    guaranteeId, loanReceiverId, loanAmount, percentSalary, accountNumber, givingLoanMethod,
                    _paymentStartDateFrom, _paymentStartDateTo, numberOfInstallments, _registerDateFrom, _registerDateTo,
                    isdone);

                return View(_charityLoanService.Pagination(result.Item1, true, pageNumber, pageSize, isDesc, sortColumn));
            }
            catch (Exception exception)
            {
                SetMessageException(new ResultStatusOperation("", "", MessageTypeResult.Danger, false, exception), MessageTypeActionMethod.Index);
                return RedirectToAction("Index", "Home"); 
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            try
            {
                ViewBag.loanReceiverId = "";
                ViewBag.guaranteeId = 0;
                await FillDropDown();
                return View(new CharityLoan());
            }
            catch (Exception exception)
            {
                SetMessageException(new ResultStatusOperation("", "", MessageTypeResult.Danger, false, exception), MessageTypeActionMethod.Index);
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CharityLoan model)
        {
            string paymentStartDateString = Request.Form["paymentStartDateString"];

            try
            {
                if (!string.IsNullOrEmpty(paymentStartDateString))
                {
                    model.PaymentStartDate = paymentStartDateString.ToConvertPersianDateToDateTime(DateTimeFormat.yyyy_mm_dd, DateTimeSpiliter.slash);
                }
                else
                {
                    model.PaymentStartDate = DateTime.Now;
                }

                var resultFillModel = _charityLoanService.FillModel(model);
                resultFillModel = await _charityLoanService.Insert(fillControllerInfo("Guarantee"), resultFillModel.Item1);
                
                if (resultFillModel.Item2.Type == MessageTypeResult.Success)
                {
                    var installments = _installmentsService.CreateListOfModel(resultFillModel.Item1);
                    var charityLoanInstallments = await _installmentsService.InsertRange(true, installments);

                    SetMessage(charityLoanInstallments.Item2);
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    SetMessage(resultFillModel.Item2);
                    ViewBag.loanReceiverId = resultFillModel.Item1.LoanReceiverId;
                    ViewBag.guaranteeId = resultFillModel.Item1.GuaranteeId;
                    await FillDropDown();
                    return View(resultFillModel.Item1);
                }
            }
            catch (Exception exception)
            {
                SetMessageException(new ResultStatusOperation("", "", MessageTypeResult.Danger, false, exception), MessageTypeActionMethod.Index);
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null) return RedirectToAction("Index");

                var resultFindModel = await _charityLoanService.Find(id.Value);

                if (resultFindModel.Item1 == null) return NotFound();

                ViewBag.loanReceiverId = resultFindModel.Item1.LoanReceiverId;
                ViewBag.guaranteeId = resultFindModel.Item1.GuaranteeId;

                SetMessage(resultFindModel.Item2);
                await FillDropDown();

                return View(resultFindModel.Item1);
            }
            catch (Exception exception)
            {
                SetMessageException(new ResultStatusOperation("", "", MessageTypeResult.Danger, false, exception), MessageTypeActionMethod.Index);
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CharityLoan model)
        {
            string paymentStartDateString = Request.Form["paymentStartDateString"];

            try
            {
                if (!string.IsNullOrEmpty(paymentStartDateString))
                {
                    model.PaymentStartDate = paymentStartDateString.ToConvertPersianDateToDateTime(DateTimeFormat.yyyy_mm_dd, DateTimeSpiliter.slash);
                }

                var resultEdit = await _charityLoanService.Update(fillControllerInfo("Guarantee"), model);

                SetMessage(resultEdit.Item2);
                
                if (resultEdit.Item2.Type == MessageTypeResult.Success)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewBag.loanReceiverId = model.LoanReceiverId;
                    ViewBag.guaranteeId = model.GuaranteeId;
                    await FillDropDown();
                    return View(model);
                }
            }
            catch (Exception exception)
            {
                SetMessageException(new ResultStatusOperation("", "", MessageTypeResult.Danger, false, exception), MessageTypeActionMethod.Index);
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null) return RedirectToAction("Index");

                var resultFindModel = await _charityLoanService.Find(id.Value);
                
                if (resultFindModel.Item1 != null)
                {
                    var installments = _installmentsService.SpecificationGetData(resultFindModel.Item1.Id).Item1.ToList();
                    installments.ForEach(x => x.IsDelete = true);
                    await _installmentsService.UpdateRange(true, installments, false);

                    resultFindModel.Item1.IsDelete = true;
                    var updateResult = await _charityLoanService.Update(true, resultFindModel.Item1);
                    SetMessage(updateResult.Item2);
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception exception)
            {
                SetMessageException(new ResultStatusOperation("", "", MessageTypeResult.Danger, false, exception), MessageTypeActionMethod.Index);
                return RedirectToAction("Index");
            }
        }

        private async Task FillDropDown()
        {
            try
            {
                string selectedUser = ViewBag.loanReceiverId as string;
                int? selectedGuarantee = ViewBag.guaranteeId as int?;

                var allUsers = await _userIdentityService.GetAllAsync();
                ViewBag.listLoanReceiver = new SelectList(
                    allUsers.Select(u => new 
                    { 
                        Id = u.Id, 
                        FullName = u.FirstName + " " + u.LastName + (!string.IsNullOrEmpty(u.FatherName) ? $" (پدر: {u.FatherName})" : "") 
                    }), 
                    "Id", "FullName", selectedUser);

                var allGuarantees = await _guaranteeService.GetAllAsync();
                ViewBag.listGuarantee = new SelectList(allGuarantees, "Id", "Description", selectedGuarantee);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}