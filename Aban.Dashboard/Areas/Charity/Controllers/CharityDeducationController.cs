using Aban.Common;
using Aban.Common.Utility;
using Aban.Domain.Entities;
using Aban.Domain.Enumerations;
using Aban.Service.Interfaces;
using Aban.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Aban.Domain.Enumerations.Enumeration;

namespace Aban.Dashboard.Areas.Charity.Controllers
{
    [Area("Charity")]
    [Authorize]
    public class CharityDeducationController : GenericController
    {
        #region constructor

        private readonly ICharityDeducationService charityDeducationService;
        private readonly IUserIdentityService userIdentityService;


        public CharityDeducationController(
            ICharityDeducationService charityDeducationService,
            IUserIdentityService _userIdentityService)
        {
            userIdentityService = _userIdentityService;

            this.charityDeducationService = charityDeducationService;
        }

        #endregion

        [HttpGet]
        public IActionResult Index(
            string userIdentityId = "",
            string userIdentityReciverId = "",
            string title = "",
            double amount = 0,
            string accountNumber = "",
            string description = "",
            DateTime? timeForActionFrom = null,
            DateTime? timeForActionTo = null,
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
                ViewBag.userIdentityId = userIdentityId;
                ViewBag.userIdentityReciverId = userIdentityReciverId;
                ViewBag.title = title;
                ViewBag.amount = amount;
                ViewBag.accountNumber = accountNumber;
                ViewBag.description = description;
                ViewBag.timeForActionFrom = timeForActionFrom;
                ViewBag.timeForActionTo = timeForActionTo;
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

                Tuple<IQueryable<CharityDeducation>, ResultStatusOperation> result =
                    charityDeducationService.SpecificationGetData(userIdentityId, userIdentityReciverId, title, amount, accountNumber, description, timeForActionFrom, timeForActionTo, registerDateFrom, registerDateTo);

                return View(charityDeducationService.Pagination(result.Item1, true, pageNumber, pageSize, isDesc, sortColumn));
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
                return View(new CharityDeducation());
            }
            catch (Exception exception)
            {
                SetMessageException(new ResultStatusOperation("", "", MessageTypeResult.Danger, false, exception), MessageTypeActionMethod.Index);
                return RedirectToAction("ShowException", "Error", new { area = "" });
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CharityDeducation model, string timeForActionString, string timeForActionTime)
        {
            try
            {
                #region DateTime Convertor

                DateTime _timeForAction = timeForActionString.ToConvertPersianDateToDateTime(DateTimeFormat.yyyy_mm_dd, DateTimeSpiliter.slash);
                TimeSpan _timeForActionTime = timeForActionTime.ToConvertStringToTime();
                model.TimeForAction = _timeForAction.MergeDateAndTime(_timeForActionTime);

                #endregion

                Tuple<CharityDeducation, ResultStatusOperation> resultFillModel = charityDeducationService.FillModel(model);

                resultFillModel.Item1.UserIdentityId = GetUserId();

                resultFillModel = await charityDeducationService.Insert(fillControllerInfo(new List<string> { "UserIdentity", "UserIdentityId", "UserIdentityReciver" }), resultFillModel.Item1);

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
                Tuple<CharityDeducation, ResultStatusOperation> resultFindModel = await charityDeducationService.Find(id.Value);

                SetMessage(resultFindModel.Item2);
                switch (resultFindModel.Item2.Type)
                {
                    case MessageTypeResult.Success:
                        ViewBag.userIdentityReciverId = resultFindModel.Item1.UserIdentityReciverId;
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
        public IActionResult Edit(
            CharityDeducation model,
            string timeForActionString,
            string timeForActionTime)
        {
            try
            {
                #region DateTime Convertor

                DateTime _timeForAction = timeForActionString.ToConvertPersianDateToDateTime(DateTimeFormat.yyyy_mm_dd, DateTimeSpiliter.slash);
                TimeSpan _timeForActionTime = timeForActionTime.ToConvertStringToTime();
                model.TimeForAction = _timeForAction.MergeDateAndTime(_timeForActionTime);

                #endregion

                model.UserIdentityId = GetUserId();

                Tuple<CharityDeducation, ResultStatusOperation> resultEdit =
                    charityDeducationService.Update(fillControllerInfo(new List<string> { "UserIdentity", "UserIdentityId", "UserIdentityReciver" }), model).Result;

                ViewBag.userIdentityReciverId = resultEdit.Item1.UserIdentityReciverId;
                FillDropDown();
                SetMessage(resultEdit.Item2);
                switch (resultEdit.Item2.Type)
                {
                    case MessageTypeResult.Success:
                        return RedirectToAction(nameof(Index));

                    case MessageTypeResult.Danger:
                    case MessageTypeResult.Warning:
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
                Tuple<CharityDeducation, ResultStatusOperation> resultFindModel = await charityDeducationService.Find(id.Value);
                SetMessage(resultFindModel.Item2);
                if (resultFindModel.Item1 != null)
                {
                    resultFindModel.Item1.IsDelete = true;
                    SetMessage(charityDeducationService.Update(true, resultFindModel.Item1).Result.Item2);
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
                string userIdentityId = ViewBag.userIdentityReciverId;
                string userIdentityReciverId = ViewBag.userIdentityId;

                ViewBag.listUserIdentity = userIdentityService.ReadAllWithFatherName(userIdentityId, true, false, new List<RoleName>() { RoleName.Admin });
                ViewBag.listUserIdentityReciver = userIdentityService.ReadAllWithFatherName(userIdentityReciverId, true, false, new List<RoleName>() { RoleName.Foreman, RoleName.Clerk });

            }
            catch (Exception exception)
            {
                SetMessageException(new ResultStatusOperation("", "", MessageTypeResult.Danger, false, exception), MessageTypeActionMethod.Index);
            }
        }
    }
}
