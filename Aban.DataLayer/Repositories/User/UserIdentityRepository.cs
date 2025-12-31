using Aban.DataLayer.Context;
using Aban.DataLayer.Interfaces;
using Aban.DataLayer.Repositories.Generics;
using Aban.Domain.Entities;

namespace Aban.DataLayer.Repositories
{
    public class UserIdentityRepository : GenericRepository<UserIdentity>, IUserIdentityRepository
    {
        public UserIdentityRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
