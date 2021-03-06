﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Courses;
using University.Users;
using University;

namespace RegAppMVC.Models
{
    public class CurrentStudent:User
    {
        public string major;
        private int credithours = 0;
        private bool isFullTime;
        public Dictionary<string, Course> schedule = new Dictionary<string, Course>();
        private static CurrentStudent instance = null;
        private CurrentStudent()
        {

        }
       public static CurrentStudent GetInstance()
        {
            if (instance ==null )
            {
                instance = new CurrentStudent();
            }
            return instance;
        }
        public void ResetCurrentStudent()
        {
            instance = null;
        }

        public void InitializeStudent (Student s)
        {
            this.ID = s.ID;
            this.FirstName = s.FirstName;
            this.LastName = s.LastName;
            this.Email = s.Email;
            this.password = s.Password;
            this.major = s.major;
            schedule =(s.GetSchedule());
            CalculateCredits();
        }
        public bool isFull
        {
            get
            {
                if (credithours >= Global.maxCredits)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public bool FullTime { get {return(isFullTime); }}
        private void CalculateCredits()
        {
            credithours = 0;
            foreach(var item in schedule)
            {
                credithours += item.Value.creditHours;
            }
            if (credithours >= 3)
                isFullTime = true;
            else
                isFullTime = false;
        }
        public int Credits
        {
            get { return credithours; }
        }


        public Dictionary<string, Course> GetSchedule()
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
                schedule.Add(course.CourseName, course);
                CalculateCredits();
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
            schedule.Remove(DataConnection.getCourse(courseID).CourseName);
            CalculateCredits();
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
            CalculateCredits();
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
                    CalculateCredits();
                }
            }
        }

        /// <summary>
        /// Adds multiple students to the list
        /// </summary>
        /// <param name="roster"></param>
        /// <return>Will throw indexOUutOfRangeException if the course does not have enough space or if the student or course is not found</return>
        public void AddCourses(Dictionary<string, Course> s)
        {
            int tempCredits = 0;
            foreach (var item in s)
            {
                tempCredits += item.Value.creditHours;
            }
            if (credithours + tempCredits <= Global.maxCredits)
            {
                foreach (var item in schedule)
                {
                    AddCourse(item.Value);
                    CalculateCredits();
                }
            }
            else
            {
                throw new IndexOutOfRangeException(Global.Errors.notEnoughSpace);
            }
        }
        public bool CheckForOverlap (Course s)
        {
            foreach (var item in schedule)
            {
                if (item.Value.startTime<s.EndTime && s.startTime <item.Value.EndTime)
                {
                    return true;
                }
            }
            return false;
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
