# FURNITRACK - Complete Program Structure Analysis

## 📋 Project Overview

**FURNITRACK** is an **ASP.NET Core 8.0 MVC web application** designed for **Furniture Inventory Management**. It provides a modern, responsive web interface for managing inventory, sales, reports, and user management.

---

## 🏗️ Architecture Summary

```
┌─────────────────────────────────────────┐
│          ASP.NET Core 8.0 MVC           │
│      (Controllers + Views + Models)     │
├─────────────────────────────────────────┤
│   Entity Framework Core 8.0 (ORM)       │
│   ↓                                     │
│   SQL Server Database                   │
└─────────────────────────────────────────┘
```

---

## 📁 Folder Structure & Purpose

### **Controllers/** 
- **HomeController.cs** - Single entry point for all app features
  - 6 main routes: `Index`, `Dashboard`, `Inventory`, `Sales`, `Reports`, `Users`
  - `Error()` action for exception handling
  - Dependency: ILogger for logging

### **Views/**
- **Shared/_Layout.cshtml** - Master layout template
  - Fixed navy sidebar (#1a335d color) with navigation links
  - Responsive Bootstrap 5 grid layout
  - Conditional sidebar (hidden on Index page)
  - Bootstrap Icons CDN integration
  - Navigation items:
    - Dashboard (grid icon)
    - Inventory (box icon)
    - Sales (cart icon)
    - Reports (chart icon)
    - Users (people icon)

- **Shared/_ViewImports.cshtml** - View imports and tag helpers
- **Shared/_ValidationScriptsPartial.cshtml** - Validation scripts
- **Shared/Error.cshtml** - Error page template
- **Home/Index.cshtml** - Landing/splash page (no sidebar)
- **Home/Dashboard.cshtml** - Dashboard view (placeholder)
- **Home/Inventory.cshtml** - Inventory management view
- **Home/Sales.cshtml** - Sales tracking view
- **Home/Reports.cshtml** - Reports/analytics view
- **Home/Users.cshtml** - User management view

### **Models/**
- **ErrorViewModel.cs** - Error page view model
  - RequestId property for error tracking
  - Optional ShowRequestId helper property

### **Data/**
- **ApplicationDBContext.cs** - Entity Framework Core DbContext
  - Currently empty (no DbSets defined)
  - Ready for entity model configuration
  - SQL Server provider configured

### **Properties/**
- **launchSettings.json** - Development server configuration
  - **HTTP Profile**: localhost:5178
  - **HTTPS Profile**: localhost:7236 (SSL)
  - **IIS Express**: port 44326
  - Auto-launches browser on startup

### **wwwroot/**
- **css/site.css** - Custom application styles
- **js/site.js** - Custom JavaScript
- **lib/** - Client-side libraries
  - Bootstrap 5 (CSS framework)
  - jQuery (DOM manipulation)
  - jQuery Validation
  - jQuery Validation Unobtrusive
  - Bootstrap Icons (via CDN)

### **Configuration Files**
- **FURNITRACK.csproj** - Project metadata
  - Target Framework: .NET 8.0
  - Nullable reference types: Enabled
  - Implicit usings: Enabled
- **appsettings.json** - Application settings
  - Logging configuration
  - Allowed hosts: "*" (unrestricted)
- **appsettings.Development.json** - Development-specific settings
- **Program.cs** - Application entry point and DI configuration

---

## 🔧 Technology Stack

### Backend
| Technology | Version | Purpose |
|-----------|---------|---------|
| .NET Framework | 8.0 | Runtime and libraries |
| ASP.NET Core | 8.0 | Web framework |
| Entity Framework Core | 8.0.0 | ORM for database |
| SQL Server Provider | 8.0.0 | Database connectivity |
| EF Tools | 8.0.0 | Migrations and scaffolding |

### Frontend
| Technology | Version | Purpose |
|-----------|---------|---------|
| Bootstrap | 5.x | Responsive UI framework |
| jQuery | Latest | DOM manipulation |
| Bootstrap Icons | 1.11.0 | Icon library |
| jQuery Validation | - | Form validation |

---

## 🔄 Application Flow

```
1. User navigates to app
   ↓
2. Program.cs initializes:
   - AddControllersWithViews() → Register MVC services
   - UseRouting() → Enable routing
   - MapControllerRoute() → Default: {Home}/{Index}/{id}
   ↓
3. Request hits HomeController
   ↓
4. Action method returns View()
   ↓
5. View renders using _Layout.cshtml master
   - Loads CSS/JS libraries
   - Renders sidebar navigation
   - Injects action-specific content
   ↓
6. Response sent to browser
```

---

## 🚀 Startup Configuration

### Program.cs Pipeline
```csharp
1. CreateBuilder() - Initialize application
2. AddControllersWithViews() - Register MVC
3. Build() - Create application
4. Conditional error handling (non-dev)
5. UseHttpsRedirection() - Force HTTPS
6. UseStaticFiles() - Serve wwwroot files
7. UseRouting() - Enable routing
8. UseAuthorization() - Enable auth middleware
9. MapControllerRoute() - Configure default route
10. Run() - Start listening
```

### Default Route Pattern
```
{controller=Home}/{action=Index}/{id?}
```
- Defaults to HomeController.Index()
- All other routes manually navigated via sidebar

---

## 📊 Current State Analysis

### ✅ Completed
- **UI Structure**: Complete responsive layout with sidebar
- **Routing**: All 6 main features routed through HomeController
- **View Templates**: All 7 views created (Index + 6 feature views)
- **Frontend Libraries**: Bootstrap, jQuery, validation libraries configured
- **Project Setup**: Project file and launch configuration properly set

### ⚠️ In Progress
- **Database Context**: ApplicationDBContext empty (no DbSets)
- **Models**: Only ErrorViewModel exists; no domain models
- **Data Access**: No repositories or data services

### ❌ Not Started
- **Business Logic**: No services or business layer
- **Database Migrations**: Not created
- **View Models**: Feature views using no models
- **API Endpoints**: No API controllers
- **Authentication**: No auth implementation
- **Validation**: Basic form validation framework only
- **Testing**: No unit or integration tests

---

## 📐 MVC Components Breakdown

### Controllers (1 total)
```
HomeController
├── Index() → Index.cshtml
├── Dashboard() → Dashboard.cshtml
├── Inventory() → Inventory.cshtml
├── Sales() → Sales.cshtml
├── Reports() → Reports.cshtml
├── Users() → Users.cshtml
└── Error() → Error.cshtml
```

### Models (1 total)
```
ErrorViewModel
└── RequestId: string
```

### Views (7 total)
```
_Layout.cshtml (Master)
├── Sidebar Navigation
├── CSS/JS Includes
└── @RenderBody()
    ├── Index.cshtml (No sidebar)
    ├── Dashboard.cshtml
    ├── Inventory.cshtml
    ├── Sales.cshtml
    ├── Reports.cshtml
    ├── Users.cshtml
    └── Error.cshtml
```

---

## 🎨 UI/UX Details

### Theme Colors
- **Primary Background**: #1a335d (Dark Navy Blue)
- **Text**: White (#FFFFFF)
- **Secondary**: Light gray (#FFFFFF-50 for muted text)

### Sidebar Features
- Fixed positioning (doesn't scroll)
- Z-index: 1000 (always on top)
- Minimum width: 250px
- Active link highlighting: `.active-link` class
- Icons from Bootstrap Icons CDN

### Responsive Behavior
- Bootstrap 5 responsive grid
- Mobile-first approach
- Fixed sidebar may need responsive adjustment for mobile

---

## 🔐 Security Features

### Configured
- HTTPS redirection enabled (UseHttpsRedirection)
- HSTS enabled (30-day default)
- Authorization middleware included

### Not Yet Implemented
- Authentication mechanism (login/logout)
- Authorization policies
- User identity management
- CSRF protection
- Input validation/sanitization

---

## 🗄️ Database Configuration

### Current State
- Provider: SQL Server
- Connection string: Not configured in shown files
- DbContext: Empty (ready for models)
- Migrations: Not created

### Next Steps
1. Define entity models (Product, Furniture, Sales, User, etc.)
2. Configure DbSets in ApplicationDBContext
3. Add connection string to appsettings.json
4. Create and apply migrations
5. Seed initial data

---

## 📝 Entry Points

### Main Entry
- **Program.cs** - Application startup and DI configuration

### Web Entry
- **HomeController** - All web requests route here
- **Default Action**: HomeController.Index()

### Starting URL
- Development: `https://localhost:7236` or `http://localhost:5178`
- IIS Express: `https://localhost:44326`

---

## 🛠️ Development Environment

### Build Configuration
- Debug output: `bin/Debug/net8.0/`
- Object files: `obj/Debug/net8.0/`
- Static web assets configured
- CSS scoped styles support

### Required Tools
- .NET 8.0 SDK
- Visual Studio 2022 or VS Code with C# extension
- SQL Server (local or remote)
- Package restore: via .NET CLI or NuGet

---

## 📦 Dependencies Graph

```
FURNITRACK
├── Microsoft.EntityFrameworkCore (8.0.0)
│   ├── Core ORM functionality
│   └── DbContext base class
├── Microsoft.EntityFrameworkCore.SqlServer (8.0.0)
│   └── SQL Server provider
├── Microsoft.EntityFrameworkCore.Tools (8.0.0)
│   └── Migration tools (CLI)
├── Bootstrap 5 (via CDN + lib)
│   └── UI components
├── jQuery (via lib)
│   └── DOM manipulation
└── Bootstrap Icons (via CDN)
    └── Icon library
```

---

## 🎯 Recommended Development Priority

### Phase 1: Data Layer
1. Define furniture/product entities
2. Define user entity
3. Configure ApplicationDBContext
4. Create migrations
5. Seed sample data

### Phase 2: Service Layer
1. Create repository pattern
2. Implement business logic services
3. Add dependency injection

### Phase 3: Views & Models
1. Create feature-specific ViewModels
2. Populate views with real data
3. Implement form handling

### Phase 4: Features
1. CRUD operations for each feature
2. Authentication & authorization
3. Validation & error handling
4. Reports/analytics

### Phase 5: Polish
1. UI/UX improvements
2. Performance optimization
3. Error handling
4. Testing & debugging

---

## 📊 Quick Stats

| Metric | Count |
|--------|-------|
| Controllers | 1 |
| Views | 7 |
| Models | 1 |
| CSS Files | 1 custom + Bootstrap |
| JS Files | 1 custom + jQuery |
| NuGet Dependencies | 3 |
| Database Entities | 0 (yet) |
| Configured Routes | 1 default + 6 actions |
| Lines of Code (approx) | ~500-600 |

---

## 🔗 File Relationships

```
Program.cs
├── Uses: HomeController
├── Configures: DbContext (not yet)
└── Serves: Static files from wwwroot/

HomeController
├── Returns: Views from ~/Views/Home/
├── Logs: Using ILogger
├── Models: ErrorViewModel
└── Uses: _Layout.cshtml

_Layout.cshtml
├── Includes: Bootstrap CSS, jQuery, Icons
├── Loads: site.css, site.js
├── Navigates: HomeController actions
└── Renders: @RenderBody() content

ApplicationDBContext
├── Inherits: DbContext
├── Configured: SqlServer provider
└── Ready for: Entity definitions
```

---

## ✨ Current Capabilities

- ✅ Browse to landing page
- ✅ Navigate between feature areas via sidebar
- ✅ Responsive UI with Bootstrap
- ✅ Error handling page
- ✅ Development HTTPS

## ❌ Missing Capabilities

- ❌ Display actual data from database
- ❌ User management/authentication
- ❌ Form submission and processing
- ❌ Real inventory operations
- ❌ Sales tracking
- ❌ Reporting functionality

---

## 📚 For Next Development

Start by implementing the data models and EF Core configuration to enable the application to interact with the SQL Server database. This is the critical path for enabling all subsequent features.
