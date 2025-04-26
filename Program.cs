using AvinaShop.Components;
using AvinaShop.Components.Account;
using AvinaShop.Data;
using AvinaShop.Repository;
using AvinaShop.Repository.IRepository;
using AvinaShop.Services;
using AvinaShop.Services.OrderServices;
using AvinaShop.Services.UserServices;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Register Razor components and enable interactive rendering
// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Configure authentication state management for Blazor components
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

// Register repositories for dependency injection (DI)
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();
builder.Services.AddScoped<SharedStateService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<PaymentService>();
builder.Services.AddScoped<IOrderService, OrderService>();

#region RoleManagement Injection

// Dependency Inversion Principle (DIP)
// We use dependency injection to inject the UserRoleService into the UserRoleService, following the DIP principle and ensuring that 
// high-level modules depend on abstractions (IUserRoleService) rather than concrete implementations.
builder.Services.AddScoped<IUserRoleService, UserRoleService>();


#endregion

// Configure authentication schemes and use Identity cookies
builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();

// Configure database connection using connection string from appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Register DbContext with SQL Server provider
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Enable detailed database exception information during development
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Configure Identity with core features and token providers
builder.Services.AddIdentityCore<ApplicationUser>(options =>
    {
        // Require users to confirm their email before they can sign in
        options.SignIn.RequireConfirmedAccount = true;
    })
    
    .AddRoles<IdentityRole>() // Enable role-based authorization by adding IdentityRole support
    .AddEntityFrameworkStores<ApplicationDbContext>() // Use Entity Framework to store identity data in the application's database
    .AddSignInManager() // Add the default SignInManager to manage user sign-in workflows
    .AddDefaultTokenProviders(); // Enable generation and validation of tokens (e.g., for email confirmation, password reset)

// Register a dummy email sender (for development/testing)
builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

var app = builder.Build();

// Configure the HTTP request / middleware pipeline.
if (app.Environment.IsDevelopment())
{
    // Apply pending migrations automatically in development
    app.UseMigrationsEndPoint();
}
else
{
    // Use global exception handler and HSTS in production
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts(); // Enforce HTTPS for 30 days by default
}

app.UseHttpsRedirection(); // Redirect HTTP requests to HTTPS


app.UseAntiforgery(); // Protect against CSRF attacks

app.MapStaticAssets();  // Serve static files (e.g., CSS, JS)
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints(); // Map Razor components with interactive server-side rendering

app.Run(); // Start the application
