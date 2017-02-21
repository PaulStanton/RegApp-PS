using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Courses;

namespace University.Users
{
    public class Administrator : User
    {
        private static Administrator instance;

        private Administrator()
        {

        }
        public override string GetInfo()
        {
            return base.ToString();
        }
        public static Administrator GetInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Administrator();
                }
                return instance;
            }
        }
        public void setPassword(string password)
        {
            this.password = password;
        }
        public static bool CloseCourse(Course thisCourseToClose)
        {
            thisCourseToClose.isClosed = true;
            Console.WriteLine("Registration for this course is now closed");
            return true;
        }
        public static bool OpenCourse(Course thisCoursetoOpen)
        {
            thisCoursetoOpen.isClosed = false;
            return true;
        }
        public bool ChangeCourseStatus(Course thisCourseToChange)
        {
            thisCourseToChange.isClosed = !(thisCourseToChange.isClosed);
            Console.WriteLine("Registration is" + ((thisCourseToChange.isClosed) ? "Closed" : "Open"));
            return true;
        }
    }


}

