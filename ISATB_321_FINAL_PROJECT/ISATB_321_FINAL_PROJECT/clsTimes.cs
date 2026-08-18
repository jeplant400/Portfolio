using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISATB_321_FINAL_PROJECT
{
    internal class clsTimes
    {

        private int mTimeID;
        private TimeSpan mStartTime;
        private TimeSpan mEndTime;

        public int TimeID
        {
            get { return mTimeID; }
            set { mTimeID = value; }
        }

        public TimeSpan StartTime
        {
            get { return mStartTime; }
            set { mStartTime = value; }
        }

        public TimeSpan EndTime
        {
            get { return mEndTime; }
            set { mEndTime = value; }
        }




        public clsTimes() { }


    }
}
