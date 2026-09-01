using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using ISATB_321_FINAL_PROJECT;

/*
    TODO:
        - James: Add Availability functionality
        - James: Availability time implementation
            - James: 15 minute intervals, Availability table lists each interval as it's own entry,
              Meetings table will have several entries with the same StudentID but diff AvailabilityID             
        - James: Add front-end to Manage Availability

        - Emrys: Write trigger for if user tries to schedule a meeting with an advisor who is
                 not available at that time

    DONE:
        - Emrys: Add ListView to Add Person, Change Person, and Delete Person
            - Emrys: Remove View Advisors and View Students once done
        - Emrys: Resolve bug where inserting a new person does not display correct ID
 */

namespace ISATB_321_FINAL_PROJECT
{
    public partial class frmMain : Form
    {
        // Variables used across the entire form
        private Dictionary<int, clsAdvisors> dctAdvisors = new Dictionary<int, clsAdvisors>();

        private Dictionary<int, clsStudents> dctStudents = new Dictionary<int, clsStudents>();

        private Dictionary<int, clsAvailability> dctAvailability = new Dictionary<int, clsAvailability>();

        private Dictionary<int, clsMeetings> dctMeetings = new Dictionary<int, clsMeetings>();

        private DateTimePicker timePicker;



        // Necessary functions for the form to even load
        public frmMain()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timePicker = new DateTimePicker();
            timePicker.Format = DateTimePickerFormat.Time;
            timePicker.ShowUpDown = true;
            timePicker.Location = new Point(25, 129);
            timePicker.Width = 284;
            Controls.Add(timePicker);



            populateAdvisorDictionary(ref dctAdvisors);
            refreshAdvisorsListview();
            personInformation_ClearTextboxes();

            populateStudentDictionary(ref dctStudents);
            refreshStudentsListview();
            personInformation_ClearTextboxes();

            populateAvailabilityDictionary(ref dctAvailability);
            refreshAvailabilityListView();

        }




        // Functions for populating the textboxes
        private void displayAdvisorInformation_Update(clsAdvisors currentAdvisor)
        {
            txtOldID.Text = currentAdvisor.AdvisorID.ToString();
            txtOldFName.Text = currentAdvisor.AdvisorFName;
            txtOldLName.Text = currentAdvisor.AdvisorLName;
            txtOldEmail.Text = currentAdvisor.AdvisorEmail;    //.ToString();

        }

        private void displayStudentInformation_Update(clsStudents currentStudent)
        {

            txtOldID.Text = currentStudent.StudentID.ToString();
            txtOldFName.Text = currentStudent.StudentFName;
            txtOldLName.Text = currentStudent.StudentLName;
            txtOldYear.Text = currentStudent.Year.ToString();

        }

        private void displayAdvisorInformation_Delete(clsAdvisors currentAdvisor)
        {
            txtPersonID.Text = currentAdvisor.AdvisorID.ToString();
            txtDeleteFName.Text = currentAdvisor.AdvisorFName;
            txtDeleteLName.Text = currentAdvisor.AdvisorLName;
            txtDeleteEmail.Text = currentAdvisor.AdvisorEmail;    //.ToString();
        }

        private void displayStudentInformation_Delete(clsStudents currentStudent)
        {

            txtPersonID.Text = currentStudent.StudentID.ToString();
            txtDeleteFName.Text = currentStudent.StudentFName;
            txtDeleteLName.Text = currentStudent.StudentLName;
            txtDeleteYear.Text = currentStudent.Year.ToString();

        }

        private void displayAvailabilityInformation_Update(clsAvailability currentAvailability)
        {

            txtAvailabilityIDChange.Text = currentAvailability.AvailabilityID.ToString();
            txtNewAvailability.Text = currentAvailability.AdvisorID.ToString();
            txtOldDateChange.Text = currentAvailability.Date.ToString();
            txtOldTimeChange.Text = currentAvailability.TimeID.ToString();
            txtOldLocationChange.Text = currentAvailability.LocationID.ToString();

        }

        private void displayAvailabilityInformation_Delete(clsAvailability currentAvailability)
        {

            txtAvailabilityIDDelete.Text = currentAvailability.AvailabilityID.ToString();
            txtAdvisorIDDelete.Text = currentAvailability.AdvisorID.ToString();
            txtDateDelete.Text = currentAvailability.Date.ToString();
            txtTimeDelete.Text = currentAvailability.TimeID.ToString();
            txtLocationDelete.Text = currentAvailability.LocationID.ToString();

        }








