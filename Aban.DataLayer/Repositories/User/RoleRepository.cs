using Aban.DataLayer.Context;
using Aban.DataLayer.Interfaces;
using Aban.DataLayer.Repositories.Generics;
using Microsoft.AspNetCore.Identity;

namespace Aban.DataLayer.Repositories
{
    public class RoleRepository : GenericRepository<IdentityRole>, IRoleRepository
    {
        public RoleRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
