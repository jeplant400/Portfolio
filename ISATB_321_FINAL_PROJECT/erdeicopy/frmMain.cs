// Developer:       Plant, James
// Last Updated:    2025.04.20


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace WindowsFormsApp1
{
    public partial class frmMain : Form
    {
        private Dictionary<int, clsAdvisors> dctAdvisors = new Dictionary<int, clsAdvisors>();

        private Dictionary<int, clsStudents> dctStudents = new Dictionary<int, clsStudents>();

        private Dictionary<int, clsAvailability> dctAvailability = new Dictionary<int, clsAvailability>();

        private Dictionary<int, clsMeetings> dctMeetings = new Dictionary<int, clsMeetings>();

        private Dictionary<int, clsTimes> dctTimes = new Dictionary<int, clsTimes>();


        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            populateAdvisorDictionary(ref dctAdvisors);
            refreshAdvisorsListview();
            advisorInformation_Update_ClearTextboxes();

            populateStudentDictionary(ref dctStudents);
            refreshStudentsListview();
            studentInformation_Update_ClearTextboxes();

            populateAvailabilityDictionary(ref dctAvailability);
            refreshAvailabilityListview();
            availabilityInformation_Update_ClearTextboxes();

            populateMeetingDictionary(ref dctMeetings);
            refreshMeetingsListview();
            meetingInformation_Update_ClearTextboxes();
            
            populateTimesDictionary(ref dctTimes);
            refreshTimesListview();
            timeInformation_Update_ClearTextboxes();
            
            //meetings V

            /*
            refreshBrowseAdvisorsListview();
            refreshStudentsBrowseListview();
            refreshAvailabilityBrowseListview();
            */

            // populateAdvisorsComboBox();

            populateStudentsComboBox();

            populateAvailabilityComboBox();

            populateTimeComboBox();

        }


        //ADVISORS*******************************************************************************************************************************************


        //populate advisor dictionary
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
        //update advisor stored procedure
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
        //refresh advisors listview
        private void refreshAdvisorsListview()
        {
            // REMEMBER: the View property of the listview must be set to 'List'
            lvwAdvisors.Clear();
            clsAdvisors currentAdvisor = new clsAdvisors();
            foreach (KeyValuePair<int, clsAdvisors> kvp in dctAdvisors)
            {
                currentAdvisor = kvp.Value;
                ListViewItem item = new ListViewItem(currentAdvisor.AdvisorLName);
                item.Tag = currentAdvisor;
                lvwAdvisors.Items.Add(item);
            }
        }
        //update listview
        private void lvwAdvisors_Update_SelectedIndexChanged(object sender, EventArgs e)
        {

            ListView.SelectedListViewItemCollection itemIsSelected = lvwAdvisors.SelectedItems;
            foreach (ListViewItem item in itemIsSelected)
            {
                clsAdvisors currentAdvisor = (clsAdvisors)item.Tag;
                displayAdvisorInformation_update(currentAdvisor);
            }
        }
        //textbox to variable for advisors
        private void displayAdvisorInformation_update(clsAdvisors currentAdvisor)
        {
            txtAdvisorID.Text = currentAdvisor.AdvisorID.ToString();
            txtAdvisorFName.Text = currentAdvisor.AdvisorFName;
            txtAdvisorLName.Text = currentAdvisor.AdvisorLName;
            txtAdvisorEmail.Text = currentAdvisor.AdvisorEmail;    //.ToString();

        }

        //update and clear advisor textboxes
        private void advisorInformation_Update_ClearTextboxes()
        {
            txtAdvisorID.Clear();
            txtAdvisorID.ReadOnly = true;

            txtAdvisorFName.Clear();
            txtAdvisorFName.ReadOnly = true;

            txtAdvisorLName.Clear();
            txtAdvisorLName.ReadOnly = true;

            txtAdvisorEmail.Clear();
            txtAdvisorEmail.ReadOnly = true;


            txtAdvisorIDInsert.Clear();

            txtAdvisorFNameInsert.Clear();

            txtAdvisorLNameInsert.Clear();

            txtAdvisorEmailInsert.Clear();







            btnEditAdvisorInfo.Visible = true;
            btnUpdateAdvisorInfo.Visible = false;
            btnDeleteAdvisorInfo.Visible = false;
            btnInsertAdvisorInfo.Visible = true;


        }



        #region Private Helper Methods: general purpose

        private void messageBoxOK(string msg)
        {
            MessageBox.Show(msg, Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private DialogResult messageBoxYesNo(string msg)
        {
            return MessageBox.Show(msg, Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

        #endregion
        //edit advisor button
        private void btnEditAdvisorInfo_Click(object sender, EventArgs e)
        {
            if (txtAdvisorID.Text == "")
            {
                return;
            }

            txtAdvisorFName.ReadOnly = false;
            txtAdvisorLName.ReadOnly = false;
            txtAdvisorEmail.ReadOnly = false;


            btnEditAdvisorInfo.Visible = false;
            btnUpdateAdvisorInfo.Visible = true;
            btnDeleteAdvisorInfo.Visible = true;
        }





        #region edit delete insert

        //update advisor button
        private void btnUpdateAdvisorInfo_Click(object sender, EventArgs e)
        {
            clsAdvisors currentAdvisor = new clsAdvisors();

            #region Validate user input & assigning to Advisor properties

            if (int.TryParse(txtAdvisorID.Text, out int AdvisorID))
            {
                currentAdvisor.AdvisorID = AdvisorID;
            }
            else
            {
                messageBoxOK("Invalid Advisor ID.");
                txtAdvisorID.Focus();
            }

            currentAdvisor.AdvisorFName = txtAdvisorFName.Text;
            currentAdvisor.AdvisorLName = txtAdvisorLName.Text;
            currentAdvisor.AdvisorEmail = txtAdvisorEmail.Text;




            /*
            if (txtAdvisorEmail.Text == "")
            {
                currentAdvisor.AdvisorEmail = "";
            }
            else if (DateTime.TryParse(txtAdvisorEmail.Text, out DateTime BirthDate))
            {
                currentAdvisor.AdvisorEmail = txtAdvisorEmail.Text;
            }
            else 
            {
                messageBoxOK("Invalid Birth Date.");
                txtAdvisorEmail.Focus();
            }
            */
            /*
            if (Double.TryParse(txtWeight.Text, out double Weight))
            {
                currentAdvisor.Weight = Weight;
            }
            else
            {
                messageBoxOK("Invalid Weight.");
                txtWeight.Focus();
            }
            */
            #endregion



            if (updateAdvisor(currentAdvisor) == true)
            {

                populateAdvisorDictionary(ref dctAdvisors);
                refreshAdvisorsListview();



                messageBoxOK("The advisor (ID: " + currentAdvisor.AdvisorID.ToString() + ") successfully updated.");
                advisorInformation_Update_ClearTextboxes();



            }
            else
            {
                messageBoxOK("Update Failed for advisor (ID: " + currentAdvisor.AdvisorID.ToString() + ").");
            }
            ;


        }





        //delete advisor stored procedure
        private bool deleteAdvisor(clsAdvisors currentAdvisor)
        {
            string myConnectionString = clsDBUtil.getConnectionString();

            using (SqlConnection conn = new SqlConnection(myConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_DeleteAdvisor", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@AdvisorID", currentAdvisor.AdvisorID);
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






        //delete advisor button
        private void btnDelete_Click(object sender, EventArgs e)
        {


            /*
            if (txtAdvisorID.Text == "")
            {
                return;
            }
            string query = "Delete from Advisors where AdvisorID= '" + txtAdvisorID.Text + "'";
            */


            clsAdvisors currentAdvisor = new clsAdvisors();



            if (int.TryParse(txtAdvisorID.Text, out int AdvisorID))
            {
                currentAdvisor.AdvisorID = AdvisorID;
            }
            else
            {
                messageBoxOK("Invalid Advisor ID.");
                txtAdvisorID.Focus();
                return;
            }

            currentAdvisor.AdvisorFName = txtAdvisorFName.Text;
            currentAdvisor.AdvisorLName = txtAdvisorLName.Text;
            currentAdvisor.AdvisorEmail = txtAdvisorEmail.Text;




            if (deleteAdvisor(currentAdvisor) == true)
            {

                populateAdvisorDictionary(ref dctAdvisors);
                refreshAdvisorsListview();



                messageBoxOK("The advisor (ID: " + currentAdvisor.AdvisorID.ToString() + ") successfully deleted.");
                advisorInformation_Update_ClearTextboxes();



            }
            else
            {
                messageBoxOK("delete Failed for advisor (ID: " + currentAdvisor.AdvisorID.ToString() + ").");
            }
            ;
        }

        //insert advisor stored procedure
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
                    //cmd.Parameters.AddWithValue("@AdvisorID", currentAdvisor.AdvisorID);

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



        //insert advisor button
        /*
        private void btnInsertAdvisorInfo_Click(object sender, EventArgs e)
        {


            clsAdvisors currentAdvisor = new clsAdvisors();



            if (int.TryParse(txtAdvisorID.Text, out int AdvisorID))
            {
                currentAdvisor.AdvisorID = AdvisorID;
            }
            else
            {
                messageBoxOK("Invalid Advisor ID.");
                txtAdvisorID.Focus();
            }

            currentAdvisor.AdvisorFName = txtAdvisorFNameInsert.Text;
            currentAdvisor.AdvisorLName = txtAdvisorLNameInsert.Text;
            currentAdvisor.AdvisorEmail = txtAdvisorEmailInsert.Text;




            if (InsertAdvisor(currentAdvisor) == true)
            {

                populateAdvisorDictionary(ref dctAdvisors);
                refreshAdvisorsListview();



                messageBoxOK("The advisor (ID: " + currentAdvisor.AdvisorID.ToString() + ") successfully updated.");
                advisorInformation_Update_ClearTextboxes();



            }
            else
            {
                messageBoxOK("Update Failed for advisor (ID: " + currentAdvisor.AdvisorID.ToString() + ").");
            }
            ;
        }
        */



        private void btnInsertAdvisorInfo_Click_1(object sender, EventArgs e)
        {






            clsAdvisors currentAdvisor = new clsAdvisors();


            

            currentAdvisor.AdvisorFName = txtAdvisorFNameInsert.Text;
            currentAdvisor.AdvisorLName = txtAdvisorLNameInsert.Text;
            currentAdvisor.AdvisorEmail = txtAdvisorEmailInsert.Text;




            if (InsertAdvisor(currentAdvisor) == true)
            {

                populateAdvisorDictionary(ref dctAdvisors);
                refreshAdvisorsListview();



                messageBoxOK("The advisor (ID: " + currentAdvisor.AdvisorID.ToString() + ") successfully updated.");
                advisorInformation_Update_ClearTextboxes();



            }

            else
            {
                messageBoxOK("Update Failed for advisor (ID: " + currentAdvisor.AdvisorID.ToString() + ").");
            }
            ;
        }
        #endregion




        //STUDENTS*****************************************************************************************************************************************





        //create student dictionary
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


        //update student method
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


        //refresh student listview
        private void refreshStudentsListview()
        {
            // REMEMBER: the View property of the listview must be set to 'List'
            lvwStudents.Clear();
            clsStudents currentStudent = new clsStudents();
            foreach (KeyValuePair<int, clsStudents> kvp in dctStudents)
            {
                currentStudent = kvp.Value;
                ListViewItem item = new ListViewItem(currentStudent.StudentLName);
                item.Tag = currentStudent;
                lvwStudents.Items.Add(item);
            }
        }



        //update student listview
        private void lvwStudents_Update_SelectedIndexChanged(object sender, EventArgs e)
        {

            ListView.SelectedListViewItemCollection itemIsSelected = lvwStudents.SelectedItems;
            foreach (ListViewItem item in itemIsSelected)
            {
                clsStudents currentStudent = (clsStudents)item.Tag;
                displayStudentInformation_update(currentStudent);
            }
        }
        // text info reflects database
        private void displayStudentInformation_update(clsStudents currentStudent)
        {
            txtStudentID.Text = currentStudent.StudentID.ToString();
            txtStudentFName.Text = currentStudent.StudentFName;
            txtStudentLName.Text = currentStudent.StudentLName;
            txtYear.Text = currentStudent.Year.ToString();

        }
        //clears student textboxes
        private void studentInformation_Update_ClearTextboxes()
        {
            txtStudentID.Clear();
            txtStudentID.ReadOnly = true;

            txtStudentFName.Clear();
            txtStudentFName.ReadOnly = true;

            txtStudentLName.Clear();
            txtStudentLName.ReadOnly = true;

            txtYear.Clear();
            txtYear.ReadOnly = true;

            txtStudentIDInsert.Clear();


            txtStudentFNameInsert.Clear();


            txtStudentLNameInsert.Clear();


            txtYearInsert.Clear();





            /*
            txtWeight.Clear();
            txtWeight.ReadOnly = true;
            */

            btnStudentEdit.Visible = true;
            btnStudentUpdate.Visible = false;
            btnStudentDelete.Visible = false;
            //btnInsertAdvisorInfo.Visible = true;


        }


        //edit students button
        private void btnStudentEdit_Click(object sender, EventArgs e)
        {
            if (txtStudentID.Text == "")
            {
                return;
            }

            txtStudentFName.ReadOnly = false;
            txtStudentLName.ReadOnly = false;
            txtYear.ReadOnly = false;

            btnStudentEdit.Visible = false;
            btnStudentUpdate.Visible = true;
            btnStudentDelete.Visible = true;

        }
        //update students button
        private void btnStudentUpdate_Click(object sender, EventArgs e)
        {
            clsStudents currentStudent = new clsStudents();

            #region Validate user input & assigning to Advisor properties

            if (int.TryParse(txtStudentID.Text, out int StudentID))
            {
                currentStudent.StudentID = StudentID;
            }
            else
            {
                messageBoxOK("Invalid Student ID.");
                txtAdvisorID.Focus();
                return;
            }





            currentStudent.StudentFName = txtStudentFName.Text;
            currentStudent.StudentLName = txtStudentLName.Text;




            if (int.TryParse(txtYear.Text, out int Year))
            {
                currentStudent.Year = Year;
            }
            else
            {
                messageBoxOK("Invalid Year.");
                txtYear.Focus();
                return;
            }




            #endregion



            if (updateStudent(currentStudent) == true)
            {

                populateStudentDictionary(ref dctStudents);
                refreshStudentsListview();



                messageBoxOK("The student (ID: " + currentStudent.StudentID.ToString() + ") successfully updated.");
                studentInformation_Update_ClearTextboxes();



            }
            else
            {
                messageBoxOK("Update Failed for student (ID: " + currentStudent.StudentID.ToString() + ").");
            }
            ;

        }


        //delete student method
        private bool deleteStudent(clsStudents currentStudent)
        {
            string myConnectionString = clsDBUtil.getConnectionString();

            using (SqlConnection conn = new SqlConnection(myConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_DeleteStudent", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@StudentID", currentStudent.StudentID);
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








        // insert student
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



        //insert student button
        private void btnInsertStudentInfo_Click_1(object sender, EventArgs e)
        {

            clsStudents currentStudent = new clsStudents();



            currentStudent.StudentFName = txtStudentFNameInsert.Text;
            currentStudent.StudentLName = txtStudentLNameInsert.Text;






            if (int.TryParse(txtYearInsert.Text, out int year) && year >= 0 && year <= 4)
            {
                currentStudent.Year = year;
            }
            else
            {
                messageBoxOK("Invalid Year. Please enter a number between 0 and 4.");
                txtYear.Focus();
                return;
            }




            if (InsertStudent(currentStudent) == true)
            {

                populateStudentDictionary(ref dctStudents);
                refreshStudentsListview();



                messageBoxOK("The student (ID: " + currentStudent.StudentID.ToString() + ") successfully updated.");
                studentInformation_Update_ClearTextboxes();



            }
            else
            {
                messageBoxOK("Update Failed for student (ID: " + currentStudent.StudentID.ToString() + ").");
            }
            ;


        }
        //delete student button
        private void btnStudentDelete_Click(object sender, EventArgs e)
        {

            clsStudents currentStudent = new clsStudents();



            if (int.TryParse(txtStudentID.Text, out int StudentID))
            {
                currentStudent.StudentID = StudentID;
            }
            else
            {
                messageBoxOK("Invalid Student ID.");
                txtStudentID.Focus();
                return;
            }

            currentStudent.StudentFName = txtStudentFName.Text;
            currentStudent.StudentLName = txtStudentLName.Text;


            if (int.TryParse(txtYear.Text, out int Year))
            {
                currentStudent.Year = Year;
            }
            else
            {
                messageBoxOK("Invalid Year.");
                txtYear.Focus();
                return;
            }
            //currentStudent.Year = txtYear.Text;

            // currentStudent.AdvisorEmail = txtAdvisorEmail.Text;




            if (deleteStudent(currentStudent) == true)
            {

                populateStudentDictionary(ref dctStudents);
                refreshStudentsListview();



                messageBoxOK("The advisor (ID: " + currentStudent.StudentID.ToString() + ") successfully deleted.");
                studentInformation_Update_ClearTextboxes();



            }
            else
            {
                messageBoxOK("delete Failed for student (ID: " + currentStudent.StudentID.ToString() + ").");
            }
            ;
        }





        //Availability*****************************************************************************************************





        // create availability dictionary
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




                            //took this away and things started working lmfao
                            /*
                            if (currentAvailability.IsTaken = (bool)rdr["IsTaken"])
                            {
                                chkIsTaken.Checked = rdr.GetBoolean(0);

                            }
                            else
                            {
                                chkIsTaken.Checked = false;
                            }
                            */



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


        //updateAvailability method
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


        //refresh the listview for availability
        private void refreshAvailabilityListview()
        {
            // REMEMBER: the View property of the listview must be set to 'List'
            lvwAvailability.Clear();
            clsAvailability currentAvailability = new clsAvailability();
            foreach (KeyValuePair<int, clsAvailability> kvp in dctAvailability)
            {
                currentAvailability = kvp.Value;
                ListViewItem item = new ListViewItem(Convert.ToString(currentAvailability.AvailabilityID));//currentAvailability.AvailabilityID);
                item.Tag = currentAvailability;
                lvwAvailability.Items.Add(item);
            }
        }



        //bring sql data to front end
        private void displayAvailabilityInformation_update(clsAvailability currentAvailability)
        {
            txtAvailabilityID.Text = currentAvailability.AvailabilityID.ToString();
            txtAvailAdvisorID.Text = currentAvailability.AdvisorID.ToString();
            txtDate.Text = currentAvailability.Date.ToString("MM/dd/yyyy");
            txtTimeID.Text = currentAvailability.TimeID.ToString();
            txtLocationID.Text = currentAvailability.LocationID.ToString();

            //IsTaken

            //first instance of the checkbox behaving
            chkIsTaken.Checked = currentAvailability.IsTaken;

        }
        //clear textboxes for availability
        private void availabilityInformation_Update_ClearTextboxes()
        {
            txtAvailabilityID.Clear();
            txtAvailabilityID.ReadOnly = true;

            txtAvailAdvisorID.Clear();
            txtAvailAdvisorID.ReadOnly = true;

            txtDate.Clear();
            txtDate.ReadOnly = true;

            txtTimeID.Clear();
            txtTimeID.ReadOnly = true;

            txtLocationID.Clear();
            txtLocationID.ReadOnly = true;

            //checkbox

            chkIsTaken.Checked = false;
            chkIsTaken.Visible = false;




            txtAvailIDInsert.Clear();


            txtAvailAdvisorIDInsert.Clear();


            txtDateInsert.Clear();


            txtTimeIDInsert.Clear();


            txtLocationIDInsert.Clear();

            //checkbox
            chkIsTakenInsert.Visible = false;
            chkIsTakenInsert.Checked = false;



            /*
            txtWeight.Clear();
            txtWeight.ReadOnly = true;
            */

            btnEditAvailability.Visible = true;
            btnUpdateAvailability.Visible = false;
            btnDeleteAvailability.Visible = false;
            //btnInsertAdvisorInfo.Visible = true;


        }






        //delete method for availability
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



        //insert method for availability
        


        //insert availability button
        private void btnAvailabilityInsert_Click_1(object sender, EventArgs e)
        {
            clsAvailability currentAvailability = new clsAvailability();



            if (int.TryParse(txtAvailAdvisorIDInsert.Text, out int AdvisorID))
            {
                currentAvailability.AdvisorID = AdvisorID;
            }
            else
            {
                messageBoxOK("Invalid AdvisorID.");
                txtAvailAdvisorID.Focus();
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
                txtDate.Focus();
                return;
            }

            /*
            if (int.TryParse(txtTimeIDInsert.Text, out int TimeID))
            {
                currentAvailability.TimeID = TimeID;
            }
            else
            {
                messageBoxOK("Invalid TimeID .");
                txtTimeID.Focus();
                return;
            }
            */


            
            if (cboTimeBrowse.SelectedItem is ComboBoxItem selectedItem &&
            selectedItem.Value is clsTimes selectedTime)
            {
                currentAvailability.TimeID = selectedTime.TimeID;
            }
            else
            {
                messageBoxOK("Please select a valid time slot.");
                cboTimeBrowse.Focus();
                return;
            }


            
            if (int.TryParse(txtLocationIDInsert.Text, out int LocationID))
            {
                currentAvailability.LocationID = LocationID;
            }
            else
            {
                messageBoxOK("Invalid locationID .");
                txtLocationID.Focus();
                return;
            }


            if (chkIsTakenInsert.Checked)
            {



                currentAvailability.IsTaken = chkIsTaken.Checked;

            }
            else
            {
                //messageBoxOK("ISTAKEN ERROR REFER BACK TO INSERT CLICK EVENT.");
                chkIsTakenInsert.Focus();
            }



            if (InsertAvailability(currentAvailability) == true)
            {

                populateAvailabilityDictionary(ref dctAvailability);
                refreshAvailabilityListview();



                messageBoxOK("The Availability (ID: " + currentAvailability.AvailabilityID.ToString() + ") successfully updated.");
                availabilityInformation_Update_ClearTextboxes();



            }
            else
            {
                messageBoxOK("Update Failed for Availability (ID: " + currentAvailability.AvailabilityID.ToString() + ").");
            }
            ;
        }


        //edit button for availability
        private void btnEditAvailability_Click_1(object sender, EventArgs e)
        {
            if (txtAvailabilityID.Text == "")
            {
                return;
            }

            txtAvailAdvisorID.ReadOnly = false;
            txtDate.ReadOnly = false;
            txtTimeID.ReadOnly = false;
            txtLocationID.ReadOnly = false;
            //turn off is taken
            chkIsTaken.Visible = false;

            btnEditAvailability.Visible = false;
            btnUpdateAvailability.Visible = true;
            btnDeleteAvailability.Visible = true;
        }
        //update button for availability
        private void btnUpdateAvailability_Click_1(object sender, EventArgs e)
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

            if (int.TryParse(txtAvailAdvisorID.Text, out int AdvisorID))
            {
                currentAvailability.AdvisorID = AdvisorID;
            }
            else
            {
                messageBoxOK("Invalid AdvisorID.");
                txtAvailAdvisorID.Focus();
                return;
            }

            //currentAvailability.Date = txtDate.Text;


            if (DateTime.TryParse(txtDate.Text, out DateTime parsedDate))
            {
                currentAvailability.Date = parsedDate;
            }
            else
            {
                messageBoxOK("Invalid Date.");
                txtDate.Focus();
                return;
            }


            if (int.TryParse(txtTimeID.Text, out int TimeID))
            {
                currentAvailability.TimeID = TimeID;
            }
            else
            {
                messageBoxOK("Invalid TimeID ID.");
                txtTimeID.Focus();
                return;
            }

            if (int.TryParse(txtLocationID.Text, out int LocationID))
            {
                currentAvailability.LocationID = LocationID;
            }
            else
            {
                messageBoxOK("Invalid TimeID ID.");
                txtLocationID.Focus();
                return;
            }

            //checkbox

            if (chkIsTaken.Checked)
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
                refreshAvailabilityListview();



                messageBoxOK("The Availability (ID: " + currentAvailability.AvailabilityID.ToString() + ") successfully updated.");
                availabilityInformation_Update_ClearTextboxes();



            }
            else
            {
                messageBoxOK("Update Failed for Availability (ID: " + currentAvailability.AvailabilityID.ToString() + ").");
            }
            ;
        }
        //delete availability button
        private void btnDeleteAvailability_Click_1(object sender, EventArgs e)
        {
            clsAvailability currentAvailability = new clsAvailability();




            if (int.TryParse(txtAvailabilityID.Text, out int AvailabilityID))
            {
                currentAvailability.AvailabilityID = AvailabilityID;
            }
            else
            {
                messageBoxOK("Invalid AvailabilityID.");
                txtAvailabilityID.Focus();
                return;
            }


            if (int.TryParse(txtAvailAdvisorID.Text, out int AdvisorID))
            {
                currentAvailability.AdvisorID = AdvisorID;
            }
            else
            {
                messageBoxOK("Invalid AdvisorID.");
                txtAvailAdvisorID.Focus();
                return;
            }

            //currentAvailability.Date = txtDate.Text;


            if (DateTime.TryParse(txtDate.Text, out DateTime parsedDate))
            {
                currentAvailability.Date = parsedDate;
            }
            else
            {
                messageBoxOK("Invalid Date.");
                txtDate.Focus();
                return;
            }


            if (int.TryParse(txtTimeID.Text, out int TimeID))
            {
                currentAvailability.TimeID = TimeID;
            }
            else
            {
                messageBoxOK("Invalid TimeID ID.");
                txtTimeID.Focus();
                return;
            }

            if (int.TryParse(txtLocationID.Text, out int LocationID))
            {
                currentAvailability.LocationID = LocationID;
            }
            else
            {
                messageBoxOK("Invalid LocationID ID.");
                txtLocationID.Focus();
                return;
            }


            //checkbox
            if (chkIsTaken.Checked)
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
                refreshAvailabilityListview();



                messageBoxOK("The Availability (ID: " + currentAvailability.AvailabilityID.ToString() + ") successfully deleted.");
                availabilityInformation_Update_ClearTextboxes();



            }
            else
            {
                messageBoxOK("delete Failed for Availability (ID: " + currentAvailability.AvailabilityID.ToString() + ").");
            }
            ;
        }

        //update availability listview
        private void lvwAvailability_Update_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListView.SelectedListViewItemCollection itemIsSelected = lvwAvailability.SelectedItems;
            foreach (ListViewItem item in itemIsSelected)
            {
                clsAvailability currentAvailability = (clsAvailability)item.Tag;
                displayAvailabilityInformation_update(currentAvailability);
            }
        }

        //checkbox
        private void chkIsTaken_CheckedChanged(object sender, EventArgs e)
        {

        }




        //MEETINGS******************************************************************************************************


        #region obselete listview code


        //refreshers for previously used list views this is now obselete
        /*
        private void refreshBrowseAdvisorsListview()
        {
            lvwBrowseAdvisors.Clear();
            foreach (var currentAdvisor in dctAdvisors.Values)
            {
                ListViewItem item = new ListViewItem(currentAdvisor.AdvisorLName + ", " + currentAdvisor.AdvisorFName);
                item.Tag = currentAdvisor;
                lvwBrowseAdvisors.Items.Add(item);
            }
        }

        private void refreshStudentsBrowseListview()
        {
            lvwBrowseStudents.Clear();
            foreach (var currentStudent in dctStudents.Values)
            {
                ListViewItem item = new ListViewItem(currentStudent.StudentLName + ", " + currentStudent.StudentFName);
                item.Tag = currentStudent;
                lvwBrowseStudents.Items.Add(item);
            }
        }

        private void refreshAvailabilityBrowseListview()
        {
            lvwBrowseAvailability.Clear();
            foreach (var currentAvailability in dctAvailability.Values)
            {
                ListViewItem item = new ListViewItem(
                    $"ID: {currentAvailability.AvailabilityID}, AdvisorID: {currentAvailability.AdvisorID}, Date: {currentAvailability.Date:d}"
                );
                item.Tag = currentAvailability;
                lvwBrowseAvailability.Items.Add(item);
            }
        }

        */

        #endregion

        //combobox class
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


        private void cboStudentsBrowse_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboStudentsBrowse.SelectedItem is ComboBoxItem selectedItem)
            {
                clsStudents currentstudent = (clsStudents)selectedItem.Value;
                txtStudentLName.Text = currentstudent.StudentLName;
            }
        }


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
        
        /*
        private void cboAvailabilityBrowse_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboAvailabilityBrowse.SelectedItem is ComboBoxItem selectedItem)
            {
                clsAvailability currentAvailability = (clsAvailability)selectedItem.Value;
                txtAvailabilityID.Text = currentAvailability.AvailabilityID.ToString();

                // Populate time fields
                if (dctTimes.TryGetValue(currentAvailability.TimeID, out clsTimes selectedTime))
                {
                    txtStartTime.Text = selectedTime.StartTime.ToString(@"hh\:mm");
                    txtEndTime.Text = selectedTime.EndTime.ToString(@"hh\:mm");
                }
                else
                {
                    txtStartTime.Text = "N/A";
                    txtEndTime.Text = "N/A";
                }
            }
        }

        */

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

            txtStartTime.Visible = true;
            txtEndTime.Visible = true;


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




            if (cboStudentsBrowse.SelectedItem is ComboBoxItem selectedItem &&
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


            if (cboAvailabilityBrowse.SelectedItem is ComboBoxItem selectedAvailabilityItem &&
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


        //TIMES***********************************************************************************************************************







        
        private void populateTimesDictionary(ref Dictionary<int, clsTimes> dctTimes)
        {

            string myConnectionString = clsDBUtil.getConnectionString();

            using (SqlConnection conn = new SqlConnection(myConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_GetTime", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        dctTimes.Clear();

                        while (rdr.Read() == true)
                        {
                            clsTimes currentTime = new clsTimes();
                            currentTime.TimeID = (int)rdr["TimeID"];
                            currentTime.StartTime = (TimeSpan)rdr["StartTime"];
                            currentTime.EndTime = (TimeSpan)rdr["EndTime"];
                            



                            dctTimes.Add(currentTime.TimeID, currentTime);
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

        //refresh the listview for Times
        private void refreshTimesListview()
        {
            // REMEMBER: the View property of the listview must be set to 'List'
            lvwTimes.Clear();
            clsTimes currentTime = new clsTimes();
            foreach (KeyValuePair<int, clsTimes> kvp in dctTimes)
            {
                currentTime = kvp.Value;
                ListViewItem item = new ListViewItem(currentTime.TimeID.ToString());
                item.Tag = currentTime;
                lvwTimes.Items.Add(item);
            }
        }

        //pull sql info to font end
        
        private void displayTimeInformation_update(clsTimes currentTime)
        {
            txtTimeID2.Text = currentTime.TimeID.ToString();
            txtStartTimeOG.Text = currentTime.StartTime.ToString();
            txtEndTimeOG.Text = currentTime.EndTime.ToString();

        }
        


        //clear meeting textboxes
        private void timeInformation_Update_ClearTextboxes()
        {
            txtTimeID.Clear();
            txtTimeID.ReadOnly = true;

            txtStartTimeOG.Clear();
            txtStartTimeOG.ReadOnly = true;

            txtEndTimeOG.Clear();
            txtEndTimeOG.ReadOnly = true;

            txtTimeID2.Visible = false;
            txtEndTimeOG.Visible = false;
            txtStartTimeOG.Visible = false;

            lvwTimes.Visible = false;


        }

        private void lvwTimes_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListView.SelectedListViewItemCollection itemIsSelected = lvwTimes.SelectedItems;
            foreach (ListViewItem item in itemIsSelected)
            {
                clsTimes currentTime = (clsTimes)item.Tag;
                displayTimeInformation_update(currentTime);
            }
        }



        
        private void populateTimeComboBox()
        {
            cboTimeBrowse.Items.Clear();
            foreach (var currentTime in dctTimes.Values)
            {
                cboTimeBrowse.Items.Add(new ComboBoxItem(currentTime.StartTime + " - " + currentTime.EndTime, currentTime));
            }
        }

        //time combobox

        private void cboTimeBrowse_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (cboTimeBrowse.SelectedItem is ComboBoxItem selectedItem)
            {
                clsTimes currentTime = (clsTimes)selectedItem.Value;
                txtTimeIDInsert.Text = currentTime.TimeID.ToString();
            }
        }
    }
}