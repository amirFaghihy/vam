using Aban.DataLayer.Context;
using Aban.Service.Interfaces;
using Aban.Service.Services.Generic;
using Microsoft.AspNetCore.Identity;

namespace Aban.Service.Services
{
    public class RoleService : GenericService<IdentityRole>, IRoleService
    {
        public readonly RoleManager<IdentityRole> roleManager;


        public RoleService(
            AppDbContext dbContext,
            RoleManager<IdentityRole> roleManager) : base(dbContext)
        {
            this.roleManager = roleManager;
        }



    }
}
