using Aban.Common;
using Aban.Common.Utility;
using Aban.Domain.Entities;
using Aban.Service.Interfaces;
using Aban.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Aban.Domain.Enumerations.Enumeration;

namespace Aban.Dashboard.Areas.Loans.Controllers
{
    [Area("Loans")]
    [Authorize(Roles = "Admin")]
    public class UserAccountDepositWithdrawalController : GenericController
    {
        #region constructor

        private readonly IUserAccountDepositWithdrawalService userAccountDepositWithdrawalService;
        private readonly IUserAccountService userAccountService;
        private readonly ICharityLoanInstallmentsService charityLoanInstallmentsService;
        ICharityLoanService charityLoanService;
        private readonly IUserIdentityService userIdentityService;


        public UserAccountDepositWithdrawalController(
            IUserAccountDepositWithdrawalService userAccountDepositWithdrawalService,
            IUserAccountService userAccountService,
            ICharityLoanInstallmentsService charityLoanInstallmentsService,
            ICharityLoanService charityLoanService,
            IUserIdentityService userIdentityService
            )
        {

            this.userAccountDepositWithdrawalService = userAccountDepositWithdrawalService;
            this.userAccountService = userAccountService;
            this.charityLoanInstallmentsService = charityLoanInstallmentsService;
            this.charityLoanService = charityLoanService;
            this.userIdentityService = userIdentityService;
        }

        #endregion

        [HttpGet]
        public IActionResult Index(
            int userAccountId = 0,
            double? price = null,
            double? totalPriceAfterTransaction = null,
            TransactionType? accountTransactionType = null,
            TransactionMethod? accountTransactionMethod = null,
            DateTime? transactionDateTimeFrom = null,
            DateTime? transactionDateTimeTo = null,
            DateTime? registerDateFrom = null,
            DateTime? registerDateTo = null,
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
                ViewBag.userAccountId = userAccountId;
                ViewBag.price = price;
                ViewBag.totalPriceAfterTransaction = totalPriceAfterTransaction;
                ViewBag.accountTransactionType = accountTransactionType;
                ViewBag.accountTransactionMethod = accountTransactionMethod;
                ViewBag.transactionDateTimeFrom = transactionDateTimeFrom;
                ViewBag.transactionDateTimeTo = transactionDateTimeTo;
                ViewBag.registerDateFrom = registerDateFrom;
                ViewBag.registerDateTo = registerDateTo;


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

                FillDropDown(accountTransactionType, accountTransactionMethod, userAccountId);
                #endregion

                Tuple<IQueryable<UserAccountDepositWithdrawal>, ResultStatusOperation> result =
                    userAccountDepositWithdrawalService.SpecificationGetData(userAccountId, price, totalPriceAfterTransaction, accountTransactionType, accountTransactionMethod, transactionDateTimeFrom, transactionDateTimeTo, registerDateFrom, registerDateTo);

                return View(userAccountDepositWithdrawalService.Pagination(result.Item1, true, pageNumber, pageSize, isDesc, sortColumn));
                //return View(result.Item1.ToPagedList(pageNumber, pageSize));
            }
            catch (Exception exception)
            {
                SetMessageException(new ResultStatusOperation("", "", MessageTypeResult.Danger, false, exception), MessageTypeActionMethod.Index);
                return RedirectToAction("ShowException", "Error", new { area = "" });
            }
        }

