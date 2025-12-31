using Aban.DataLayer.Context;
using Aban.DataLayer.Interfaces;
using Aban.DataLayer.Repositories.Generics;
using Aban.Domain.Entities;

namespace Aban.DataLayer.Repositories
{
    public class UserAccountRepository : GenericRepository<UserAccount>, IUserAccountRepository
    {
        public UserAccountRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}