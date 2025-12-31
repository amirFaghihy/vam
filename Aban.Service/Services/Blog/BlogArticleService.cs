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
    public class BlogArticleService : GenericService<BlogArticle>, IBlogArticleService
    {
        private readonly IBlogArticleRepository blogArticleRepository;

        public BlogArticleService(AppDbContext _dbContext) : base(_dbContext)
        {
            blogArticleRepository = new DataLayer.Repositories.BlogArticleRepository(_dbContext);
        }


        public Tuple<IQueryable<BlogArticle>, ResultStatusOperation> SpecificationGetData(string userIdentityId = "", string writerId = "", string title = "", DateTime? registerDateFrom = null, DateTime? registerDateTo = null, bool? isvisible = null)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation();
            resultStatusOperation.IsSuccessed = true;
            resultStatusOperation.Message = "عملیات با موفیقت انجام شد.";
            resultStatusOperation.Type = Enumeration.MessageTypeResult.Success;
            try
            {
                IQueryable<BlogArticle> query = blogArticleRepository.GetAll()
                    .Where(x => !x.IsDelete);

                if (!string.IsNullOrEmpty(userIdentityId))
                {
                    query = query.Where(x => x.UserIdentityId == userIdentityId);
                }
                if (!string.IsNullOrEmpty(writerId))
                {
                    query = query.Where(x => x.WriterId.Contains(writerId));
                }
                if (!string.IsNullOrEmpty(title))
                {
                    query = query.Where(x => x.Title.Contains(title));
                }
                if (registerDateFrom != null || registerDateTo != null)
                {
                    DateTime _registerDateFrom = registerDateFrom != null ? registerDateFrom.Value : new DateTime(1930, 1, 1, 1, 0, 0, 0);
                    DateTime _registerDateTo = registerDateTo != null ? registerDateTo.Value : new DateTime(DateTime.Now.AddYears(1).Year, 1, 1, 1, 0, 0, 0);
                    query = query.Where(x => x.RegisterDate >= _registerDateFrom && x.RegisterDate <= _registerDateTo);
                }
                if (isvisible != null)
                {
                    query = query.Where(x => x.Isvisible == isvisible);
                }

                query = query.Include(x => x.UserIdentity)
                    .Include(x => x.Writer);

                return Tuple.Create(query, resultStatusOperation);
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

        public Tuple<BlogArticle, ResultStatusOperation> FillModel(BlogArticle blogArticle)
        {
            ResultStatusOperation resultStatusOperation = new ResultStatusOperation();
            resultStatusOperation.IsSuccessed = true;
            resultStatusOperation.Type = Enumeration.MessageTypeResult.Success;
            try
            {
                blogArticle.IsDelete = false;
                blogArticle.RegisterDate = DateTime.Now;

                return Tuple.Create(blogArticle, resultStatusOperation);
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

        public List<SelectListItem> ReadAll(Guid selectedValue)
        {

            {
                ResultStatusOperation resultStatusOperation = new ResultStatusOperation();
                resultStatusOperation.IsSuccessed = true;
                resultStatusOperation.Type = Enumeration.MessageTypeResult.Success;

                try
                {
                    var query = blogArticleRepository.GetAll()
                        .Where(x => !x.IsDelete)
                        .OrderBy(x => x.Id).ToList();

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

    }
}