        // Event handlers for triggering text box population
        private void lsvChangePerson_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (radChangeAdvisor.Checked)
            {

                ListView.SelectedListViewItemCollection itemIsSelected = lsvChangePerson.SelectedItems;
                foreach (ListViewItem item in itemIsSelected)
                {
                    clsAdvisors currentAdvisor = (clsAdvisors)item.Tag;
                    displayAdvisorInformation_Update(currentAdvisor);
                }

            }
            if (radChangeStudent.Checked)
            {

                ListView.SelectedListViewItemCollection itemIsSelected = lsvChangePerson.SelectedItems;
                foreach (ListViewItem item in itemIsSelected)
                {
                    clsStudents currentStudent = (clsStudents)item.Tag;
                    displayStudentInformation_Update(currentStudent);
                }

            }

        }

        private void lsvDeletePerson_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (radDeleteAdvisor.Checked)
            {

                ListView.SelectedListViewItemCollection itemIsSelected = lsvDeletePerson.SelectedItems;
                foreach (ListViewItem item in itemIsSelected)
                {
                    clsAdvisors currentAdvisor = (clsAdvisors)item.Tag;
                    displayAdvisorInformation_Delete(currentAdvisor);
                }

            }
            if (radDeleteStudent.Checked)
            {

                ListView.SelectedListViewItemCollection itemIsSelected = lsvDeletePerson.SelectedItems;
                foreach (ListViewItem item in itemIsSelected)
                {
                    clsStudents currentStudent = (clsStudents)item.Tag;
                    displayStudentInformation_Delete(currentStudent);
                }

            }

        }

        private void lsvChangeAvailability_SelectedIndexChanged(object sender, EventArgs e)
        {

            ListView.SelectedListViewItemCollection itemIsSelected = lsvChangeAvailability.SelectedItems;
            foreach (ListViewItem item in itemIsSelected)
            {
                clsAvailability currentChangeAvailability = (clsAvailability)item.Tag;
                displayAvailabilityInformation_Update(currentChangeAvailability);
            }

        }

        private void lsvDeleteAvailability_SelectedIndexChanged(object sender, EventArgs e)
        {

            ListView.SelectedListViewItemCollection itemIsSelected = lsvDeleteAvailability.SelectedItems;
            foreach (ListViewItem item in itemIsSelected)
            {
                clsAvailability currentDeleteAvailability = (clsAvailability)item.Tag;
                displayAvailabilityInformation_Delete(currentDeleteAvailability);
            }

        }



        // Functions for loading and refreshing the dictionaries and ListViews
        private void populateAdvisorDictionary(ref Dictionary<int, clsAdvisors> dctAdvisors)
        {

            string myConnectionString = clsDBUtil.getConnectionString();

            using (SqlConnection conn = new SqlConnection(myConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_GetAdvisor", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        dctAdvisors.Clear();

                        while (rdr.Read() == true)
                        {
                            clsAdvisors currentAdvisor = new clsAdvisors();
                            currentAdvisor.AdvisorID = (int)rdr["AdvisorID"];
                            currentAdvisor.AdvisorFName = clsDBUtil.convertFromDBType_VarcharToString(rdr["AdvisorFName"]);
                            currentAdvisor.AdvisorLName = clsDBUtil.convertFromDBType_VarcharToString(rdr["AdvisorLName"]);
                            currentAdvisor.AdvisorEmail = clsDBUtil.convertFromDBType_VarcharToString(rdr["AdvisorEmail"]);


                            dctAdvisors.Add(currentAdvisor.AdvisorID, currentAdvisor);
                        }

                    }

                }
                catch (Exception ex)
                {
                    messageBoxOK(ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }

        }

        private void populateStudentDictionary(ref Dictionary<int, clsStudents> dctStudents)
        {

            string myConnectionString = clsDBUtil.getConnectionString();

            using (SqlConnection conn = new SqlConnection(myConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_GetStudent", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        dctStudents.Clear();

                        while (rdr.Read() == true)
                        {
                            clsStudents currentStudent = new clsStudents();
                            currentStudent.StudentID = (int)rdr["StudentID"];
                            currentStudent.StudentFName = clsDBUtil.convertFromDBType_VarcharToString(rdr["StudentFName"]);
                            currentStudent.StudentLName = clsDBUtil.convertFromDBType_VarcharToString(rdr["StudentLName"]);
                            currentStudent.Year = (int)rdr["Year"];


                            dctStudents.Add(currentStudent.StudentID, currentStudent);
                        }

                    }

                }
                catch (Exception ex)
                {
                    messageBoxOK(ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }

        }

        private void populateAvailabilityDictionary(ref Dictionary<int, clsAvailability> dctAvailability)
        {

            string myConnectionString = clsDBUtil.getConnectionString();

            using (SqlConnection conn = new SqlConnection(myConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_GetAvailability", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        dctAvailability.Clear();

                        while (rdr.Read() == true)
                        {
                            clsAvailability currentAvailability = new clsAvailability();
                            currentAvailability.AvailabilityID = (int)rdr["AvailabilityID"];
                            currentAvailability.AdvisorID = (int)rdr["AdvisorID"];
                            //currentAvailability.Date = clsDBUtil.convertFromDBType_DateTimeToString(rdr["Date"]);
                            //Date in the SQL database has been changed into DateTime from DATE and probably should not saty this way.
                            //It was only changed so this would possibly work
                            currentAvailability.Date = (DateTime)rdr["Date"];
                            currentAvailability.TimeID = (int)rdr["TimeID"];
                            currentAvailability.LocationID = (int)rdr["LocationID"];
                            currentAvailability.IsTaken = (bool)rdr["IsTaken"];

                            dctAvailability.Add(currentAvailability.AvailabilityID, currentAvailability);
                        }

                    }

                }
                catch (Exception ex)
                {
                    messageBoxOK(ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }

        }

        private void refreshAdvisorsListview()
        {

            switch (tabMain.SelectedTab.Name)
            {
                case "tabPage6":
                    lsvAddPerson.Clear();
                    clsAdvisors newCurrentAdvisor = new clsAdvisors();
                    foreach (KeyValuePair<int, clsAdvisors> kvp in dctAdvisors)
                    {
                        newCurrentAdvisor = kvp.Value;
                        ListViewItem item = new ListViewItem(newCurrentAdvisor.AdvisorLName);
                        item.Tag = newCurrentAdvisor;
                        lsvAddPerson.Items.Add(item);
                    }
                    break;
                case "tabPage5":
                    lsvChangePerson.Clear();
                    clsAdvisors changeCurrentAdvisor = new clsAdvisors();
                    foreach (KeyValuePair<int, clsAdvisors> kvp in dctAdvisors)
                    {
                        changeCurrentAdvisor = kvp.Value;
                        ListViewItem item = new ListViewItem(changeCurrentAdvisor.AdvisorLName);
                        item.Tag = changeCurrentAdvisor;
                        lsvChangePerson.Items.Add(item);
                    }
                    break;
                case "tabPage8":
                    lsvDeletePerson.Clear();
                    clsAdvisors deleteCurrentAdvisor = new clsAdvisors();
                    foreach (KeyValuePair<int, clsAdvisors> kvp in dctAdvisors)
                    {
                        deleteCurrentAdvisor = kvp.Value;
                        ListViewItem item = new ListViewItem(deleteCurrentAdvisor.AdvisorLName);
                        item.Tag = deleteCurrentAdvisor;
                        lsvDeletePerson.Items.Add(item);
                    }
                    break;

            }

        }

        private void refreshStudentsListview()
        {

            switch (tabMain.SelectedTab.Name)
            {
                case "tabPage6":
                    lsvAddPerson.Clear();
                    clsStudents newCurrentStudent = new clsStudents();
                    foreach (KeyValuePair<int, clsStudents> kvp in dctStudents)
                    {
                        newCurrentStudent = kvp.Value;
                        ListViewItem item = new ListViewItem(newCurrentStudent.StudentLName);
                        item.Tag = newCurrentStudent;
                        lsvAddPerson.Items.Add(item);
                    }
                    break;
                case "tabPage5":
                    lsvChangePerson.Clear();
                    clsStudents changeCurrentStudent = new clsStudents();
                    foreach (KeyValuePair<int, clsStudents> kvp in dctStudents)
                    {
                        changeCurrentStudent = kvp.Value;
                        ListViewItem item = new ListViewItem(changeCurrentStudent.StudentLName);
                        item.Tag = changeCurrentStudent;
                        lsvChangePerson.Items.Add(item);
                    }
                    break;
                case "tabPage8":
                    lsvDeletePerson.Clear();
                    clsStudents deleteCurrentStudent = new clsStudents();
                    foreach (KeyValuePair<int, clsStudents> kvp in dctStudents)
                    {
                        deleteCurrentStudent = kvp.Value;
                        ListViewItem item = new ListViewItem(deleteCurrentStudent.StudentLName);
                        item.Tag = deleteCurrentStudent;
                        lsvDeletePerson.Items.Add(item);
                    }
                    break;
            }

        }

        private void refreshAvailabilityListView()
        {

            switch (tabMain.SelectedTab.Name)
            {
                case "tabPage3":
                    lsvCreateAvailability.Clear();
                    clsAvailability newAvailability = new clsAvailability();
                    foreach (KeyValuePair<int, clsAvailability> kvp in dctAvailability)
                    {
                        newAvailability = kvp.Value;
                        ListViewItem item = new ListViewItem(Convert.ToString(newAvailability.AvailabilityID));
                        item.Tag = newAvailability;
                        lsvCreateAvailability.Items.Add(item);
                    }
                    break;
                case "tabPage7":
                    lsvChangeAvailability.Clear();
                    clsAvailability changeCurrentAvailability = new clsAvailability();
                    foreach (KeyValuePair<int, clsAvailability> kvp in dctAvailability)
                    {
                        changeCurrentAvailability = kvp.Value;
                        ListViewItem item = new ListViewItem(Convert.ToString(changeCurrentAvailability.AvailabilityID));
                        item.Tag = changeCurrentAvailability;
                        lsvChangeAvailability.Items.Add(item);
                    }
                    break;
                case "tabPage4":
                    lsvDeleteAvailability.Clear();
                    clsAvailability deleteAvailability = new clsAvailability();
                    foreach (KeyValuePair<int, clsAvailability> kvp in dctAvailability)
                    {
                        deleteAvailability = kvp.Value;
                        ListViewItem item = new ListViewItem(Convert.ToString(deleteAvailability.AvailabilityID));
                        item.Tag = deleteAvailability;
                        lsvDeleteAvailability.Items.Add(item);
                    }
                    break;
            }

        }


        // Updating the ListViews
        // Insert Person
        private void radStudentNew_CheckedChanged(object sender, EventArgs e)
        {
            if (radStudentNew.Checked == true)
            {

                refreshStudentsListview();

            }

        }

        private void radAdvisorNew_CheckedChanged(object sender, EventArgs e)
        {

            if (radAdvisorNew.Checked == true)
            {

                refreshAdvisorsListview();

            }

        }



        // Update Person
        private void radChangeStudent_CheckedChanged(object sender, EventArgs e)
        {

            if (radChangeStudent.Checked == true)
            {

                refreshStudentsListview();

            }

        }

        private void radChangeAdvisor_CheckedChanged(object sender, EventArgs e)
        {

            if (radChangeAdvisor.Checked == true)
            {

                refreshAdvisorsListview();

            }

        }



        // Delete Person
        private void radDeleteStudent_CheckedChanged(object sender, EventArgs e)
        {

            if (radDeleteStudent.Checked == true)
            {

                refreshStudentsListview();

            }

        }

        private void radDeleteAdvisor_CheckedChanged(object sender, EventArgs e)
        {

            if (radDeleteAdvisor.Checked == true)
            {

                refreshAdvisorsListview();

            }

        }



        // Insert Availability
        private void btnNewAvailRefresh_Click(object sender, EventArgs e)
        {

            refreshAvailabilityListView();

        }



        // Update Availability
        private void btnRefreshAvailChange_Click(object sender, EventArgs e)
        {

            refreshAvailabilityListView();

        }



        // Delete Availability
        private void btnRefreshDeleteAvail_Click(object sender, EventArgs e)
        {

            refreshAvailabilityListView();

        }



        // Functions for adding
        private bool InsertAdvisor(clsAdvisors currentAdvisor)
        {
            string myConnectionString = clsDBUtil.getConnectionString();

            using (SqlConnection conn = new SqlConnection(myConnectionString))
            {
                try
                {

                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_InsertAdvisor", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@AdvisorFName", currentAdvisor.AdvisorFName);
                    cmd.Parameters.AddWithValue("@AdvisorLName", currentAdvisor.AdvisorLName);
                    cmd.Parameters.AddWithValue("@AdvisorEmail", currentAdvisor.AdvisorEmail);


                    cmd.ExecuteNonQuery();
                    return true;

                }
                catch (Exception ex)
                {
                    messageBoxOK(ex.Message);
                    return false;
                }
                finally
                {
                    conn.Close();
                }
            }

        }

        private bool InsertStudent(clsStudents currentStudent)
        {
            string myConnectionString = clsDBUtil.getConnectionString();

            using (SqlConnection conn = new SqlConnection(myConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_InsertStudent", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.AddWithValue("@AdvisorID", currentAdvisor.AdvisorID);

                    cmd.Parameters.AddWithValue("@StudentFName", currentStudent.StudentFName);
                    cmd.Parameters.AddWithValue("@StudentLName", currentStudent.StudentLName);
                    cmd.Parameters.AddWithValue("@Year", currentStudent.Year);


                    cmd.ExecuteNonQuery();
                    return true;

                }
                catch (Exception ex)
                {
                    messageBoxOK(ex.Message);
                    return false;

                }
                finally
                {
                    conn.Close();
                }
            }

        }

        private void btnSubmitNew_Click(object sender, EventArgs e)
        {

            string myConnectionString = clsDBUtil.getConnectionString();

            using (SqlConnection conn = new SqlConnection(myConnectionString))
            {
                try
                {

                    if (radAdvisorNew.Checked)
                    {

                        clsAdvisors currentAdvisor = new clsAdvisors();

                        conn.Open();
                        SqlCommand cmdGetAdvisor = new SqlCommand("sp_GetAdvisor", conn);
                        cmdGetAdvisor.CommandType = CommandType.StoredProcedure;

                        cmdGetAdvisor.ExecuteNonQuery();

                        currentAdvisor.AdvisorFName = txtFNameNew.Text;
                        currentAdvisor.AdvisorLName = txtLNameNew.Text;
                        currentAdvisor.AdvisorEmail = txtEmailNew.Text;



                        if (InsertAdvisor(currentAdvisor) == true)
                        {

                            SqlCommand cmdGetAdvisorID = new SqlCommand("SELECT dbo.fnGetAdvisorID(@AdvisorFName, @AdvisorLName)", conn);
                            cmdGetAdvisorID.CommandType = CommandType.Text;

                            // Adding parameters to the command
                            cmdGetAdvisorID.Parameters.AddWithValue("@AdvisorFName", currentAdvisor.AdvisorFName ?? string.Empty);
                            cmdGetAdvisorID.Parameters.AddWithValue("@AdvisorLName", currentAdvisor.AdvisorLName ?? string.Empty);

                            // Execute the command and store the result
                            int currentAdvisorID = (int)cmdGetAdvisorID.ExecuteScalar();

                            // Setting dct-associated value to result
                            currentAdvisor.AdvisorID = currentAdvisorID;

                            populateAdvisorDictionary(ref dctAdvisors);
                            refreshAdvisorsListview();

                            messageBoxOK("The advisor (ID: " + currentAdvisor.AdvisorID.ToString() + ") successfully added.");
                            personInformation_ClearTextboxes();

                        }
                        else
                        {

                            SqlCommand cmdGetAdvisorID = new SqlCommand("SELECT dbo.fnGetAdvisorID(@AdvisorFName, @AdvisorLName)", conn);
                            cmdGetAdvisorID.CommandType = CommandType.Text;

                            // Adding parameters to the command
                            cmdGetAdvisorID.Parameters.AddWithValue("@AdvisorFName", currentAdvisor.AdvisorFName ?? string.Empty);
                            cmdGetAdvisorID.Parameters.AddWithValue("@AdvisorLName", currentAdvisor.AdvisorLName ?? string.Empty);

                            // Execute the command and store the result
                            int currentAdvisorID = (int)cmdGetAdvisorID.ExecuteScalar();

                            // Setting dct-associated value to result
                            currentAdvisor.AdvisorID = currentAdvisorID;

                            messageBoxOK("Creation failed for advisor (ID: " + currentAdvisor.AdvisorID.ToString() + ").");

                        }
            ;

                    }
                    else if (radStudentNew.Checked)
                    {

                        clsStudents currentStudent = new clsStudents();


                        currentStudent.StudentFName = txtFNameNew.Text;
                        currentStudent.StudentLName = txtLNameNew.Text;


                        if (int.TryParse(txtYearNew.Text, out int year) && year >= 0 && year <= 4)
                        {
                            currentStudent.Year = year;
                        }
                        else
                        {
                            messageBoxOK("Invalid Year. Please enter a number between 0 and 4.");
                            txtYearNew.Focus();
                            return;
                        }

                        if (InsertStudent(currentStudent) == true)
                        {

                            populateStudentDictionary(ref dctStudents);
                            refreshStudentsListview();

                            messageBoxOK("The student (ID: " + currentStudent.StudentID.ToString() + ") successfully added.");
                            personInformation_ClearTextboxes();

                        }
                        else
                        {

                            messageBoxOK("Creation failed for student (ID: " + currentStudent.StudentID.ToString() + ").");

                        }
                    ;
                    }

                }
                catch (Exception ex)
                {
                    messageBoxOK(ex.Message);
                }
                finally
                {
                    conn.Close();
                }

            }

        }

        private bool InsertAvailability(clsAvailability currentAvailability)
        {
            string myConnectionString = clsDBUtil.getConnectionString();

            using (SqlConnection conn = new SqlConnection(myConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_InsertAvailability", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@AdvisorID", currentAvailability.AdvisorID);
                    cmd.Parameters.AddWithValue("@Date", currentAvailability.Date);
                    cmd.Parameters.AddWithValue("@TimeID", currentAvailability.TimeID);
                    cmd.Parameters.AddWithValue("@LocationID", currentAvailability.LocationID);
                    cmd.Parameters.AddWithValue("@IsTaken", currentAvailability.IsTaken);

                    cmd.ExecuteNonQuery();
                    return true;
                }
                catch (SqlException ex)
                {
                    // 2627 = primary key
                    // 2601 = duplicate
                    if (ex.Number == 2627 || ex.Number == 2601)
                    {
                        messageBoxOK("This availability slot already exists. Please choose a different date, time, or location.");
                    }
                    else
                    {
                        messageBoxOK("Database error: " + ex.Message);
                    }

                    return false;
                }
                catch (Exception ex)
                {
                    messageBoxOK("Unexpected error: " + ex.Message);
                    return false;
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        private void btnNewAvailability_Click(object sender, EventArgs e)
        {


            clsAvailability currentAvailability = new clsAvailability();



            if (int.TryParse(txtAvailAdvisorIDInsert.Text, out int AdvisorID))
            {
                currentAvailability.AdvisorID = AdvisorID;
            }
            else
            {
                messageBoxOK("Invalid AdvisorID.");
                txtAvailAdvisorIDInsert.Focus();
                return;
            }

            //currentAvailability.Date = txtDate.Text;


            if (DateTime.TryParse(txtDateInsert.Text, out DateTime parsedDate))
            {
                currentAvailability.Date = parsedDate;
            }
            else
            {
                messageBoxOK("Invalid Date.");
                txtDateInsert.Focus();
                return;
            }

            /*if (cboTimeBrowse.SelectedItem is ComboBoxItemNew selectedItem &&
            selectedItem.Value is clsTimes selectedTime)
            {
                currentAvailability.TimeID = selectedTime.TimeID;
            }
            else
            {
                messageBoxOK("Please select a valid time slot.");
                cboTimeBrowse.Focus();
                return;
            }*/



            if (int.TryParse(txtLocationIDInsert.Text, out int LocationID))
            {
                currentAvailability.LocationID = LocationID;
            }
            else
            {
                messageBoxOK("Invalid locationID .");
                txtLocationIDInsert.Focus();
                return;
            }


            if (chkIsTakenInsert.Checked)
            {

                currentAvailability.IsTaken = chkIsTakenInsert.Checked;

            }
            else
            {
                //messageBoxOK("ISTAKEN ERROR REFER BACK TO INSERT CLICK EVENT.");
                chkIsTakenInsert.Focus();
            }



            if (InsertAvailability(currentAvailability) == true)
            {

                populateAvailabilityDictionary(ref dctAvailability);
                refreshAvailabilityListView();



                messageBoxOK("The Availability (ID: " + currentAvailability.AvailabilityID.ToString() + ") successfully updated.");



            }
            else
            {
                messageBoxOK("Update Failed for Availability (ID: " + currentAvailability.AvailabilityID.ToString() + ").");
            }
            ;
        }



        // Functions for updating
        private bool updateStudent(clsStudents currentStudent)
        {
            string myConnectionString = clsDBUtil.getConnectionString();

            using (SqlConnection conn = new SqlConnection(myConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_UpdateStudent", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@StudentID", currentStudent.StudentID);
                    cmd.Parameters.AddWithValue("@StudentFName", currentStudent.StudentFName);
                    cmd.Parameters.AddWithValue("@StudentLName", currentStudent.StudentLName);
                    cmd.Parameters.AddWithValue("@Year", currentStudent.Year);


                    cmd.ExecuteNonQuery();
                    return true;

                }
                catch (Exception ex)
                {
                    messageBoxOK(ex.Message);
                    return false;
                }
                finally
                {
                    conn.Close();
                }
            }

        }

        private bool updateAdvisor(clsAdvisors currentAdvisor)
        {
            string myConnectionString = clsDBUtil.getConnectionString();

            using (SqlConnection conn = new SqlConnection(myConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_UpdateAdvisor", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@AdvisorID", currentAdvisor.AdvisorID);
                    cmd.Parameters.AddWithValue("@AdvisorFName", currentAdvisor.AdvisorFName);
                    cmd.Parameters.AddWithValue("@AdvisorLName", currentAdvisor.AdvisorLName);
                    cmd.Parameters.AddWithValue("@AdvisorEmail", currentAdvisor.AdvisorEmail);


                    cmd.ExecuteNonQuery();
                    return true;

                }
                catch (Exception ex)
                {
                    messageBoxOK(ex.Message);
                    return false;
                }
                finally
                {
                    conn.Close();
                }
            }

        }

        private void btnSubmitChanges_Click(object sender, EventArgs e)
        {
            if (radChangeAdvisor.Checked == true)
            {

                clsAdvisors currentAdvisor = new clsAdvisors();

                if (int.TryParse(txtOldID.Text, out int AdvisorID))
                {
                    currentAdvisor.AdvisorID = AdvisorID;
                }
                else
                {
                    messageBoxOK("Invalid Advisor ID.");
                    txtOldID.Focus();
                }

                currentAdvisor.AdvisorFName = txtNewFName.Text;
                currentAdvisor.AdvisorLName = txtNewLName.Text;
                currentAdvisor.AdvisorEmail = txtNewEmail.Text;



                if (updateAdvisor(currentAdvisor) == true)
                {

                    populateAdvisorDictionary(ref dctAdvisors);
                    refreshAdvisorsListview();



                    messageBoxOK("The advisor (ID: " + currentAdvisor.AdvisorID.ToString() + ") successfully updated.");
                    personInformation_ClearTextboxes();



                }
                else
                {
                    messageBoxOK("Update Failed for advisor (ID: " + currentAdvisor.AdvisorID.ToString() + ").");
                }
            ;
            }
            else if (radChangeStudent.Checked == true)
            {

                clsStudents currentStudent = new clsStudents();


                if (int.TryParse(txtOldID.Text, out int StudentID))
                {
                    currentStudent.StudentID = StudentID;
                }
                else
                {
                    messageBoxOK("Invalid Student ID.");
                    txtOldID.Focus();
                }





                currentStudent.StudentFName = txtNewFName.Text;
                currentStudent.StudentLName = txtNewLName.Text;




                if (int.TryParse(txtNewYear.Text, out int Year))
                {
                    currentStudent.Year = Year;
                }
                else
                {
                    messageBoxOK("Invalid Year.");
                    txtNewYear.Focus();
                    return;
                }

                if (updateStudent(currentStudent) == true)
                {

                    populateStudentDictionary(ref dctStudents);
                    refreshStudentsListview();



                    messageBoxOK("The student (ID: " + currentStudent.StudentID.ToString() + ") successfully updated.");
                    personInformation_ClearTextboxes();



                }
                else
                {
                    messageBoxOK("Update Failed for student (ID: " + currentStudent.StudentID.ToString() + ").");
                }
            ;


            }
        }

        private bool updateAvailability(clsAvailability currentAvailability)
        {
            string myConnectionString = clsDBUtil.getConnectionString();

            using (SqlConnection conn = new SqlConnection(myConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_UpdateAvailability", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@AvailabilityID", currentAvailability.AvailabilityID);
                    cmd.Parameters.AddWithValue("@AdvisorID", currentAvailability.AdvisorID);
                    cmd.Parameters.AddWithValue("@Date", currentAvailability.Date);
                    cmd.Parameters.AddWithValue("@TimeID", currentAvailability.TimeID);
                    cmd.Parameters.AddWithValue("@LocationID", currentAvailability.LocationID);
                    cmd.Parameters.AddWithValue("@IsTaken", currentAvailability.IsTaken);


                    cmd.ExecuteNonQuery();
                    return true;

                }
                catch (Exception ex)
                {
                    messageBoxOK(ex.Message);
                    return false;
                }
                finally
                {
                    conn.Close();
                }
            }

        }

        private void btnChangeAvailability_Click(object sender, EventArgs e)
        {

            clsAvailability currentAvailability = new clsAvailability();

            #region Validate user input & assigning to Availability properties

            if (int.TryParse(txtAvailabilityID.Text, out int AvailabilityID))
            {
                currentAvailability.AvailabilityID = AvailabilityID;
            }
            else
            {
                messageBoxOK("Invalid Availability ID.");
                txtAvailabilityID.Focus();
                return;
            }

            //  currentAvailability.StudentFName = txtStudentFName.Text;
            // currentAvailability.StudentLName = txtStudentLName.Text;

            if (int.TryParse(txtNewAvailability.Text, out int AdvisorID))
            {
                currentAvailability.AdvisorID = AdvisorID;
            }
            else
            {
                messageBoxOK("Invalid AdvisorID.");
                txtNewAvailability.Focus();
                return;
            }

            //currentAvailability.Date = txtDate.Text;


            if (DateTime.TryParse(txtNewDate.Text, out DateTime parsedDate))
            {
                currentAvailability.Date = parsedDate;
            }
            else
            {
                messageBoxOK("Invalid Date.");
                txtNewDate.Focus();
                return;
            }


            if (int.TryParse(txtNewTime.Text, out int TimeID))
            {
                currentAvailability.TimeID = TimeID;
            }
            else
            {
                messageBoxOK("Invalid TimeID ID.");
                txtNewTime.Focus();
                return;
            }

            if (int.TryParse(txtNewLocation.Text, out int LocationID))
            {
                currentAvailability.LocationID = LocationID;
            }
            else
            {
                messageBoxOK("Invalid TimeID ID.");
                txtNewLocation.Focus();
                return;
            }

            //checkbox

            if (chkIsTakenUpdate.Checked)
            {
                currentAvailability.IsTaken = true;
            }
            else
            {
                currentAvailability.IsTaken = false;

            }


            #endregion

            if (updateAvailability(currentAvailability) == true)
            {

                populateAvailabilityDictionary(ref dctAvailability);
                refreshAvailabilityListView();



                messageBoxOK("The Availability (ID: " + currentAvailability.AvailabilityID.ToString() + ") successfully updated.");



            }
            else
            {
                messageBoxOK("Update Failed for Availability (ID: " + currentAvailability.AvailabilityID.ToString() + ").");
            }
            ;

        }


        // Functions for deleting
        private bool deleteAdvisor(clsAdvisors delCurrentAdvisor)
        {
            string myConnectionString = clsDBUtil.getConnectionString();

            using (SqlConnection conn = new SqlConnection(myConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_DeleteAdvisor", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@AdvisorID", delCurrentAdvisor.AdvisorID);
                    /*
                    cmd.Parameters.AddWithValue("@AdvisorFName", currentAdvisor.AdvisorFName);
                    cmd.Parameters.AddWithValue("@AdvisorLName", currentAdvisor.AdvisorLName);
                    cmd.Parameters.AddWithValue("@AdvisorEmail", currentAdvisor.AdvisorEmail);
                    */

                    cmd.ExecuteNonQuery();
                    return true;

                }
                catch (Exception ex)
                {
                    messageBoxOK(ex.Message);
                    return false;
                }
                finally
                {
                    conn.Close();
                }
            }

        }

        private bool deleteStudent(clsStudents delCurrentStudent)
        {

            string myConnectionString = clsDBUtil.getConnectionString();

            using (SqlConnection conn = new SqlConnection(myConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_DeleteStudent", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@StudentID", delCurrentStudent.StudentID);
                    /*
                    cmd.Parameters.AddWithValue("@AdvisorFName", currentAdvisor.AdvisorFName);
                    cmd.Parameters.AddWithValue("@AdvisorLName", currentAdvisor.AdvisorLName);
                    cmd.Parameters.AddWithValue("@AdvisorEmail", currentAdvisor.AdvisorEmail);
                    */

                    cmd.ExecuteNonQuery();
                    return true;

                }
                catch (Exception ex)
                {
                    messageBoxOK(ex.Message);
                    return false;
                }
                finally
                {
                    conn.Close();
                }
            }

        }

        private void btnDeletePerson_Click(object sender, EventArgs e)
        {

            if (radDeleteAdvisor.Checked)
            {

                clsAdvisors currentAdvisor = new clsAdvisors();



                if (int.TryParse(txtPersonID.Text, out int AdvisorID))
                {
                    currentAdvisor.AdvisorID = AdvisorID;
                }
                else
                {
                    messageBoxOK("Invalid Advisor ID.");
                    txtPersonID.Focus();
                }

                currentAdvisor.AdvisorFName = txtDeleteFName.Text;
                currentAdvisor.AdvisorLName = txtDeleteLName.Text;
                currentAdvisor.AdvisorEmail = txtDeleteEmail.Text;




                if (deleteAdvisor(currentAdvisor) == true)
                {

                    messageBoxOK("The advisor (ID: " + currentAdvisor.AdvisorID.ToString() + ") successfully deleted.");

                    txtDeleteFName.Clear();

                    txtDeleteLName.Clear();

                    txtDeleteEmail.Clear();

                    txtPersonID.Clear();

                }
                else
                {

                    messageBoxOK("delete Failed for advisor (ID: " + currentAdvisor.AdvisorID.ToString() + ").");

                }

            }
            else if (radDeleteStudent.Checked)
            {

                clsStudents currentStudent = new clsStudents();



                if (int.TryParse(txtPersonID.Text, out int StudentID))
                {
                    currentStudent.StudentID = StudentID;
                }
                else
                {
                    messageBoxOK("Invalid Student ID.");
                    txtPersonID.Focus();
                }

                currentStudent.StudentFName = txtDeleteFName.Text;
                currentStudent.StudentLName = txtDeleteLName.Text;


                if (int.TryParse(txtDeleteYear.Text, out int Year))
                {
                    currentStudent.Year = Year;
                }
                else
                {
                    messageBoxOK("Invalid Year.");
                    txtDeleteYear.Focus();
                    return;
                }
                //currentStudent.Year = txtYear.Text;

                // currentStudent.AdvisorEmail = txtAdvisorEmail.Text;




                if (deleteStudent(currentStudent) == true)
                {

                    populateStudentDictionary(ref dctStudents);
                    refreshStudentsListview();



                    messageBoxOK("The student (ID: " + currentStudent.StudentID.ToString() + ") successfully deleted.");
                    personInformation_ClearTextboxes();



                }
                else
                {
                    messageBoxOK("delete Failed for student (ID: " + currentStudent.StudentID.ToString() + ").");
                }
            ;

            }
            else
            {

                messageBoxOK("Delete failed. Please select Student or Advisor.");

            }



        }

        private bool deleteAvailability(clsAvailability currentAvailability)
        {
            string myConnectionString = clsDBUtil.getConnectionString();

            using (SqlConnection conn = new SqlConnection(myConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_DeleteAvailability", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@AvailabilityID", currentAvailability.AvailabilityID);

                    cmd.ExecuteNonQuery();
                    return true;

                }
                catch (Exception ex)
                {
                    messageBoxOK(ex.Message);
                    return false;
                }
                finally
                {
                    conn.Close();
                }
            }

        }

        private void btnDeleteAvailability_Click(object sender, EventArgs e)
        {
            clsAvailability currentAvailability = new clsAvailability();




            if (int.TryParse(txtAvailabilityIDDelete.Text, out int AvailabilityID))
            {
                currentAvailability.AvailabilityID = AvailabilityID;
            }
            else
            {
                messageBoxOK("Invalid AvailabilityID.");
                txtAvailabilityIDDelete.Focus();
                return;
            }


            if (int.TryParse(txtAdvisorIDDelete.Text, out int AdvisorID))
            {
                currentAvailability.AdvisorID = AdvisorID;
            }
            else
            {
                messageBoxOK("Invalid AdvisorID.");
                txtAdvisorIDDelete.Focus();
                return;
            }

            //currentAvailability.Date = txtDate.Text;


            if (DateTime.TryParse(txtDateDelete.Text, out DateTime parsedDate))
            {
                currentAvailability.Date = parsedDate;
            }
            else
            {
                messageBoxOK("Invalid Date.");
                txtDateDelete.Focus();
                return;
            }


            if (int.TryParse(txtTimeDelete.Text, out int TimeID))
            {
                currentAvailability.TimeID = TimeID;
            }
            else
            {
                messageBoxOK("Invalid TimeID ID.");
                txtTimeDelete.Focus();
                return;
            }

            if (int.TryParse(txtLocationDelete.Text, out int LocationID))
            {
                currentAvailability.LocationID = LocationID;
            }
            else
            {
                messageBoxOK("Invalid LocationID ID.");
                txtLocationDelete.Focus();
                return;
            }


            //checkbox
            if (chkIsTakenDelete.Checked)
            {
                currentAvailability.IsTaken = true;
            }
            else
            {
                currentAvailability.IsTaken = false;

            }

            currentAvailability.AvailabilityID = currentAvailability.AvailabilityID;


            if (deleteAvailability(currentAvailability) == true)
            {

                populateAvailabilityDictionary(ref dctAvailability);
                refreshAvailabilityListView();



                messageBoxOK("The Availability (ID: " + currentAvailability.AvailabilityID.ToString() + ") successfully deleted.");


            }
            else
            {
                messageBoxOK("delete Failed for Availability (ID: " + currentAvailability.AvailabilityID.ToString() + ").");
            }
            ;
        }

        // Clear Form Functions
        private void personInformation_ClearTextboxes()
        {

            txtOldFName.Clear();

            txtOldLName.Clear();


            txtNewYear.Clear();

            txtNewFName.Clear();

            txtNewLName.Clear();

            txtNewEmail.Clear();







            /*            btnEditAdvisorInfo.Visible = true;
                        btnUpdateAdvisorInfo.Visible = false;
                        btnDeleteAdvisorInfo.Visible = false;
                        btnInsertAdvisorInfo.Visible = true;*/


        }

        private void btnChangePersonClearForm_Click(object sender, EventArgs e)
        {

            txtOldFName.Clear();

            txtOldLName.Clear();



            txtNewFName.Clear();

            txtNewLName.Clear();

            txtNewYear.Clear();

            txtNewEmail.Clear();



            if (radChangeStudent.Checked)
            {

                radChangeStudent.Checked = false;

            }
            if (radChangeAdvisor.Checked)
            {

                radChangeAdvisor.Checked = false;

            }


        }

        private void btnDeletePersonClearForm_Click(object sender, EventArgs e)
        {
            txtDeleteFName.Clear();

            txtDeleteLName.Clear();

            txtDeleteEmail.Clear();

            txtPersonID.Clear();
        }

        private void btnClearCreateAvail_Click(object sender, EventArgs e)
        {

            txtAvailabilityID.Clear();

            txtAvailAdvisorIDInsert.Clear();

            txtDateInsert.Clear();


            txtLocationIDInsert.Clear();

        }

        private void btnClearChange_Click(object sender, EventArgs e)
        {

            txtAvailabilityIDChange.Clear();

            txtNewAvailability.Clear();

            txtOldDateChange.Clear();

            txtOldTimeChange.Clear();

            txtOldLocationChange.Clear();


            txtNewAvailability.Clear();

            txtNewAdvisorID.Clear();

            txtNewDate.Clear();

            txtNewTime.Clear();

            txtNewLocation.Clear();

        }

        private void btnClearDelete_Click(object sender, EventArgs e)
        {

            txtAvailabilityIDDelete.Clear();

            txtAdvisorIDDelete.Clear();

            txtDateDelete.Clear();

            txtTimeDelete.Clear();

            txtLocationDelete.Clear();

        }



        //MEETING CODE MESSY*****************************************************************************
        //*************************************************************************************************
        //***************************************************************************************************

        public class ComboBoxItem
        {
            public string DisplayText { get; }
            public object Value { get; }

            public ComboBoxItem(string text, object value)
            {
                DisplayText = text;
                Value = value;
            }

            public override string ToString()
            {
                return DisplayText;
            }
        }



        //student combobox logic
        private void populateStudentsComboBox()
        {
            cboStudentsBrowse.Items.Clear();
            foreach (var currentStudent in dctStudents.Values)
            {
                cboStudentsBrowse.Items.Add(new ComboBoxItem(currentStudent.StudentID.ToString() + " " + currentStudent.StudentLName, currentStudent));
            }
        }


