using Aban.DataLayer.Context;
using Aban.DataLayer.Interfaces;
using Aban.DataLayer.Repositories.Generics;
using Aban.Domain.Entities;

namespace Aban.DataLayer.Repositories
{
    public class CharityWageCharityDeductionRepository : GenericRepository<CharityWageCharityDeduction>, ICharityWageCharityDeductionRepository
    {
        public CharityWageCharityDeductionRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}