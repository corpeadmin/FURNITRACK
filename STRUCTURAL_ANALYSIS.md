# FURNITRACK - Comprehensive Structural Analysis

## 1. Application Purpose and Type

**Name:** FURNITRACK  
**Type:** ASP.NET Core MVC Web Application  
**Domain:** Furniture Inventory Management System  
**Target Framework:** .NET 8.0  
**Architecture Pattern:** Model-View-Controller (MVC)

**Purpose:** FURNITRACK is a browser-based inventory management system designed specifically for furniture businesses. It provides a centralized platform for managing inventory, tracking sales, generating reports, and managing user accounts.

---

## 2. Main Architecture and Components

### Overall Architecture Overview
```
┌─────────────────────────────────────────────────┐
│           FURNITRACK MVC Application            │
│                  (ASP.NET Core 8.0)             │
├─────────────────────────────────────────────────┤
│  Controllers (Request Handling)                 │
│  ├── HomeController (6 main actions)            │
├─────────────────────────────────────────────────┤
│  Views (Razor Templates)                        │
│  ├── Dashboard, Inventory, Sales, Reports       │
│  ├── Users, Error, Index                        │
│  └── Shared Layout (_Layout.cshtml)             │
├─────────────────────────────────────────────────┤
│  Models (Data Structures)                       │
│  └── ErrorViewModel                             │
├─────────────────────────────────────────────────┤
│  Data Layer (Entity Framework Core 8.0)         │
│  └── ApplicationDBContext (SQL Server)           │
├─────────────────────────────────────────────────┤
│  Static Assets (wwwroot/)                       │
│  ├── CSS (Bootstrap, Custom Styles)             │
│  ├── JavaScript (jQuery, Bootstrap JS)          │
│  └── Libraries (Bootstrap Icons)                │
└─────────────────────────────────────────────────┘
```

### Component Breakdown

#### **Controllers Layer**
- **Location:** `Controllers/HomeController.cs`
- **Pattern:** Single controller managing multiple related actions
- **Dependency Injection:** Uses `ILogger<HomeController>` for logging
- **Route Configuration:** Default ASP.NET Core MVC routing

#### **Views Layer**
- **Location:** `Views/` directory
- **Layout System:** Master page pattern using `_Layout.cshtml`
- **View Imports:** `_ViewImports.cshtml` for shared directives
- **Structure:**
  - `Views/Home/` - Primary business views
  - `Views/Shared/` - Reusable layout and components

#### **Models Layer**
- **Location:** `Models/` directory
- **Current Models:** ErrorViewModel
- **Namespace:** `FURNITRACK.Models`
- **Purpose:** Data structures for view models and domain objects

#### **Data Layer**
- **Location:** `Data/ApplicationDBContext.cs`
- **Technology:** Entity Framework Core 8.0
- **Database:** SQL Server
- **Current State:** Skeleton structure (ready for DbSet definitions)

#### **Static Assets Layer**
- **Location:** `wwwroot/` directory
- **CSS:** Bootstrap framework + custom styling
- **JavaScript:** jQuery + Bootstrap JS bundle
- **Icons:** Bootstrap Icons from CDN

---

## 3. Key Dependencies and NuGet Packages

### Project Configuration (`FURNITRACK.csproj`)

| Package | Version | Purpose |
|---------|---------|---------|
| `Microsoft.EntityFrameworkCore` | 8.0.0 | ORM for database operations |
| `Microsoft.EntityFrameworkCore.SqlServer` | 8.0.0 | SQL Server provider for EF Core |
| `Microsoft.EntityFrameworkCore.Tools` | 8.0.0 | CLI tools for migrations & scaffolding |

### Implicit Framework References
- ASP.NET Core MVC runtime (included in Web SDK)
- Dependency Injection container (built-in)
- Configuration system (built-in)
- Logging framework (built-in)

### Frontend Libraries (via wwwroot/lib/)

| Library | Purpose | Version |
|---------|---------|---------|
| Bootstrap | Responsive CSS framework | (Latest from build) |
| jQuery | DOM manipulation & AJAX | (Latest from build) |
| jQuery Validation | Client-side form validation | (Latest from build) |
| jQuery Validation Unobtrusive | Unobtrusive validation integration | (Latest from build) |
| Bootstrap Icons | Icon library | 1.11.0 (CDN) |

### Build Configuration
```xml
<PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <Nullable>enable</Nullable>              <!-- Nullable reference types enabled -->
    <ImplicitUsings>enable</ImplicitUsings>  <!-- Global using statements enabled -->
</PropertyGroup>
```

