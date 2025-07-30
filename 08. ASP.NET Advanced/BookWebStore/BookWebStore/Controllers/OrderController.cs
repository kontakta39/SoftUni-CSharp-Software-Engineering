using BookWebStore.Data.Models;
using BookWebStore.Services.Interfaces;
using BookWebStore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookWebStore;

public class OrderController : Controller
{
    private readonly IOrderService _orderService;
    private readonly IAccountService _accountService;
    private readonly IBookService _bookService;

    public OrderController(IOrderService orderService, IAccountService accountService, IBookService bookService)
    {
        _orderService = orderService;
        _accountService = accountService;
        _bookService = bookService;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Cart()
    {
        ApplicationUser? user = await _accountService.GetCurrentUserAsync(User);

        if (user == null)
        {
            return NotFound();
        }

        List<OrderBook> orderBooks = await _orderService.GetCartItemsAsync(user.Id);

        List<OrderCartViewModel> cart = orderBooks
            .Select(ob => new OrderCartViewModel
            {
                OrderId = ob.OrderId,
                BookId = ob.BookId,
                BookTitle = ob.Book.Title,
                ImageUrl = ob.Book.ImageUrl,
                Quantity = ob.Quantity,
                UnitPrice = ob.UnitPrice
            })
            .ToList();

        return View(cart);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddToCart(Guid bookId)
    {
        ApplicationUser? user = await _accountService.GetCurrentUserAsync(User);

        if (user == null)
        {
            return NotFound();
        }

        //Get the book that should be added to the cart
        Book? getBook = await _bookService.GetBookByIdAsync(bookId);

        if (getBook == null)
        {
            TempData["ErrorMessage"] = "The book you want to add to the cart does not exist.";
            return RedirectToAction("Index", "Book");
        }

        Order? order = await _orderService.GetUserCurrentOrderAsync(user.Id);

        if (order == null)
        {
            //Creating the order number
            Random random = new Random();

            //Create a list of numbers from 1 to 9
            List<int> digits = Enumerable.Range(1, 9).ToList();
            List<int> shuffledDigits = digits.OrderBy(x => random.Next()).ToList();

            //Concatenate the digits into a single number
            string randomNumber = "#" + string.Join("", shuffledDigits);

            order = await _orderService.CreateNewOrderAsync(user.Id, randomNumber);
        }

        //Check if the book is not already added to the Cart
        OrderBook? orderBook = await _orderService.GetOrderBookAsync(order!.Id, getBook.Id);

        if (orderBook != null)
        {
            TempData["ErrorMessage"] = "The book has already been added to the cart.";
            return RedirectToAction("Cart", "Order");
        }

        orderBook = await _orderService.AddBookToOrderAsync(order, getBook.Id);
        await _orderService.UpdateQuantityAsync(orderBook, 1);
        await _orderService.RecalculatePricesAsync(order, orderBook);

        return RedirectToAction("Cart", "Order");
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> UpdateQuantity(Guid orderId, Guid bookId, int quantity)
    {
        ApplicationUser? user = await _accountService.GetCurrentUserAsync(User);

        if (user == null)
        {
            return NotFound();
        }

        if (quantity < 1)
        {
            return Json(new { success = false, error = "Quantity must be at least 1.", resetTo = 1 });
        }

        Order? order = await _orderService.GetUserCurrentOrderAsync(user.Id, orderId);

        if (order == null)
        {
            return Json(new { success = false, error = "The order does not exist." });
        }

        OrderBook? orderBook = await _orderService.GetOrderBookAsync(orderId, bookId);

        if (orderBook == null)
        {
            return Json(new { success = false, error = "Book not found." });
        }

        int availableStock = orderBook.Book.Stock + orderBook.Quantity; 

        if (quantity > availableStock)
        {
            return Json(new
            {
                success = false,
                error = $"Only {availableStock} book{(availableStock == 1 ? "" : "s")} available in stock.",
                resetTo = orderBook.Quantity
            });
        }

        await _orderService.UpdateQuantityAsync(orderBook, quantity);
        await _orderService.RecalculatePricesAsync(order, orderBook);

        return Json(new
        {
            success = true,
            itemTotal = orderBook.UnitPrice.ToString("F2"),
            totalPrice = order.TotalPrice.ToString("F2")
        });
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> RemoveFromCart(Guid orderId, Guid bookId)
    {
        ApplicationUser? user = await _accountService.GetCurrentUserAsync(User);

        if (user == null)
        {
            return NotFound();
        }

        Order? order = await _orderService.GetUserCurrentOrderAsync(user.Id, orderId);

        if (order == null)
        {
            TempData["ErrorMessage"] = "The order does not exist.";
            return RedirectToAction("Cart", "Order");
        }

        OrderBook? orderBook = await _orderService.GetOrderBookAsync(order.Id, bookId);

        if (orderBook == null)
        {
            TempData["ErrorMessage"] = "The book does not exist in the order.";
            return RedirectToAction("Cart", "Order");
        }

        await _orderService.UpdateQuantityAsync(orderBook);
        await _orderService.RemoveFromCartAsync(order, orderBook);

        return RedirectToAction("Cart", "Order");
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CompleteOrder(Guid orderId)
    {
        ApplicationUser? user = await _accountService.GetCurrentUserAsync(User);

        if (user == null)
        {
            return NotFound();
        }

        Order? order = await _orderService.GetUserCurrentOrderAsync(user.Id, orderId);

        if (order == null)
        {
            TempData["ErrorMessage"] = "The order does not exist.";
            return RedirectToAction("Cart", "Order");
        }

        await _orderService.CompleteOrderAsync(order);

        TempData["OrderNumber"] = order.OrderNumber;
        TempData["RedirectFromCompleteOrder"] = true;
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
        ApplicationUser? user = await _accountService.GetCurrentUserAsync(User);

        if (user == null)
        {
            return NotFound();
        }

        Order? order = await _orderService.FindCompletedOrderAsync(user.Id, orderId);

        if (order == null)
        {
            TempData["ErrorMessage"] = "The order does not exist.";
            return RedirectToAction("Manage", "Account", new { page = "Orders" });
        }

        OrderBook? orderBook = await _orderService.GetOrderBookAsync(order.Id, bookId, false);

        if (orderBook == null)
        {
            TempData["ErrorMessage"] = "This book has already been returned.";
            return RedirectToAction("Manage", "Account", new { page = "Orders" });
        }

        OrderReturnBookViewModel returnBook = new OrderReturnBookViewModel()
        {
            OrderId = orderBook.OrderId,
            BookId = orderBook.BookId,
            OrderNumber = orderBook.Order.OrderNumber,
            OrderDate = orderBook.Order.OrderDate,
            Title = orderBook.Book.Title
        };

        return View(returnBook);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> ReturnBook(OrderReturnBookViewModel returnBook)
    {
        ApplicationUser? user = await _accountService.GetCurrentUserAsync(User);

        if (user == null)
        {
            return NotFound();
        }

        Order? order = await _orderService.FindCompletedOrderAsync(user.Id, returnBook.OrderId);

        if (order == null)
        {
            TempData["ErrorMessage"] = "The order does not exist.";
            return RedirectToAction("Manage", "Account", new { page = "Orders" });
        }

        DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);
        bool hasDateExpired = currentDate > order.OrderDate.AddDays(30);

        if (hasDateExpired)
        {
            TempData["RedirectFromReturnBook"] = true;
            return RedirectToAction("ReturnExpired", "Order");
        }

        OrderBook? orderBook = await _orderService.GetOrderBookAsync(order.Id, returnBook.BookId, false);

        if (orderBook == null)
        {
            TempData["ErrorMessage"] = "This book has already been returned.";
            return RedirectToAction("Manage", "Account", new { page = "Orders" });
        }

        await _orderService.UpdateQuantityAsync(orderBook);
        await _orderService.ReturnBookAsync(orderBook);

        TempData["BookTitle"] = orderBook.Book.Title;
        TempData["OrderNumber"] = order.OrderNumber;
        TempData["RedirectFromReturnBook"] = true;
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