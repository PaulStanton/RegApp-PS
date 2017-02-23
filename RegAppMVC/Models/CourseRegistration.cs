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
        private int coursetoalter = -1;
        public Student student { get; set; }
        public Dictionary<int, bool> isDropped = new Dictionary<int, bool>();
        public int CourseToAlter { get {return coursetoalter; } set {coursetoalter=value; } }
        public Dictionary<string,Course> courses { get; set; }
        public bool CheckForCourse(int courseID)
        {
            foreach(var item in student.schedule)
            {
                if (item.Value.ID == courseID)
                    return true;
            }
            return false;
        }

    }
}