using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using University.Courses;
using University.Users;

namespace University
{
    /// <summary>
    /// Various Methods will read and write to RegApp Database
    /// Must give the class a ConnectionString before using other methods.
    /// All methods will throw an InvalidOperation or an SQL exception if they are unable to connect to the database
    /// </summary>
    public static class DataConnection
    {

        private static string connection = "Data Source=demodatabase.clnb0ldm68iw.us-west-2.rds.amazonaws.com,1433;Initial Catalog=RegistrationApp;Persist Security Info=True;User Id=master;Password=password;Encrypt=False;";
        public static string GetConnection()
        {
            return connection;
            //return "Data Source=demodatabase.clnb0ldm68iw.us-west-2.rds.amazonaws.com,1433;Initial Catalog=RegistrationApp;Persist Security Info=True;User Id=master;Password=password;Encrypt=False;";
        }
        public static void SetConnection (string s)
        {
            connection = s;
        }

        /* READS (connected)
            * Get Student (StudentID)
            *      - Individual
            * Get Course Info (CourseID)
            *      - Individual
            * Get Student Schedule
            * Get Course Roster
        */
        public static bool CheckLogInInfo(Student s)
        {
            string query = "SELECT * FROM StudentMajorJoin";

            using (SqlConnection sqlcon = new SqlConnection(connection))
            {
                SqlCommand command = new SqlCommand(query, sqlcon);
                
                    DataSet ds = new DataSet();
                    SqlDataAdapter adapter = new SqlDataAdapter();

                    adapter.SelectCommand = new SqlCommand(query, sqlcon);

                    adapter.Fill(ds);
                foreach (var item in ds.Tables[0].AsEnumerable())
                {
                    if ((string)item["password"]==s.Password && (string)item["Email"]==s.Email)
                        {
                        s.FirstName = (string)item["First Name"];
                        s.LastName = (string)item["Last Name"];
                        s.major = (string)item["MajorName"];
                        s.ID = (int)item[0];
                        s.schedule=(getStudentSchedule(s.ID));
                            return true;
                        }
                }
                return false;

                
            }
        }
        /// <summary>
        /// Takes the connection and the students ID num as params and returns the requested student.
        /// </summary>
        /// <param name="idnum"></param>
        /// <returns>will Will throw indexoutofrange exception if student is not found</returns>
        public static Student getStudent(int idnum)
        {
        
                Student tempStudent = new Student();

                string query = "SELECT * FROM StudentMajorJoin";
            try
            {
                using (SqlConnection sqlcon = new SqlConnection(connection))
                {
                    SqlCommand command = new SqlCommand(query, sqlcon);

                    sqlcon.Open(); //InvalidOperationException
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        int tempid = (int)reader["StudentID"];
                        if (tempid == idnum)

                        {
                            string first = (string)reader["First Name"];
                            string last = (string)reader["Last Name"];
                            string email = (string)reader["Email"];
                            string password = (string)reader["Password"];
                            string major = (string)reader["MajorName"];

                            tempStudent = new Student(first, last, password, email, idnum, major);

                        }


                    }
                    reader.Close();
                    if (tempStudent.ID != idnum) //the student with that ID was not found
                    {
                        throw new IndexOutOfRangeException(Global.Errors.studentNotFound);
                    }
                    else
                    {
                        return tempStudent;
                    }
                }
            }
            catch(InvalidOperationException e)
            {
                throw new InvalidOperationException(Global.Errors.coundNotConnectToDatabase, e);
            }
            

      
        }
        /// <summary>
        /// Takes the connection and the course ID num as params and returns the requested student.
        /// </summary>
        /// <param name="idnum"></param>
        /// <returns>will throw indexoutofrange exception if course is not found</returns>
        public static Course getCourse(int idnum)
        {
            Course tempCourse = new Course();

            string query = "Select * From CourseMajorJoin";
            try
            {
                using (SqlConnection sqlcon = new SqlConnection(connection))
                {
                    SqlCommand command = new SqlCommand(query, sqlcon);

                    sqlcon.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        int tempid = (int)reader["CourseID"];
                        if (tempid == idnum)

                        {
                            string name = (string)reader["CourseName"];
                            TimeSpan startTime = (TimeSpan)reader["StartTime"];
                            int creditHours = (int)reader["CreditHours"];
                            string major = (string)reader["MajorName"];


                            tempCourse = new Course(name, startTime, creditHours, major);
                            tempCourse.ID = idnum;
                        }

                    }
                    reader.Close();
                    if (tempCourse.ID != idnum)//Course not found 
                    {
                        throw new IndexOutOfRangeException(Global.Errors.courseNotFound);
                    }
                    else
                    {
                        return tempCourse;
                    }
                }
            }
            catch(InvalidOperationException e)
            {
                throw new InvalidOperationException(Global.Errors.coundNotConnectToDatabase, e);
            }

        }
        /// <summary>
        /// Returns a Dictionary of all of the registered courses of a given student
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="idnum"></param>
        /// <returns>Will throw indexoutofrange exception if the student is not found</returns>
        public static Dictionary<string, Course> getStudentSchedule(int idnum)
        {
            Dictionary<string, Course> schedule = new Dictionary<string, Course>();

            string query = $"Select * From GetStudentSchedule({idnum})";
            try
            {
                using (SqlConnection sqlcon = new SqlConnection(connection))
                {
                    SqlCommand command = new SqlCommand(query, sqlcon);

                    sqlcon.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {


                        string name = (string)reader["CourseName"];
                        TimeSpan startTime = (TimeSpan)reader["StartTime"];
                        int creditHours = (int)reader["CreditHours"];
                        string major = (string)reader["MajorName"];
                        int courseID = (int)reader["CourseID"];

                        schedule.Add(name, new Course(name, startTime, creditHours, major));
                        schedule[name].ID = courseID;


                    }
                    reader.Close();

                }
            }
            catch(InvalidOperationException e)
            {
                throw new InvalidOperationException(Global.Errors.coundNotConnectToDatabase, e);
            }
               return schedule;

        }
        /// <summary>
        /// Returns List of students in a given course
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="idnum"></param>
        /// <returns>will throw indexoutofrange exception if the course is not found</returns>
        public static List<Student> getCourseRoster(int idnum)
        {
            List<Student> roster = new List<Student>();

            string query = $"Select * From GetCourseRoster({idnum})";
            try
            {
                using (SqlConnection sqlcon = new SqlConnection(connection))
                {
                    SqlCommand command = new SqlCommand(query, sqlcon);

                    sqlcon.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {


                        string first = (string)reader["First Name"];
                        string last = (string)reader["Last Name"];
                        string password = (string)reader["Email"];
                        string email = (string)reader["Password"];
                        string major = (string)reader["MajorName"];
                        int tempid = (int)reader["StudentID"];

                        roster.Add(new Student(first, last, password, email, tempid, major));



                    }
                    reader.Close();

                }
            }
            catch (InvalidOperationException e)
            {
                throw new InvalidOperationException(Global.Errors.coundNotConnectToDatabase, e);
            }
            if (roster.Count == 0)//Course Not Found
            {
                throw new IndexOutOfRangeException(Global.Errors.courseNotFound);
            }
            else
            {
                return roster;
            }
        }
        /*READS (disconnected)
         * getMajorID
         * getMaxStudnetID
         * getMaxCourseID
         */
         /// <summary>
         /// returns the majorID associated with a given course string WILL RETURN -1 IF STUDENT IS NOT FOUND
         /// </summary>
         /// <param name="majorName"></param>
         /// <returns>Will return -1 if student is not found</returns>
        private static int getMajorId(string majorName)
        {
            using (SqlConnection con = new SqlConnection(connection))
            {
                string query = "Select * From Major";
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = new SqlCommand(query, con);
                da.Fill(ds);
                int majorID = -1;
                foreach (var k in ds.Tables[0].AsEnumerable())
                {
                    if ((string)k["MajorName"] == majorName)
                    {
                        majorID = (int)k[0];
                    }
                }
                return majorID;
            }
        }
        /// <summary>
        /// Gets the highest studentID in the database.  Will be the StudentID of the last student added if no other changes are made
        /// </summary>
        /// <param name="connection"></param>
        /// <returns>Returns -1 if no students are found</returns>
        public static int getMaxStudentID()
        {
            using (SqlConnection con = new SqlConnection(connection))
            {
                string query = "Select * From Student";
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = new SqlCommand(query, con);
                da.Fill(ds);
                int studentID = -1;
                foreach (var k in ds.Tables[0].AsEnumerable())
                {
                    if ((int)k[0] > studentID)
                    {
                        studentID = (int)k[0];
                    }
                }
                return studentID;
            }

        }
        /// <summary>
        /// gets the highest courseID in the database.  Will be the courseID of the last course added if no other changes are made to the table.
        /// </summary>
        /// <returns>returns -1 if no courses are found</returns>
        public static int getMaxCourseID()
        {
            using (SqlConnection con = new SqlConnection(connection))
            {
                string query = "Select * From Course";
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = new SqlCommand(query, con);
                da.Fill(ds);
                int courseID = -1;
                foreach (var k in ds.Tables[0].AsEnumerable())
                {
                    if ((int)k[0] > courseID)
                    {
                        courseID = (int)k[0];
                    }
                }
                return courseID;
            }

        }
        /* WRITES (disconnected)
            * Update Student Entry
            * Add Student Entry
            * Delete Student Entry
            * Update Course Entry
            * Add Course Entry
            * Delete Course Entry
            * Addto StudentCourse
            * Delete StudentCourse
        */
        /// <summary>
        /// Add's a given student to the database.  Returns the Student ID number
        /// </summary>
        /// <param name="s"></param>
        /// <param name="connection"></param>
        /// <returns> Will throw an invalid Operation Expression if either the student is invalid, or if it cannot connect to the database.</returns>
        public static int AddStudent(Student s)
        {
            // The student is invalid in the any of the following scenarios
            if (s==null || string.IsNullOrEmpty(s.FirstName) == true||string.IsNullOrEmpty(s.LastName)==true|| string.IsNullOrEmpty(s.Email)==true) 
            {
                throw new InvalidOperationException(Global.Errors.invalidStudent);
            }
            try
            {
                using (SqlConnection con = new SqlConnection(connection))
                {
                    int majorID = getMajorId(s.major);

                    SqlCommand cmd = new SqlCommand("AddStudent", con);
                    cmd.CommandType = CommandType.StoredProcedure;


                    cmd.Parameters.Add("@first", SqlDbType.VarChar).Value = s.FirstName;
                    cmd.Parameters.Add("@last", SqlDbType.VarChar).Value = s.LastName;
                    cmd.Parameters.Add("@email", SqlDbType.VarChar).Value = s.Email;
                    cmd.Parameters.Add("@password", SqlDbType.VarChar).Value = s.Password;
                    cmd.Parameters.Add("@major", SqlDbType.Int).Value = majorID;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    s.ID = getMaxStudentID();
                    return s.ID;

                }
            }
            catch(InvalidOperationException e)
            {
                throw new InvalidOperationException(Global.Errors.coundNotConnectToDatabase,e);
            }


        }
        /// <summary>
        /// Removes Student from database given student ID
        /// </summary>
        /// <param name="c"></param>
        /// <param name="connection"></param>
        /// <returns>Will Throw indexOutOfRange Exception if student is not found.</returns>
        public static bool DeleteStudent(int studentID)
        {
            getStudent(studentID); //Will Throw an indexoutofrange exception of the student is not found.
            try
            {
                using (SqlConnection con = new SqlConnection(connection))
                {
                    SqlCommand cmd = new SqlCommand("DeleteStudent", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@studentID", SqlDbType.Int).Value = studentID;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    return true;
                }
            }
            catch(InvalidOperationException e)
            {
                throw new InvalidOperationException(Global.Errors.coundNotConnectToDatabase,e);
            }
        }
        /// <summary>
        /// Updates the student with the given studentID to the students information passed as a parameter,
        /// </summary>
        /// <param name="s"></param>
        /// <param name="connection"></param>
        /// <returns>Will throw an indexoutofrange if student is not found or an invalidoperation exception if it cannot connect or if the passed student object is invalid</returns>
        public static bool UpdateStudent(int studentID, Student s)
        {
            // The student is invalid in the any of the following scenarios
            if (s == null || string.IsNullOrEmpty(s.FirstName) == true || string.IsNullOrEmpty(s.LastName) == true || string.IsNullOrEmpty(s.Email) == true)
            {
                throw new InvalidOperationException(Global.Errors.invalidStudent);
            }
            getStudent(studentID); //Will throw indexOutOfRange exception if the student is not found.
            try
            {
                using (SqlConnection con = new SqlConnection(connection))
                {
                    int majorID = getMajorId(s.major);
                    SqlCommand cmd = new SqlCommand("UpdateStudent", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@first", SqlDbType.VarChar).Value = s.FirstName;
                    cmd.Parameters.Add("@last", SqlDbType.VarChar).Value = s.LastName;
                    cmd.Parameters.Add("@email", SqlDbType.VarChar).Value = s.Email;
                    cmd.Parameters.Add("@password", SqlDbType.VarChar).Value = s.Password;
                    cmd.Parameters.Add("@major", SqlDbType.Int).Value = majorID;
                    cmd.Parameters.Add("@studentID", SqlDbType.Int).Value = studentID;

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    return true;
                }
            }
            catch(InvalidOperationException e)
            {
                throw new InvalidOperationException(Global.Errors.coundNotConnectToDatabase, e);
            }
        }
        /// <summary>
        /// Adds the given course to the Database.  Returns the Course ID number
        /// </summary>
        /// <param name="c"></param>
        /// <param name="connection"></param>
        /// <returns> Will throw an invalid Operation Expression if either the course is invalid, or if it cannot connect to the database. </returns>
        public static int AddCourse(Course c)
        {
            //Course is invalid in any of the following scenarios
            if (c == null || c.creditHours <=0 || string.IsNullOrEmpty(c.CourseName) == true || (c.isClosed) == true)
            {
                throw new InvalidOperationException(Global.Errors.invalidCourse);
            }
            try
            {
                using (SqlConnection con = new SqlConnection(connection))
                {
                    int majorID = getMajorId(c.major);

                    SqlCommand cmd = new SqlCommand("AddCourse", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@coursename", SqlDbType.VarChar).Value = c.CourseName;
                    cmd.Parameters.Add("@startTime", SqlDbType.Time).Value = c.startTime.TimeOfDay;
                    cmd.Parameters.Add("@credithours", SqlDbType.Int).Value = c.creditHours;
                    cmd.Parameters.Add("@MajorID", SqlDbType.Int).Value = majorID;

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);

                    c.ID = getMaxStudentID();
                    return c.ID;


                }
            }catch(InvalidOperationException e)
            {
                throw new InvalidOperationException(Global.Errors.coundNotConnectToDatabase, e);
            }

        }
        /// <summary>
        /// Delets the course with the given course ID from the database
        /// </summary>
        /// <param name="courseID"></param>
        /// <param name="connection"></param>
        /// <returns>Will Throw indexOutOfRange Exception if course is not found.</returns>
        public static bool DeleteCourse(int courseID)
        {
            getCourse(courseID); //will throw indexOutOfRange exception if course is not found.
            try
            {
                using (SqlConnection con = new SqlConnection(connection))
                {
                    SqlCommand cmd = new SqlCommand("DeleteCourse", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@courseID", SqlDbType.Int).Value = courseID;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    return true;
                }
            }catch(InvalidOperationException e)
            {
                throw new InvalidOperationException(Global.Errors.coundNotConnectToDatabase,e);
            }
        }
        /// <summary>
        /// Updates the course with the given course ID to the given course information
        /// </summary>
        /// <param name="courseID"></param>
        /// <param name="c"></param>
        /// <param name="connection"></param>
        /// <returns>Will throw an indexoutofrange if course is not found or an invalidoperation exception if it cannot connect or if the passed course object is invalid</returns>
        public static bool UpdateCourse(int courseID, Course c)
        {
            //Course is invalid in any of the following scenarios
            if (c == null || c.creditHours <= 0 || string.IsNullOrEmpty(c.CourseName) == true)
            {
                throw new InvalidOperationException(Global.Errors.invalidCourse);
            }
            getCourse(courseID);//Will throw indexOutRange exception if the course is not found
            try
            {
                using (SqlConnection con = new SqlConnection(connection))
                {
                    int majorID = getMajorId(c.major);

                    SqlCommand cmd = new SqlCommand("UpdateCourse", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@coursename", SqlDbType.VarChar).Value = c.CourseName;
                    cmd.Parameters.Add("@startTime", SqlDbType.Time).Value = c.startTime.TimeOfDay;
                    cmd.Parameters.Add("@credithours", SqlDbType.Int).Value = c.creditHours;
                    cmd.Parameters.Add("@MajorID", SqlDbType.Int).Value = majorID;
                    cmd.Parameters.Add("@courseID", SqlDbType.Int).Value = courseID;

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);

                    c.ID = getMaxStudentID();
                    return true; ;


                }
            }catch(InvalidOperationException e)
            {
                throw new InvalidOperationException(Global.Errors.coundNotConnectToDatabase, e);
            }


        }
        /// <summary>
        /// Adds the student ID and courseID to junction table signifying the student is registered for that course
        /// </summary>
        /// <param name="courseID"></param>
        /// <param name="studentID"></param>
        /// <returns>Will throw an indexOutOfRange exception if the student or course is not found</returns>
        public static bool RegisterStudentForCourse(int courseID, int studentID)
        {
            getCourse(courseID); //Will Throw IndexOutOfRange exception if the course is not found
            getStudent(studentID); //Will Throw IndexOUtOfRange exception if the student is not found
            try
            {
                using (SqlConnection con = new SqlConnection(connection))
                {

                    SqlCommand cmd = new SqlCommand("RegisterStudentForCourse", con);
                    cmd.CommandType = CommandType.StoredProcedure;


                    cmd.Parameters.Add("@studentID", SqlDbType.Int).Value = studentID;
                    cmd.Parameters.Add("@courseID", SqlDbType.Int).Value = courseID;

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    return true;

                }
            }
            catch(InvalidOperationException e)
            {
                throw new InvalidOperationException(Global.Errors.coundNotConnectToDatabase, e);
            }


        }
        /// <summary>
        /// Drops the student/course combo from the Junction table in the database.
        /// </summary>
        /// <param name="courseID"></param>
        /// <param name="studentID"></param>
        /// <returns>Will throw an indexOutOfRange exception if the student or course is not found</returns>
        public static bool DropStudentFromCourse(int courseID , int studentID)
        {
            getCourse(courseID); //Will Throw IndexOutOfRange exception if the course is not found
            getStudent(studentID); //Will Throw IndexOUtOfRange exception if the student is not found

            try
            {
                using (SqlConnection con = new SqlConnection(connection))
                {

                    SqlCommand cmd = new SqlCommand("DropStudentFromCourse", con);
                    cmd.CommandType = CommandType.StoredProcedure;


                    cmd.Parameters.Add("@studentID", SqlDbType.Int).Value = studentID;
                    cmd.Parameters.Add("@courseID", SqlDbType.Int).Value = courseID;

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    return true;

                }
            } catch(InvalidOperationException e)
            {
                throw new InvalidOperationException(Global.Errors.coundNotConnectToDatabase, e);
            }
        }
    }
}
