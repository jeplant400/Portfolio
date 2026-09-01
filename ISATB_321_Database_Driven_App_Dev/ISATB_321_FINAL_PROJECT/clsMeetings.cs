using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISATB_321_FINAL_PROJECT
{
    internal class clsMeetings
    {

        private int mMeetingID;
        private int mStudentID;
        private int mAvailabilityID;


        public int MeetingID
        {
            get { return mMeetingID; }
            set { mMeetingID = value; }
        }

        public int StudentID
        {
            get { return mStudentID; }
            set { mStudentID = value; }
        }

        public int AvailabilityID
        {
            get { return mAvailabilityID; }
            set { mAvailabilityID = value; }
        }


        public clsMeetings() { }



    }
}
