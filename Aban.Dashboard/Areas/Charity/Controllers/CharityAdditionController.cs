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
    public class CharityAdditionController : GenericController
    {
        #region constructor

        private readonly ICharityAdditionService charityAdditionService;
        private readonly IUserIdentityService userIdentityService;


        public CharityAdditionController(
            ICharityAdditionService charityAdditionService,
            IUserIdentityService _userIdentityService)
        {
            userIdentityService = _userIdentityService;

            this.charityAdditionService = charityAdditionService;
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
            DateTime? documentRegisterDateTimeFrom = null,
            DateTime? documentRegisterDateTimeTo = null,
            DateTime? registerDateFrom = null,
            DateTime? registerDateTo = null,
            DateTime? timeForActionFrom = null,
            DateTime? timeForActionTo = null,
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
                ViewBag.documentRegisterDateTimeFrom = documentRegisterDateTimeFrom;
                ViewBag.documentRegisterDateTimeTo = documentRegisterDateTimeTo;
                ViewBag.registerDateFrom = registerDateFrom;
                ViewBag.registerDateTo = registerDateTo;
                ViewBag.timeForActionFrom = timeForActionFrom;
                ViewBag.timeForActionTo = timeForActionTo;


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

                Tuple<IQueryable<CharityAddition>, ResultStatusOperation> result = charityAdditionService.SpecificationGetData(userIdentityId, userIdentityReciverId, title, amount, accountNumber, description, documentRegisterDateTimeFrom, documentRegisterDateTimeTo, registerDateFrom, registerDateTo);
                return View(charityAdditionService.Pagination(result.Item1, true, pageNumber, pageSize, isDesc, sortColumn));
                //return View(result.Item1.ToPagedList(pageNumber, pageSize));
            }
            catch (Exception exception)
            {
                SetMessageException(new ResultStatusOperation("", "", Enumeration.MessageTypeResult.Danger, false, exception), Enumeration.MessageTypeActionMethod.Index);
                return RedirectToAction("ShowException", "Error", new { area = "" });
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            try
            {
                FillDropDown();
                return View(new CharityAddition());
            }
            catch (Exception exception)
            {
                SetMessageException(new ResultStatusOperation("", "", Enumeration.MessageTypeResult.Danger, false, exception), Enumeration.MessageTypeActionMethod.Index);
                return RedirectToAction("ShowException", "Error", new { area = "" });
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CharityAddition model,
            string documentRegisterDateTimeString = "",
            string documentRegisterDateTimeTime = "")
        {
            try
            {
                #region DateTime Convertor
                if (!string.IsNullOrEmpty(documentRegisterDateTimeString) && !string.IsNullOrEmpty(documentRegisterDateTimeTime))
                {
                    DateTime _timeForAction = documentRegisterDateTimeString.ToConvertPersianDateToDateTime(Enumeration.DateTimeFormat.yyyy_mm_dd, Enumeration.DateTimeSpiliter.slash);
                    TimeSpan _timeForActionTime = documentRegisterDateTimeTime.ToConvertStringToTime();
                    model.DocumentRegisterDateTime = _timeForAction.MergeDateAndTime(_timeForActionTime);
                }

                #endregion

                Tuple<CharityAddition, ResultStatusOperation> resultFillModel = charityAdditionService.FillModel(model);

                resultFillModel.Item1.UserIdentityId = GetUserId();

                resultFillModel = await charityAdditionService.Insert(fillControllerInfo(new List<string> { "UserIdentityId", "UserIdentity", "UserIdentityReciver" }), resultFillModel.Item1);

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
                SetMessageException(new ResultStatusOperation("", "", Enumeration.MessageTypeResult.Danger, false, exception), Enumeration.MessageTypeActionMethod.Index);
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
                Tuple<CharityAddition, ResultStatusOperation> resultFindModel = await charityAdditionService.Find(id.Value);


                SetMessage(resultFindModel.Item2);
                switch (resultFindModel.Item2.Type)
                {
                    case MessageTypeResult.Success:
                        //Pass Parameter like ViewBag.cityId = resultFindModel.Item1.CityId;
                        FillDropDown();
                        return View(resultFindModel.Item1);

                    case MessageTypeResult.Danger:
                    case MessageTypeResult.Warning:
                        return RedirectToAction("Index");

                    default:
                        return RedirectToAction("Index");

                }
            }
            catch (Exception exception)
            {
                SetMessageException(new ResultStatusOperation("", "", Enumeration.MessageTypeResult.Danger, false, exception), Enumeration.MessageTypeActionMethod.Index);
                return RedirectToAction("ShowException", "Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CharityAddition model)
        {
            try
            {
                #region DateTime Convertor

                //DateTime _timeForAction = timeForActionString.ToConvertPersianDateToDateTime(Enumeration.DateTimeFormat.yyyy_mm_dd, Enumeration.DateTimeSpiliter.slash);
                //TimeSpan _timeForActionTime = timeForActionTime.ToConvertStringToTime();
                //model.TimeForAction = _timeForAction.MergeDateAndTime(_timeForActionTime);

                #endregion

                model.UserIdentityId = GetUserId();

                Tuple<CharityAddition, ResultStatusOperation> resultEdit = await charityAdditionService.Update(fillControllerInfo(new List<string> { "UserIdentityId", "UserIdentity", "UserIdentityReciver" }), model);

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
                SetMessageException(new ResultStatusOperation("", "", Enumeration.MessageTypeResult.Danger, false, exception), Enumeration.MessageTypeActionMethod.Index);
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
                Tuple<CharityAddition, ResultStatusOperation> resultFindModel = await charityAdditionService.Find(id.Value);
                SetMessage(resultFindModel.Item2);
                if (resultFindModel.Item1 != null)
                {
                    resultFindModel.Item1.IsDelete = true;
                    SetMessage(charityAdditionService.Update(true, resultFindModel.Item1).Result.Item2);
                }
                return RedirectToAction("Index");
            }
            catch (Exception exception)
            {
                SetMessageException(new ResultStatusOperation("", "", Enumeration.MessageTypeResult.Danger, false, exception), Enumeration.MessageTypeActionMethod.Index);
                return RedirectToAction("ShowException", "Error");
            }
        }

        private void FillDropDown()
        {
            try
            {
                string userIdentityId = ViewBag.userIdentityId;
                string userIdentityReciverId = ViewBag.userIdentityReciverId;


                ViewBag.listUserIdentity = userIdentityService.ReadAllWithFatherName(userIdentityId, true, false, new List<RoleName>() { RoleName.Admin });
                ViewBag.listUserIdentityReciver = userIdentityService.ReadAllWithFatherName(userIdentityReciverId, true, false, new List<RoleName>() { RoleName.Foreman, RoleName.Clerk });

            }
            catch (Exception exception)
            {
                SetMessageException(new ResultStatusOperation("", "", Enumeration.MessageTypeResult.Danger, false, exception), Enumeration.MessageTypeActionMethod.Index);
            }
        }
    }
}
