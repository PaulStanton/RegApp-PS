using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Users;

namespace University.Courses
{
    interface ICourse
    {
        void AddStudent(Student student);
        void RemoveStudent(int id);
        void RemoveStudent(Student student);
        void RemoveStudent(string firstname, string lastname);
        bool isFull { get; }
        List<Student> GetStudentRoster();
        void AddStudents(List<Student> roster);
        int RosterCount { get;}
        string CourseName { get; set; }

    }
}
