using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using University;
using University.Courses;
using University.Users;

namespace RegAppMVC.Models
{
    public class CourseRegistration
    {
        public Student student { get; set; }
        public Dictionary<string,Course> courses { get; set; }

    }
}