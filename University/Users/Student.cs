using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Courses;

namespace University.Users
{
    public class Student : User
    {
        public string major;
        private bool isFullTime;
        public Dictionary<string, Course> schedule = new Dictionary<string, Course>();
            public Student()
        {

        }
        public Student(string firstname, string lastname, string password, string email, int id, string major) : base(firstname, lastname, password, email, id)
        {
            this.major = major;
            isFullTime = false;
        }
        public bool isFull
        {
            get
            {
                if (schedule.Count >= Global.maxCourses)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public Dictionary<string,Course> GetSchedule()
        {
            return schedule;
        }
        /// <summary>
        /// Will add the student/course combo to the database junction table.  Will also add the student to the courseRoster.
        /// </summary>
        /// <param name="student"></param>
        /// <returns>Will throw an indexOutOfRange exception if the student or course is not found or if the course is full</returns>
        public void AddCourse(Course course)
        {
            if (isFull == false)
            {
                DataConnection.RegisterStudentForCourse(course.ID, ID);
                schedule.Add(course.CourseName,course);
            }
            else
            {
                throw new IndexOutOfRangeException(Global.Errors.notEnoughSpace);
            }
        }
        /// <summary>
        /// Will remove the student form the database as well as the local courseRoster given the students ID
        /// </summary>
        /// <param name="id"></param>
        /// <return>Will throw an indexOutOfRange exception if the student or course is not found</return>
        public void RemoveCourse(int courseID)
        {
            DataConnection.DropStudentFromCourse(courseID, ID);
            foreach (var item in schedule)
            {
                if (item.Value.ID == courseID)
                {
                    schedule.Remove(item.Value.CourseName);
                }
            }
        }
        /// <summary>
        /// Will remove the student form the database as well as the local courseRoster given the student object
        /// </summary>
        /// <param name="id"></param>
        /// <return>Will throw an indexOutOfRange exception if the student or course is not found</return>
        public void RemoveCourse(Course course)
        {
            DataConnection.DropStudentFromCourse(course.ID, ID);
            schedule.Remove(course.CourseName);
        }
        /// <summary>
        /// Will remove the student from the database as well as the local courseRoster given the students first and last name
        /// </summary>
        /// <param name="firstname"></param>
        /// <param name="lastname"></param>
        /// <return>Will throw an indexOutOfRange exception if the student or course is not found</return>
        public void RemoveCourse(string coursename)
        {
            foreach (var item in schedule)
            {
                if (item.Value.CourseName == coursename)
                {
                    DataConnection.DropStudentFromCourse(item.Value.ID, ID);
                    schedule.Remove(item.Value.CourseName);
                }
            }
        }

        /// <summary>
        /// Adds multiple students to the list
        /// </summary>
        /// <param name="roster"></param>
        /// <return>Will throw indexOUutOfRangeException if the course does not have enough space or if the student or course is not found</return>
        public void AddCourses(Dictionary<string,Course> s)
        {
            if (schedule.Count + s.Count <= Global.maxCourses)
            {
                foreach (var item in schedule)
                {
                    AddCourse(item.Value);
                }
            }
            else
            {
                throw new IndexOutOfRangeException(Global.Errors.notEnoughSpace);
            }
        }

        public override string GetInfo()
        {
            StringBuilder info = new StringBuilder(base.ToString());
            info.Append($"\n{major}");
            info.Append($"\nfulltime: {isFullTime}");
            if (schedule.Count == 0)
            {
                info.Append($"\nnot registered for classes");
            }
            else
                foreach (KeyValuePair<String, Course> item in schedule)
                {
                    info.Append($"\n{item.Value.CourseName}");
                }
            return info.ToString();
        }

    }
}