---

## 4. Database Configuration and Context

### Entity Framework Core Setup

**File:** `Data/ApplicationDBContext.cs`

```csharp
namespace FURNITRACK.Data
{
    public class ApplicationDBContext
    {
    }
}
```

**Current Status:** Skeleton implementation  
**Database Provider:** SQL Server (configured in NuGet packages)  
**ORM Version:** EF Core 8.0.0

**What Needs to be Implemented:**
1. DbContext inheritance from `DbContext`
2. Constructor accepting `DbContextOptions<ApplicationDBContext>`
3. DbSet<T> properties for each entity (Furniture, Inventory, Sales, Users, etc.)
4. OnModelCreating() for entity configuration
5. Connection string in appsettings.json

**Expected Entities (based on views):**
- Furniture / Products
- Inventory (Stock management)
- Sales (Transactions)
- Users (System users)
- Reports (Aggregated data)

---

## 5. Startup Configuration (`Program.cs`)

### Startup Pipeline

```csharp
// 1. Build configuration and dependency injection
var builder = WebApplication.CreateBuilder(args);

// 2. Add services
builder.Services.AddControllersWithViews();

// 3. Build the application
var app = builder.Build();

// 4. Configure HTTP pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();  // HTTP Strict Transport Security
}

app.UseHttpsRedirection();      // Enforce HTTPS
app.UseStaticFiles();           // Serve static files from wwwroot
app.UseRouting();               // Enable routing
app.UseAuthorization();         // Authorization middleware (prepared for auth)

// 5. Map default MVC route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
```

### Key Configuration Points

| Configuration | Purpose | Status |
|---------------|---------|--------|
| Development environment check | Conditional error handling | Active |
| HTTPS redirection | Security enforcement | Active |
| Static file serving | CSS, JS, images | Active |
| MVC routing | Controller action mapping | Active |
| Authorization middleware | Ready for auth implementation | Configured but unused |
| Exception handling | Error page fallback | Active |

### Launch Profiles (`Properties/launchSettings.json`)

| Profile | Protocol | Port | Notes |
|---------|----------|------|-------|
| http | HTTP | 5178 | Development |
| https | HTTPS | 7236, 5178 | Development with SSL |
| IIS Express | HTTP/HTTPS | 5402, 44326 | IIS hosting |

---

## 6. Controllers and Their Purposes

### HomeController
**File:** `Controllers/HomeController.cs`

**Actions (Routes):**

| Action | Route | Purpose | Returns |
|--------|-------|---------|---------|
| `Index()` | `/Home/Index` or `/` | Landing page (hidden sidebar) | View (Home/Index.cshtml) |
| `Dashboard()` | `/Home/Dashboard` | Main dashboard view | View (Home/Dashboard.cshtml) |
| `Inventory()` | `/Home/Inventory` | Inventory management page | View (Home/Inventory.cshtml) |
| `Sales()` | `/Home/Sales` | Sales tracking page | View (Home/Sales.cshtml) |
| `Reports()` | `/Home/Reports` | Reporting and analytics page | View (Home/Reports.cshtml) |
| `Users()` | `/Home/Users` | User management page | View (Home/Users.cshtml) |
| `Error()` | `/Home/Error` (fallback) | Error page (exception handler) | View (Shared/Error.cshtml) |

**Dependencies:**
- `ILogger<HomeController>` - Logging service (injected via constructor)

**Current Implementation Level:** 🔴 Views-only (No data binding, no business logic)

---

## 7. Models and ViewModels

### Current Models

#### ErrorViewModel
**File:** `Models/ErrorViewModel.cs`

```csharp
namespace FURNITRACK.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
```

**Purpose:** Represents error page data  
**Properties:**
- `RequestId` - Unique error identifier
- `ShowRequestId` - Display flag based on RequestId presence

### Expected Models (Not Yet Implemented)

Based on the application domain, the following models should be created:

1. **Furniture/Product Model**
   - ProductId, Name, Category, Description, Price, SKU

2. **Inventory Model**
   - InventoryId, ProductId, Quantity, Location, LastUpdated

3. **Sale Model**
   - SaleId, ProductId, Quantity, SaleDate, Amount, Salesperson

4. **User Model**
   - UserId, Username, Email, Role, CreatedDate, IsActive

5. **Report Model**
   - ReportId, ReportType, GeneratedDate, Data (JSON or aggregated)

---

## 8. View Structure and Pages

