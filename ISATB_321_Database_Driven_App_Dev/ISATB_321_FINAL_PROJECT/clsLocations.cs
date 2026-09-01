using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISATB_321_FINAL_PROJECT
{
    internal class clsLocations
    {

        private int mLocationID;
        private string mDescription;


        public int LoactionID
        {
            get { return mLocationID; }
            set { mLocationID = value; }
        }

        public string Description
        {
            get { return mDescription; }
            set { mDescription = value; }
        }








        public clsLocations() { }
    }
}
