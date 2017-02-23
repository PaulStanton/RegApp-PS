﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using University.Users;
using University;
using University.Courses;
using RegAppMVC.Models;
namespace RegAppMVC.Controllers
{
    public class RegisterController : Controller
    {
        // GET: Register
        public ViewResult StudentPage()
        {
            return View();
        }
        public PartialViewResult ViewSchedule ()
        {
            Student s = new Student();
            s.FirstName = CurrentStudent.GetInstance().FirstName;
            s.LastName = CurrentStudent.GetInstance().LastName;
            s.Email = CurrentStudent.GetInstance().Email;
            s.Password = CurrentStudent.GetInstance().Password;
            s.major = CurrentStudent.GetInstance().major;
            s.schedule=(CurrentStudent.GetInstance().GetSchedule());
            Global.lastpartialView = "ViewSchedule";
            return PartialView(s);
        }
        public PartialViewResult StudentInfo()
        {
            Student s = new Student();
            s.FirstName = CurrentStudent.GetInstance().FirstName;
            s.LastName = CurrentStudent.GetInstance().LastName;
            s.Email = CurrentStudent.GetInstance().Email;
            s.Password = CurrentStudent.GetInstance().Password;
            s.major = CurrentStudent.GetInstance().major;
            s.schedule=(CurrentStudent.GetInstance().GetSchedule());
            s.CalculateCredits();
            return PartialView(s);
        }
        public ActionResult UpdateStudentInfo(Student s)
        {
            CurrentStudent.GetInstance().Email = s.Email;
            CurrentStudent.GetInstance().Password = s.Password;
            CurrentStudent.GetInstance().FirstName = s.FirstName;
            CurrentStudent.GetInstance().LastName = s.LastName;
            s.ID = CurrentStudent.GetInstance().ID;
            s.major = CurrentStudent.GetInstance().major;
            s.schedule =(CurrentStudent.GetInstance().GetSchedule());
            DataConnection.UpdateStudent(CurrentStudent.GetInstance().ID, s);
            Global.lastpartialView = "StudentInfo";
            return RedirectToAction("StudentPage", "Register");
        }
        public PartialViewResult AddCourse()
        {
            CourseRegistration r = new CourseRegistration();
            r.student = new Student(CurrentStudent.GetInstance().FirstName, CurrentStudent.GetInstance().LastName, CurrentStudent.GetInstance().Password, CurrentStudent.GetInstance().Email, CurrentStudent.GetInstance().ID, CurrentStudent.GetInstance().major);
            r.student.schedule = CurrentStudent.GetInstance().GetSchedule();
            r.student.CalculateCredits();
            if (r.student.isFull)
            {
                Global.currentError = Global.Errors.fullschedule;
            }
            else if(Global.currentError != Global.Errors.timeOverlap)
                Global.currentError = "";
            r.courses = DataConnection.getAllCourses();
            
            return PartialView(r);
        }
        public ActionResult RegisterCourse(CourseRegistration r)
        {
            Course tempCourse = DataConnection.getCourse(r.CourseToAlter);
            if (CurrentStudent.GetInstance().CheckForOverlap(tempCourse))
            {
                Global.currentError = Global.Errors.timeOverlap;
            }
            else
            {
                if (Global.currentError != Global.Errors.fullschedule)
                    Global.currentError = "";
                CurrentStudent.GetInstance().AddCourse(tempCourse);
            }
            Global.lastpartialView = "AddCourse";
            return RedirectToAction("StudentPage", "Register");
        }
        public PartialViewResult DropCourse()
        {
            CourseRegistration r = new CourseRegistration();
            r.student = new Student(CurrentStudent.GetInstance().FirstName, CurrentStudent.GetInstance().LastName, CurrentStudent.GetInstance().Password, CurrentStudent.GetInstance().Email, CurrentStudent.GetInstance().ID, CurrentStudent.GetInstance().major);
            r.student.schedule = CurrentStudent.GetInstance().GetSchedule();
            foreach(var item in r.student.schedule)
            {
                r.isDropped.Add(item.Value.ID, false);
            }
            return PartialView(r);
        }
        public ActionResult DropStudentFromCourse(CourseRegistration r)
        {
            if (r.CourseToAlter >= 0)
            {
                CurrentStudent.GetInstance().RemoveCourse(r.CourseToAlter);
            }
            Global.lastpartialView = "DropCourse";
            return RedirectToAction("StudentPage","Register");
        }

    }
}