### Master Layout
**File:** `Views/Shared/_Layout.cshtml`

**Features:**
- Responsive Bootstrap-based design
- Fixed sidebar navigation (250px width)
- Conditional sidebar rendering (hidden on Index page)
- Sidebar highlighting for active page
- Main content area with proper offset

**Sidebar Navigation:**
- Dashboard (`bi-grid-1x2-fill`)
- Inventory (`bi-box-seam`)
- Sales (`bi-cart`)
- Reports (`bi-bar-chart-line`)
- Users (`bi-people`)
- Logout (placeholder)

**Color Scheme:**
- Sidebar Background: `#1a335d` (Navy Blue)
- Active Link: `#3b5998` (Lighter Blue)
- Page Background: `#f8f9fa` (Light Gray)

### View Pages

| View File | Route | Purpose | Sidebar |
|-----------|-------|---------|---------|
| `Home/Index.cshtml` | `/` | Landing/welcome page | Hidden |
| `Home/Dashboard.cshtml` | `/Home/Dashboard` | Main dashboard | Visible |
| `Home/Inventory.cshtml` | `/Home/Inventory` | Inventory management | Visible |
| `Home/Sales.cshtml` | `/Home/Sales` | Sales management | Visible |
| `Home/Reports.cshtml` | `/Home/Reports` | Reports and analytics | Visible |
| `Home/Users.cshtml` | `/Home/Users` | User management | Visible |
| `Shared/_Layout.cshtml` | (Master) | Layout template | N/A |
| `Shared/Error.cshtml` | `/Home/Error` | Error display page | Hidden |

### View Imports
**File:** `Views/_ViewImports.cshtml`

```csharp
@using FURNITRACK
@using FURNITRACK.Models
@addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
```

**Provides:**
- Namespace shortcuts
- ASP.NET Core built-in tag helpers

### View Start
**File:** `Views/_ViewStart.cshtml`

Sets the layout for all views:
```csharp
@{
    Layout = "_Layout";
}
```

---

## 9. Static Assets and Frontend Setup

### Directory Structure
```
wwwroot/
├── css/
│   ├── site.css              # Custom styles (52 lines)
│   └── _Layout.cshtml.css    # Layout-specific styles
├── js/
│   └── site.js               # Custom JavaScript
└── lib/
    ├── bootstrap/            # Bootstrap framework (CSS/JS)
    ├── jquery/               # jQuery library
    ├── jquery-validation/    # Form validation
    └── jquery-validation-unobtrusive/  # Unobtrusive validation
```

### CSS Styling

**`site.css` Overview:**
- Responsive typography (14px mobile, 16px desktop+)
- Bootstrap integration
- Custom sidebar styles
- Fixed positioning for sidebar
- Hover effects and transitions
- Color scheme (navy blue, lighter blue for active, light gray background)

### JavaScript

**`site.js`:** Custom JavaScript (minimal setup for future functionality)

### Frontend Frameworks

| Framework | Version | Purpose |
|-----------|---------|---------|
| Bootstrap | Latest | Responsive grid, components |
| jQuery | Latest | DOM manipulation, AJAX |
| Bootstrap Icons | 1.11.0 (CDN) | Icon library |
| jQuery Validate | Latest | Client-side form validation |

---

## 10. Overall Data Flow

### Request-Response Cycle

```
┌─────────────────────────────────────────────────────────────────────────┐
│                        USER REQUEST (Browser)                           │
└──────────────────────────────┬──────────────────────────────────────────┘
                               │
                               ▼
              ┌────────────────────────────────┐
              │  ASP.NET Core Routing Engine   │
              │  (Maps URL to Controller)      │
              └────────────┬───────────────────┘
                           │
                           ▼
              ┌────────────────────────────────┐
              │      HomeController Action     │
              │  (Dashboard, Inventory, etc.)  │
              └────────────┬───────────────────┘
                           │
                           ▼
        ┌──────────────────────────────────────────┐
        │  Service/Business Logic Layer (TBD)      │
        │  Could interact with ApplicationDBContext│
        └──────────┬───────────────────────────────┘
                   │
                   ▼
        ┌──────────────────────────────────────────┐
        │   ApplicationDBContext (EF Core)         │
        │   Queries/Commands to SQL Server         │
        └──────────────────────────────────────────┘
                   │
                   ▼
        ┌──────────────────────────────────────────┐
        │       SQL Server Database                │
        │  (Furniture, Inventory, Sales, Users)    │
        └──────────────────────────────────────────┘
```

