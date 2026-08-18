using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISATB_321_FINAL_PROJECT
{
    internal class clsStudents
    {



        private int mStudentID;
        private string mStudentFName;
        private string mStudentLName;
        private int mAdvisorID;
        private int mYear;

        public int StudentID
        {
            get { return mStudentID; }
            set { mStudentID = value; }
        }

        public string StudentFName
        {
            get { return mStudentFName; }
            set { mStudentFName = value; }
        }
        public string StudentLName
        {
            get { return mStudentLName; }
            set { mStudentLName = value; }
        }



        public int Year
        {
            get { return mYear; }
            set { mYear = value; }
        }


        public clsStudents() { }
    }
}
