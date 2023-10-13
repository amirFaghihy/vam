using Aban.DataLayer.Context;
using Aban.DataLayer.Interfaces;
using Aban.DataLayer.Repositories.Generics;
using Aban.Domain.Entities;

namespace Aban.DataLayer.Repositories
{
    public class CharityAdditionRepository : GenericRepository<CharityAddition>, ICharityAdditionRepository
    {
        public CharityAdditionRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}