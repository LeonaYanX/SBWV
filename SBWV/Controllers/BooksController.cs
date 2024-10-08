﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SBWV.Abstractions;
using SBWV.Models.ViewModels;



namespace SBWV.Controllers
{
    public class BooksController : BaseController
    {
        private readonly IRepository? _repo;

        public BooksController(IRepository repository)
        {
            _repo = repository;
        }

        public IActionResult List(int idCategory)
        {

            return View("BooksList", _repo?.GetBooksByCategory(idCategory, HttpContext.User.Identity.IsAuthenticated ? GetUserId() : null));
        }

        public IActionResult Delete(int idBook)
        {

            _repo?.DeleteBook(idBook);
            return RedirectToAction("Info", "Account");

        }

        public IActionResult Details(int idBook)
        {


            return View("BookDetails", _repo?.GetBookVM(idBook));
        }


        [HttpGet]
        public IActionResult Edit(int idBook)
        {

            using (SwapBookDbContext dbContext = new SwapBookDbContext())
            {


                var e = dbContext.Books.Include(e => e.Galaries).Include("IdCatalogNavigation")
                       .FirstOrDefault(b => b.Id == idBook);

                var categories = dbContext.Catalogs.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Value
                ,
                    Selected = e.IdCatalog == c.Id
                }).ToList();



                ViewBag.Categories = categories;

                var eVM = GetBookModel.GetBookVM(e);




                return View("EditBook", eVM);
            }

        }
        [HttpPost]
        public IActionResult Edit(BookVM eVM, IFormFile[] files)
        {



            Book book = new Book()
            {
                Author = eVM.Author,
                Id = eVM.Id,
                Title = eVM.Title,
                Info = eVM.Info
                ,
                Price = eVM.Price,
                Swap = eVM.Swap == true ? 1 : 0,

                IdUser = GetUserId()
            };


            book.IdCatalog = Convert.ToInt32(eVM.Category);

            foreach (var f in files)
            {
                using var st = f.OpenReadStream();
                byte[] images = new byte[f.Length];
                st.Read(images, 0, images.Length);

                Galary galary = new() { Photo = images };
                book.Galaries.Add(galary);
            }

            _repo?.UpdateBook(book);



            return RedirectToAction("Info", "Account");


        }
        // Удаление фото книги
        public JsonResult DeletePhoto(int id)
        {
            using var db = new SwapBookDbContext();

            var galary = _repo?.GetGalary(id);
            if (galary != null)
            {

                _repo?.RemoveGalary(galary);
                return Json(new { success = true, msg = "Фото книги удалено" });
            }
            else
            {
                return Json(new { success = false, msg = "Фото книги не найдено" });
            }
        }




        public IActionResult Create()
        {
            using (var dbContext = new SwapBookDbContext())
            {
                var categories = dbContext.Catalogs.Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.Value }).ToList();
                categories.First().Selected = true;

                ViewBag.Categories = categories;
            }




            return View("CreateBook");
        }

        public IActionResult MyBooks()
        {
            return View();
        }

        [HttpPost]

        public IActionResult Create(BookVM newBook, IFormFile[] files)
        {
            if ((newBook.Title == null) || (newBook.Author == null))
            {
                return RedirectToAction("Create", "Books");
            }


            Book book = new Book
            {
                Author = newBook.Author,
                Title = newBook.Title,
                Info = newBook.Info,
                Price = newBook.Price,
                IdCatalog = Convert.ToInt32(newBook.Category)
            };

            book.Swap = newBook.Swap == true ? 1 : 0;


            foreach (var f in files)
            {
                using var st = f.OpenReadStream();
                byte[] images = new byte[f.Length];
                st.Read(images, 0, images.Length);

                Galary galary = new() { Photo = images };
                book.Galaries.Add(galary);
            }

            book.IdUser = GetUserId();



            _repo?.AddBook(book);

            return RedirectToAction("Details", "Books", new { idBook = book.Id });

        }

        private static string GetBase64Image(byte[] bytes)
        {
            if (bytes == null)
                return String.Empty;

            return "data:image/png;base64, " + Convert.ToBase64String(bytes);
        }

        private static string[] GetBase64Images(ICollection<Galary> galaries)
        {
            List<string> images = new List<string>();

            foreach (Galary g in galaries)
            {
                if (g.Photo != null)
                {
                    images.Add("data:image/png;base64, " + Convert.ToBase64String(g.Photo));
                }
            }

            return images.ToArray();




        }
    }
}
