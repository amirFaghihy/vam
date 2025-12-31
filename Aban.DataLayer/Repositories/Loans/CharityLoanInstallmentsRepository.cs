using Aban.DataLayer.Context;
using Aban.DataLayer.Interfaces;
using Aban.DataLayer.Repositories.Generics;
using Aban.Domain.Entities;

namespace Aban.DataLayer.Repositories
{
    public class CharityLoanInstallmentsRepository : GenericRepository<CharityLoanInstallments>, ICharityLoanInstallmentsRepository
    {
        public CharityLoanInstallmentsRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}