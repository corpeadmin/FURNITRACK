using System.Diagnostics;
using System.Security.Claims;
using FURNITRACK.Models;
using FURNITRACK.Services;
using FURNITRACK.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FURNITRACK.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IUserService _userService;
        private readonly ISalesService _salesService;

        public HomeController(
            ILogger<HomeController> logger,
            IProductService productService,
            ICategoryService categoryService,
            IUserService userService,
            ISalesService salesService)
        {
            _logger = logger;
            _productService = productService;
            _categoryService = categoryService;
            _userService = userService;
            _salesService = salesService;
        }

        [AllowAnonymous]
        public IActionResult Index(string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction(nameof(Dashboard));
            }
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userService.ValidateUserAsync(model.Email, model.Password);
                    if (user != null)
                    {
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                            new Claim(ClaimTypes.Email, user.Email),
                            new Claim(ClaimTypes.Role, user.Role),
                            new Claim("UserId", user.UserId.ToString())
                        };

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        var authProperties = new AuthenticationProperties
                        {
                            IsPersistent = model.RememberMe,
                            ExpiresUtc = DateTimeOffset.UtcNow.AddHours(2)
                        };

                        await HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity),
                            authProperties);

                        _logger.LogInformation($"User {user.Email} logged in at {DateTime.UtcNow}");

                        if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                        {
                            return Redirect(model.ReturnUrl);
                        }
                        return RedirectToAction(nameof(Dashboard));
                    }

                    ModelState.AddModelError(string.Empty, "Invalid login attempt. Please check your email and password.");
                }
                return View("Index", model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Login error: {ex.Message}");
                ModelState.AddModelError(string.Empty, "An error occurred during login. Please try again later.");
                return View("Index", model);
            }
        }

        public async Task<IActionResult> Dashboard()
        {
            try
            {
                var startOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                var endOfMonth = DateTime.Now;

                var lowStockProducts = await _productService.GetLowStockProductsAsync();
                var totalProducts = (await _productService.GetAllProductsAsync()).Count;
                var totalUsers = (await _userService.GetAllUsersAsync()).Count;
                var salesThisMonth = await _salesService.GetSalesByDateRangeAsync(startOfMonth, endOfMonth);
                var recentSales = (await _salesService.GetAllSalesAsync()).Take(5).ToList();

                decimal totalSalesThisMonth = salesThisMonth.Sum(s => s.TotalAmount);
                decimal averageSaleValue = salesThisMonth.Count > 0 ? totalSalesThisMonth / salesThisMonth.Count : 0;

                var viewModel = new DashboardViewModel
                {
                    TotalProducts = totalProducts,
                    LowStockProducts = lowStockProducts.Count,
                    TotalUsers = totalUsers,
                    TotalSalesThisMonth = totalSalesThisMonth,
                    SalesThisMonth = salesThisMonth.Count,
                    AverageSaleValue = averageSaleValue,
                    LowStockItems = lowStockProducts.Take(5).ToList(),
                    RecentSales = recentSales
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error loading dashboard: {ex.Message}");
                return View(new DashboardViewModel());
            }
        }

        public async Task<IActionResult> Inventory()
        {
            try
            {
                var products = await _productService.GetAllProductsAsync();
                var categories = await _categoryService.GetAllCategoriesAsync();
                var lowStockProducts = await _productService.GetLowStockProductsAsync();

                var viewModel = new InventoryViewModel
                {
                    Products = products,
                    Categories = categories,
                    TotalProducts = products.Count,
                    LowStockCount = lowStockProducts.Count,
                    TotalInventoryValue = products.Sum(p => p.UnitPrice * p.QuantityInStock)
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error loading inventory: {ex.Message}");
                return View(new InventoryViewModel());
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduct(Product product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _productService.CreateProductAsync(product);
                    return RedirectToAction(nameof(Inventory));
                }

                // Log validation errors
                var errors = string.Join(" | ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                _logger.LogWarning($"Product creation failed validation: {errors}");

                return await Inventory();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating product: {ex.Message}");
                return RedirectToAction(nameof(Inventory));
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProduct(Product product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _productService.UpdateProductAsync(product);
                    return RedirectToAction(nameof(Inventory));
                }
                return await Inventory();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating product: {ex.Message}");
                return RedirectToAction(nameof(Inventory));
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                await _productService.DeleteProductAsync(id);
                return RedirectToAction(nameof(Inventory));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting product: {ex.Message}");
                return RedirectToAction(nameof(Inventory));
            }
        }

        public async Task<IActionResult> Sales()
        {
            try
            {
                var sales = await _salesService.GetAllSalesAsync();
                var products = await _productService.GetAllProductsAsync();

                var viewModel = new SalesViewModel
                {
                    Sales = sales,
                    Products = products,
                    TotalOrders = sales.Count,
                    TotalRevenue = sales.Sum(s => s.TotalAmount),
                    AverageOrderValue = sales.Count > 0 ? sales.Sum(s => s.TotalAmount) / sales.Count : 0
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error loading sales: {ex.Message}");
                return View(new SalesViewModel());
            }
        }

        [HttpPost]
        public async Task<IActionResult> RecordSale([FromBody] SaleRequestDto request)
        {
            try
            {
                if (request == null || !request.Items.Any())
                {
                    return BadRequest("Invalid sale request.");
                }

                // Get current logged in user ID from claims
                var userIdClaim = User.FindFirst("UserId")?.Value;
                int userId;
                if (!int.TryParse(userIdClaim, out userId))
                {
                    // Fallback to first user if claim is missing (should not happen with [Authorize])
                    var user = (await _userService.GetAllUsersAsync()).FirstOrDefault();
                    userId = user?.UserId ?? 1;
                }

                decimal subTotal = request.Items.Sum(i => i.Price * i.Quantity);
                decimal tax = subTotal * 0.12m; // 12% VAT
                decimal total = subTotal + tax;

                var sale = new Sales
                {
                    OrderNumber = $"ORD-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 4).ToUpper()}",
                    SalesDate = DateTime.Now,
                    CustomerName = request.CustomerName ?? "Walk-in Customer",
                    CustomerEmail = request.CustomerEmail ?? "walkin@example.com",
                    SubTotal = subTotal,
                    TaxAmount = tax,
                    TotalAmount = total,
                    Status = "Completed",
                    UserId = userId,
                    SalesItems = request.Items.Select(i => new SalesItem
                    {
                        ProductId = i.ProductId,
                        Quantity = i.Quantity,
                        UnitPrice = i.Price,
                        LineTotal = i.Price * i.Quantity
                    }).ToList()
                };

                // Create the sale
                await _salesService.CreateSaleAsync(sale);

                // Update inventory for each item
                foreach (var item in request.Items)
                {
                    var product = await _productService.GetProductByIdAsync(item.ProductId);
                    if (product != null)
                    {
                        product.QuantityInStock -= item.Quantity;
                        await _productService.UpdateProductAsync(product);
                    }
                }

                return Ok(new { message = "Sale recorded successfully!", orderNumber = sale.OrderNumber });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error recording sale: {ex.Message}");
                return StatusCode(500, "Internal server error.");
            }
        }
        // update 111
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Reports()
        {
            try
            {
                var startOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                var endOfMonth = DateTime.Now;

                var salesThisMonth = await _salesService.GetSalesByDateRangeAsync(startOfMonth, endOfMonth);
                var allProducts = await _productService.GetAllProductsAsync();
                var lowStockProducts = await _productService.GetLowStockProductsAsync();

                // Prepare data for the chart: Group sales by date
                var chartData = salesThisMonth
                    .GroupBy(s => s.SalesDate.Date)
                    .OrderBy(g => g.Key)
                    .Select(g => new {
                        Date = g.Key.ToString("MMM dd"),
                        Amount = g.Sum(s => s.TotalAmount)
                    }).ToList();

                var viewModel = new ReportsViewModel
                {
                    StartDate = startOfMonth,
                    EndDate = endOfMonth,
                    TotalSales = salesThisMonth.Sum(s => s.TotalAmount),
                    TotalOrders = salesThisMonth.Count,
                    AverageOrderValue = salesThisMonth.Count > 0 ? salesThisMonth.Sum(s => s.TotalAmount) / salesThisMonth.Count : 0,
                    TotalProducts = allProducts.Count,
                    LowStockItems = lowStockProducts.Count,
                    TotalInventoryValue = allProducts.Sum(p => p.UnitPrice * p.QuantityInStock),
                    // Pass the serialized data to the view
                    SalesChartData = System.Text.Json.JsonSerializer.Serialize(chartData)
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error loading reports: {ex.Message}");
                return View(new ReportsViewModel());
            }
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Users()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();
                var usersByRole = users.GroupBy(u => u.Role).ToDictionary(g => g.Key, g => g.Count());

                var viewModel = new UsersViewModel
                {
                    Users = users,
                    TotalUsers = users.Count,
                    ActiveUsers = users.Count(u => u.IsActive),
                    UsersByRole = usersByRole
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error loading users: {ex.Message}");
                return View(new UsersViewModel());
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser(User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _userService.CreateUserAsync(user);
                    return RedirectToAction(nameof(Users));
                }

                // Log validation errors
                var errors = string.Join(" | ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                _logger.LogWarning($"User creation failed validation: {errors}");

                return await Users();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating user: {ex.Message}");
                return RedirectToAction(nameof(Users));
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _userService.UpdateUserAsync(user);
                    return RedirectToAction(nameof(Users));
                }
                return await Users();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating user: {ex.Message}");
                return RedirectToAction(nameof(Users));
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                await _userService.DeleteUserAsync(id);
                return RedirectToAction(nameof(Users));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting user: {ex.Message}");
                return RedirectToAction(nameof(Users));
            }
        }

        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Index));
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
