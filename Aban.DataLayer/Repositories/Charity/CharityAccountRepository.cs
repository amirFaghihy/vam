using Aban.DataLayer.Context;
using Aban.DataLayer.Interfaces;
using Aban.DataLayer.Repositories.Generics;
using Aban.Domain.Entities;

namespace Aban.DataLayer.Repositories
{
    public class CharityAccountRepository : GenericRepository<CharityAccount>, ICharityAccountRepository
    {
        public CharityAccountRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}