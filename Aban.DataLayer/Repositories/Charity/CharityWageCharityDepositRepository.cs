using Aban.DataLayer.Context;
using Aban.DataLayer.Interfaces;
using Aban.DataLayer.Repositories.Generics;
using Aban.Domain.Entities;

namespace Aban.DataLayer.Repositories
{
    public class CharityWageCharityDepositRepository : GenericRepository<CharityWageCharityDeposit>, ICharityWageCharityDepositRepository
    {
        public CharityWageCharityDepositRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}