using Aban.Domain.Entities;
using Aban.Service.IServices.Generic;
using Microsoft.AspNetCore.Identity;

namespace Aban.Service.Interfaces
{
    public interface IUserRoleService : IGenericService<IdentityUserRole<string>>
    {
    }
}
