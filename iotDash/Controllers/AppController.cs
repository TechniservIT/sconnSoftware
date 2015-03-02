using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iotDash.Controllers
{

    [Authorize]
    public class AppController : Controller
    {
        //
        // GET: /App/
        public ActionResult Index(string app)
        {
            return View();
        }

        //
        // GET: /App/Details/5
        public ActionResult Details(string app, int id)
        {
            return View();
        }

        //
        // GET: /App/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /App/Create
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

        //
        // GET: /App/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /App/Edit/5
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

        //
        // GET: /App/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /App/Delete/5
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
