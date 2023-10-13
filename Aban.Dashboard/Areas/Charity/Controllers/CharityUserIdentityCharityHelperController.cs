using Aban.Common;
using Aban.Common.Utility;
using Aban.Domain.Entities;
using Aban.Domain.Enumerations;
using Aban.Service.Interfaces;
using Aban.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using static Aban.Domain.Enumerations.Enumeration;

namespace Aban.Dashboard.Areas.Charity.Controllers
{
    [Area("Charity")]
    [Authorize]
    public class CharityUserIdentityCharityHelperController : GenericController
    {
        #region constructor

        private readonly ICharityUserIdentityCharityHelperService charityUserIdentityCharityHelperService;
        private readonly IUserIdentityService userIdentityService;


        public CharityUserIdentityCharityHelperController(
            ICharityUserIdentityCharityHelperService charityUserIdentityCharityHelperService,
            IUserIdentityService _userIdentityService)
        {
            userIdentityService = _userIdentityService;

            this.charityUserIdentityCharityHelperService = charityUserIdentityCharityHelperService;
        }

        #endregion

        [HttpGet]
        public IActionResult Index(
            string userIdentityId = "",
            string helperId = "",
            string registerDateFrom = "",
            string registerDateTo = "",
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
                ViewBag.userIdentityId = userIdentityId;
                ViewBag.helperId = helperId;
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


                Tuple<IQueryable<CharityUserIdentityCharityHelper>, ResultStatusOperation> result = charityUserIdentityCharityHelperService.SpecificationGetData(userIdentityId, helperId, _registerDateFrom, _registerDateTo);

                return View(charityUserIdentityCharityHelperService.Pagination(result.Item1, true, pageNumber, pageSize, isDesc, sortColumn));
            }
            catch (Exception exception)
            {
                SetMessageException(new ResultStatusOperation("", "", Enumeration.MessageTypeResult.Danger, false, exception), Enumeration.MessageTypeActionMethod.Index);
                return RedirectToAction("ShowException", "Error", new { area = "" });
            }
        }


