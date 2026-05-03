# FURNITRACK - Architecture Diagrams

## System Architecture

```
┌─────────────────────────────────────────────────────────────┐
│                     CLIENT LAYER (Browser)                  │
│  HTML/CSS/JavaScript - Bootstrap 5 + jQuery + Bootstrap-Icons
└────────────────────────┬────────────────────────────────────┘
                         │
                    HTTPS/HTTP
                         │
        ┌────────────────▼────────────────┐
        │   ASP.NET Core 8.0 Web Server   │
        │   Kestrel (or IIS Express)      │
        └────────────────┬────────────────┘
                         │
        ┌────────────────▼────────────────┐
        │    Routing Middleware            │
        │ (MapControllerRoute)             │
        └────────────────┬────────────────┘
                         │
        ┌────────────────▼────────────────────────────┐
        │         HomeController (MVC)                │
        │  • Index         • Dashboard                │
        │  • Inventory     • Sales                    │
        │  • Reports       • Users                    │
        └────────┬──────────────────────┬─────────────┘
                 │                      │
        ┌────────▼──────────┐  ┌────────▼──────────┐
        │  View Renderer    │  │  Model Binding    │
        │  (.cshtml)        │  │  (ErrorViewModel) │
        └────────────────────┘  └────────┬──────────┘
                                        │
                                ┌───────▼──────────────┐
                                │  Data Layer (EF Core)│
                                │ (ApplicationDBContext)
                                └───────┬──────────────┘
                                        │
                        ┌───────────────▼─────────────┐
                        │   SQL Server Database       │
                        │   (Connection String: TBD)  │
                        └─────────────────────────────┘
```

## Request/Response Flow

```
User Request
    ↓
[Browser] → HTTPS/HTTP → [Kestrel Server]
    ↓
[Program.cs - Middleware Pipeline]
├─ UseHttpsRedirection()
├─ UseStaticFiles() [/wwwroot]
├─ UseRouting()
├─ UseAuthorization()
└─ MapControllerRoute()
    ↓
[Routing Engine] → Match route pattern: {Home}/{Index}/{id?}
    ↓
[HomeController] ← ILogger dependency injected
    ↓
[Action Method] (Dashboard/Inventory/Sales/Reports/Users)
    ↓
View() → Render view file
    ↓
[_Layout.cshtml] ← Master template
├─ Load CSS: Bootstrap + site.css
├─ Load JS: jQuery + Bootstrap Icons
├─ Render Sidebar Navigation (unless Index)
└─ Inject @RenderBody() content
    ↓
[HTML Response] + CSS + JS
    ↓
[Browser] ← Render and display
    ↓
User sees page
```

## MVC Pattern Implementation

```
                    MODEL
                      ↑
                      │
              (Data/Business Logic)
                      │
    ┌──────────────────┼──────────────────┐
    │                  │                  │
    ↓                  ↓                  ↓
CONTROLLER          VIEW              MODELS
    │               (Display)         (Data)
[HomeController]  [.cshtml files]  [ErrorViewModel]
    │               (Templates)
    │
    ├─ Dashboard()
    ├─ Inventory()
    ├─ Sales()
    ├─ Reports()
    ├─ Users()
    └─ Index()

Controller ← User Input (Forms/Navigation)
    ↓
Process Request
    ↓
Query/Update Model
    ↓
Pass Model to View
    ↓
View Renders HTML Response
    ↓
Back to User
```

## Application Lifecycle

```
1. [Startup]
   ├─ Program.cs runs
   ├─ ServiceCollection configured:
   │  └─ AddControllersWithViews()
   ├─ WebApplication built
   └─ Middleware pipeline configured

2. [Request Arrives]
   ├─ HTTPS redirect middleware
   ├─ Static file middleware (check /wwwroot)
   ├─ Routing middleware
   ├─ Authorization middleware
   └─ Controller routing

3. [Controller Action Executes]
   ├─ Dependency injection (ILogger)
   ├─ Business logic (currently: none)
   └─ Return View()

4. [View Rendering]
   ├─ Layout template (_Layout.cshtml) loads
   ├─ CSS/JS dependencies loaded:
   │  ├─ Bootstrap 5
   │  ├─ jQuery
   │  ├─ site.css + site.js
   │  └─ Bootstrap Icons
   ├─ Sidebar navigation rendered
   ├─ @RenderBody() injects action view
   └─ Final HTML generated

5. [Response Sent]
   ├─ Status 200 OK
   ├─ HTML content
   └─ JavaScript execution

6. [Browser Rendering]
   ├─ Parse HTML
   ├─ Load styles (Bootstrap + custom)
   ├─ Execute scripts (jQuery)
   └─ Display interactive page
```

