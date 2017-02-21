using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Courses;

namespace University.Users
{
    public abstract class User
    {

        #region fields
        private string firstname;
        private string lastname;
        protected string password;
        private string email;
        private int id;
        #endregion fields
        #region CTOR

        public User()
        {

        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="name">person's name</param>
        /// <param name="password">person's password</param>
        /// <param name="email">person's email</param>
        /// <param name="id">person's id number</param>
        public User(string firstname, string lastname, string password, string email, int id)
        {
            this.firstname = firstname;
            this.lastname = lastname;
            this.password = password;
            this.email = email;
            this.id = id;

        }
        #endregion CTOR
        #region PROP
        public string FullName
        {
            get
            {
                string fullname = $"{firstname} {lastname}";
                return Global.CreateFullName(firstname, lastname);
            }
        }
        public string Password
        {
            get { return password; }
            set { password = value; }

        }
        public string FirstName
        {
            get { return firstname; }
            set { firstname = value; }
        }
        public string LastName
        {
            get { return lastname; }
            set { email = value; }
        }
        public string Email
        {
            get { return email; }
            set { email = value; }
        }
        public int ID
        {
            get { return id; }
            set { id = value; }
        }


        #endregion PROP

        public override string ToString()
        {
            string result = "";
            result += FullName;
            result += "\n";
            result += $"email: {Email}";
            return result;

        }
        public abstract string GetInfo();

    }

    

}