        [HttpGet]
        public IActionResult GetUserAllHelper(
            string userIdentityId = "",
            string helperId = "",
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
                ViewBag.helperId = helperId;
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

                IQueryable<UserIdentity> result = charityUserIdentityCharityHelperService.GetUserAllHelper(userIdentityId);

                UserIdentity userIdentity = userIdentityService.Find(userIdentityId).Result.Item1;

                return View(Tuple.Create(userIdentityService.Pagination(result, true, pageNumber, pageSize, isDesc, sortColumn), userIdentity));
            }
            catch (Exception exception)
            {
                SetMessageException(new ResultStatusOperation("", "", Enumeration.MessageTypeResult.Danger, false, exception), Enumeration.MessageTypeActionMethod.Index);
                return RedirectToAction("ShowException", "Error", new { area = "" });
            }
        }




        [HttpGet]
        public IActionResult GetAdminAllForeman(
            string userIdentityId = "",
            string helperId = "",
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
                ViewBag.helperId = helperId;
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

                FillDropDown();
                #endregion


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


                if (userIdentityId.IsNullOrEmpty())
                    userIdentityId = GetUserId();

                IQueryable<UserIdentity> result = charityUserIdentityCharityHelperService.GetAdminAllForeman(userIdentityId);

                UserIdentity userIdentity = userIdentityService.Find(userIdentityId).Result.Item1;

                return View(Tuple.Create(userIdentityService.Pagination(result, true, pageNumber, pageSize, isDesc, sortColumn), userIdentity));
            }
            catch (Exception exception)
            {
                SetMessageException(new ResultStatusOperation("", "", Enumeration.MessageTypeResult.Danger, false, exception), Enumeration.MessageTypeActionMethod.Index);
                return RedirectToAction("ShowException", "Error", new { area = "" });
            }
        }



        [HttpGet]
        public IActionResult GetForemanAllClerk(
            string userIdentityId = "",
            string clerkId = "",
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

                if (userIdentityId.IsNullOrEmpty())
                    userIdentityId = GetUserId();

                ViewBag.pageNumber = pageNumber;
                ViewBag.pageSize = pageSize;
                ViewBag.sortColumn = sortColumn;
                ViewBag.lastColumn = lastColumn;
                ViewBag.isDesc = isDesc;
                ViewBag.userIdentityId = userIdentityId;
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

                FillDropDownGetForemanAllClerk();
                #endregion

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

                IQueryable<UserIdentity> result = charityUserIdentityCharityHelperService.GetForemanAllClerk(userIdentityId, clerkId);

                UserIdentity userIdentity = userIdentityService.Find(userIdentityId).Result.Item1;

                return View(Tuple.Create(userIdentityService.Pagination(result, true, pageNumber, pageSize, isDesc, sortColumn), userIdentity));
            }
            catch (Exception exception)
            {
                SetMessageException(new ResultStatusOperation("", "", Enumeration.MessageTypeResult.Danger, false, exception), Enumeration.MessageTypeActionMethod.Index);
                return RedirectToAction("ShowException", "Error", new { area = "" });
            }
        }



        [HttpGet]
        public IActionResult GetClerkAllHelper(
            string userIdentityId = "",
            string helperId = "",
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
                ViewBag.helperId = helperId;
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

                //FillDropDown();

                ViewBag.listHelper = userIdentityService.ReadAll(userIdentityId, null, null, new List<RoleName>() { RoleName.Helper }, GetUserId());

                #endregion

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

                if (userIdentityId.IsNullOrEmpty())
                    userIdentityId = GetUserId();

                IQueryable<UserIdentity> result = charityUserIdentityCharityHelperService.GetClerkAllHelper(userIdentityId, helperId, _registerDateFrom, _registerDateTo);

                UserIdentity userIdentity = userIdentityService.Find(userIdentityId).Result.Item1;

                return View(Tuple.Create(userIdentityService.Pagination(result, true, pageNumber, pageSize, isDesc, sortColumn), userIdentity));
            }
            catch (Exception exception)
            {
                SetMessageException(new ResultStatusOperation("", "", Enumeration.MessageTypeResult.Danger, false, exception), Enumeration.MessageTypeActionMethod.Index);
                return RedirectToAction("ShowException", "Error", new { area = "" });
            }
        }




        [HttpGet]
        public IActionResult GetAdminAllHelper(
            string userIdentityId = "",
            string helperId = "",
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
                ViewBag.helperId = helperId;
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

                if (userIdentityId.IsNullOrEmpty())
                    userIdentityId = GetUserId();

                IQueryable<UserIdentity> result = charityUserIdentityCharityHelperService.GetAdminAllHelper(userIdentityId);

                UserIdentity userIdentity = userIdentityService.Find(userIdentityId).Result.Item1;

                return View(Tuple.Create(userIdentityService.Pagination(result, true, pageNumber, pageSize, isDesc, sortColumn), userIdentity));
            }
            catch (Exception exception)
            {
                SetMessageException(new ResultStatusOperation("", "", Enumeration.MessageTypeResult.Danger, false, exception), Enumeration.MessageTypeActionMethod.Index);
                return RedirectToAction("ShowException", "Error", new { area = "" });
            }
        }


        [HttpGet]
        public IActionResult GetForemanAllHelper(
            string userIdentityId = "",
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
                //ViewBag.helperId = helperId;
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

                if (userIdentityId.IsNullOrEmpty())
                    userIdentityId = GetUserId();

                IQueryable<UserIdentity> result = charityUserIdentityCharityHelperService.GetForemanAllHelper(userIdentityId);

                UserIdentity userIdentity = userIdentityService.Find(userIdentityId).Result.Item1;

                return View(Tuple.Create(userIdentityService.Pagination(result, true, pageNumber, pageSize, isDesc, sortColumn), userIdentity));
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
                return View(new CharityUserIdentityCharityHelper());
            }
            catch (Exception exception)
            {
                SetMessageException(new ResultStatusOperation("", "", Enumeration.MessageTypeResult.Danger, false, exception), Enumeration.MessageTypeActionMethod.Index);
                return RedirectToAction("ShowException", "Error", new { area = "" });
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CharityUserIdentityCharityHelper model)
        {
            try
            {

                Tuple<CharityUserIdentityCharityHelper, ResultStatusOperation> resultFillModel = charityUserIdentityCharityHelperService.FillModel(model);
                resultFillModel = await charityUserIdentityCharityHelperService.Insert(fillControllerInfo(), resultFillModel.Item1);

                SetMessage(resultFillModel.Item2);
                switch (resultFillModel.Item2.Type)
                {
                    case MessageTypeResult.Success:
                        return RedirectToAction(nameof(Index));

                    case MessageTypeResult.Danger:
                        SetMessageException(resultFillModel.Item2, Enumeration.MessageTypeActionMethod.CreatePost);
                        return RedirectToAction("ShowException", "Error");

                    case MessageTypeResult.Warning:
                        FillDropDown();
                        return View(resultFillModel.Item1);

                    default:
                        FillDropDown();
                        return View(resultFillModel.Item1);

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
                Tuple<CharityUserIdentityCharityHelper, ResultStatusOperation> resultFindModel = await charityUserIdentityCharityHelperService.Find(id.Value);

                SetMessage(resultFindModel.Item2);
                switch (resultFindModel.Item2.Type)
                {
                    case MessageTypeResult.Success:
                        //Pass Parameter like ViewBag.cityId = resultFindModel.Item1.CityId;
                        FillDropDown();
                        return View(resultFindModel.Item1);

                    case MessageTypeResult.Danger:
                        SetMessageException(resultFindModel.Item2, Enumeration.MessageTypeActionMethod.EditGet);
                        return RedirectToAction("ShowException", "Error");

                    case MessageTypeResult.Warning:
                        SetMessage(resultFindModel.Item2);
                        return RedirectToAction("Index");

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CharityUserIdentityCharityHelper model)
        {
            try
            {

                Tuple<CharityUserIdentityCharityHelper, ResultStatusOperation> resultEdit = await charityUserIdentityCharityHelperService.Update(fillControllerInfo(), model);

                SetMessage(resultEdit.Item2);
                switch (resultEdit.Item2.Type)
                {
                    case MessageTypeResult.Success:
                        return RedirectToAction("Edit", new { id = resultEdit.Item1.Id });

                    case MessageTypeResult.Danger:
                        SetMessageException(resultEdit.Item2, Enumeration.MessageTypeActionMethod.EditPost);
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
                Tuple<CharityUserIdentityCharityHelper, ResultStatusOperation> resultFindModel = await charityUserIdentityCharityHelperService.Find(id.Value);

                SetMessage(charityUserIdentityCharityHelperService.LogicDelete(resultFindModel.Item1));
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


                ViewBag.listUserIdentity = userIdentityService.ReadAll(userIdentityId);

            }
            catch (Exception exception)
            {
                SetMessageException(new ResultStatusOperation("", "", Enumeration.MessageTypeResult.Danger, false, exception), Enumeration.MessageTypeActionMethod.Index);
            }
        }

        private void FillDropDownGetForemanAllClerk()
        {
            try
            {
                string userIdentityId = ViewBag.userIdentityId;


                ViewBag.clerkList = charityUserIdentityCharityHelperService.ReadAllForemanAllClerk(userIdentityId);

            }
            catch (Exception exception)
            {
                SetMessageException(new ResultStatusOperation("", "", Enumeration.MessageTypeResult.Danger, false, exception), Enumeration.MessageTypeActionMethod.Index);
            }
        }
    }

}
