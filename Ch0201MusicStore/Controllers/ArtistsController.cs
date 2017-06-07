﻿using Ch0201MusicStore.Models;
using Ch0201MusicStore.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ch0201MusicStore.Controllers
{
    public class ArtistsController : Controller
    {
        private ArtistRepository _repo = new ArtistRepository();

        // GET: Artists
        public ActionResult Index()
        {
            return View(_repo.GetAll());
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
    }
}