using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookListMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookListMVC.Controllers
{
    public class BooksController : Controller
    {
        private readonly AppDbContext _db;

        public BooksController(AppDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public Book Book { get; set; }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            //var bkList = new BooksViewModel();//
            //bkList.Books = new List<Book>();

            //Create // populate the Upsert page for new record
            if (id == null || id == 0)
            {
                Book = new Book();
                // bkList.Books.Append(Book);
                return View(Book);
            }

            //Update
            Book = _db.Books.FirstOrDefault(u => u.Id == id);
            if (Book == null)
            {
                //https://docs.microsoft.com/en-us/aspnet/core/web-api/action-return-types?view=aspnetcore-3.1
                return NotFound();
            }

            // bkList.Books.Append(Book);
            return View(Book);
        }

        #region API Calls

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Json(new { data = await _db.Books.ToListAsync() });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var bookFromDb = await _db.Books.FirstOrDefaultAsync(u => u.Id == id);

            if (bookFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _db.Books.Remove(bookFromDb);

            await _db.SaveChangesAsync();

            return Json(new { success = true, message = "Delete Successful" });
        }
        #endregion

        [HttpPost]
        [ValidateAntiForgeryToken]//to use inbuilt security
        //Book book) // because the Book has [BindProperty], dont need the params
        public IActionResult Upsert()
        {
            if (ModelState.IsValid)
            {
                if (Book.Id == 0)
                {
                    //Create
                    _db.Books.Add(Book);
                }
                else
                {
                    _db.Books.Update(Book);
                }
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(Book);
        }
    }
}
