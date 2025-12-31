using Aban.DataLayer.Context;
using Aban.DataLayer.Interfaces;
using Aban.DataLayer.Repositories.Generics;
using Aban.Domain.Entities;

namespace Aban.DataLayer.Repositories
{
    public class BlogArticleRepository : GenericRepository<BlogArticle>, IBlogArticleRepository
    {
        public BlogArticleRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}


