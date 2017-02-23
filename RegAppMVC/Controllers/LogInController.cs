using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using University.Users;
using University;
using System.Data.SqlClient;
using System.Web.Routing;
using RegAppMVC.Models;

namespace RegAppMVC.Controllers
{
    public class LogInController : Controller
    {
        public ActionResult CheckLogInInfo(Student s)
            {
            if (s.Email=="Admin"&&s.Password=="1234")
            {
                return RedirectToAction("AdminPage", "Admin");
            }
            try
            {
                if (DataConnection.CheckLogInInfo(s) == true)
                {
                    CurrentStudent.GetInstance().InitializeStudent(s);

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
            CurrentStudent.GetInstance().ResetCurrentStudent();
            return View();
        }
        public PartialViewResult RegisterStudent()
        {
            return PartialView();
        }
    }
}