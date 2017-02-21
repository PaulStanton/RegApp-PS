using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using University.Users;
using University;


namespace RegAppMVC.Controllers
{
    public class LogInController : Controller
    {
        public ActionResult CheckLogInInfo(Student s)
            {
            if (DataConnection.CheckLogInInfo(s) == true)
            {
                return RedirectToAction("ViewSchedule", "Register");
            }
            else
                return RedirectToAction("LogIn", "LogIn");
        }
        public ViewResult LogIn()
        {
            DataConnection.SetConnection("Data Source=demodatabase.clnb0ldm68iw.us-west-2.rds.amazonaws.com,1433;Initial Catalog=RegistrationApp;Persist Security Info=True;User Id=master;Password=password;Encrypt=False;");
            return View();
        }
        public PartialViewResult RegisterStudent()
        {
            return PartialView();
        }
    }
}