## Folder Structure Tree with Responsibilities

```
FURNITRACK/
│
├─ Program.cs ⭐ APP ENTRY POINT
│  └─ Initializes services, middleware, routing
│
├─ Controllers/ 🎛️ REQUEST HANDLERS
│  └─ HomeController.cs
│     ├─ Index() → Landing page
│     ├─ Dashboard() → Dashboard view
│     ├─ Inventory() → Inventory view
│     ├─ Sales() → Sales view
│     ├─ Reports() → Reports view
│     └─ Users() → Users view
│
├─ Views/ 📄 PRESENTATION
│  ├─ Shared/
│  │  ├─ _Layout.cshtml ← Master template
│  │  │  ├─ HTML structure
│  │  │  ├─ CSS includes (Bootstrap)
│  │  │  ├─ JS includes (jQuery)
│  │  │  └─ Sidebar navigation (conditional)
│  │  ├─ _ViewImports.cshtml ← Global directives
│  │  ├─ _ValidationScriptsPartial.cshtml
│  │  └─ Error.cshtml ← Error page
│  │
│  └─ Home/
│     ├─ Index.cshtml (no sidebar)
│     ├─ Dashboard.cshtml
│     ├─ Inventory.cshtml
│     ├─ Sales.cshtml
│     ├─ Reports.cshtml
│     └─ Users.cshtml
│
├─ Models/ 📊 DATA STRUCTURES
│  └─ ErrorViewModel.cs (1 view model)
│     └─ RequestId: string
│
├─ Data/ 🗄️ DATABASE LAYER
│  └─ ApplicationDBContext.cs (EMPTY - READY FOR ENTITIES)
│     ├─ Inherits: DbContext
│     └─ Provider: SQL Server (configured)
│
├─ Properties/ ⚙️ CONFIGURATION
│  └─ launchSettings.json
│     ├─ HTTP: localhost:5178
│     ├─ HTTPS: localhost:7236
│     └─ IIS Express: port 44326
│
├─ wwwroot/ 🎨 STATIC ASSETS
│  ├─ css/
│  │  └─ site.css (custom styles)
│  ├─ js/
│  │  └─ site.js (custom scripts)
│  └─ lib/ (third-party libraries)
│     ├─ bootstrap/ (CSS framework)
│     ├─ jquery/ (DOM manipulation)
│     ├─ jquery-validation/
│     └─ jquery-validation-unobtrusive/
│
├─ appsettings.json 📋 APP SETTINGS
│  ├─ Logging configuration
│  └─ AllowedHosts: "*"
│
├─ appsettings.Development.json 📋 DEV SETTINGS
│
├─ FURNITRACK.csproj 📦 PROJECT FILE
│  ├─ TargetFramework: net8.0
│  ├─ Nullable: enable
│  ├─ ImplicitUsings: enable
│  └─ NuGet Packages:
│     ├─ EntityFrameworkCore 8.0.0
│     ├─ EntityFrameworkCore.SqlServer 8.0.0
│     └─ EntityFrameworkCore.Tools 8.0.0
│
└─ bin/ & obj/ 🔨 BUILD ARTIFACTS
   └─ Compiled assemblies and dependencies
```

## Dependency Injection Graph

```
Program.cs
    │
    ├─ builder.Services.AddControllersWithViews()
    │  └─ Registers MVC services
    │
    └─ IServiceProvider created
       │
       ├─ HomeController (uses ILogger<HomeController>)
       │  └─ ILogger<HomeController> injected into constructor
       │
       └─ [Ready for] DbContext injection (ApplicationDBContext)
          └─ SQL Server connection
```

## Navigation Structure

```
Landing Page (Index)
    ↓ (sidebar hidden)
    
↙─────────────────────────────────────────────────────────↘

Dashboard          Inventory          Sales          Reports          Users
   ↓                  ↓                 ↓               ↓                ↓
(Dashboard.cshtml) (Inventory.cshtml) (Sales.cshtml) (Reports.cshtml) (Users.cshtml)
   ↓                  ↓                 ↓               ↓                ↓
All have sidebar navigation available
```

## Technology Stack Layers

