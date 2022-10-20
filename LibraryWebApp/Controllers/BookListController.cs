using BibliotekaWeb.Models;
using LibraryWebApp.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryWebApp.Controllers
{
    public class BookListController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public BookListController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            IEnumerable<Book> booksList = _dbContext.Books;
            return View(booksList);
        }
        public IActionResult AddNewBook()
        {
           
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddNewBook(Book book)
        {
            
            if (ModelState.IsValid)
            {
                _dbContext.Books.Add(book);
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(book);
        }

    }
}
