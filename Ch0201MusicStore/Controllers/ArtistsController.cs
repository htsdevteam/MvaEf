using Ch0201MusicStore.Models;
using Ch0201MusicStore.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;

namespace Ch0201MusicStore.Controllers
{
    public class ArtistsController : Controller
    {
        private ArtistRepository _repo = new ArtistRepository();

        public ActionResult Details(int id)
        {
            Artist artist = _repo.Get(id);
            if (artist == null)
            {
                return HttpNotFound();
            }
            return View(artist);
        }

        // GET: Artists
        public ActionResult Index()
        {
            return View(_repo.GetAll());
            //return View(_repo.GetSoloArtists());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Artist artist)
        {
            if (!ModelState.IsValid)
            {
                return View(artist);
            }

            _repo.Add(artist);
            _repo.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            Artist artist = _repo.Get(id);
            if (artist == null)
            {
                return HttpNotFound();
            }
            return View(artist);
        }

        [HttpPost]
        public ActionResult Edit(Artist artist)
        {
            if (!ModelState.IsValid)
            {
                return View(artist);
            }

            try
            {
                // NOTE: should be in the Service layer
                // NOTE 2: 3 SaveChanges
                using (var scope = new TransactionScope())
                {
                    _repo.Update(artist);
                    _repo.SaveChanges();
                    scope.Complete();
                }
                return RedirectToAction("Index");
            }
            catch (DbUpdateConcurrencyException)
            {
                ViewBag.Message = "Sorry, that didn't work!";
                return View(artist);
            }
        }


        protected override void Dispose(bool disposing)
        {
            _repo.Dispose();
            base.Dispose(disposing);
        }
    }
}