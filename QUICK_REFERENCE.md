# FURNITRACK - Quick Reference Guide

## 🎯 What is FURNITRACK?

A **furniture inventory management web application** built with **ASP.NET Core 8.0 MVC** and **SQL Server**. It's a modern, responsive platform for tracking furniture stock, sales, reports, and user management.

---

## ⚡ Quick Facts

| Aspect | Details |
|--------|---------|
| **Framework** | ASP.NET Core 8.0 MVC |
| **Database** | SQL Server (via EF Core 8.0) |
| **Frontend** | Bootstrap 5 + jQuery |
| **Controllers** | 1 (HomeController with 6 routes) |
| **Views** | 7 (.cshtml files) |
| **Models** | 1 (ErrorViewModel) |
| **Entry Point** | Program.cs |
| **Default Route** | https://localhost:7236 |
| **Sidebar Theme** | Navy blue (#1a335d) |

---

## 🗺️ Navigation Map

```
Landing Page (Index) ──────────────┐
                                   │ (sidebar hidden)
                                   │
                    ┌──────────────┴──────────────┐
                    │                             │
              Click sidebar → Options:
                    │
    ┌───────────────┼───────────────┬───────────┬─────────┐
    │               │               │           │         │
Dashboard      Inventory         Sales      Reports     Users
```

---

## 📁 Key Files at a Glance

### Application Start
- **Program.cs** - Application initialization, middleware setup, service registration

### Web Entry Point
- **HomeController.cs** - Handles all 6 feature routes (Dashboard, Inventory, Sales, Reports, Users, Index)

### Views
- **_Layout.cshtml** - Master template with sidebar (hidden on Index)
- **Index.cshtml** - Landing/splash page
- **Dashboard.cshtml** - Dashboard page
- **Inventory.cshtml** - Inventory management
- **Sales.cshtml** - Sales tracking
- **Reports.cshtml** - Reports & analytics
- **Users.cshtml** - User management

### Data
- **ApplicationDBContext.cs** - Entity Framework Core context (currently empty)

### Configuration
- **FURNITRACK.csproj** - Project metadata
- **appsettings.json** - Application settings
- **launchSettings.json** - Development server configuration

---

## 🚀 How to Run

### Prerequisites
- .NET 8.0 SDK installed
- SQL Server accessible
- VS Code or Visual Studio 2022

### Steps
1. Navigate to project folder
2. Run: `dotnet restore` (restore NuGet packages)
3. Run: `dotnet build` (compile)
4. Run: `dotnet run` (start server)
5. Browser opens to: https://localhost:7236

### Alternative: Via Visual Studio
- Open FURNITRACK.sln
- Press F5 (Debug) or Ctrl+F5 (Run)

---

## 📊 Current Implementation Status

### ✅ Complete
- [x] UI/UX design (Bootstrap 5)
- [x] Responsive sidebar navigation
- [x] View templates (7 pages)
- [x] Routing configuration
- [x] Static asset setup (CSS, JS)
- [x] Project structure

### 🟡 Partial
- [x] Database provider configured (EF Core)
- [ ] Database connection string (not set)
- [ ] Database models (not defined)
- [ ] Database migrations (not created)

### ❌ Not Started
- [ ] Data models/entities
- [ ] Business logic/services
- [ ] View models (for features)
- [ ] Form handling
- [ ] Authentication/Authorization
- [ ] API endpoints
- [ ] Data validation
- [ ] Unit tests

---

## 🏗️ Architecture Layers

```
1. PRESENTATION (Browser)
   ↓ HTTPS/HTTP
2. WEB LAYER (ASP.NET Core MVC)
   Controllers → Views + Razor templates
   ↓
3. BUSINESS LOGIC (EMPTY - TODO)
   Services, repositories, DTOs
   ↓
4. DATA ACCESS (EF Core - READY)
   ApplicationDBContext
   ↓
5. DATABASE (SQL Server - CONFIGURED)
   Tables, relationships, stored procedures
```

---

## 🔗 Technology Stack Breakdown

### Backend
```
ASP.NET Core 8.0
├─ MVC Framework
├─ Razor View Engine
├─ Dependency Injection (built-in)
└─ Middleware Pipeline

Entity Framework Core 8.0
├─ ORM for data access
├─ LINQ query support
├─ Migrations
└─ SQL Server provider
```

### Frontend
```
Bootstrap 5
├─ CSS framework
├─ Responsive grid
├─ Pre-built components
└─ Utilities

jQuery
├─ DOM manipulation
├─ Event handling
├─ AJAX (future use)

Bootstrap Icons
└─ 1.11.0 CDN version
```

---

## 🎨 UI Features

### Sidebar
- **Color**: Navy blue (#1a335d)
- **Width**: 250px (minimum)
- **Position**: Fixed (doesn't scroll)
- **Visibility**: Shown on all pages except Index
- **Icons**: Bootstrap Icons (GitHub styling)

### Navigation Items
1. Dashboard (grid icon)
2. Inventory (box icon)
3. Sales (cart icon)
4. Reports (chart icon)
5. Users (people icon)

### Responsive Design
- Mobile-first approach
- Bootstrap grid system
- Sidebar may need responsive redesign for mobile

---

## 🔧 Development Workflow

### Local Development
1. **Run application**: `dotnet run`
2. **Hot reload**: Automatic on code changes (Development mode)
3. **Debug**: Press F5 in VS Code/Visual Studio
4. **Terminal**: Access at https://localhost:7236

### Common Commands
```powershell
# Restore dependencies
dotnet restore

# Build project
dotnet build

# Run application
dotnet run

# Create migration (after models added)
dotnet ef migrations add MigrationName

# Apply migration
dotnet ef database update

# Clean build
dotnet clean && dotnet build
```

---

## 📋 Configuration Files

### appsettings.json
- Logging levels
- AllowedHosts: "*" (all hosts allowed)
- Ready for: connection strings, API keys

### launchSettings.json
- **HTTP**: localhost:5178
- **HTTPS**: localhost:7236
- **IIS Express**: localhost:44326
- Auto-launch browser: enabled

### FURNITRACK.csproj
- Target Framework: .NET 8.0
- Nullable reference types: Enabled
- Implicit usings: Enabled
- NuGet packages: EF Core 8.0

---

## 🎯 Next Steps (Priority Order)

### Phase 1: Database Setup (CRITICAL)
1. Define furniture entity model
2. Add DbSet to ApplicationDBContext
3. Configure connection string in appsettings.json
4. Create and apply migrations
5. Seed sample data

### Phase 2: Service Layer
1. Create repository pattern
2. Add business logic services
3. Implement CRUD operations
4. Add dependency injection

### Phase 3: Feature Implementation
1. Create feature-specific view models
2. Populate views with real data
3. Implement form submission
4. Add validation

### Phase 4: Advanced Features
1. Authentication (login/logout)
2. Authorization (roles/permissions)
3. API endpoints (REST)
4. Error handling
5. Logging

### Phase 5: Quality & Deployment
1. Unit tests
2. Integration tests
3. Performance optimization
4. Security hardening
5. Deployment preparation

---

## 🐛 Common Issues & Solutions

### Issue: Application won't start
**Solution**: 
- Ensure .NET 8.0 is installed: `dotnet --version`
- Restore packages: `dotnet restore`
- Clean and rebuild: `dotnet clean && dotnet build`

### Issue: HTTPS certificate error
**Solution**:
- Trust dev certificate: `dotnet dev-certs https --trust`
- Restart application

### Issue: Cannot connect to database
**Solution**:
- Check connection string in appsettings.json
- Verify SQL Server is running
- Run migrations: `dotnet ef database update`

### Issue: Changes not reflected
**Solution**:
- Stop and restart application
- Check for compilation errors: `dotnet build`
- Clear browser cache (Ctrl+Shift+Del)

---

## 📚 Documentation Files

This analysis includes:
1. **STRUCTURE_ANALYSIS.md** - Detailed structural breakdown
2. **ARCHITECTURE_DIAGRAMS.md** - Visual architecture diagrams
3. **QUICK_REFERENCE.md** - This file

---

## 🔐 Security Notes

### Currently Configured
- ✅ HTTPS enforcement (UseHttpsRedirection)
- ✅ HSTS headers (30-day default)
- ✅ Static file serving (from wwwroot only)
- ✅ Authorization middleware (infrastructure ready)

### Not Yet Implemented
- ❌ Authentication (no login system)
- ❌ Authorization policies
- ❌ CSRF protection
- ❌ Input validation/sanitization
- ❌ SQL injection prevention (ready via EF Core)
- ❌ XSS protection

### Recommendations
1. Implement authentication before deployment
2. Add authorization policies for features
3. Validate all user input
4. Use HTTPS in production
5. Keep dependencies updated

---

## 📞 Key Contacts/Resources

### Official Documentation
- [ASP.NET Core](https://docs.microsoft.com/aspnet/core)
- [Entity Framework Core](https://docs.microsoft.com/ef/core)
- [Bootstrap 5](https://getbootstrap.com)
- [jQuery](https://jquery.com)

### NuGet Packages Used
```xml
<PackageReference Include="Microsoft.EntityFrameworkCore" Version="8.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="8.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="8.0.0" />
```

---

## 💡 Pro Tips

1. **Use migrations**: Never manually create database tables
   ```powershell
   dotnet ef migrations add YourMigrationName
   dotnet ef database update
   ```

2. **Debug with breakpoints**: Set breakpoints in controllers
   - F5 to debug
   - F10 to step over
   - F11 to step into

3. **Check logs**: Look at Application Output window for diagnostics

4. **Use Razor IntelliSense**: @Model. provides autocomplete in views

5. **Keep controllers thin**: Move logic to services

6. **Use dependency injection**: Don't create new instances directly

---

## 📊 Project Metrics

| Metric | Value |
|--------|-------|
| Total Files | ~50 (including bin/obj) |
| Source Files | ~15 |
| Views | 7 |
| Controllers | 1 |
| Models | 1 |
| CSS Rules | Custom + Bootstrap |
| JavaScript Lines | Basic setup |
| Database Entities | 0 (ready for development) |

---

## ✨ Summary

**FURNITRACK** is a well-structured ASP.NET Core 8.0 MVC application with a complete UI framework and routing setup. The presentation layer is ready, but the data layer needs development. Start by creating entity models and populating the ApplicationDBContext to enable database operations.

**Status**: 🟡 **50% Complete** - UI ready, Data layer pending

**Ready to**: Build data models, create migrations, implement business logic

**Not yet ready**: User operations, data persistence, reporting

---

*Last Updated: May 4, 2026*
