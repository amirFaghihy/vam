using Aban.Common;
using Aban.Common.Utility;
using Aban.Domain.Entities;
using Aban.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Aban.Domain.Enumerations.Enumeration;

namespace Aban.Dashboard.Areas.Loans.Controllers
{
    [Area("Loans")]
    [Authorize(Roles = "Admin")]
    public class CharityLoanController : GenericController
    {
        #region constructor

        private readonly ICharityLoanService charityLoanService;
        private readonly IUserIdentityService userIdentityService;
        private readonly ICharityLoanInstallmentsService charityLoanInstallmentsService;
        private readonly IGuaranteeService guaranteeService;


        public CharityLoanController(
            ICharityLoanService charityLoanService,
            IUserIdentityService userIdentityService,
            ICharityLoanInstallmentsService charityLoanInstallmentsService,
            IGuaranteeService guaranteeService
            )
        {
            this.charityLoanService = charityLoanService;
            this.userIdentityService = userIdentityService;
            this.charityLoanInstallmentsService = charityLoanInstallmentsService;
            this.guaranteeService = guaranteeService;
        }

        #endregion

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


                #region selectedValue

                ViewBag.pageNumber = pageNumber;
                ViewBag.pageSize = pageSize;
                ViewBag.sortColumn = sortColumn;
                ViewBag.lastColumn = lastColumn;
                ViewBag.isDesc = isDesc;
                ViewBag.guaranteeId = guaranteeId;
                ViewBag.loanReceiverId = loanReceiverId;
                ViewBag.loanAmount = loanAmount;
                ViewBag.percentSalary = percentSalary;
                ViewBag.accountNumber = accountNumber;
                ViewBag.givingLoanMethod = givingLoanMethod;
                ViewBag.paymentStartDateFrom = paymentStartDateFrom;
                ViewBag.paymentStartDateTo = paymentStartDateTo;
                ViewBag.numberOfInstallments = numberOfInstallments;
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

                #endregion

                FillDropDown();

                #region DateTime Convertor

                DateTime? _paymentStartDateFrom = null;
                DateTime? _paymentStartDateTo = null;
                DateTime? _registerDateFrom = null;
                DateTime? _registerDateTo = null;


                if (!string.IsNullOrEmpty(paymentStartDateFrom))
                {
                    _paymentStartDateFrom = await paymentStartDateFrom.ToConvertPersianDateToDateTimeAsync(DateTimeFormat.yyyy_mm_dd, DateTimeSpiliter.slash);
                }
                if (!string.IsNullOrEmpty(paymentStartDateTo))
                {
                    _paymentStartDateTo = await paymentStartDateTo.ToConvertPersianDateToDateTimeAsync(DateTimeFormat.yyyy_mm_dd, DateTimeSpiliter.slash);
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


                Tuple<IQueryable<CharityLoan>, ResultStatusOperation> result = charityLoanService.SpecificationGetData(
                    guaranteeId, loanReceiverId, loanAmount, percentSalary, accountNumber, givingLoanMethod,
                    _paymentStartDateFrom, _paymentStartDateTo, numberOfInstallments, _registerDateFrom, _registerDateTo,
                    isdone);

                return View(charityLoanService.Pagination(result.Item1, true, pageNumber, pageSize, isDesc, sortColumn));
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
                return View(new CharityLoan());
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
            CharityLoan model,
            string paymentStartDateString)
        {
            try
            {

                #region DateTime Convertor

                DateTime _paymentStartDate = paymentStartDateString.ToConvertPersianDateToDateTime(DateTimeFormat.yyyy_mm_dd, DateTimeSpiliter.slash);
                TimeSpan _paymentStartDateTime = TimeSpan.Zero;
                model.PaymentStartDate = _paymentStartDate.MergeDateAndTime(_paymentStartDateTime);

                #endregion

                Tuple<CharityLoan, ResultStatusOperation> resultFillModel = charityLoanService.FillModel(model);
                resultFillModel = await charityLoanService.Insert(fillControllerInfo(), resultFillModel.Item1);
                switch (resultFillModel.Item2.Type)
                {
                    case MessageTypeResult.Success:
                        Tuple<IEnumerable<CharityLoanInstallments>, ResultStatusOperation> charityLoanInstallments =
                            await charityLoanInstallmentsService.InsertRange(true, charityLoanInstallmentsService.CreateListOfModel(resultFillModel.Item1));

                        SetMessage(charityLoanInstallments.Item2);
                        return RedirectToAction(nameof(Index));

                    case MessageTypeResult.Danger:
                    case MessageTypeResult.Warning:
                        SetMessage(resultFillModel.Item2);
                        FillDropDown();
                        return View(resultFillModel.Item1);

                    default:
                        SetMessage(resultFillModel.Item2);
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
                Tuple<CharityLoan, ResultStatusOperation> resultFindModel = await charityLoanService.Find(id.Value);

                ViewBag.loanReceiverId = resultFindModel.Item1.LoanReceiverId;
                SetMessage(resultFindModel.Item2);
                switch (resultFindModel.Item2.Type)
                {
                    case MessageTypeResult.Success:
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
            CharityLoan model,
            string paymentStartDateString)
        {
            try
            {

                #region DateTime Convertor

                DateTime _paymentStartDate = paymentStartDateString.ToConvertPersianDateToDateTime(DateTimeFormat.yyyy_mm_dd, DateTimeSpiliter.slash);
                TimeSpan _paymentStartDateTime = TimeSpan.Zero;
                model.PaymentStartDate = _paymentStartDate.MergeDateAndTime(_paymentStartDateTime);

                #endregion

                Tuple<CharityLoan, ResultStatusOperation> resultEdit = await charityLoanService.Update(fillControllerInfo(new List<string>() { "paymentStartDateString", "UserIdentityId" }), model);

                SetMessage(resultEdit.Item2);
                switch (resultEdit.Item2.Type)
                {
                    case MessageTypeResult.Success:
                        return RedirectToAction(nameof(Index));

                    case MessageTypeResult.Danger:
                    case MessageTypeResult.Warning:
                        FillDropDown();
                        return View(resultEdit.Item1);

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
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    SetMessageEditnotFound();
                    return RedirectToAction("Index");
                }
                Tuple<CharityLoan, ResultStatusOperation> resultFindModel = await charityLoanService.Find(id.Value);
                SetMessage(resultFindModel.Item2);
                if (resultFindModel.Item1 != null)
                {
                    List<CharityLoanInstallments> charityLoanInstallments = charityLoanInstallmentsService.SpecificationGetData(resultFindModel.Item1.Id).Item1.ToList();
                    charityLoanInstallments.ForEach(x => x.IsDelete = true);
                    await charityLoanInstallmentsService.UpdateRange(true, charityLoanInstallments, false);

                    resultFindModel.Item1.IsDelete = true;

                    SetMessage(charityLoanService.Update(true, resultFindModel.Item1).Result.Item2);

                }
                return RedirectToAction(nameof(Index));
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
                string loanReceiverId = ViewBag.loanReceiverId;
                int guaranteeId = ViewBag.guaranteeId == null ? 0 : ViewBag.guaranteeId;
                ViewBag.listLoanReceiver = userIdentityService.ReadAllWithFatherName(loanReceiverId);
                ViewBag.listGuarantee = guaranteeService.ReadAll(guaranteeId);
            }
            catch (Exception exception)
            {
                SetMessageException(new ResultStatusOperation("", "", MessageTypeResult.Danger, false, exception), MessageTypeActionMethod.Index);
            }
        }
    }
}
