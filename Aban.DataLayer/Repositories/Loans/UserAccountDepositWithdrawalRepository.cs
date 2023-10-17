using Aban.DataLayer.Context;
using Aban.DataLayer.Interfaces;
using Aban.DataLayer.Repositories.Generics;
using Aban.Domain.Entities;

namespace Aban.DataLayer.Repositories
{
    public class UserAccountDepositWithdrawalRepository : GenericRepository<UserAccountDepositWithdrawal>, IUserAccountDepositWithdrawalRepository
    {
        public UserAccountDepositWithdrawalRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}