### Complete Data Flow (Populated)

```
User Interaction
    ▼
Browser sends HTTP Request to {controller}/{action}
    ▼
Program.cs Middleware Pipeline
    ├─ HTTPS Redirect
    ├─ Static Files (if wwwroot match)
    └─ Routing
    ▼
HomeController.Action() executes
    ▼
Service Layer (future) queries data
    ▼
ApplicationDBContext sends EF Core queries
    ▼
SQL Server Provider translates to T-SQL
    ▼
Database executes queries
    ▼
Results mapped to Model objects
    ▼
Controller passes Model to View
    ▼
Razor Engine renders HTML
    ├─ _Layout.cshtml (master template)
    ├─ Action View (content)
    └─ Static Assets (CSS, JS from wwwroot)
    ▼
HTML Response + Bootstrap/jQuery
    ▼
Browser renders page with sidebar navigation
```

### Current Application State

**🔴 Development Phase:** Views and routing structure exist, but:
- ❌ No data models with DbSet properties
- ❌ No service layer for business logic
- ❌ No entity relationships defined
- ❌ No data being displayed
- ❌ No form submission handlers
- ❌ No authentication/authorization logic
- ⚠️ Database context is empty skeleton

**✅ Ready for Development:**
- Basic MVC structure in place
- UI framework configured
- Routing established
- Logging infrastructure ready
- EF Core and SQL Server packages installed

---

## 11. Technology Stack Summary

| Layer | Technology | Version |
|-------|-----------|---------|
| **Runtime** | .NET | 8.0 |
| **Web Framework** | ASP.NET Core | 8.0 (Web SDK) |
| **Architecture** | MVC | Native |
| **ORM** | Entity Framework Core | 8.0.0 |
| **Database** | SQL Server | (latest provider) |
| **Frontend Framework** | Bootstrap | Latest |
| **JavaScript** | jQuery | Latest |
| **Icons** | Bootstrap Icons | 1.11.0 |

---

## 12. Key File Relationships

```
Program.cs
    ├─ Registers Services (ControllersWithViews)
    ├─ Configures Pipeline
    └─ Routes to HomeController

HomeController.cs
    ├─ Uses Services (ILogger)
    ├─ Returns Views (Views/Home/*.cshtml)
    └─ Should use ApplicationDBContext (not yet)

Views/Shared/_Layout.cshtml
    ├─ Imports Bootstrap/jQuery/Icons
    ├─ Includes CSS (site.css)
    ├─ Renders sidebar navigation
    └─ RenderBody() for page content

ApplicationDBContext.cs
    └─ Should connect to SQL Server (configuration missing)

appsettings.json
    ├─ Logging configuration
    └─ AllowedHosts
    └─ (Missing: ConnectionStrings)

Models/
    ├─ ErrorViewModel (exists)
    └─ (Additional models needed)
```

---

## 13. Recommended Next Steps for Development

1. **Database Setup**
   - Define entity models (Furniture, Inventory, Sales, Users)
   - Create DbSet<T> properties in ApplicationDBContext
   - Add connection string to appsettings.json
   - Create and apply EF Core migrations

2. **Data Layer Enhancement**
   - Configure entity relationships (1-to-many, many-to-many)
   - Add data validation annotations
   - Create repository pattern classes

3. **Service Layer**
   - Create services for business logic
   - Register services in Program.cs dependency injection

4. **Controller Enhancement**
   - Inject services into controllers
   - Implement action methods to fetch and display data
   - Add POST methods for create/update operations

5. **View Development**
   - Populate views with data models
   - Create forms for data entry
   - Implement data tables/grids
   - Add client-side validation

6. **Security**
   - Implement authentication (ASP.NET Core Identity)
   - Add authorization policies
   - Secure sensitive endpoints

7. **Testing**
   - Add unit tests for services
   - Add integration tests for controllers
   - Add UI tests

---

## 14. Application Summary

**FURNITRACK** is a furniture inventory management system built with ASP.NET Core 8.0 MVC. It features a responsive Bootstrap-based UI with a fixed sidebar navigation for core business areas: Dashboard, Inventory Management, Sales Tracking, Reporting, and User Management. The application uses Entity Framework Core 8.0 with SQL Server as the database provider. Currently, the MVC structure and routing are in place, but the data layer needs to be populated with entity models and the business logic layer needs to be implemented. The application follows ASP.NET Core best practices with dependency injection, configuration management, and a clear separation of concerns between controllers, views, and data access layers.