/*        private void cboStudentsBrowse_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboStudentsBrowse.SelectedItem is ComboBoxItem selectedItem)
            {
                clsStudents currentstudent = (clsStudents)selectedItem.Value;
                txtStudentLName.Text = currentstudent.StudentLName;
            }
        }*/


        //Availability combobox logic


        private void populateAvailabilityComboBox()
        {
            cboAvailabilityBrowse.Items.Clear();
            foreach (var currentAvailability in dctAvailability.Values)
            {
                cboAvailabilityBrowse.Items.Add(new ComboBoxItem("AvailabilityID:" + currentAvailability.AvailabilityID + ", " + currentAvailability.Date.ToString("MM/dd/yyyy"), currentAvailability));
            }
        }

        //availability combobox

        private void cboAvailabilityBrowse_SelectedIndexChanged(object sender, EventArgs e)
        {


            if (cboAvailabilityBrowse.SelectedItem is ComboBoxItem selectedItem)
            {
                clsAvailability currentAvailability = (clsAvailability)selectedItem.Value;
                txtAvailabilityID.Text = currentAvailability.AvailabilityID.ToString();
            }

        }
        //creates the meeting dictionary
        private void populateMeetingDictionary(ref Dictionary<int, clsMeetings> dctMeetings)
        {

            string myConnectionString = clsDBUtil.getConnectionString();

            using (SqlConnection conn = new SqlConnection(myConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_GetMeeting", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        dctMeetings.Clear();

                        while (rdr.Read() == true)
                        {
                            clsMeetings currentMeeting = new clsMeetings();
                            currentMeeting.MeetingID = (int)rdr["MeetingID"];
                            currentMeeting.StudentID = (int)rdr["StudentID"];
                            currentMeeting.AvailabilityID = (int)rdr["AvailabilityID"];



                            dctMeetings.Add(currentMeeting.MeetingID, currentMeeting);
                        }

                    }

                }
                catch (Exception ex)
                {
                    messageBoxOK(ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }

        }

        //refresh the listview for meetings
        private void refreshMeetingsListview()
        {
            // REMEMBER: the View property of the listview must be set to 'List'
            lvwMeetings.Clear();
            clsMeetings currentMeeting = new clsMeetings();
            foreach (KeyValuePair<int, clsMeetings> kvp in dctMeetings)
            {
                currentMeeting = kvp.Value;
                ListViewItem item = new ListViewItem(currentMeeting.MeetingID.ToString());
                item.Tag = currentMeeting;
                lvwMeetings.Items.Add(item);
            }
        }

        //pull sql info to font end
        private void displayMeetingInformation_update(clsMeetings currentMeeting)
        {
            txtMeetingID.Text = currentMeeting.MeetingID.ToString();
            txtMeetStudentID.Text = currentMeeting.StudentID.ToString();
            txtMeetAvailabilityID.Text = currentMeeting.AvailabilityID.ToString();

        }



        //clear meeting textboxes
        private void meetingInformation_Update_ClearTextboxes()
        {
            txtMeetingID.Clear();
            txtMeetingID.ReadOnly = true;

            txtMeetStudentID.Clear();
            txtMeetStudentID.ReadOnly = true;

            txtMeetAvailabilityID.Clear();
            txtMeetAvailabilityID.ReadOnly = true;

            //txtStartTime.Visible = true;
            //txtEndTime.Visible = true;


            cboStudentsBrowse.SelectedIndex = -1;
            cboAvailabilityBrowse.SelectedIndex = -1;




        }
        //change selected index
        private void lvwMeetings_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListView.SelectedListViewItemCollection itemIsSelected = lvwMeetings.SelectedItems;
            foreach (ListViewItem item in itemIsSelected)
            {
                clsMeetings currentMeeting = (clsMeetings)item.Tag;
                displayMeetingInformation_update(currentMeeting);
            }
        }



        //delete method for meetings
        private bool deleteMeeting(clsMeetings currentMeeting)
        {
            string myConnectionString = clsDBUtil.getConnectionString();

            using (SqlConnection conn = new SqlConnection(myConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_DeleteMeeting", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MeetingID", currentMeeting.MeetingID);


                    cmd.ExecuteNonQuery();
                    return true;

                }
                catch (Exception ex)
                {
                    messageBoxOK(ex.Message);
                    return false;
                }
                finally
                {
                    conn.Close();
                }
            }

        }

        //insert method for meetings
        private bool InsertMeeting(clsMeetings currentMeeting)
        {
            string myConnectionString = clsDBUtil.getConnectionString();

            using (SqlConnection conn = new SqlConnection(myConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_InsertMeeting", conn);
                    cmd.CommandType = CommandType.StoredProcedure;


                    cmd.Parameters.AddWithValue("@StudentID", currentMeeting.StudentID);
                    cmd.Parameters.AddWithValue("@AvailabilityID", currentMeeting.AvailabilityID);



                    cmd.ExecuteNonQuery();
                    return true;

                }
                catch (SqlException ex)
                {
                    if (ex.Number == 2627 || ex.Number == 2601)
                    {
                        messageBoxOK("A meeting already exists for this student and time. Please choose a different slot.");
                    }
                    else
                    {
                        messageBoxOK("An error occurred while scheduling the meeting: " + ex.Message);
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    messageBoxOK("Unexpected error: " + ex.Message);
                    return false;
                }
                finally
                {
                    conn.Close();
                }
            }

        }



        //delete button for meetings
        private void btnDeleteMeeting_Click(object sender, EventArgs e)
        {
            clsMeetings currentMeeting = new clsMeetings();



            if (int.TryParse(txtMeetingID.Text, out int MeetingID))
            {
                currentMeeting.MeetingID = MeetingID;
            }
            else
            {
                messageBoxOK("Invalid meeting ID.");
                txtMeetingID.Focus();
                return;
            }


            if (int.TryParse(txtMeetAvailabilityID.Text, out int AvailabilityID))
            {
                currentMeeting.AvailabilityID = AvailabilityID;
            }
            else
            {
                messageBoxOK("Invalid availability ID.");
                txtMeetAvailabilityID.Focus();
                return;
            }





            if (deleteMeeting(currentMeeting) == true)
            {

                populateMeetingDictionary(ref dctMeetings);
                refreshMeetingsListview();



                messageBoxOK("The meeting (ID: " + currentMeeting.MeetingID.ToString() + ") successfully deleted.");
                meetingInformation_Update_ClearTextboxes();



            }
            else
            {
                messageBoxOK("delete Failed for meeting (ID: " + currentMeeting.MeetingID.ToString() + ").");
            }
            ;
        }

        //create meeting button
        private void btnCreateMeeting_Click(object sender, EventArgs e)
        {
            clsMeetings currentMeeting = new clsMeetings();




            if (cboStudentsBrowse.SelectedItem is ComboBoxItemNew selectedItem &&
            selectedItem.Value is clsStudents selectedStudent)
            {
                currentMeeting.StudentID = selectedStudent.StudentID;
            }
            else
            {
                messageBoxOK("Invalid student selection.");
                cboStudentsBrowse.Focus();
                return;
            }


            if (cboAvailabilityBrowse.SelectedItem is ComboBoxItemNew selectedAvailabilityItem &&
            selectedAvailabilityItem.Value is clsAvailability selectedAvailability)
            {
                currentMeeting.AvailabilityID = selectedAvailability.AvailabilityID;
            }
            else
            {
                messageBoxOK("Invalid availability selection.");
                cboAvailabilityBrowse.Focus();
                return;
            }





            if (InsertMeeting(currentMeeting) == true)
            {

                populateMeetingDictionary(ref dctMeetings);
                refreshMeetingsListview();



                messageBoxOK("The meeting (ID: " + currentMeeting.MeetingID.ToString() + ") successfully updated.");
                meetingInformation_Update_ClearTextboxes();



            }

            else
            {
                messageBoxOK("Create Failed for meeting (ID: " + currentMeeting.MeetingID.ToString() + ").");
            }
           ;
        }

        public class ComboBoxItemNew
        {
            public string DisplayText { get; }
            public object Value { get; }

            public ComboBoxItemNew(string text, object value)
            {
                DisplayText = text;
                Value = value;
            }

            public override string ToString()
            {
                return DisplayText;
            }
        }

        // DialogBoxes
        private void messageBoxOK(string msg)
        {
            MessageBox.Show(msg, Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private DialogResult messageBoxYesNo(string msg)
        {
            return MessageBox.Show(msg, Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

    }
}
