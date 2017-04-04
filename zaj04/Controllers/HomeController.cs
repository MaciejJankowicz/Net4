﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using zaj04.Models;

namespace zaj04.Controllers
{
    public class HomeController : Controller
    {
        public void CreateDatabase()
        {
            using (var db = new CompanyContext())
            {
                var f = db.Teams.FirstOrDefault();
            }
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}