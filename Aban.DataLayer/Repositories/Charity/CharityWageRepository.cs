using Aban.DataLayer.Context;
using Aban.DataLayer.Interfaces;
using Aban.DataLayer.Repositories.Generics;
using Aban.Domain.Entities;

namespace Aban.DataLayer.Repositories
{
    public class CharityWageRepository : GenericRepository<CharityWage>, ICharityWageRepository
    {
        public CharityWageRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}