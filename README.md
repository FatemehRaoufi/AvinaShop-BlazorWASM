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
