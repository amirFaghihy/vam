using Aban.DataLayer.Interfaces.Generics;
using Aban.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using static Aban.Domain.Enumerations.Enumeration;

namespace Aban.DataLayer.Interfaces
{
    public interface IUserRoleRepository : IGenericRepository<IdentityUserRole<string>>
    {
        Task<List<UserIdentity>> GetUserByRole(List<RoleName> roleNames);

    }
}
