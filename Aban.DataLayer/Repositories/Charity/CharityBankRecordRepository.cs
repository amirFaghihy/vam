using Aban.DataLayer.Context;
using Aban.DataLayer.Interfaces;
using Aban.DataLayer.Repositories.Generics;
using Aban.Domain.Entities;

namespace Aban.DataLayer.Repositories
{
    public class CharityBankRecordRepository : GenericRepository<CharityBankRecord>, ICharityBankRecordRepository
    {
        public CharityBankRecordRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}