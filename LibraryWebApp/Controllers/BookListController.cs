using BibliotekaWeb.Models;
using ContosoUniversity;
using LibraryWeb.Models;
using LibraryWebApp.Data;
using LibraryWebApp.Models;
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
        //        [AllowAnonymous]
        //        //Get

        //        public IActionResult Index()
        //        {
        //            IEnumerable<Book> booksList = _dbContext.Books              
        //                .Take(20)

        //;
        //            return View(booksList);
        //        }
        //        [AllowAnonymous]

        //        public IActionResult Index(PageInfo page)
        //        {
        //            if (page.PageLength == 0)
        //            {
        //                page.PageLength = 20;
        //            }
        //            IEnumerable<Book> booksList = _dbContext.Books
        //                .Skip(page.PageLength * (page.PageNumber - 1))
        //                .Take(page.PageLength)

        //;
        //            return View(booksList);
        //        }
        [AllowAnonymous]
        public async Task<IActionResult> Index(
        string sortOrder,
        string currentFilter,
        string searchString,
        int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["AuthorSortParm"] = String.IsNullOrEmpty(sortOrder) ? "Author_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            var books = from s in _dbContext.Books
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                books = books.Where(s => s.Name.Contains(searchString)
                                       || s.Author.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    books = books.OrderByDescending(s => s.Name);
                    break;
                case "Author_desc":
                    books = books.OrderByDescending(s => s.Author);
                    break;
                case "Date":
                    books = books.OrderBy(s => s.PublishDate);
                    break;
                case "date_desc":
                    books = books.OrderByDescending(s => s.PublishDate);
                    break;
                default:
                    books = books.OrderBy(s => s.Author);
                    break;
            }
            int pageSize = 20;
            return View(await PaginatedList<Book>.CreateAsync(books.AsNoTracking(), pageNumber ?? 1, pageSize));
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
