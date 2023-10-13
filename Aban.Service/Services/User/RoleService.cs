using Aban.DataLayer.Context;
using Aban.DataLayer.Interfaces;
using Aban.Domain.Entities;
using Aban.Service.Interfaces;
using Aban.Service.Services.Generic;
using Aban.DataLayer.Repositories;
using Microsoft.AspNetCore.Mvc.Rendering;
using Aban.Domain.Enumerations;
using static Aban.Domain.Enumerations.Enumeration;
using Microsoft.AspNetCore.Identity;
using Aban.Common;
using Aban.Common.Utility;
using Aban.ViewModels;

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
