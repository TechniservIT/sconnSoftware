using System.Web.Mvc;
using iotDash.Controllers.domain.navigation;

namespace iotDash.Controllers.global
{

    [DomainAuthorize]
    public class DashboardController : Controller
    {
        //
        // GET: /Dashboard/
        public ActionResult Index()
        {
            return View();
        }



        //
        // GET: /Dashboard/Users
        public ActionResult Users()
        {
            return View();
        }




        //
        // GET: /Dashboard/AddUser
        public ActionResult AddUser()
        {
            return View();
        }




        /**************** SAMPLE VIEWS ******************/


        //
        // GET: /Dashboard/grid
        public ActionResult grid()
        {
            return View();
        }

        //
        // GET: /Dashboard/forms
        public ActionResult forms()
        {
            return View();
        }


        //
        // GET: /Dashboard/login
        public ActionResult login()
        {
            return View();
        }


        //
        // GET: /Dashboard/notifications
        public ActionResult notifications()
        {
            return View();
        }

        //
        // GET: /Dashboard/tables
        public ActionResult tables()
        {
            return View();
        }

        //
        // GET: /Dashboard/panelswells
        public ActionResult panelswells()
        {
            return View();
        }


        //
        // POST: /Dashboard/Create
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
        // GET: /Dashboard/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Dashboard/Edit/5
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




    }
}
