using Aban.DataLayer.Context;
using Aban.DataLayer.Interfaces;
using Aban.DataLayer.Repositories.Generics;
using Aban.Domain.Entities;

namespace Aban.DataLayer.Repositories
{
    public class GuaranteeRepository : GenericRepository<Guarantee>, IGuaranteeRepository
    {
        public GuaranteeRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}