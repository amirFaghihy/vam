using Aban.DataLayer.Context;
using Aban.DataLayer.Interfaces;
using Aban.DataLayer.Repositories.Generics;
using Aban.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using static Aban.Domain.Enumerations.Enumeration;

namespace Aban.DataLayer.Repositories
{
    public class UserRoleRepository : GenericRepository<IdentityUserRole<string>>, IUserRoleRepository
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<UserIdentity> _userManager;
        private readonly IUserIdentityRepository _userIdentityRepository;
        public UserRoleRepository(AppDbContext applicationDbContext,
            RoleManager<IdentityRole> roleManager,
            UserManager<UserIdentity> userManager
            ) : base(applicationDbContext)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _userIdentityRepository = new UserIdentityRepository(applicationDbContext);
        }

        public async Task<List<UserIdentity>> GetUserByRole(List<RoleName> roleNames)
        {
            var query = new List<UserIdentity>();
            foreach (var item in roleNames)
            {
                var list = await _userManager.GetUsersInRoleAsync(item.ToString());
                query.AddRange(list);
            }
            return query;
        }
    }
}
