using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aban.Common;
using Aban.Common.Utility;
using Aban.Domain;
using Aban.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Aban.Domain.Enumerations;
using Aban.Service.Interfaces;
using Aban.Domain.Configuration;
using Microsoft.Extensions.Options;
using static Aban.Domain.Enumerations.Enumeration;
using Aban.Service.Services;

namespace Aban.Dashboard.Areas.Blog.Controllers
{
    [Area("Blog")]
    [Authorize(Roles = "Admin")]
    public class BlogArticleController : GenericController
    {
        #region constructor

        IOptions<PathsConfiguration> pathsConfiguratin;
        private readonly IBlogArticleService blogArticleService;
        private readonly IUserIdentityService userIdentityService;


        public BlogArticleController(
            IBlogArticleService blogArticleService,
            IUserIdentityService userIdentityService,
            IOptions<PathsConfiguration> pathsConfiguratin
            ) : base(pathsConfiguratin)
        {
            this.userIdentityService = userIdentityService;
            this.blogArticleService = blogArticleService;
            this.pathsConfiguratin = pathsConfiguratin;
        }

        #endregion

        [HttpGet]
        public async Task<IActionResult> Index(
            string userIdentityId = "",
            string writerId = "",
            string title = "",
            string registerDateFrom = "",
            string registerDateTo = "",
            bool? isvisible = null,
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
                ViewBag.writerId = writerId;
                ViewBag.title = title;
                ViewBag.registerDateFrom = registerDateFrom;
                ViewBag.registerDateTo = registerDateTo;
                ViewBag.isvisible = isvisible;


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
                    _registerDateFrom = await registerDateFrom.ToConvertPersianDateToDateTimeAsync(DateTimeFormat.yyyy_mm_dd, DateTimeSpiliter.slash);
                }
                if (!string.IsNullOrEmpty(registerDateTo))
                {
                    _registerDateTo = await registerDateTo.ToConvertPersianDateToDateTimeAsync(DateTimeFormat.yyyy_mm_dd, DateTimeSpiliter.slash);
                }

                #endregion

                Tuple<IQueryable<BlogArticle>, ResultStatusOperation> result = blogArticleService.SpecificationGetData(userIdentityId, writerId, title, _registerDateFrom, _registerDateTo, isvisible);

                SetMessage(result.Item2);

                return View(blogArticleService.Pagination(result.Item1, true, pageNumber, pageSize, isDesc, sortColumn));
            }
            catch (Exception exception)
            {
                SetMessageException(new ResultStatusOperation("", "", MessageTypeResult.Danger, false, exception), MessageTypeActionMethod.Index);
                return RedirectToAction("ShowException", "Error", new { area = "" });
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [Authorize]
        public async Task<IActionResult> LatestBlogArticles(
            string userIdentityId = "",
            string writerId = "",
            string title = "",
            string registerDateFrom = "",
            string registerDateTo = "",
            bool? isvisible = null,
            int pageNumber = 1,
            int pageSize = 20,
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
                ViewBag.writerId = writerId;
                ViewBag.title = title;
                ViewBag.registerDateFrom = registerDateFrom;
                ViewBag.registerDateTo = registerDateTo;
                ViewBag.isvisible = isvisible;


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
                    _registerDateFrom = await registerDateFrom.ToConvertPersianDateToDateTimeAsync(DateTimeFormat.yyyy_mm_dd, DateTimeSpiliter.slash);
                }
                if (!string.IsNullOrEmpty(registerDateTo))
                {
                    _registerDateTo = await registerDateTo.ToConvertPersianDateToDateTimeAsync(DateTimeFormat.yyyy_mm_dd, DateTimeSpiliter.slash);
                }

                #endregion

                Tuple<IQueryable<BlogArticle>, ResultStatusOperation> result = blogArticleService.SpecificationGetData(userIdentityId, writerId, title, _registerDateFrom, _registerDateTo, isvisible);

                SetMessage(result.Item2);

                return View(blogArticleService.Pagination(result.Item1, true, pageNumber, pageSize, isDesc, sortColumn));
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
                return View(new BlogArticle());
            }
            catch (Exception exception)
            {
                SetMessageException(new ResultStatusOperation("", "", MessageTypeResult.Danger, false, exception), MessageTypeActionMethod.Index);
                return RedirectToAction("ShowException", "Error", new { area = "" });
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogArticle model)
        {
            try
            {
                model.UserIdentityId = GetUserId();

                Tuple<BlogArticle, ResultStatusOperation> resultFillModel = blogArticleService.FillModel(model);
                resultFillModel = await blogArticleService.Insert(fillControllerInfo(new List<string>() { "UserIdentityId", "RegisterDate" }), resultFillModel.Item1);

                SetMessage(resultFillModel.Item2);
                switch (resultFillModel.Item2.Type)
                {
                    case MessageTypeResult.Success:

                        //TODO: Add Category for BlogArticle

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
        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                if (id.IsNullOrEmpty())
                {
                    SetMessageEditnotFound();
                    return RedirectToAction("Index");
                }
                Tuple<BlogArticle, ResultStatusOperation> resultFindModel = await blogArticleService.Find(Guid.Parse(id));

                SetMessage(resultFindModel.Item2);
                switch (resultFindModel.Item2.Type)
                {
                    case MessageTypeResult.Success:

                        //TODO: Read Category for BlogArticle

                        ViewBag.writerId = resultFindModel.Item1.WriterId;
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
        public async Task<IActionResult> Edit(BlogArticle model)
        {
            try
            {
                model.UserIdentityId = GetUserId();

                Tuple<BlogArticle, ResultStatusOperation> resultEdit = await blogArticleService.Update(fillControllerInfo(new List<string>() { "UserIdentityId" }), model);

                SetMessage(resultEdit.Item2);
                switch (resultEdit.Item2.Type)
                {
                    case MessageTypeResult.Success:

                        //TODO: Add Category for BlogArticle

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
                Tuple<BlogArticle, ResultStatusOperation> resultFindModel = await blogArticleService.Find(id.Value);
                SetMessage(resultFindModel.Item2);
                if (resultFindModel.Item1 != null)
                {
                    SetMessage(blogArticleService.Update(true, resultFindModel.Item1).Result.Item2);
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
                string userIdentity = ViewBag.userIdentity;
                string writerId = ViewBag.writerId;

                ViewBag.listUserIdentity = userIdentityService.ReadAll(userIdentity, null, null, new List<RoleName>() { RoleName.Admin });
                ViewBag.listWriter = userIdentityService.ReadAll(writerId, null, null, new List<RoleName>() { RoleName.Admin });

            }
            catch (Exception exception)
            {
                SetMessageException(new ResultStatusOperation("", "", MessageTypeResult.Danger, false, exception), MessageTypeActionMethod.Index);
            }
        }
    }
}
