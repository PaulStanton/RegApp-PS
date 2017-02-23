using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Users;
namespace University.Courses
{
    public class Course : ICourse
    {
        private string courseName = "";
        private string Major;
        public bool isClosed;
        private List<Student> studentRoster = new List<Student>();
        public DateTime startTime;
        public int creditHours;
        public int ID;
        #region Constructors
        /// <summary>
        /// Will initialize blank course
        /// </summary>
        public Course()
        {

        }
        /// <summary>
        /// Will initialize a course given all of the following parameters
        /// </summary>
        /// <param name="courseName"></param>
        /// <param name="startTime"></param>
        /// <param name="creditHours"></param>
        /// <param name="major"></param>
        public Course(string courseName, TimeSpan startTime, int creditHours, string major)
        {
            this.courseName = courseName;
            this.startTime = new DateTime(1, 1, 1, 0, 0, 0, 0) + startTime;
            this.creditHours = creditHours;
            this.major = major;
        }
        #endregion Constructors

        #region Properties

        public string major
        {
            get { return Major; }
            set { Major = value; }
        }
        public bool isFull
        {
            get
            {
                if (RosterCount >= Global.maxStudents)
                    return true;
                else
                    return false;
            }
        }

        public int RosterCount
        {
            get
            {
                return studentRoster.Count();
            }
        }

        public string CourseName
        {
            get
            {
                return courseName;
            }

            set
            {
                courseName = value;
            }
        }

        #endregion Properties
        /// <summary>
        /// Will add the student/course combo to the database junction table.  Will also add the student to the courseRoster.
        /// </summary>
        /// <param name="student"></param>
        /// <returns>Will throw an indexOutOfRange exception if the student or course is not found or if the course is full</returns>
        public DateTime EndTime { get { return (startTime.AddHours(creditHours)); } }
        public void AddStudent(Student student)
        {
            if (isFull == false)
            {
                DataConnection.RegisterStudentForCourse(ID, student.ID);
                studentRoster.Add(student);
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
        public void RemoveStudent(int studentID)
        {
            DataConnection.DropStudentFromCourse(ID, studentID);
            foreach (var item in studentRoster)
            {
                if (item.ID == studentID)
                {
                    studentRoster.Remove(item);
                }
            }
        }
        /// <summary>
        /// Will remove the student form the database as well as the local courseRoster given the student object
        /// </summary>
        /// <param name="id"></param>
        /// <return>Will throw an indexOutOfRange exception if the student or course is not found</return>
        public void RemoveStudent(Student student)
        {
            DataConnection.DropStudentFromCourse(ID, student.ID);
            studentRoster.Remove(student);
        }
        /// <summary>
        /// Will remove the student from the database as well as the local courseRoster given the students first and last name
        /// </summary>
        /// <param name="firstname"></param>
        /// <param name="lastname"></param>
        /// <return>Will throw an indexOutOfRange exception if the student or course is not found</return>
        public void RemoveStudent(string firstname, string lastname)
        {
            foreach (var item in studentRoster)
            {
                if (item.FirstName==firstname && item.LastName==lastname)
                {
                    DataConnection.DropStudentFromCourse(ID, item.ID);
                    studentRoster.Remove(item);
                }
            }
        }
        /// <summary>
        /// returns the studentRoster list
        /// </summary>
        /// <returns></returns>
        public List<Student> GetStudentRoster()
        {
            return studentRoster;
        }
        /// <summary>
        /// Adds multiple students to the list
        /// </summary>
        /// <param name="roster"></param>
        /// <return>Will throw indexOUutOfRangeException if the course does not have enough space or if the student or course is not found</return>
        public void AddStudents(List<Student> roster)
        {
            if (studentRoster.Count + roster.Count <= Global.maxStudents)
            {
                foreach (var item in roster)
                {
                    AddStudent(item);
                }
            }
            else
            {
                throw new IndexOutOfRangeException(Global.Errors.notEnoughSpace);
            }
        }
        public void setRoster(List<Student>roster)
        {
            studentRoster = roster;
        }
    }
}

