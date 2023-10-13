using Aban.DataLayer.Context;
using Aban.DataLayer.Interfaces;
using Aban.DataLayer.Repositories.Generics;
using Aban.Domain.Entities;

namespace Aban.DataLayer.Repositories
{
    public class CharityWageCharityLoanInstallmentRepository : GenericRepository<CharityWageCharityLoanInstallment>, ICharityWageCharityLoanInstallmentRepository
    {
        public CharityWageCharityLoanInstallmentRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}