using BookWebStore.Data;
using BookWebStore.Data.Models;
using BookWebStore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookWebStore;

public class OrderController : Controller
{
    private readonly BookStoreDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public OrderController(BookStoreDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Cart()
    {
        ApplicationUser? user = await _userManager.GetUserAsync(User);

        if (user == null)
        {
            return NotFound();
        }

        List<OrderCartViewModel> cart = await _context.OrdersBooks
        .Where(ob => ob.Order.BuyerId == user.Id && !ob.Order.IsCompleted)
        .Include(ob => ob.Order)
        .Include(ob => ob.Book)
        .Select(ob => new OrderCartViewModel
        {
            OrderId = ob.OrderId,
            BookId = ob.BookId,
            BookTitle = ob.Book.Title,
            ImageUrl = ob.Book.ImageUrl,
            Quantity = ob.Quantity,
            UnitPrice = ob.UnitPrice
        })
        .ToListAsync();

        return View(cart);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddToCart(Guid bookId)
    {
        ApplicationUser? user = await _userManager.GetUserAsync(User);

        if (user == null)
        {
            return NotFound();
        }

        int quantity = 1;

        Order? order = await _context.Orders
            .FirstOrDefaultAsync(o => o.BuyerId == user.Id && !o.IsCompleted);

        if (order == null)
        {
            //Creating the order number
            Random random = new Random();

            //Create a list of numbers from 1 to 9
            List<int> digits = Enumerable.Range(1, 9).ToList();
            List<int> shuffledDigits = digits.OrderBy(x => random.Next()).ToList();

            // Concatenate the digits into a single number
            string randomNumber = "#" + string.Join("", shuffledDigits);

            order = new Order
            {
                BuyerId = user.Id,
                OrderNumber = randomNumber,
                TotalPrice = 0
            };

            _context.Orders.Add(order);
        }

        Book? book = await _context.Books
            .FirstOrDefaultAsync(b => b.Id == bookId && !b.IsDeleted);

        if (book == null)
        {
            TempData["ErrorMessage"] = "The book is out of stock.";
            return RedirectToAction("Index", "Book");
        }

        //Check if the book is not already added to the Cart
        OrderBook? existingOrderBook = await _context.OrdersBooks
            .FirstOrDefaultAsync(ob => ob.OrderId == order.Id && ob.BookId == book.Id);

        if (existingOrderBook != null)
        {
            TempData["ErrorMessage"] = "The book has already been added to the cart.";
            return RedirectToAction("Cart", "Order");
        }
        else
        {
            order.OrdersBooks.Add(new OrderBook
            {
                OrderId = order.Id,
                BookId = book.Id,
                Quantity = quantity,
                UnitPrice = book.Price
            });

            //Calculating the total price
            order.TotalPrice += book.Price;
            book.Stock -= quantity;

            if (book.Stock <= 0)
            {
                book.IsDeleted = true;
            }
        }

        await _context.SaveChangesAsync();
        return RedirectToAction("Cart", "Order");
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> UpdateQuantity(Guid orderId, Guid bookId, int quantity)
    {
        if (quantity < 1)
        {
            return Json(new { success = false, error = "Quantity must be at least 1.", resetTo = 1 });
        }

        List<OrderBook>? orderBooks = await _context.OrdersBooks
            .Include(ob => ob.Book)
            .Include(ob => ob.Order)
            .Where(ob => ob.OrderId == orderId && !ob.Order.IsCompleted)
            .ToListAsync();

        OrderBook? orderBook = orderBooks.FirstOrDefault(ob => ob.BookId == bookId);

        if (orderBook == null)
        {
            return Json(new { success = false, error = "Item not found." });
        }

        int currentQuantityInCart = orderBook.Quantity;
        //Restoring the full available stock
        int? stockBeforeUpdate = orderBook.Book.Stock + currentQuantityInCart;
        int? newStock = stockBeforeUpdate - quantity;

        if (newStock < 0)
        {
            return Json(new
            {
                success = false,
                error = $"Only {stockBeforeUpdate} book{(stockBeforeUpdate == 1 ? "" : "s")} in stock.",
                resetTo = stockBeforeUpdate
            });
        }

        //Update availability
        orderBook.Book.Stock = newStock;
        orderBook.Book.IsDeleted = orderBook.Book.Stock == 0;

        orderBook.Quantity = quantity;
        decimal? itemTotal = orderBook.Quantity * orderBook.UnitPrice;
        orderBook.Order.TotalPrice = orderBooks.Sum(ob => ob.Quantity * ob.UnitPrice);

        await _context.SaveChangesAsync();

        return Json(new
        {
            success = true,
            itemTotal = itemTotal?.ToString("F2"),
            totalPrice = orderBook.Order.TotalPrice?.ToString("F2")
        });
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> RemoveFromCart(Guid orderId, Guid bookId)
    {
        ApplicationUser? user = await _userManager.GetUserAsync(User);

        if (user == null)
        {
            return NotFound();
        }

        Order? currentOrder = await _context.Orders
            .FirstOrDefaultAsync(o => o.BuyerId == user.Id && o.Id == orderId && !o.IsCompleted);

        if (currentOrder == null)
        {
            TempData["ErrorMessage"] = "The order does not exist.";
            return RedirectToAction("Cart", "Order");
        }

        OrderBook? orderBookToBeDeleted = await _context.OrdersBooks
            .Include(ob => ob.Book)
            .FirstOrDefaultAsync(ob => ob.OrderId == currentOrder.Id && ob.BookId == bookId);

        if (orderBookToBeDeleted == null)
        {
            TempData["ErrorMessage"] = "The book does not exist in the order.";
            return RedirectToAction("Cart", "Order");
        }

        currentOrder.TotalPrice -= orderBookToBeDeleted.Quantity * orderBookToBeDeleted.UnitPrice;
        orderBookToBeDeleted.Book.Stock += orderBookToBeDeleted.Quantity;

        if (orderBookToBeDeleted.Book.Stock >= 1)
        {
            orderBookToBeDeleted.Book.IsDeleted = false;
        }

        _context.OrdersBooks.Remove(orderBookToBeDeleted);

        if (currentOrder.TotalPrice == 0)
        {
            _context.Orders.Remove(currentOrder);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction("Cart", "Order");
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CompleteOrder(Guid orderId)
    {
        ApplicationUser? user = await _userManager.GetUserAsync(User);

        if (user == null)
        {
            return NotFound();
        }

        Order? order = await _context.Orders
            .FirstOrDefaultAsync(o => o.BuyerId == user.Id && o.Id == orderId && !o.IsCompleted);

        if (order == null)
        {
            TempData["ErrorMessage"] = "The order does not exist.";
            return RedirectToAction("Cart", "Order");
        }

        order.IsCompleted = true;
        TempData["OrderNumber"] = order.OrderNumber;
        TempData["RedirectFromCompleteOrder"] = true;

        await _context.SaveChangesAsync();
        return RedirectToAction("OrderSuccess", "Order");
    }

    [HttpGet]
    [Authorize]
    public IActionResult OrderSuccess()
    {
        bool isRedirected = (TempData["RedirectFromCompleteOrder"] as bool?) ?? false;

        if (!isRedirected)
        {
            return RedirectToAction("AccessDenied", "Home");
        }

        return View();
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> ReturnBook(Guid orderId, Guid bookId)
    {
        ApplicationUser? user = await _userManager.GetUserAsync(User);

        if (user == null)
        {
            return NotFound();
        }

        Order? finishedOrder = await _context.Orders
            .FirstOrDefaultAsync(o => o.BuyerId == user.Id && o.Id == orderId && o.IsCompleted);

        if (finishedOrder == null)
        {
            TempData["ErrorMessage"] = "The order does not exist.";
            return RedirectToAction("Manage", "Account", new { page = "Orders" });
        }

        OrderBook? orderBookToBeReturned = await _context.OrdersBooks
            .Include(ob => ob.Order)
            .Include(ob => ob.Book)
            .FirstOrDefaultAsync(ob => ob.OrderId == finishedOrder.Id && ob.BookId == bookId && !ob.IsReturned);

        if (orderBookToBeReturned == null)
        {
            TempData["ErrorMessage"] = "The book does not exist in the order.";
            return RedirectToAction("Manage", "Account", new { page = "Orders" });
        }

        OrderReturnBookViewModel returnBook = new OrderReturnBookViewModel()
        {
            OrderId = orderBookToBeReturned.OrderId,
            BookId = orderBookToBeReturned.BookId,
            OrderNumber = orderBookToBeReturned.Order.OrderNumber,
            OrderDate = orderBookToBeReturned.Order.OrderDate,
            Title = orderBookToBeReturned.Book.Title
        };

        return View(returnBook);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> ReturnBook(OrderReturnBookViewModel returnBook)
    {
        ApplicationUser? user = await _userManager.GetUserAsync(User);

        if (user == null)
        {
            return NotFound();
        }

        Order? finishedOrder = await _context.Orders
            .FirstOrDefaultAsync(o => o.BuyerId == user.Id && o.Id == returnBook.OrderId && o.IsCompleted);

        if (finishedOrder == null)
        {
            TempData["ErrorMessage"] = "The order does not exist.";
            return RedirectToAction("Manage", "Account", new { page = "Orders" });
        }

        DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);
        bool hasDateExpired = currentDate > finishedOrder.OrderDate.AddDays(30);

        if (hasDateExpired)
        {
            TempData["RedirectFromReturnBook"] = true;
            return RedirectToAction("Manage", "Account", new { page = "Orders" });
        }

        OrderBook? orderBookToBeReturned = await _context.OrdersBooks
            .Include(ob => ob.Book)
            .FirstOrDefaultAsync(ob => ob.OrderId == returnBook.OrderId && ob.BookId == returnBook.BookId && !ob.IsReturned);

        if (orderBookToBeReturned == null)
        {
            TempData["ErrorMessage"] = "The book does not exist in the order.";
            return RedirectToAction("OrdersList", "Order");
        }

        orderBookToBeReturned.Book.Stock += orderBookToBeReturned.Quantity;

        if (orderBookToBeReturned.Book.Stock >= 1)
        {
            orderBookToBeReturned.Book.IsDeleted = false;
        }

        orderBookToBeReturned.IsReturned = true;
        TempData["BookTitle"] = orderBookToBeReturned.Book.Title;
        TempData["OrderNumber"] = orderBookToBeReturned.Order.OrderNumber;
        TempData["RedirectFromReturnBook"] = true;

        await _context.SaveChangesAsync();
        return RedirectToAction("ReturnSuccess", "Order");
    }

    [HttpGet]
    [Authorize]
    public IActionResult ReturnSuccess()
    {
        bool isRedirected = (TempData["RedirectFromReturnBook"] as bool?) ?? false;

        if (!isRedirected)
        {
            return RedirectToAction("AccessDenied", "Home");
        }

        return View();
    }

    [HttpGet]
    [Authorize]
    public IActionResult ReturnExpired()
    {
        bool isRedirected = (TempData["RedirectFromReturnBook"] as bool?) ?? false;

        if (!isRedirected)
        {
            return RedirectToAction("AccessDenied", "Home");
        }

        return View();
    }
}