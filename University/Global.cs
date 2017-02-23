using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University
{
    public static class Global
    {
        public static int maxStudents=20;
        public static int maxCredits = 6;
        public static string currentError = "";
        public static string lastpartialView = "ViewSchedule";

        public static string CreateFullName(string first, string last)
        {

            return $"{first} {last}";
        }
        static public class Errors
        {
            public static string notEnoughSpace = "not enough space";
            public static string studentNotFound = "Student Not Found";
            public static string courseNotFound = "Course Not Found";
            public static string invalidStudent = "Invalid Student";
            public static string invalidCourse = "Invalid Course";
            public static string coundNotConnectToDatabase = "Could Not Connect To Database";
            public static string invalidLogIn = "Incorrect email or password";
            public static string fullschedule = "Schedule is full";
            public static string timeOverlap = "Time Overlap";

        }

    }
}
