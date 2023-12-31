using Aban.Common;
using Aban.Common.Utility;
using Aban.Domain.Entities;
using Aban.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Aban.Domain.Enumerations;
using static Aban.Domain.Enumerations.Enumeration;

namespace Aban.Dashboard.Areas.Loans.Controllers
{
    [Area("Loans")]
    [Authorize(Roles = "Admin")]
    public class CharityLoanInstallmentsController : GenericController
    {
        #region constructor

        private readonly ICharityLoanService charityLoanService;
        private readonly ICharityLoanInstallmentsService charityLoanInstallmentsService;


        public CharityLoanInstallmentsController(
            ICharityLoanService charityLoanService,
            ICharityLoanInstallmentsService charityLoanInstallmentsService
            )
        {
            this.charityLoanService = charityLoanService;
            this.charityLoanInstallmentsService = charityLoanInstallmentsService;
        }

        #endregion

        [HttpGet]
        public async Task<IActionResult> Index(
            int? charityLoanId = null,
            double? installmentAmount = null,
            string paymentDueFrom = "",
            string paymentDueTo = "",
            string paymentDate = "",
            TransactionMethod? paymentMethod = null,
            string registerDateFrom = "",
            string registerDateTo = "",
            bool? isdone = null,
            int pageNumber = 1,
            int pageSize = 10,
            string sortColumn = "Id",
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
                ViewBag.charityLoanId = charityLoanId;
                ViewBag.installmentAmount = installmentAmount;
                ViewBag.paymentDueFrom = paymentDueFrom;
                ViewBag.paymentDueTo = paymentDueTo;
                ViewBag.paymentDate = paymentDate;
                ViewBag.paymentMethod = paymentMethod;
                ViewBag.registerDateFrom = registerDateFrom;
                ViewBag.registerDateTo = registerDateTo;
                ViewBag.isdone = isdone;


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

                DateTime? _paymentDueFrom = null;
                DateTime? _paymentDueTo = null;
                DateTime? _paymentDate = null;
                DateTime? _registerDateFrom = null;
                DateTime? _registerDateTo = null;


                if (!string.IsNullOrEmpty(paymentDueFrom))
                {
                    _paymentDueFrom = await paymentDueFrom.ToConvertPersianDateToDateTimeAsync(DateTimeFormat.yyyy_mm_dd, DateTimeSpiliter.slash);
                }
                if (!string.IsNullOrEmpty(paymentDueTo))
                {
                    _paymentDueTo = await paymentDueTo.ToConvertPersianDateToDateTimeAsync(DateTimeFormat.yyyy_mm_dd, DateTimeSpiliter.slash);
                }
                if (!string.IsNullOrEmpty(paymentDate))
                {
                    _paymentDate = await paymentDate.ToConvertPersianDateToDateTimeAsync(DateTimeFormat.yyyy_mm_dd, DateTimeSpiliter.slash);
                }
                if (!string.IsNullOrEmpty(registerDateFrom))
                {
                    _registerDateFrom = await registerDateFrom.ToConvertPersianDateToDateTimeAsync(DateTimeFormat.yyyy_mm_dd, DateTimeSpiliter.slash);
                }
                if (!string.IsNullOrEmpty(registerDateTo))
                {
                    _registerDateTo = await registerDateTo.ToConvertPersianDateToDateTimeAsync(DateTimeFormat.yyyy_mm_dd, DateTimeSpiliter.slash);
                }

                #endregion


                Tuple<IQueryable<CharityLoanInstallments>, ResultStatusOperation> result =
                    charityLoanInstallmentsService.SpecificationGetData(charityLoanId, installmentAmount,
                    _paymentDueFrom, _paymentDueTo, _paymentDate, paymentMethod,
                    _registerDateFrom, _registerDateTo, isdone);
                return View(charityLoanInstallmentsService.Pagination(result.Item1, true, pageNumber, pageSize, isDesc, sortColumn));
                //return View(result.Item1.ToPagedList(pageNumber, pageSize));
            }
            catch (Exception exception)
            {
                SetMessageException(new ResultStatusOperation("", "", MessageTypeResult.Danger, false, exception), MessageTypeActionMethod.Index);
                return RedirectToAction("ShowException", "Error", new { area = "" });
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            try
            {
                FillDropDown();
                return View(new CharityLoanInstallments());
            }
            catch (Exception exception)
            {
                SetMessageException(new ResultStatusOperation("", "", MessageTypeResult.Danger, false, exception), MessageTypeActionMethod.Index);
                return RedirectToAction("ShowException", "Error", new { area = "" });
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            CharityLoanInstallments model,
            string paymentDateString,
            string paymentDueString)
        {
            try
            {
                #region DateTime Convertor

                if (!paymentDateString.IsNullOrEmpty())
                {
                    DateTime _paymentDate = paymentDateString.ToConvertPersianDateToDateTime(DateTimeFormat.yyyy_mm_dd, DateTimeSpiliter.slash);
                    TimeSpan _paymentDateTime = TimeSpan.Zero;
                    model.PaymentDate = _paymentDate.MergeDateAndTime(_paymentDateTime);
                }

                DateTime _paymentDue = paymentDueString.ToConvertPersianDateToDateTime(DateTimeFormat.yyyy_mm_dd, DateTimeSpiliter.slash);
                TimeSpan _paymentDueTime = TimeSpan.Zero;
                model.PaymentDue = _paymentDue.MergeDateAndTime(_paymentDueTime);

                #endregion

                Tuple<CharityLoanInstallments, ResultStatusOperation> resultFillModel = charityLoanInstallmentsService.FillModel(model);
                resultFillModel = await charityLoanInstallmentsService.Insert(fillControllerInfo(new List<string>() { "PaymentDate", "paymentDateString" }), resultFillModel.Item1);

                SetMessage(resultFillModel.Item2);
                switch (resultFillModel.Item2.Type)
                {
                    case MessageTypeResult.Success:
                        return RedirectToAction(nameof(Index));

                    case MessageTypeResult.Danger:
                    case MessageTypeResult.Warning:
                        FillDropDown();
                        return View(resultFillModel.Item1);

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
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    SetMessageEditnotFound();
                    return RedirectToAction("Index");
                }
                Tuple<CharityLoanInstallments, ResultStatusOperation> resultFindModel = await charityLoanInstallmentsService.Find(id.Value);

                SetMessage(resultFindModel.Item2);
                switch (resultFindModel.Item2.Type)
                {
                    case MessageTypeResult.Success:

                        ViewBag.charityLoanId = resultFindModel.Item1.Id;
                        FillDropDown();
                        return View(resultFindModel.Item1);

                    case MessageTypeResult.Danger:
                    case MessageTypeResult.Warning:
                        FillDropDown();
                        return View(resultFindModel.Item1);

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
        public async Task<IActionResult> Edit(
            CharityLoanInstallments model,
            string paymentDateString,
            string paymentDueString)
        {
            try
            {

                #region DateTime Convertor

                if (!paymentDateString.IsNullOrEmpty())
                {
                    DateTime _paymentDate = paymentDateString.ToConvertPersianDateToDateTime(DateTimeFormat.yyyy_mm_dd, DateTimeSpiliter.slash);
                    TimeSpan _paymentDateTime = TimeSpan.Zero;
                    model.PaymentDate = _paymentDate.MergeDateAndTime(_paymentDateTime);
                }

                DateTime _paymentDue = paymentDueString.ToConvertPersianDateToDateTime(DateTimeFormat.yyyy_mm_dd, DateTimeSpiliter.slash);
                TimeSpan _paymentDueTime = TimeSpan.Zero;
                model.PaymentDue = _paymentDue.MergeDateAndTime(_paymentDueTime);

                #endregion


                Tuple<CharityLoanInstallments, ResultStatusOperation> resultEdit = await charityLoanInstallmentsService.Update(fillControllerInfo(new List<string>() { "PaymentDate", "paymentDateString" }), model);

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
                }

                FillDropDown();
                return View(resultEdit.Item1);
            }
            catch (Exception exception)
            {
                SetMessageException(new ResultStatusOperation("", "", MessageTypeResult.Danger, false, exception), MessageTypeActionMethod.Index);
                return RedirectToAction("ShowException", "Error");
            }
        }

        public async Task<IActionResult> PayInstallment(string paymentDateString, int loanInstallmentId, int loanId)
        {

            if (!string.IsNullOrEmpty(paymentDateString))
            {
                Tuple<CharityLoanInstallments, ResultStatusOperation> resultFindModel = await charityLoanInstallmentsService.Find(loanInstallmentId);

                DateTime _paymentDate = paymentDateString.ToConvertPersianDateToDateTime(DateTimeFormat.yyyy_mm_dd, DateTimeSpiliter.slash);
                TimeSpan _paymentDateTime = TimeSpan.Zero;
                resultFindModel.Item1.PaymentDate = _paymentDate.MergeDateAndTime(_paymentDateTime);

                resultFindModel.Item1.IsDone = true;

                Tuple<CharityLoanInstallments, ResultStatusOperation> resultEdit = await charityLoanInstallmentsService.Update(fillControllerInfo(), resultFindModel.Item1);


            }

            return RedirectToAction(nameof(Index), new { charityLoanId = loanId });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    SetMessageEditnotFound();
                    return RedirectToAction("Index");
                }
                Tuple<CharityLoanInstallments, ResultStatusOperation> resultFindModel = await charityLoanInstallmentsService.Find(id.Value);

                SetMessage(resultFindModel.Item2);
                if (resultFindModel.Item1 != null)
                {
                    SetMessage(charityLoanInstallmentsService.Update(true, resultFindModel.Item1).Result.Item2);
                }

                return RedirectToAction("Index");
            }
            catch (Exception exception)
            {
                SetMessageException(new ResultStatusOperation("", "", MessageTypeResult.Danger, false, exception), MessageTypeActionMethod.Index);
                return RedirectToAction("ShowException", "Error");
            }
        }

        private void FillDropDown()
        {
            try
            {
                int charityLoanId = ViewBag.charityLoanId != null ? ViewBag.charityLoanId : 0;

                ViewBag.listCharityLoan = charityLoanService.ReadAll(charityLoanId);
            }
            catch (Exception exception)
            {
                SetMessageException(new ResultStatusOperation("", "", MessageTypeResult.Danger, false, exception), MessageTypeActionMethod.Index);
            }
        }
    }
}
