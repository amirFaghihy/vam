using Aban.Common;
using Aban.Common.Utility;
using Aban.Domain.Entities;
using Aban.Service.Interfaces;
using Aban.ViewModel;
using Aban.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using static Aban.Domain.Enumerations.Enumeration;

namespace Aban.Dashboard.Areas.Loans.Controllers
{
    [Area("Loans")]
    [Authorize(Roles = "Admin")]
    public class UserAccountController : GenericController
    {
        #region constructor

        private readonly IUserAccountService userAccountService;
        private readonly IUserAccountDepositWithdrawalService userAccountDepositWithdrawalService;
        private readonly IUserIdentityService userIdentityService;
        private readonly ICharityLoanService charityLoanService;
        private readonly ICharityLoanInstallmentsService charityLoanInstallmentsService;


        public UserAccountController(
            IUserAccountService userAccountService,
            IUserAccountDepositWithdrawalService userAccountDepositWithdrawalService,
            IUserIdentityService userIdentityService,
            ICharityLoanService charityLoanService,
            ICharityLoanInstallmentsService charityLoanInstallmentsService)
        {
            this.userAccountService = userAccountService;
            this.userAccountDepositWithdrawalService = userAccountDepositWithdrawalService;
            this.userIdentityService = userIdentityService;
            this.charityLoanService = charityLoanService;
            this.charityLoanInstallmentsService = charityLoanInstallmentsService;
        }

        #endregion

