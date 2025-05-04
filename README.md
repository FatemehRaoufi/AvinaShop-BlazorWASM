# AvinaShop (BlazorWASM)
**Current Status:** In Development

## Description
AvinaShop is an e-commerce web application built using Blazor WebAssembly (WASM) and .NET 9. The project uses Entity Framework Core (EF Core) for database interaction and follows Clean Code and SOLID principles to ensure a clean and maintainable codebase.

## Features
- **Admin Panel:** Manage products, orders, and users.
- **Homepage with Search and Filter functionality:** Allows users to easily search and filter products in real-time without reloading the page.
- **Shopping Process:** A complete shopping process from product selection to checkout.
- **Infinite Scroll:** Progressive product loading for enhanced user experience (e.g., "Load More" button).
- **Add to Cart with Immediate Feedback:** Adds products to the cart with real-time success or error notifications using toast messages.
- **Identity Authentication:** User authentication with role-based access control.
- **Role-Based Access:** Manage user roles and permissions.
- **Reactive UI for Filtering:**
   - Real-time filtering by product name and category selection without reloading the page.
   - Provides a fast and responsive search and filter experience.
     
## Technologies Used
- **Blazor (WASM):** To build an interactive web application running in the browser.
- **.NET 9:** The latest version of the .NET framework for backend development.
- **Entity Framework Core (EF Core):** For database interaction.
- **Clean Code:** To maintain a clear and maintainable codebase.
- **SOLID Principles:** For a flexible, extendable, and maintainable architecture.
- **xUnit:** For unit testing repository methods to ensure correct data access logic.

## Recent Updates
1. **Refactored Product Loading with Client-Side Pagination:**
   - Products are now loaded in pages with a "Load More" functionality.
   - This reduces initial load time and improves user experience by progressively fetching more products.

2. **Implemented Infinite Scroll:**
   - Added support for infinite scrolling using a "Load More" button to fetch more products as the user scrolls, providing a smooth browsing experience.

3. **Refactored Add to Cart Logic Using Message-Based Abstraction:**
   - Decoupled cart update logic into a message-based system using `AddToCartMessage`.
   - This change improves code readability, testability, and modularity.
   - Real-time feedback is provided via toast messages for success or error events when adding items to the cart.

## Testing
The project includes a Test Project built with xUnit, focusing on testing the repository methods to ensure that the data access logic is functioning correctly.


## 🔧 Implementation Details

### 🛍️ OrderService – Order Management with Role-Based Access


