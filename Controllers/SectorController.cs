using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SAILI.Models;
using PagedList;


namespace SAILI.Controllers
{
    public class SectorController : Controller
    {
        ApplicationDbContext DefaultConnection = new ApplicationDbContext();
        // GET: Sector
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            string url = null;


            if (Request.IsSecureConnection && !Request.IsAuthenticated)
            {
                SecuredSocketLayer SSL = new SecuredSocketLayer();
                url = SSL.IsNotAuthenticated();
                Response.Redirect(url);
            }

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var sectors = from s in DefaultConnection.Sectors
                             select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                sectors = sectors.Where(s => s.SectorName.Contains(searchString));
            }
            int pageSize = 3;
            int pageNumber = (page ?? 1);

            return View(sectors.OrderBy(s => s.SectorName).ToPagedList(pageNumber, pageSize));
        }

        // GET: Sector/Details/5
        public ActionResult Details(int id)
        {
            var sector = DefaultConnection.Sectors.Where(s => s.SectorID == id).FirstOrDefault();

            if (sector == null)
            {
                return HttpNotFound();
            }

            return View(sector);
        }

        // GET: Sector/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Sector/Create
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

        // GET: Sector/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Sector/Edit/5
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

        // GET: Sector/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Sector/Delete/5
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