        [HttpGet]
        public IActionResult Create(
            TransactionType? accountTransactionType = null,
            int userAccountId = 0)
        {
            try
            {
                FillDropDown(accountTransactionType: accountTransactionType, userAccountId: userAccountId);
                return View(new UserAccountDepositWithdrawal());
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
            UserAccountDepositWithdrawal model,
            string transactionDateTimeString,
            string transactionTimeString)
        {
            try
            {
                bool isPayInstallment = false;

                #region DateTime Convertor

                DateTime _transactionDateTime = transactionDateTimeString.ToConvertPersianDateToDateTime(DateTimeFormat.yyyy_mm_dd, DateTimeSpiliter.slash);
                TimeSpan _transactionDateTimeTime = transactionTimeString.ToConvertStringToTime();
                model.TransactionDateTime = _transactionDateTime.MergeDateAndTime(_transactionDateTimeTime);

                #endregion

                double latestTotalPrice = userAccountDepositWithdrawalService.GetLatestTotalPriceAfterTransaction(model.UserAccountId);
                if (model.UserAccountId != 0)
                {
                    switch (model.AccountTransactionType)
                    {
                        case TransactionType.واریز:
                            model.TotalPriceAfterTransaction = latestTotalPrice + model.Price;
                            break;
                        case TransactionType.برداشت:
                            model.TotalPriceAfterTransaction = latestTotalPrice - model.Price;
                            break;
                        case TransactionType.پرداخت_قسط:
                            isPayInstallment = true;
                            model.TotalPriceAfterTransaction = latestTotalPrice + model.Price;
                            break;
                        default:
                            FillDropDown();
                            return View(model);
                    }
                }
                else
                {
                    FillDropDown();
                    return View(model);
                }

                Tuple<UserAccountDepositWithdrawal, ResultStatusOperation> resultFillModel = userAccountDepositWithdrawalService.FillModel(model);
                resultFillModel = await userAccountDepositWithdrawalService.Insert(fillControllerInfo(), resultFillModel.Item1);
                SetMessage(resultFillModel.Item2);
                switch (resultFillModel.Item2.Type)
                {
                    case MessageTypeResult.Success:
                        {
                            if (isPayInstallment)
                            {
                                int loanId = await PayInstallment(resultFillModel.Item1);
                                charityLoanService.ChangeLoanStatus(loanId);
                            }
                            return RedirectToAction(nameof(Index), new { userAccountId = model.UserAccountId });
                        }
                    case MessageTypeResult.Danger:
                        {
                            SetMessageException(resultFillModel.Item2, MessageTypeActionMethod.CreatePost);
                            return RedirectToAction("ShowException", "Error");
                        }
                    case MessageTypeResult.Warning:
                        {
                            FillDropDown();

                            return View(resultFillModel.Item1);
                        }
                }
                FillDropDown();
                SetMessage(resultFillModel.Item2);
                return View(resultFillModel.Item1);
            }
            catch (Exception exception)
            {
                SetMessageException(new ResultStatusOperation("", "", MessageTypeResult.Danger, false, exception), MessageTypeActionMethod.Index);
                return RedirectToAction("ShowException", "Error");
            }
        }

        /// <summary>
        /// پرداخت قسط بر اساس مبلغ واریزی
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private async Task<int> PayInstallment(UserAccountDepositWithdrawal model)
        {
            string userReciverId = userAccountService.Find(model.UserAccountId).Result.Item1.AccountOwnerId;
            CharityLoan? charityLoan = charityLoanService.SpecificationGetData(loanReceiverId: userReciverId).Item1.FirstOrDefault();

            if (charityLoan != null)
            {

                List<CharityLoanInstallments> charityLoanInstallments =
                    charityLoanInstallmentsService.SpecificationGetData(charityLoanId: charityLoan.Id, isdone: false)
                    .Item1.OrderBy(x => x.PaymentDue).ToList();

                int countOfInstallments = (int)(model.Price / charityLoanInstallments.First().InstallmentAmount);

                for (int i = 0; i < countOfInstallments; i++)
                {
                    Tuple<CharityLoanInstallments, ResultStatusOperation> resultFindModel = await charityLoanInstallmentsService.Find(charityLoanInstallments[i].Id);

                    resultFindModel.Item1.PaymentMethod = TransactionMethod.برداشت_سیستمی;
                    resultFindModel.Item1.PaymentDate = DateTime.Now;
                    resultFindModel.Item1.IsDone = true;

                    UserAccountDepositWithdrawal newModel = new UserAccountDepositWithdrawal()
                    {
                        UserAccountId = model.UserAccountId,
                        Price = resultFindModel.Item1.InstallmentAmount,
                        AccountTransactionType = TransactionType.برداشت,
                        AccountTransactionMethod = TransactionMethod.برداشت_سیستمی,
                        TotalPriceAfterTransaction = userAccountDepositWithdrawalService.GetLatestTotalPriceAfterTransaction(model.UserAccountId) - resultFindModel.Item1.InstallmentAmount,
                        TransactionDateTime = DateTime.Now,
                        Description = "پرداخت قسط سیستمی"
                    };


                    await charityLoanInstallmentsService.Update(fillControllerInfo(), resultFindModel.Item1, false);

                    Tuple<UserAccountDepositWithdrawal, ResultStatusOperation> resultFillModel = userAccountDepositWithdrawalService.FillModel(newModel);
                    await userAccountDepositWithdrawalService.Insert(fillControllerInfo(), resultFillModel.Item1);
                }

                return charityLoan.Id;
            }

            return 0;
        }

        /// <summary>
        /// Not working
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
                Tuple<UserAccountDepositWithdrawal, ResultStatusOperation> resultFindModel = await userAccountDepositWithdrawalService.Find(id.Value);
                switch (resultFindModel.Item2.Type)
                {
                    case MessageTypeResult.Success:
                        {
                            //Pass Parameter like ViewBag.cityId = resultFindModel.Item1.CityId;
                            FillDropDown();
                            SetMessage(resultFindModel.Item2);
                            return View(resultFindModel.Item1);
                        }
                    case MessageTypeResult.Danger:
                        {
                            SetMessageException(resultFindModel.Item2, MessageTypeActionMethod.EditGet);
                            return RedirectToAction("ShowException", "Error");
                        }
                    case MessageTypeResult.Warning:
                        {
                            SetMessage(resultFindModel.Item2);
                            return RedirectToAction("Index");
                        }
                }
                SetMessage(resultFindModel.Item2);
                return RedirectToAction("Index");
            }
            catch (Exception exception)
            {
                SetMessageException(new ResultStatusOperation("", "", MessageTypeResult.Danger, false, exception), MessageTypeActionMethod.Index);
                return RedirectToAction("ShowException", "Error");
            }
        }

        /// <summary>
        /// Not working
        /// </summary>
        /// <param name="model"></param>
        /// <param name="transactionDateTimeString"></param>
        /// <param name="transactionTimeString"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            UserAccountDepositWithdrawal model,
            string transactionDateTimeString,
            string transactionTimeString)
        {
            try
            {
                #region DateTime Convertor

                DateTime _transactionDateTime = transactionDateTimeString.ToConvertPersianDateToDateTime(DateTimeFormat.yyyy_mm_dd, DateTimeSpiliter.slash);
                TimeSpan _transactionDateTimeTime = transactionTimeString.ToConvertStringToTime();
                model.TransactionDateTime = _transactionDateTime.MergeDateAndTime(_transactionDateTimeTime);

                #endregion

                double latestTotalPrice = userAccountDepositWithdrawalService.GetLatestTotalPriceAfterTransaction(model.UserAccountId);
                if (model.UserAccountId != 0)
                {
                    switch (model.AccountTransactionType)
                    {
                        case TransactionType.واریز:
                            model.TotalPriceAfterTransaction = latestTotalPrice + model.Price;
                            break;
                        case TransactionType.برداشت:
                            model.TotalPriceAfterTransaction = latestTotalPrice - model.Price;
                            break;
                        case TransactionType.پرداخت_قسط:
                            break;
                        default:
                            FillDropDown();
                            return View(model);
                    }
                }
                else
                {
                    FillDropDown();
                    return View(model);
                }

                Tuple<UserAccountDepositWithdrawal, ResultStatusOperation> resultEdit = await userAccountDepositWithdrawalService.Update(fillControllerInfo(), model);
                SetMessage(resultEdit.Item2);
                switch (resultEdit.Item2.Type)
                {
                    case MessageTypeResult.Success:
                        {
                            FillDropDown();
                            return RedirectToAction(nameof(Index));
                        }
                    case MessageTypeResult.Danger:
                        {
                            SetMessageException(resultEdit.Item2, MessageTypeActionMethod.EditPost);
                            return RedirectToAction("ShowException", "Error");
                        }
                    case MessageTypeResult.Warning:
                        {
                            FillDropDown();
                            return View(resultEdit.Item1);
                        }
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


        //TODO: fix delete action

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
        //        Tuple<UserAccountDepositWithdrawal, ResultStatusOperation> resultFindModel = await userAccountDepositWithdrawalService.Find(id.Value);
        //        SetMessage(userAccountDepositWithdrawalService.LogicDelete(resultFindModel.Item1));
        //        return RedirectToAction("Index");
        //    }
        //    catch (Exception exception)
        //    {
        //        SetMessageException(new ResultStatusOperation("", "", MessageTypeResult.Danger, false, exception), MessageTypeActionMethod.Index);
        //        return RedirectToAction("ShowException", "Error");
        //    }
        //}

        /// <summary>
        /// آخرین باقیمانده حساب را باز میگرداند
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SetDepositWithdrawal(
            int accountNumber,
            double price,
            TransactionType accountTransactionType)
        {
            bool isSuccess = false;

            double totalPriceAfterTransaction =
                userAccountDepositWithdrawalService.GetLatestTotalPriceAfterTransaction(accountNumber);

            if (totalPriceAfterTransaction != 0)
            {
                isSuccess = true;

                switch (accountTransactionType)
                {
                    case TransactionType.واریز:
                        totalPriceAfterTransaction += price;
                        break;
                    case TransactionType.برداشت:
                        totalPriceAfterTransaction -= price;
                        break;
                    case TransactionType.پرداخت_قسط:
                        totalPriceAfterTransaction += price;
                        break;
                    default:
                        break;
                }

                return Json(new
                {
                    isSuccess = isSuccess,
                    latestDepositWithdrawal = totalPriceAfterTransaction
                });
            }
            else
                return Json(new
                {
                    isSuccess = isSuccess,
                    latestDepositWithdrawal = 0
                });
        }

        private void FillDropDown(
            TransactionType? accountTransactionType = null,
            TransactionMethod? accountTransactionMethod = null,
            int userAccountId = 0)
        {
            try
            {
#pragma warning disable CS8604
                ViewBag.listAccountTransactionType = GenericEnumList.GetSelectValueEnum<TransactionType>(accountTransactionType != null ? accountTransactionType.ToString() : "");
                ViewBag.listAccountTransactionMethod = GenericEnumList.GetSelectValueEnum<TransactionMethod>(accountTransactionMethod != null ? accountTransactionMethod.ToString() : "");
                ViewBag.listUserAccount = userAccountService.ReadAll(userAccountId);
            }
            catch (Exception exception)
            {
                SetMessageException(new ResultStatusOperation("", "", MessageTypeResult.Danger, false, exception), MessageTypeActionMethod.Index);
            }
        }
    }
}
