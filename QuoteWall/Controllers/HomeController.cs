using Microsoft.AspNetCore.Mvc;
using QuoteWall.Models;
using System.Diagnostics;
using QuoteWall.Data;
using Microsoft.EntityFrameworkCore;

namespace QuoteWall.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;

        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index(string category, string sort)
        {
            var quotes = _db.Quotes.AsQueryable();

            if (!string.IsNullOrEmpty(category) && Enum.TryParse<CategoryEnum>(category, out var cat))
                quotes = quotes.Where(q => q.Category == cat);

            quotes = sort switch
            {
                "likes" => quotes.OrderByDescending(q => q.Likes),
                "date" => quotes.OrderByDescending(q => q.CreatedAt),
                "author" => quotes.OrderBy(q => q.Author),
                _ => quotes.OrderByDescending(q => q.CreatedAt)
            };

            ViewBag.TotalQuotes = await _db.Quotes.CountAsync();
            ViewBag.TopAuthor = await _db.Quotes
                .GroupBy(q => q.Author)
                .OrderByDescending(g => g.Sum(x => x.Likes))
                .Select(g => g.Key)
                .FirstOrDefaultAsync();

            return View(await quotes.ToListAsync());
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Quote quote)
        {
            if (ModelState.IsValid)
            {
                quote.CreatedAt = DateTime.Now;
                _db.Add(quote);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(quote);
        }

        public async Task<IActionResult> Details(int id)
        {
            var quote = await _db.Quotes.FindAsync(id);
            if (quote == null) return NotFound();
            return View(quote);
        }

        [HttpPost]
        public async Task<IActionResult> Like(int id)
        {
            var quote = await _db.Quotes.FindAsync(id);
            if (quote != null)
            {
                quote.Likes++;
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
