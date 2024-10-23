using DeskMarket.Data;
using DeskMarket.Data.Models;
using DeskMarket.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DeskMarket.Controllers;

public class ProductController : Controller
{
    private readonly ApplicationDbContext _context;

    public ProductController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        string? currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        List<IndexViewModel>? model = await _context.Products
            .Where(p => p.IsDeleted != true)
            .Select(p => new IndexViewModel()
            {
                Id = p.Id,
                ImageUrl = p.ImageUrl,
                ProductName = p.ProductName,
                Price = p.Price,
                IsSeller = p.SellerId == currentUserId,
                HasBought = p.ProductsClients.Any(pc => pc.ClientId == currentUserId)
            })
            .AsNoTracking()
            .ToListAsync();

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Add()
    {
        AddViewModel inputModel = new AddViewModel();
        inputModel.Categories = await _context.Categories
            .AsNoTracking()
            .ToListAsync();

        return View(inputModel);
    }

    [HttpPost]
    public async Task<IActionResult> Add(AddViewModel inputModel)
    {
        List<Category> categories = await _context.Categories.ToListAsync();
        inputModel.Categories = categories;

        if (!ModelState.IsValid)
        {
            return View(inputModel);
        }

        string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        Product product = new Product()
        {
            Id = inputModel.Id,
            ProductName = inputModel.ProductName,
            Price = inputModel.Price,
            Description = inputModel.Description,
            ImageUrl = inputModel.ImageUrl,
            AddedOn = DateTime.ParseExact(inputModel.AddedOn, "yyyy-MM-dd", null),
            CategoryId = inputModel.CategoryId,
            SellerId = userId ?? string.Empty
        };

        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        string? currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        DetailsViewModel? model = await _context.Products
            .Where(g => g.Id == id)
            .Select(g => new DetailsViewModel()
            {
                Id = id,
                ImageUrl = g.ImageUrl,
                ProductName = g.ProductName,
                Price = g.Price,
                Description = g.Description,
                Seller = g.Seller.UserName ?? string.Empty,
                AddedOn = g.AddedOn.ToString("dd-MM-yyyy"),
                CategoryName = g.Category.Name,
                HasBought = g.ProductsClients.Any(pc => pc.ClientId == currentUserId)
            })
            .AsNoTracking()
            .FirstOrDefaultAsync();

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        Product? product = await _context.Products.FindAsync(id);

        if (product == null)
        {
            return NotFound(); // Return a 404 error if the game is not found
        }

        List<Category> categories = await _context.Categories.ToListAsync();

        EditViewModel? model = new EditViewModel
        {
            ProductName = product.ProductName,
            Price = product.Price,
            Description = product.Description,
            ImageUrl = product.ImageUrl,
            AddedOn = product.AddedOn.ToString("yyyy-MM-dd"),
            SellerId = product.SellerId,
            CategoryId = product.CategoryId,
            Categories = categories
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(EditViewModel inputModel, int id)
    {
        List<Category> categories = await _context.Categories.ToListAsync();
        inputModel.Categories = categories;

        if (!ModelState.IsValid)
        {
            return View(inputModel);
        }

        Product product = _context.Products.FirstOrDefault(g => g.Id == id)!;

        product.ProductName = inputModel.ProductName;
        product.Price = inputModel.Price;
        product.Description = inputModel.Description;
        product.ImageUrl = inputModel.ImageUrl;
        product.AddedOn = DateTime.ParseExact(inputModel.AddedOn, "yyyy-MM-dd", null);
        product.SellerId = inputModel.SellerId;
        product.CategoryId = inputModel.CategoryId;

        await _context.SaveChangesAsync();

        return RedirectToAction("Details", "Product", new { id = product.Id });
    }

    [HttpGet]
    public async Task<IActionResult> Cart()
    {
        string getCurrentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

        List<CartViewModel>? model = await _context.Products
        .Where(g => g.IsDeleted == false)
        .Where(g => g.ProductsClients.Any(gg => gg.ClientId == getCurrentUserId))
        .Select(g => new CartViewModel()
        {
            Id = g.Id,
            ImageUrl = g.ImageUrl,
            ProductName = g.ProductName,
            Price = g.Price
        })
        .AsNoTracking()
        .ToListAsync();

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> AddToCart(int id)
    {
        string? clientId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        ProductClient? productClientCheck = await _context.ProductsClients
            .Where(pc => pc.ProductId == id && pc.ClientId == clientId)
            .FirstOrDefaultAsync();

        if (productClientCheck != null)
        {
            return RedirectToAction(nameof(Index));
        }

        ProductClient productClient = new ProductClient()
        {
            ProductId = id,
            ClientId = clientId ?? string.Empty
        };

        await _context.ProductsClients.AddAsync(productClient!);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> RemoveFromCart(int id)
    {
        string? clientId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        ProductClient? productClient = await _context.ProductsClients
            .Where(pc => pc.ProductId == id && pc.ClientId == clientId)
            .FirstOrDefaultAsync();

        if (productClient == null)
        {
            return RedirectToAction(nameof(Cart));
        }

        _context.ProductsClients.Remove(productClient);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Cart));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        DeleteViewModel? product = await _context.Products
            .Where(p => p.Id == id && p.IsDeleted == false)
            .Select(p => new DeleteViewModel()
            {
                Id = id,
                ProductName = p.ProductName,
                SellerId = p.SellerId,
                Seller = p.Seller.UserName ?? string.Empty
            })
            .AsNoTracking()
            .FirstOrDefaultAsync();

        return View(product);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(DeleteViewModel model)
    {
        Product? product = await _context.Products
           .Where(p => p.Id == model.Id && p.IsDeleted == false)
           .FirstOrDefaultAsync();

        if (product != null)
        {
            product.IsDeleted = true;
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }
}