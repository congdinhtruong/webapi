using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApi.Models;

namespace WebApi.Controllers
{
    public class AdminsMVCController : BaseMVCController
    {
        // GET: AdminsMVC
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Admin loginAdmin)
        {
            try
            {
                // compare AdminName when enter
                var admin = db.Admins.SingleOrDefault(a => a.AdminName.Equals(loginAdmin.AdminName));
                if (admin == null)
                {
                    ModelState.AddModelError("", "Admin is not found!");
                }
                else
                {
                    // compare Password when enter
                    if (admin.Password.Equals(loginAdmin.Password))
                    {
                        Session["AdminName"] = admin.AdminName.ToString();
                        Session["AdminId"] = admin.AdminId.ToString();
                        return Redirect("~/Library.html");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Admin is not found!");
                    }
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
            }
            return View();
        }

        public ActionResult Logout()
        {
            Session["AdminName"] = null;
            Session["AdminName"] = null;
            return RedirectToAction("Login", "AdminsMVC");
        }
    }
}