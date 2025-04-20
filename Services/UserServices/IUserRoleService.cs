using AvinaShop.Data;

namespace AvinaShop.Services.UserServices
{
    public interface IUserRoleService
    {
        Task EnsureRolesExistAsync();
        Task AssignRoleToUserAsync(ApplicationUser user, string? role);
    }
}


