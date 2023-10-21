using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aban.Common;
using Aban.Common.Utility;
using Aban.Domain;
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
    public class UserAccountController : GenericController
    {
        #region constructor

        private readonly IUserAccountService userAccountService;


        public UserAccountController(IUserAccountService userAccountService)
        {
            this.userAccountService = userAccountService;
        }

        #endregion

        [HttpGet]
        public async Task<IActionResult> Index(
            string accountOwnerId = "",
            BankName? bankName = null,
            string title = "",
            string? accountNumber = null,
            DateTime? registerDateFrom = null,
            DateTime? registerDateTo = null,
            int pageNumber = 1,
            int pageSize = 10,
            string search = "",
            string sortColumn = "RegisterDate",
            string lastColumn = "",
            bool isDesc = true)
        {
            try
            {

                #region selectedValue

                ViewBag.pageNumber = pageNumber;
                ViewBag.pageSize = pageSize;
                ViewBag.search = search;
                ViewBag.sortColumn = sortColumn;
                ViewBag.lastColumn = lastColumn;
                ViewBag.isDesc = isDesc;
                ViewBag.accountOwnerId = accountOwnerId;
                ViewBag.bankName = bankName;
                ViewBag.title = title;
                ViewBag.accountNumber = accountNumber;
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

                FillDropDown();
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
                            return RedirectToAction("Edit", new { id = resultEdit.Item1.Id });
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

        private void FillDropDown()
        {
            try
            {
                BankName bankName = ViewBag.bankName;

                ViewBag.listBankName = GenericEnumList.GetSelectValueEnum<BankName>(bankName.ToString());

            }
            catch (Exception exception)
            {
                SetMessageException(new ResultStatusOperation("", "", MessageTypeResult.Danger, false, exception), MessageTypeActionMethod.Index);
            }
        }
    }
}
