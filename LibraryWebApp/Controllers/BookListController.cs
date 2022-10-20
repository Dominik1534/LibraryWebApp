using BibliotekaWeb.Models;
using LibraryWeb.Models;
using LibraryWebApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

using System.Net;
using System.Security.Claims;


namespace LibraryWebApp.Controllers
{
    [Authorize]
    public class BookListController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public BookListController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [AllowAnonymous]
        //Get
       
        public IActionResult Index()
        {
            IEnumerable<Book> booksList = _dbContext.Books
                .Skip(20 *(2 -1))
                .Take(20)

;
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
            
            //var clasimsIdentity = User.Identity.Name;
            //ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
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
        //Get
        public IActionResult Book(int? id)
        {
            var clasimsIdentity = User.Identity.Name;

            if (id == null)
            {
                return NotFound();
            }
            var bookFromDb = _dbContext.Books.Find(id);
            Reservation reservation = new Reservation();
            reservation.BookId = bookFromDb.bookID;
            reservation.UserId = clasimsIdentity;
            var user = _dbContext.Users;
            if (bookFromDb == null)
            {
                return NotFound();
            }
            return View(reservation);
        }
        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Book(Reservation reservation)
        {
            if (reservation == null)
            {
                return NotFound();
            }           
            _dbContext.Reservations.Add(reservation);
            _dbContext.SaveChanges();

            TempData["success"] = "Reservation created successfully";

            return RedirectToAction("Index");

        }
    }
}
