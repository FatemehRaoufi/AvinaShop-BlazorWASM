using AvinaShop.Data;
using AvinaShop.Utility;
using Microsoft.AspNetCore.Identity;

namespace AvinaShop.Services.UserServices
{
    public class UserRoleService : IUserRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRoleService(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task EnsureRolesExistAsync()
        {
            if (!await _roleManager.RoleExistsAsync(AppConstants.Role_Customer))
                await _roleManager.CreateAsync(new IdentityRole(AppConstants.Role_Customer));

            if (!await _roleManager.RoleExistsAsync(AppConstants.Role_Admin))
                await _roleManager.CreateAsync(new IdentityRole(AppConstants.Role_Admin));
        }

        public async Task AssignRoleToUserAsync(ApplicationUser user, string? role)
        {
            var finalRole = role ?? AppConstants.Role_Customer;
            await _userManager.AddToRoleAsync(user, finalRole);
        }
    }
}
#region SOLID Principles Applied

// Single Responsibility Principle (SRP):
// This service is responsible for handling roles and assigning them to users. 
// It does not take responsibility for other tasks like managing users, which is handled by UserManager.

// Open/Closed Principle (OCP):
// The class is open for extension (e.g., adding new roles or logic for role assignment) without changing the existing code. 
// For example, new roles can be added easily by extending this service or by injecting new services if needed.

// Liskov Substitution Principle (LSP):
// Any derived classes or mocks of IUserRoleService can be used interchangeably without breaking functionality.
// For instance, during testing, we can easily mock IUserRoleService for unit tests.

// Interface Segregation Principle (ISP):
// IUserRoleService interface is specifically focused on role management and does not include unnecessary methods 
// that are not related to the role functionality. This keeps the interface focused on its sole responsibility.

// Dependency Inversion Principle (DIP):
// Dependencies (RoleManager and UserManager) are injected via the constructor, allowing the service to depend on abstractions 
// rather than concrete implementations. This makes the service more flexible and easier to test by mocking the dependencies.
#endregion