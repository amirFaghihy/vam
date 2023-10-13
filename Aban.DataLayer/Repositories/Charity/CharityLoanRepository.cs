using Aban.DataLayer.Context;
using Aban.DataLayer.Interfaces;
using Aban.DataLayer.Repositories.Generics;
using Aban.Domain.Entities;

namespace Aban.DataLayer.Repositories
{
    public class CharityLoanRepository : GenericRepository<CharityLoan>, ICharityLoanRepository
    {
        public CharityLoanRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}