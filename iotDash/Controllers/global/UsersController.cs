using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iotDash.Controllers
{
    [DomainAuthorize]
    public class UsersController : Controller
    {

        //
        // GET: /Users/
        public ActionResult Index()
        {   
            //automaticly select user domain

            return View();
        }


        // GET: /Users/Add
        public ActionResult Add()
        {
            return View();
        }


        // GET: /Users/Edit/<number>
        public ActionResult Edit(int number)
        {
            return View();
        }


        // GET: /Users/View/<number>
        public ActionResult View( int number)
        {
            return View();
        }

        public ActionResult Show()
        {
            return View();
        }


	}
}