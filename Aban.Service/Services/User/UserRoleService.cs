using Aban.DataLayer.Context;
using Aban.Service.Interfaces;
using Aban.Service.Services.Generic;
using Microsoft.AspNetCore.Identity;

namespace Aban.Service.Services
{
    public class UserRoleService : GenericService<IdentityUserRole<string>>, IUserRoleService
    {
        public UserRoleService(AppDbContext appDbContext) : base(appDbContext)
        {
        }
    }
}
