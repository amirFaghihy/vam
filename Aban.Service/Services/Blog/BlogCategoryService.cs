using Aban.Domain.Entities;
using Aban.Service.Interfaces;
using Aban.DataLayer.Context;
using Microsoft.AspNetCore.Mvc.Rendering;
using Aban.Service.Services.Generic;
using Aban.DataLayer.Interfaces;
using Aban.Domain.Enumerations;
using Microsoft.EntityFrameworkCore;

namespace Aban.Service.Services
{
    public class BlogCategoryService : GenericService<BlogCategory>, IBlogCategoryService
    {

        #region Constructor

        private readonly IBlogCategoryRepository blogCategoryRepository;

        public BlogCategoryService(AppDbContext _dbContext) : base(_dbContext)
        {
            blogCategoryRepository = new DataLayer.Repositories.BlogCategoryRepository(_dbContext);
        }

        #endregion


        public Task<Tuple<IQueryable<BlogCategory>, ResultStatusOperation>> SpecificationGetData(
            string? userIdentityId = "",
            int? parentId = null,
            string? title = null,
            bool? isvisible = null,
            string? blogCategoryURL = null)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation();
            resultStatusOperation.IsSuccessed = true;
            resultStatusOperation.Type = Enumeration.MessageTypeResult.Success;
            try
            {
                IQueryable<BlogCategory> query = blogCategoryRepository.GetAll();

                if (!string.IsNullOrEmpty(userIdentityId))
                {
                    query = query.Where(x => x.UserIdentityId == userIdentityId);
                }
                if (parentId != null)
                {
                    query = query.Where(x => x.ParentId == parentId);
                }
                if (title != null)
                {
                    query = query.Where(x => x.Title == title);
                }
                if (isvisible != null)
                {
                    query = query.Where(x => x.IsVisible == isvisible);
                }
                if (blogCategoryURL != null)
                {
                    query = query.Where(x => x.BlogCategoryURL == blogCategoryURL);
                }

                query = query.Include(x => x.UserIdentity);

                return Task.FromResult(Tuple.Create(query, resultStatusOperation));
            }
            catch (Exception exception)
            {
                resultStatusOperation.IsSuccessed = false;
                resultStatusOperation.Message = "خطایی رخ داده است";
                resultStatusOperation.Type = Enumeration.MessageTypeResult.Danger;
                resultStatusOperation.ErrorException = exception;
                throw new Exception("", exception);
            }
        }

        public Task<Tuple<BlogCategory, ResultStatusOperation>> FillModel(BlogCategory blogCategory)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation();
            resultStatusOperation.IsSuccessed = true;
            resultStatusOperation.Type = Enumeration.MessageTypeResult.Success;
            try
            {
                //blogCategory.IsDelete = false;
                //blogCategory.RegisterDate = DateTime.Now;

                return Task.FromResult(Tuple.Create(blogCategory, resultStatusOperation));
            }
            catch (Exception exception)
            {
                resultStatusOperation.IsSuccessed = false;
                resultStatusOperation.Message = "خطایی رخ داده است";
                resultStatusOperation.Type = Enumeration.MessageTypeResult.Danger;
                resultStatusOperation.ErrorException = exception;
                throw new Exception("", exception);
            }
        }

        public List<SelectListItem> ReadAll(int selectedValue)
        {

            {
                ResultStatusOperation resultStatusOperation = new ResultStatusOperation();
                resultStatusOperation.IsSuccessed = true;
                resultStatusOperation.Type = Enumeration.MessageTypeResult.Success;

                try
                {
                    var query = blogCategoryRepository.GetAll().OrderBy(x => x.Id).ToList();

                    List<SelectListItem> item = query.ConvertAll(x =>
                    {
                        return new SelectListItem()
                        {
                            Text = x.Title,
                            Value = x.Id.ToString(),
                            Selected = (x.Id == selectedValue) ? true : false
                        };
                    });


                    return item;
                }
                catch (Exception exception)
                {
                    resultStatusOperation.IsSuccessed = false;
                    resultStatusOperation.Message = "خطایی رخ داده است";
                    resultStatusOperation.Type = Enumeration.MessageTypeResult.Danger;
                    resultStatusOperation.ErrorException = exception;
                    throw new Exception("", exception);
                }
            }
        }


        public List<SelectListItem> ReadAll(List<int> selectedValue)
        {
            {
                ResultStatusOperation resultStatusOperation = new ResultStatusOperation();
                resultStatusOperation.IsSuccessed = true;
                resultStatusOperation.Type = Enumeration.MessageTypeResult.Success;

                try
                {
                    var query = blogCategoryRepository.GetAll().OrderBy(x => x.Id).ToList();

                    List<SelectListItem> item = query.ConvertAll(x =>
                    {
                        return new SelectListItem()
                        {
                            Text = x.Title,
                            Value = x.Id.ToString(),
                            Selected = selectedValue.Any(y => x.Id == y)
                        };
                    });


                    return item;
                }
                catch (Exception exception)
                {
                    resultStatusOperation.IsSuccessed = false;
                    resultStatusOperation.Message = "خطایی رخ داده است";
                    resultStatusOperation.Type = Enumeration.MessageTypeResult.Danger;
                    resultStatusOperation.ErrorException = exception;
                    throw new Exception("", exception);
                }
            }
        }

    }
}
