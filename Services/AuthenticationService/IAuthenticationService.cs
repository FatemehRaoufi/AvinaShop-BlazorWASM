namespace AvinaShop.Services.AuthenticationService
{
    /// <summary>
    /// Interface for the AuthenticationService
    /// Defines the contract for authentication-related operations
    /// </summary>
    public interface IAuthenticationService
    {
        // Asynchronously checks if the user is authenticated
        // Returns true if the user is authenticated, otherwise false
        Task<bool> IsAuthenticatedAsync();

        // Asynchronously retrieves the user's ID
        // Returns the user ID as a string if found, otherwise null
        Task<string> GetUserIdAsync();
    }
}