        [HttpGet]
        public IActionResult Index(
            string accountOwnerId = "",
            BankName? bankName = null,
            string title = "",
            double? accountNumber = null,
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
                ViewBag.accountOwnerId = accountOwnerId;
                ViewBag.bankName = bankName;
                ViewBag.title = title;
                ViewBag.accountNumber = accountNumber;
                ViewBag.registerDateFrom = registerDateFrom;
                ViewBag.registerDateTo = registerDateTo;


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

                //FillDropDown(bankName);
                #endregion

                Tuple<IQueryable<UserAccount>, ResultStatusOperation> result = userAccountService.SpecificationGetData(accountOwnerId, bankName, title, accountNumber, registerDateFrom, registerDateTo);
                return View(userAccountService.Pagination(result.Item1, true, pageNumber, pageSize, isDesc, sortColumn));
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
                return View(new UserAccount());
            }
            catch (Exception exception)
            {
                SetMessageException(new ResultStatusOperation("", "", MessageTypeResult.Danger, false, exception), MessageTypeActionMethod.Index);
                return RedirectToAction("ShowException", "Error", new { area = "" });
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserAccount model)
        {
            try
            {

                Tuple<UserAccount, ResultStatusOperation> resultFillModel = userAccountService.FillModel(model);
                resultFillModel = await userAccountService.Insert(fillControllerInfo(), resultFillModel.Item1);
                switch (resultFillModel.Item2.Type)
                {
                    case MessageTypeResult.Success:
                        {
                            SetMessage(resultFillModel.Item2);
                            return RedirectToAction("Index");
                        }
                    case MessageTypeResult.Danger:
                        {
                            SetMessageException(resultFillModel.Item2, MessageTypeActionMethod.CreatePost);
                            return RedirectToAction("ShowException", "Error");
                        }
                    case MessageTypeResult.Warning:
                        {
                            FillDropDown();
                            SetMessage(resultFillModel.Item2);
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
                Tuple<UserAccount, ResultStatusOperation> resultFindModel = await userAccountService.Find(id.Value);
                switch (resultFindModel.Item2.Type)
                {
                    case MessageTypeResult.Success:
                        {
                            //Pass Parameter like ViewBag.cityId = resultFindModel.Item1.CityId;
                            FillDropDown(resultFindModel.Item1.BankName);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserAccount model)
        {
            try
            {

                Tuple<UserAccount, ResultStatusOperation> resultEdit = await userAccountService.Update(fillControllerInfo(), model);
                switch (resultEdit.Item2.Type)
                {
                    case MessageTypeResult.Success:
                        {
                            SetMessage(resultEdit.Item2);
                            return RedirectToAction(nameof(Index));
                        }
                    case MessageTypeResult.Danger:
                        {
                            SetMessageException(resultEdit.Item2, MessageTypeActionMethod.EditPost);
                            return RedirectToAction("ShowException", "Error");
                        }
                    case MessageTypeResult.Warning:
                        {
                            SetMessage(resultEdit.Item2);
                            FillDropDown();
                            return View(resultEdit.Item1);
                        }
                }
                SetMessage(resultEdit.Item2);
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
        //        Tuple<UserAccount, ResultStatusOperation> resultFindModel = await userAccountService.Find(id.Value);
        //        SetMessage(userAccountService.LogicDelete(resultFindModel.Item1));
        //        return RedirectToAction("Index");
        //    }
        //    catch (Exception exception)
        //    {
        //        SetMessageException(new ResultStatusOperation("", "", MessageTypeResult.Danger, false, exception), MessageTypeActionMethod.Index);
        //        return RedirectToAction("ShowException", "Error");
        //    }
        //}


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

            List<UserIdentity> userData = userIdentityService.SpecificationGetData(userIds).Result.Item1.ToList();

            List<UserIdentityViewModel> resultViewModel = userData.Select(x => new UserIdentityViewModel
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                FatherName = x.FatherName
            }).ToList();


            return Json(new { data = resultViewModel });
        }


        /// <summary>
        /// آخرین باقیمانده حسابها
        /// (موجودی حساب = (اقساط پرداخت شده) + (مبلغ وام با درصد سود) - موجودی حساب)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetLatestRemaining(string stringAccountIds)
        {

            if (stringAccountIds.IsNullOrEmpty())
            {
                return Json(null);
            }

#pragma warning disable CS8604

            List<int> accountIds =
                (new List<string>(JsonConvert.DeserializeObject<string[]>(stringAccountIds)))
                .Select(int.Parse).ToList();

#pragma warning restore CS8604

            List<LatestDepositWithdrawal> latestDepositWithdrawal = new List<LatestDepositWithdrawal>();

            List<UserAccountDepositWithdrawal> depositWithdrawal =
                userAccountDepositWithdrawalService.SpecificationGetData(accountIds).Item1.ToList();

            // لیست وام‌های کاربر
            List<CharityLoan> listCharityLoans = charityLoanService.SpecificationGetData(
                lstLoanReceiverId: depositWithdrawal.Select(x => x.UserAccount!.AccountOwnerId).ToList(),
                isdone: false).Item1.ToList();

            // لیست اقساط پرداخت شده‌ی وام
            List<CharityLoanInstallments> listCharityLoanInstallments = charityLoanInstallmentsService.SpecificationGetData(
                listCharityLoanId: listCharityLoans.Select(x => x.Id).ToList(),
                isdone: true).Item1.ToList();

            foreach (var item in depositWithdrawal.OrderByDescending(x => x.RegisterDate).GroupBy(x => x.UserAccountId))
            {
                if (listCharityLoans.Where(x => x.LoanReceiverId == item.FirstOrDefault()!.UserAccount!.AccountOwnerId).Count() != 0)
                {

                    double singleTotalPrice = 0;
                    foreach (var charityLoan in listCharityLoans.Where(x => x.LoanReceiverId == item.FirstOrDefault()!.UserAccount!.AccountOwnerId))
                    {
                        if (charityLoan != null)
                        {
                            // مبلغ وام + سود وام
                            double loanAmount = ((charityLoan.LoanAmount * charityLoan.PercentSalary) / 100) + charityLoan.LoanAmount;

                            // جمع اقساط پرداخت شده وام
                            double loanInstallmentsAmount = listCharityLoanInstallments.Where(
                                x => x.CharityLoanId == charityLoan.Id
                                ).Select(x => x.InstallmentAmount).ToList().Sum();


                            singleTotalPrice += (loanAmount - loanInstallmentsAmount);

                        }
                    }
                    // باقیمانده حساب منهای وام بعلاوه اقساط پرداخت شده
                    latestDepositWithdrawal.Add(new LatestDepositWithdrawal()
                    {
                        UserAccountId = item.FirstOrDefault()!.UserAccountId,
                        TotalPriceAfterTransaction = item.FirstOrDefault()!.TotalPriceAfterTransaction - singleTotalPrice
                    });
                }
                else
                {
                    // باقیمانده حساب بدون محاسبه وام و اقساط پرداخت شده
                    latestDepositWithdrawal.Add(new LatestDepositWithdrawal()
                    {
                        UserAccountId = item.FirstOrDefault()!.UserAccountId,
                        TotalPriceAfterTransaction = item.FirstOrDefault()!.TotalPriceAfterTransaction
                    });
                }
            }

            if (depositWithdrawal.Count() != 0)
            {
                return Json(new
                {
                    depositWithdrawal = latestDepositWithdrawal
                }
                );
            }
            else
                return Json(new { });
        }


        /// <summary>
        /// آخرین شماره حساب
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetLatestAccountNumber()
        {
            bool isSuccess = false;
            UserAccount? userAccount =
                userAccountService.GetAll().OrderByDescending(x => x.AccountNumber).FirstOrDefault();

            if (userAccount != null)
            {
                isSuccess = true;
                return Json(new { isSuccess = isSuccess, latestAccountNumber = userAccount.AccountNumber });
            }
            else
                return Json(new { isSuccess = isSuccess });
        }

        private void FillDropDown(BankName? bankName = null, string accountOwnerId = "")
        {
            try
            {
#pragma warning disable CS8604
                ViewBag.listBankName = GenericEnumList.GetSelectValueEnum<BankName>(bankName != null ? bankName.ToString() : "");
#pragma warning disable CS8604
                ViewBag.listAccountOwner = userIdentityService.ReadAllWithFatherName(accountOwnerId);
            }
            catch (Exception exception)
            {
                SetMessageException(new ResultStatusOperation("", "", MessageTypeResult.Danger, false, exception), MessageTypeActionMethod.Index);
            }
        }

        [HttpPost("/filldropdownajax")]
        public JsonResult FillDropDownAjax(BankName? bankName = null, string accountOwnerId = "")
        {
            try
            {
#pragma warning disable CS8604
                List<SelectListItem> listBankName =
                    GenericEnumList.GetSelectValueEnum<BankName>(bankName != null ? bankName.ToString() : "");
#pragma warning restore CS8604
                List<SelectListItem> listAccountOwner =
                    userIdentityService.ReadAllWithFatherName(accountOwnerId);

                return Json(new
                {
                    listBankName = listBankName,
                    listAccountOwner = listAccountOwner
                });
            }
            catch (Exception exception)
            {
                SetMessageException(new ResultStatusOperation("", "", MessageTypeResult.Danger, false, exception), MessageTypeActionMethod.Index);
                return Json(new { });
            }
        }
    }
}
