using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using CashTicket.Models;

namespace CashTicket.Controllers
{
    public class HomeController : Controller
    {
        private CashDeskEntities db = new CashDeskEntities();

        public ActionResult Index()
        {
            return View();
        }

    }
}