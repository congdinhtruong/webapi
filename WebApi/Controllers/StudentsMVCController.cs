using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApi.Models;
namespace WebApi.Controllers
{
    public class StudentsMVCController : BaseMVCController
    {
        // GET: StudentsMVC
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Student loginStudent)
        {
            try
            {
                // compare StudentName when enter
                var student = db.Students.SingleOrDefault(s => s.StudentName.Equals(loginStudent.StudentName));
                if (student == null)
                {
                    ModelState.AddModelError("", "Student is not found!");
                }
                else
                {
                    // compare Password when enter 
                    if (student.Password.Equals(loginStudent.Password))
                    {                        
                        Session["StudentName"] = student.StudentName.ToString();
                        Session["StudentId"] = student.StudentId.ToString();
                        // if succeed, direct to Visitors
                        return Redirect("~/Visitors.html");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Student is not found!");
                    }
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
            }

            return View();
        }
    }
}