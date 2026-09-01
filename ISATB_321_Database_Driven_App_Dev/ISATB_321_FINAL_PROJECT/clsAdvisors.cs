using System;

namespace ISATB_321_FINAL_PROJECT
{
    internal class clsAdvisors
    {
        private int mAdvisorID;
        private string mAdvisorFName;
        private string mAdvisorLName;
        private string mAdvisorEmail;


        public int AdvisorID
        {
            get { return mAdvisorID; }
            set { mAdvisorID = value; }
        }

        public string AdvisorFName
        {
            get { return mAdvisorFName; }
            set { mAdvisorFName = value; }
        }
        public string AdvisorLName
        {
            get { return mAdvisorLName; }
            set { mAdvisorLName = value; }
        }

        public string AdvisorEmail
        {
            get { return mAdvisorEmail; }
            set { mAdvisorEmail = value; }
        }


        public clsAdvisors() { }
    }
}
