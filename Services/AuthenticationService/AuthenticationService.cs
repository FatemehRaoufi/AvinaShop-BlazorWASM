using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace AvinaShop.Services.AuthenticationService
{
    /// <summary>
    /// This service responsible for handling authentication-related operations.
    /// And provides methods to check if a user is authenticated and retrieve the user's ID
    /// AuthenticationService implements the IAuthenticationService interface.
    /// </summary>

    public class AuthenticationService : IAuthenticationService
    {
        
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        // Constructor to inject the AuthenticationStateProvider
        // AuthenticationStateProvider is responsible for providing authentication state
        public AuthenticationService(AuthenticationStateProvider authenticationStateProvider)
        {
            _authenticationStateProvider = authenticationStateProvider;
        }

        // Asynchronously checks if the user is authenticated
        public async Task<bool> IsAuthenticatedAsync()
        {           
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();// Get the current authentication state of the user

            // Check if the user is authenticated          
            if (authState.User.Identity == null) // If the Identity is null, return false, otherwise check if IsAuthenticated is true
            {
                return false; // User is not authenticated
            }
            return authState.User.Identity.IsAuthenticated; // Return the authentication status
        }

        // Asynchronously retrieves the user's ID based on the claims
        public async Task<string> GetUserIdAsync()
        {
            
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();// Get the current authentication state of the user

            var nameIdentifierClaim = authState.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier);// Find the claim with the NameIdentifier (user ID) in the user's claims

            // If the NameIdentifier claim is found, return its value (user ID)
            if (nameIdentifierClaim != null)
            {
                return nameIdentifierClaim.Value;
            }
            return null;// Return null if the NameIdentifier claim is not found
        }
    }
}
