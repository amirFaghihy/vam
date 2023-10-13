using Aban.Common;
using Aban.Common.Utility;
using Aban.Domain.Entities;
using Aban.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Aban.Domain.Enumerations.Enumeration;

namespace Aban.Dashboard.Areas.Charity.Controllers
{

    [Area("Charity")]
    [Authorize(Roles = "Admin")]
    public class CharityAccountController : GenericController
    {

        private readonly ICharityAccountService _charityAccountService;
        public CharityAccountController(ICharityAccountService charityAccountService)
        {
            _charityAccountService = charityAccountService;
        }


        public IActionResult Index(
            string search = "",
            BankName? bankName = null,
            string registerDateFrom = "",
            string registerDateTo = "",
            bool isDesc = true,
            int pageNumber = 1,
            int pageSize = 10,
            string lastColumn = "",
            string sortColumn = "RegisterDate")
        {
            ViewBag.pageNumber = pageNumber;
            ViewBag.pageSize = pageSize;
            ViewBag.sortColumn = sortColumn;
            ViewBag.lastColumn = lastColumn;
            ViewBag.isDesc = isDesc;
            ViewBag.bankName = bankName;
            ViewBag.search = search;

            #region DateTime Convertor

            DateTime? _registerDateFrom = null;
            DateTime? _registerDateTo = null;


            if (!string.IsNullOrEmpty(registerDateFrom))
            {
                _registerDateFrom = registerDateFrom.ToConvertPersianDateToDateTimeAsync(DateTimeFormat.yyyy_mm_dd, DateTimeSpiliter.slash).Result;
            }
            if (!string.IsNullOrEmpty(registerDateTo))
            {
                _registerDateTo = registerDateTo.ToConvertPersianDateToDateTimeAsync(DateTimeFormat.yyyy_mm_dd, DateTimeSpiliter.slash).Result;
            }

            #endregion

            Tuple<IQueryable<CharityAccount>, ResultStatusOperation> resultSearch =
                _charityAccountService.SpecificationGetData(title: search, accountNumber: search, bankName: bankName, registerDateFrom: _registerDateFrom, registerDateTo: _registerDateTo);

            FillDropDown(bankName);

            return View(_charityAccountService.Pagination(resultSearch.Item1, false, pageNumber, pageSize, isDesc, sortColumn));
        }

        [HttpGet]
        public IActionResult Create()
        {
            FillDropDown();
            return View(new CharityAccount() { UserIdentityId = GetUserId() });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CharityAccount model)
        {
#pragma warning disable CS0168 // Variable is declared but never used
            try
            {
                //model.UserIdentityId = GetUserId();
                Tuple<CharityAccount, ResultStatusOperation> result = _charityAccountService.FillModel(model);

                Tuple<CharityAccount, ResultStatusOperation> resultAdd = await
                    _charityAccountService.Insert(fillControllerInfo("UserIdentity"), result.Item1);

                SetMessage(resultAdd.Item2);
                switch (resultAdd.Item2.Type)
                {
                    case Domain.Enumerations.Enumeration.MessageTypeResult.Success:
                        return RedirectToAction(nameof(Index));

                    case Domain.Enumerations.Enumeration.MessageTypeResult.Danger:
                    case Domain.Enumerations.Enumeration.MessageTypeResult.Warning:
                        FillDropDown(resultAdd.Item1.BankName);
                        return View(resultAdd.Item1);

                    default:
                        return RedirectToAction(nameof(Index));
                }

            }
            catch (Exception excepetion)
            {
                return RedirectToAction(nameof(Index));
            }
#pragma warning restore CS0168 // Variable is declared but never used

            //return View(new CharityAccount());
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {

            var resultFind = await _charityAccountService.Find(id);

            SetMessage(resultFind.Item2);
            switch (resultFind.Item2.Type)
            {
                case Domain.Enumerations.Enumeration.MessageTypeResult.Success:
                    FillDropDown(resultFind.Item1.BankName);
                    return View(resultFind.Item1);

                case Domain.Enumerations.Enumeration.MessageTypeResult.Danger:
                case Domain.Enumerations.Enumeration.MessageTypeResult.Warning:
                    return RedirectToAction(nameof(Index));

                default:
                    return RedirectToAction(nameof(Index));
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CharityAccount model)
        {

#pragma warning disable CS0168 // Variable is declared but never used
            try
            {
                //model.UserIdentityId = GetUserId();
                Tuple<CharityAccount, ResultStatusOperation> result = _charityAccountService.FillModel(model);


                Tuple<CharityAccount, ResultStatusOperation> resultUpdate = await
                    _charityAccountService.Update(base.fillControllerInfo("UserIdentity"), result.Item1);

                SetMessage(resultUpdate.Item2);
                switch (resultUpdate.Item2.Type)
                {
                    case MessageTypeResult.Success:
                        return RedirectToAction(nameof(Index));

                    case MessageTypeResult.Danger:
                    case MessageTypeResult.Warning:
                        FillDropDown(resultUpdate.Item1.BankName);
                        return View(resultUpdate.Item1);

                    default:
                        return RedirectToAction(nameof(Index));
                }

            }
            catch (Exception excepetion)
            {
                return RedirectToAction(nameof(Index));

            }
#pragma warning restore CS0168 // Variable is declared but never used
        }


        public async Task<IActionResult> Delete(int id)
        {
            var resultFind = await _charityAccountService.LogicDeleteAllRelatedData(id);

            SetMessage(resultFind);
            switch (resultFind.Type)
            {
                case MessageTypeResult.Success:
                    return RedirectToAction(nameof(Index));

                case MessageTypeResult.Danger:
                case MessageTypeResult.Warning:
                    return RedirectToAction(nameof(Index));

                default:
                    return RedirectToAction(nameof(Index));
            }
        }

        public void FillDropDown(BankName? bankName = null)
        {
#pragma warning disable CS8604 // Possible null reference argument.
            ViewBag.listBankName = bankName != null ? GenericEnumList.GetSelectValueEnum<BankName>(bankName.ToString())
                : GenericEnumList.GetSelectValueEnum<BankName>();
#pragma warning restore CS8604 // Possible null reference argument.
        }
    }
}
