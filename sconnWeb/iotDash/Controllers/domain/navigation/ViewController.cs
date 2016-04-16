using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iotDash.Identity;
using iotDash.Identity.Roles;
using iotDash.Models;
using iotDash.Session;
using iotDatabaseConnector.DAL.Repository.Connector.Entity;
using iotDbConnector.DAL;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using iotDash.Identity.Attributes;

namespace iotDash.Controllers.domain.navigation
{

    [DomainAuthorize]
    public class ViewController : Controller
    {
        //
        // GET: /View/
        public ActionResult Index(string app)
        {
            return View();
        }

        //
        // GET: /View/Details/5
        public ActionResult Details(string app, int id)
        {
            return View();
        }

        //
        // GET: /View/Create
        public ActionResult Create(string app)
        {
            return View();
        }

        //
        // POST: /View/Create
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
        // GET: /View/Edit/5
        public ActionResult Edit(string app, int id)
        {
            return View();
        }

        //
        // POST: /View/Edit/5
        [HttpPost]
        public ActionResult Edit(string app, int id, FormCollection collection)
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
        // GET: /View/Delete/5
        public ActionResult Delete(string app, int id)
        {
            return View();
        }

        //
        // POST: /View/Delete/5
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
