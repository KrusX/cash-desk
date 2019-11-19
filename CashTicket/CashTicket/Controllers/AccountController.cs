using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using CashTicket.Models;

namespace CashTicket.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Client model)
        {
            if (ModelState.IsValid)
            {
                Client client = null;
                using (CashDeskEntities db = new CashDeskEntities())
                {
                    client = db.Clients.FirstOrDefault(u => u.login == model.login);
                }
                if (client == null)
                {
                    using (CashDeskEntities db = new CashDeskEntities())
                    {
                        db.Clients.Add(new Client
                        {
                            surname = model.surname,
                            name = model.name,
                            patronymic = model.patronymic,
                            pass = model.pass,
                            login = model.login,
                            password = model.password,
                            role_id = 3
                        });
                        db.SaveChanges();

                        client = db.Clients.Where(u => u.login == model.login && u.password == model.password).FirstOrDefault();
                    }
                    if (client != null)
                    {
                        FormsAuthentication.SetAuthCookie(model.login, true);
                        return RedirectToAction("Index", "Reys");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Пользователь с таким логином уже существует!");
                }
            }
            return View(model);
        }
        
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Client model)
        {
            if (ModelState.IsValid)
            {
                Client client = null;
                using (CashDeskEntities db = new CashDeskEntities())
                {
                    client = db.Clients.FirstOrDefault(u => u.login == model.login && u.password == model.password);
                }
                if (client != null)
                {
                    FormsAuthentication.SetAuthCookie(model.login, true);
                    return RedirectToAction("Index", "Reys");
                }
                else
                {
                    ModelState.AddModelError("", "Неверное имя пользователя или пароль!");
                }
            }

            return View(model);
        }

        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Reys");
        }
    }
}