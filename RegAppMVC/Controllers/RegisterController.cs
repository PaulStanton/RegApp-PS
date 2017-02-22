using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using University.Users;

namespace RegAppMVC.Controllers
{
    public class RegisterController : Controller
    {
        // GET: Register
        public ViewResult StudentPage()
        {
            Student s = (Student)TempData["Student"];
            return View(s);
        }
        public PartialViewResult ViewSchedule ()
        {
            return PartialView();
        }
        public PartialViewResult StudentInfo()
        {
            return PartialView();
        }
        public PartialViewResult AddCourse()
        {
            return PartialView();
        }
        public PartialViewResult DropCourse()
        {
            return PartialView();
        }
        public PartialViewResult HelpPage()
        {
            return PartialView();
        }
    }
}