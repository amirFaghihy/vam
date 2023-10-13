using Aban.DataLayer.Context;
using Aban.DataLayer.Interfaces;
using Aban.DataLayer.Repositories.Generics;
using Aban.Domain.Entities;

namespace Aban.DataLayer.Repositories
{
    public class CharityWageCharityAdditionRepository : GenericRepository<CharityWageCharityAddition>, ICharityWageCharityAdditionRepository
    {
        public CharityWageCharityAdditionRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}