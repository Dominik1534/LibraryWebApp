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
        //Get
        public IActionResult Index()
        {
            IEnumerable<Book> booksList = _dbContext.Books;
            return View(booksList);
        }
        //Get
        public IActionResult AddNewBook()
        {
           
            return View();
        }
        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddNewBook(Book book)
        {
            
            if (ModelState.IsValid)
            {
                book.bookID=_dbContext.Books.Max(x => x.bookID+1);
                _dbContext.Books.Add(book);
                _dbContext.SaveChanges();
                TempData["success"] = "Book added successfully";

                return RedirectToAction("Index");
            }
            return View(book);
        }
        //Get
        public IActionResult EditBook(int? id)
        {
            if (id==null )
            {
                return NotFound();
            }
            var bookFromDb=_dbContext.Books.Find(id);
            if (bookFromDb == null)
            {
                return NotFound();
            }
            return View(bookFromDb);
        }
        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditBook(Book book)
        {

            if (ModelState.IsValid)
            {
                //book.bookID = book.bookID;
                _dbContext.Books.Update(book);
                _dbContext.SaveChanges();
                TempData["success"] = "Book updated successfully";

                return RedirectToAction("Index");
            }
            return View(book);
        }
        //Get
        public IActionResult DeleteBook(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var bookFromDb = _dbContext.Books.Find(id);
            if (bookFromDb == null)
            {
                return NotFound();
            }
            return View(bookFromDb);
        }
        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteBookPOST(Book book)
        {
            var bookToDelete = _dbContext.Books.Find(book.bookID);

            if (bookToDelete == null)
            {
                return NotFound();
            }         
                _dbContext.Books.Remove(bookToDelete);
                _dbContext.SaveChanges();
                TempData["success"] = "Book deleted successfully";

                return RedirectToAction("Index");
            
        }

    }
}
