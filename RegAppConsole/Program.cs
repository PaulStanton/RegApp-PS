using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University;
using University.Users;
using University.Courses;
using System.Configuration;


namespace RegAppConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            string s = ConfigurationManager.ConnectionStrings["RegAppDB"].ConnectionString;
            DataConnection.SetConnection(s);

            Student st = new Student();
            st.Password = "123456";
            st.Email = "123456@gmail.com";
            Console.WriteLine("{0}", DataConnection.CheckLogInInfo(st));
            Console.WriteLine(st.ID);
            foreach(var item in st.GetSchedule())
            {
                Console.WriteLine(item.Value.ID);
            }
            Console.ReadLine();
        }
    }
}