The `OrderService` class is responsible for managing order-related operations such as retrieving and updating orders. It leverages `ClaimsPrincipal` to identify the currently authenticated user and adjusts access based on the user's role.
### Table of Contents
- [Service Overview](#service-overview)
- [Methods](#methods)
  - [GetOrderHeadersAsync](#getorderheadersasync)
  - [GetOrderByIdAsync](#getorderbyidasync)
  - [UpdateOrderStatusAsync](#updateorderstatusasync)
  - [CreateOrderAsync](#createorderasync)
- [SOLID Principles](#solid-principles)

---
### Key Features:
- **Role-Based Order Retrieval**:  
  - **Admin users** can view all orders.  
  - **Regular users** can only view their own orders.
  
- **Claims-Based User Identification**:  
  Retrieves the current user's ID using `ClaimTypes.NameIdentifier` from the `ClaimsPrincipal`.

- **Status Update Support**:  
  Allows updating order status along with an optional session ID for tracking.

### Code Highlights:

**Retrieve Orders Based on Role:**
```csharp
public async Task<IEnumerable<OrderHeader>> GetOrderHeadersAsync()
{
    var user = await GetCurrentUserAsync();

    if (user.IsInRole(AppConstants.Role_Admin))
    {
        return await GetAllOrdersAsync();
    }

    var userId = GetUserId(user);
    return await GetUserOrdersAsync(userId);
}
```
Get Current User ID Using Claims:

```csharp
private string GetUserId(ClaimsPrincipal user)
{
    return user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
}
```
Authentication Integration:

```csharp
private async Task<ClaimsPrincipal> GetCurrentUserAsync()
{
    var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
    return authState.User;
}
```

| **SOLID Principle**                       | **Implemented?** | **Notes**                                                                                                                                        |
| ----------------------------------------- | ---------------- | ------------------------------------------------------------------------------------------------------------------------------------------------ |
| **Single Responsibility Principle (SRP)** | Yes              | The class manages only order-related logic (retrieving, updating orders). No other concerns are handled in this class.                           |
| **Open/Closed Principle (OCP)**           | Yes              | The service allows adding new methods without modifying existing ones. New features can be added by extending the service or repository.         |
| **Liskov Substitution Principle (LSP)**   | Yes              | The class can be replaced by another class implementing the `IOrderService` interface without breaking functionality.                            |
| **Interface Segregation Principle (ISP)** | Yes              | The `IOrderService` interface seems focused on order management. However, as the project grows, it may need to be split into smaller interfaces. |
| **Dependency Inversion Principle (DIP)**  | Yes              | Dependencies like `IOrderRepository` and `AuthenticationStateProvider` are injected via constructor, ensuring low coupling and high flexibility. |

## Conclusion
OrderService is a vital part of the order management system in AvinaShop, encapsulating all business logic related to handling orders. It efficiently manages user roles and ensures that only authorized users can view or modify order data. With a solid adherence to SOLID principles, this service is well-structured for future growth and maintainability.

### 🔐 AuthenticationService – Authentication Handling with Claims

The `AuthenticationService` class is responsible for managing authentication-related operations, such as checking if a user is authenticated and retrieving the user's ID. It utilizes `AuthenticationStateProvider` to retrieve the user's authentication state and work with claims for user identification.

### Key Features:
- **User Authentication Check**:  
  Asynchronously checks if the current user is authenticated by verifying the authentication state.

- **User ID Retrieval**:  
  Retrieves the user's unique ID (`NameIdentifier`) from the authentication claims, allowing for user-specific functionality.

### Code Highlights:

**Check if the User is Authenticated:**
```csharp
public async Task<bool> IsAuthenticatedAsync()
{
    var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();

    if (authState.User.Identity == null)
    {
        return false;
    }
    return authState.User.Identity.IsAuthenticated;
}
```
Retrieve the User ID from Claims:

```csharp
public async Task<string> GetUserIdAsync()
{
    var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
    var nameIdentifierClaim = authState.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier);

    if (nameIdentifierClaim != null)
    {
        return nameIdentifierClaim.Value;
    }
    return null;
}
```
### :shopping_cart:ShoppingCartService Documentation

## Overview

The `ShoppingCartService` is a service that provides functionality for managing a user's shopping cart. It interacts with the `IShoppingCartRepository` to perform operations such as adding/removing items, updating item quantities, and clearing the cart. This service is designed to handle the business logic related to shopping carts while adhering to SOLID principles.

## Table of Contents

1. [Dependencies](#dependencies)
2. [Methods](#methods)
   - [GetUserCartAsync](#getusercartasync)
   - [AddItemToCartAsync](#additemtocartasync)
   - [UpdateItemQuantityAsync](#updateitemquantityasync)
   - [GetCartItemCountAsync](#getcartitemcountasync)
   - [ClearCartAsync](#clearcartasync)
   - [RemoveItemFromCartAsync](#removeitemfromcartasync)
3. [SOLID Principles](#solid-principles)
4. [Usage Example](#usage-example)

---

## Dependencies

- **IShoppingCartRepository**: A repository interface that defines the data operations (e.g., adding, removing, and updating cart items).
- **ShoppingCart**: A model representing the items in the shopping cart.
- **Task**: All methods are asynchronous and return `Task` or `Task<bool>` to handle asynchronous operations.

---

## Methods

### GetUserCartAsync

Retrieves all items in the user's shopping cart.

```csharp
public async Task<IEnumerable<ShoppingCart>> GetUserCartAsync(string userId)
Parameters:
userId (string): The unique identifier of the user.

Returns:
IEnumerable<ShoppingCart>: A list of shopping cart items for the user.
```
AddItemToCartAsync
Adds a product to the user's cart if it's not already in the cart.

```csharp
public async Task<bool> AddItemToCartAsync(string userId, int productId, int quantity)
Parameters:
userId (string): The unique identifier of the user.

productId (int): The product ID to be added to the cart.

quantity (int): The quantity of the product to be added.

Returns:
bool: Returns true if the item was successfully added to the cart, otherwise false.
```
UpdateItemQuantityAsync
Updates the quantity of a specific item in the user's cart.

```csharp
public async Task<bool> UpdateItemQuantityAsync(string userId, int productId, int quantityChange)
Parameters:
userId (string): The unique identifier of the user.

productId (int): The product ID whose quantity will be updated.

quantityChange (int): The change in the quantity (can be positive or negative).

Returns:
bool: Returns true if the quantity was successfully updated, otherwise false.
```
GetCartItemCountAsync
Gets the total count of items in the user's cart.

```csharp
public Task<int> GetCartItemCountAsync(string userId)
Parameters:
userId (string): The unique identifier of the user.

Returns:
int: The total number of items in the user's cart.
```
ClearCartAsync
Clears all items from the user's cart.

```csharp
public Task<bool> ClearCartAsync(string userId)
Parameters:
userId (string): The unique identifier of the user.

Returns:
bool: Returns true if the cart was successfully cleared, otherwise false.
```
RemoveItemFromCartAsync
Removes an item from the user's cart.

```csharp
public async Task<bool> RemoveItemFromCartAsync(string userId, int productId)
Parameters:
userId (string): The unique identifier of the user.

productId (int): The product ID to be removed from the cart.

Returns:
bool: Returns true if the item was successfully removed, otherwise false.
```
| **SOLID Principle**                       | **Implemented?** | **Notes**                                                                                                                                                                            |
| ----------------------------------------- | ---------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| **Single Responsibility Principle (SRP)** | Yes              | The ShoppingCartService is responsible for managing shopping cart operations, and it does not handle concerns like order processing or payment handling.                             |
| **Open/Closed Principle (OCP)**           | Yes              | The service is open for extension (you can add new methods or functionality), but closed for modification (no need to change existing code when adding new features).                |
| **Liskov Substitution Principle (LSP)**   | Yes              | The `IShoppingCartService` interface can be implemented by different classes, and any class implementing this interface can be substituted without affecting the program’s behavior. |
| **Interface Segregation Principle (ISP)** | Yes              | The `IShoppingCartService` interface is focused only on shopping cart operations, avoiding unnecessary methods unrelated to cart management.                                         |
| **Dependency Inversion Principle (DIP)**  | Yes              | The service depends on the `IShoppingCartRepository` interface, not a concrete repository implementation, allowing for easy swapping, flexibility, and testing.                      |

### Conclusion
The ShoppingCartService provides a clean, maintainable, and SOLID-compliant way to manage the shopping cart operations in an e-commerce application. It ensures the business logic is separate from the data access logic, making the application more flexible and testable.