```
┌─────────────────────────────────────────────────────────────┐
│ PRESENTATION LAYER (Client)                                 │
│ ├─ HTML/CSS/JavaScript                                      │
│ ├─ Bootstrap 5 Framework                                    │
│ ├─ jQuery                                                   │
│ └─ Bootstrap Icons (CDN)                                    │
└─────────────────────────────────────────────────────────────┘
                              ↓
┌─────────────────────────────────────────────────────────────┐
│ WEB APPLICATION LAYER                                       │
│ ├─ ASP.NET Core 8.0 MVC                                     │
│ ├─ Kestrel Server                                           │
│ ├─ Routing Engine                                           │
│ ├─ Controller (HomeController)                              │
│ └─ View Engine (Razor)                                      │
└─────────────────────────────────────────────────────────────┘
                              ↓
┌─────────────────────────────────────────────────────────────┐
│ BUSINESS LOGIC LAYER                                        │
│ ├─ Services (NOT YET IMPLEMENTED)                           │
│ ├─ Repositories (NOT YET IMPLEMENTED)                       │
│ └─ View Models (ErrorViewModel only)                        │
└─────────────────────────────────────────────────────────────┘
                              ↓
┌─────────────────────────────────────────────────────────────┐
│ DATA ACCESS LAYER                                           │
│ ├─ Entity Framework Core 8.0 (ORM)                          │
│ ├─ ApplicationDBContext                                     │
│ └─ SQL Server Provider                                      │
└─────────────────────────────────────────────────────────────┘
                              ↓
┌─────────────────────────────────────────────────────────────┐
│ DATABASE LAYER                                              │
│ ├─ SQL Server Database                                      │
│ └─ Connection String (NOT YET CONFIGURED)                   │
└─────────────────────────────────────────────────────────────┘
```

## Current vs. Future Architecture

```
CURRENT STATE (UI + Routing):
┌──────────────┐
│   Views      │ ← Static HTML pages
│  (7 pages)   │   No data binding
└────┬─────────┘
     │
     ↓
┌──────────────┐
│  Controller  │ ← Basic routing only
│  (1 only)    │   No business logic
└────┬─────────┘
     │
     ↓
   [Nothing]   ← No data layer yet


FUTURE STATE (Full Stack):
┌──────────────┐
│   Views      │ ← Dynamic pages with data
│  (7 pages)   │   Model binding, validation
└────┬─────────┘
     │
     ↓
┌──────────────┐
│  Controller  │ ← Request handling
│  (1-N)       │   Data orchestration
└────┬─────────┘
     │
     ↓
┌──────────────┐
│  Services    │ ← Business logic
│  Layer       │   Validation, operations
└────┬─────────┘
     │
     ↓
┌──────────────┐
│  Repository  │ ← Data access abstraction
│  Pattern     │   Query building
└────┬─────────┘
     │
     ↓
┌──────────────┐
│  DbContext   │ ← Entity mapping
│  (EF Core)   │   Database operations
└────┬─────────┘
     │
     ↓
┌──────────────┐
│   Database   │ ← Persisted data
│ (SQL Server) │   Tables, relationships
└──────────────┘
```

## Session Object Diagram

```
HTTP Request Session
├─ Request
│  ├─ Headers (HTTPS, cookies, etc.)
│  ├─ Query parameters {id?}
│  ├─ Form body (if POST)
│  └─ Route data {controller, action}
│
├─ Processing
│  ├─ Route matching: {Home}/{Index}
│  ├─ Controller resolution: HomeController
│  ├─ Action selection: Index()
│  ├─ Dependency injection: ILogger
│  ├─ Action execution: return View()
│  └─ Model passed to view: (null currently)
│
└─ Response
   ├─ StatusCode: 200 OK
   ├─ Content-Type: text/html
   ├─ Rendered HTML (from _Layout + view)
   ├─ CSS/JS assets
   └─ Sent to browser

```

## Key Entry Points Map

```
USER ENTRY POINTS:
  ↓
1. Browser URL: https://localhost:7236
   ↓
2. Program.cs starts application
   ├─ Initialize ASP.NET Core
   ├─ Build middleware pipeline
   ├─ Add services (MVC)
   └─ Run server
   ↓
3. Routing Engine intercepts request
   ├─ Match: "{controller=Home}/{action=Index}/{id?}"
   ├─ Extract: controller=Home, action=Index
   └─ Route to: HomeController.Index()
   ↓
4. HomeController.Index() executes
   ├─ ILogger dependency injected
   ├─ No business logic (placeholder)
   └─ return View()
   ↓
5. View Engine renders
   ├─ Find: ~/Views/Home/Index.cshtml
   ├─ Use layout: _Layout.cshtml
   ├─ Skip sidebar (isIndexPage == true)
   ├─ Include styles + scripts
   └─ Generate HTML response
   ↓
6. Browser receives HTML/CSS/JS
   └─ Render and display page
      With sidebar navigation to:
      - Dashboard, Inventory, Sales, Reports, Users
```

---

These diagrams provide comprehensive visualization of the application's structure, data flow, technology stack, and architecture at different levels of abstraction.
