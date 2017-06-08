using Ch0401.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ch0401.Controllers
{
    public class AlbumsController : Controller
    {
        // GET: Albums
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Edit(int id)
        {
            return null; //TODO
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AlbumEditViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var album = new Album
            {
                Title = vm.Title,
                AlbumArtUrl = vm.AlbumArtUrl,
                AlbumId = vm.AlbumId,
                ArtistId = vm.ArtistId,
                GenreId = vm.GenreId,
                Price = 9.99m,
            };
            // _db.Entry(album).State = EntityState.Modified; // TODO
            try
            {
                // _db.SaveChanges(); //TODO
                return RedirectToAction("Index");
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var result in ex.EntityValidationErrors)
                {
                    foreach (var error in result.ValidationErrors)
                    {
                        ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                    }
                }
                return View(album);
            }
        }
    }
}