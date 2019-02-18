using SAILI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SAILI.Controllers
{
    public class SaltOwnerController : Controller
    {
        // GET: SaltOwner
        public ActionResult Index()
        {
            return View();
        }

        // GET: SaltOwner/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SaltOwner/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SaltOwner/Create
        [HttpPost]
        public ActionResult Create(Owner model)
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

        // GET: SaltOwner/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SaltOwner/Edit/5
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

        // GET: SaltOwner/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SaltOwner/Delete/5
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
