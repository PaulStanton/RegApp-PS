using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using University.Users;
using University;
using System.Data.SqlClient;
using System.Web.Routing;


namespace RegAppMVC.Controllers
{
    public class LogInController : Controller
    {
        public ActionResult CheckLogInInfo(Student s)
            {
            try
            {
                if (DataConnection.CheckLogInInfo(s) == true)
                {
                    TempData["Student"] = s;
                    return RedirectToAction("StudentPage", "Register");
                }
                else
                {
                    Global.currentError = Global.Errors.invalidLogIn;
                    return RedirectToAction("LogIn", "LogIn");
                }
            }
            catch (SqlException e)
            {
                Global.currentError = Global.Errors.coundNotConnectToDatabase;
                return RedirectToAction("LogIn", "LogIn");
                
            }
        }
        public ViewResult LogIn()
        {

            return View();
        }
        public PartialViewResult RegisterStudent()
        {
            return PartialView();
        }
    }
}