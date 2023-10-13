using Aban.DataLayer.Interfaces.Generics;
using Microsoft.AspNetCore.Identity;

namespace Aban.DataLayer.Interfaces
{
    public interface IRoleRepository : IGenericRepository<IdentityRole>
    {
    }
}
