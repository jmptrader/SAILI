using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SAILI.Models;
using System.Net;

namespace SAILI.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class PostcodeController : Controller
    {
        private ApplicationDbContext DefaultConnection = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View(DefaultConnection.Postcodes.ToList());
        }

        // GET: Postcode/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Postcode/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Postcode/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Postcode/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var postcode = DefaultConnection.Postcodes.Where(c => c.PostcodeID == id).FirstOrDefault();

            return View(postcode);
        }

        // POST: Postcode/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Postcode/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Postcode/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
