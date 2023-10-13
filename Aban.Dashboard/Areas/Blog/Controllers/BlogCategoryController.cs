using Aban.Common;
using Aban.Domain.Configuration;
using Aban.Domain.Entities;
using Aban.Domain.Enumerations;
using Aban.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Aban.Dashboard.Areas.Blog.Controllers
{
    [Area("Blog")]
    [Authorize(Roles = "Admin")]
    public class BlogCategoryController : GenericController
    {
        #region Constructor

        private readonly IBlogCategoryService blogCategoryService;
        IOptions<PathsConfiguration> pathsConfiguratin;


        public BlogCategoryController(IBlogCategoryService blogCategoryService,
            IOptions<PathsConfiguration> pathsConfiguratin
            ) : base(pathsConfiguratin)
        {
            this.blogCategoryService = blogCategoryService;
            this.pathsConfiguratin = pathsConfiguratin;
        }

        #endregion

        [HttpGet]
        //[ActionName("")] // حتما بررسی شود
        public async Task<IActionResult> Index(
            string? userIdentityId = "",
            int? parentId = null,
            string? title = null,
            bool? isvisible = null,
            string? blogCategoryURL = null,
            int pageNumber = 1,
            int pageSize = 10,
            string search = "",
            string sortColumn = "Id",
            string lastColumn = "",
            bool isDesc = true,
            List<int>? categoryId = null,
            int brandId = 0)
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
                ViewBag.parentId = parentId;
                ViewBag.title = title;
                ViewBag.isvisible = isvisible;
                ViewBag.blogCategoryURL = blogCategoryURL;
                ViewBag.categoryId = categoryId;


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

                Tuple<IQueryable<BlogCategory>, ResultStatusOperation> result = await blogCategoryService.SpecificationGetData(userIdentityId, parentId, title, isvisible, blogCategoryURL);

                return View(await blogCategoryService.Paginationasync(result.Item1, true, pageNumber, pageSize, isDesc, sortColumn));
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
                return View(new BlogCategory());
            }
            catch (Exception exception)
            {
                SetMessageException(new ResultStatusOperation("", "", Enumeration.MessageTypeResult.Danger, false, exception), Enumeration.MessageTypeActionMethod.Index);
                return RedirectToAction("ShowException", "Error", new { area = "" });
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogCategory model)
        {
            try
            {

                model.UserIdentityId = GetUserId();
                Tuple<BlogCategory, ResultStatusOperation> resultFillModel = await blogCategoryService.FillModel(model);
                ModelState.Remove(nameof(model.UserIdentityId));

                resultFillModel = await blogCategoryService.Insert(fillControllerInfo(), resultFillModel.Item1);
                switch (resultFillModel.Item2.Type)
                {
                    case Enumeration.MessageTypeResult.Success:
                        {
                            SetMessage(resultFillModel.Item2);
                            return RedirectToAction("Index");
                        }
                    case Enumeration.MessageTypeResult.Danger:
                        {
                            SetMessageException(resultFillModel.Item2, Enumeration.MessageTypeActionMethod.CreatePost);
                            return RedirectToAction("ShowException", "Error");
                        }
                    case Enumeration.MessageTypeResult.Warning:
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
                SetMessageException(new ResultStatusOperation("", "", Enumeration.MessageTypeResult.Danger, false, exception), Enumeration.MessageTypeActionMethod.Index);
                return RedirectToAction("ShowException", "Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null || id == 0)
                {
                    SetMessageEditnotFound();
                    return RedirectToAction("Index");
                }
                Tuple<BlogCategory, ResultStatusOperation> resultFindModel = await blogCategoryService.Find(id.Value);
                switch (resultFindModel.Item2.Type)
                {
                    case Enumeration.MessageTypeResult.Success:
                        {
                            //Pass Parameter like ViewBag.cityId = resultFindModel.Item1.CityId;
                            FillDropDown();
                            SetMessage(resultFindModel.Item2);
                            return View(resultFindModel.Item1);
                        }
                    case Enumeration.MessageTypeResult.Danger:
                        {
                            SetMessageException(resultFindModel.Item2, Enumeration.MessageTypeActionMethod.EditGet);
                            return RedirectToAction("ShowException", "Error");
                        }
                    case Enumeration.MessageTypeResult.Warning:
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
                SetMessageException(new ResultStatusOperation("", "", Enumeration.MessageTypeResult.Danger, false, exception), Enumeration.MessageTypeActionMethod.Index);
                return RedirectToAction("ShowException", "Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BlogCategory model)
        {
            try
            {

                Tuple<BlogCategory, ResultStatusOperation> resultEdit = await blogCategoryService.Update(fillControllerInfo(), model);
                switch (resultEdit.Item2.Type)
                {
                    case Enumeration.MessageTypeResult.Success:
                        {
                            SetMessage(resultEdit.Item2);
                            return RedirectToAction("Edit", new { id = resultEdit.Item1.Id });
                        }
                    case Enumeration.MessageTypeResult.Danger:
                        {
                            SetMessageException(resultEdit.Item2, Enumeration.MessageTypeActionMethod.EditPost);
                            return RedirectToAction("ShowException", "Error");
                        }
                    case Enumeration.MessageTypeResult.Warning:
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
                Tuple<BlogCategory, ResultStatusOperation> resultFindModel = await blogCategoryService.Find(id.Value);
                //resultFindModel.Item1.isdele
                //SetMessage(blogCategoryService.Delete(resultFindModel.Item1));
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
                List<int> categoryIds = ViewBag.categoryId ?? new List<int>();

                ViewBag.listParent = blogCategoryService.ReadAll(categoryIds);

            }
            catch (Exception exception)
            {
                SetMessageException(new ResultStatusOperation("", "", Enumeration.MessageTypeResult.Danger, false, exception), Enumeration.MessageTypeActionMethod.Index);
            }
        }
    }
}
