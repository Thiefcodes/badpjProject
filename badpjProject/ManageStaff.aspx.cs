using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Web.UI.WebControls;

namespace badpjProject
{
    public partial class ManageStaff : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadStaffAccounts();
            }
        }

        private void LoadStaffAccounts()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT Id, Login_Name, Email FROM [Table] WHERE Role = 'Staff'";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        StaffGridView.DataSource = dt;
                        StaffGridView.DataBind();
                    }
                }
            }
        }
        protected void AddStaffButton_Click(object sender, EventArgs e)
        {
            string username = StaffUsernameTextBox.Text.Trim();
            string password = StaffPasswordTextBox.Text.Trim();
            string email = StaffEmailTextBox.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(email))
            {
                Response.Write("<script>alert('Please provide all required fields: username, password, and email.');</script>");
                return;
            }

            string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // Check if username already exists
                    string checkQuery = "SELECT COUNT(*) FROM [Table] WHERE Login_Name = @Login_Name";
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@Login_Name", username);
                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                        if (count > 0)
                        {
                            Response.Write("<script>alert('Username already exists. Please choose another.');</script>");
                            return;
                        }
                    }

                    // Determine the next available Id using MAX(Id)
                    int nextId = 1;
                    string maxIdQuery = "SELECT ISNULL(MAX(Id), 0) + 1 FROM [Table]";
                    using (SqlCommand maxIdCmd = new SqlCommand(maxIdQuery, conn))
                    {
                        nextId = Convert.ToInt32(maxIdCmd.ExecuteScalar());
                    }

                    // Insert new staff account into the database
                    string insertQuery = "INSERT INTO [Table] (Id, Login_Name, Password, Email, Role) VALUES (@Id, @Login_Name, @Password, @Email, 'Staff')";
                    using (SqlCommand insertCmd = new SqlCommand(insertQuery, conn))
                    {
                        insertCmd.Parameters.AddWithValue("@Id", nextId);
                        insertCmd.Parameters.AddWithValue("@Login_Name", username);
                        insertCmd.Parameters.AddWithValue("@Password", password);
                        insertCmd.Parameters.AddWithValue("@Email", email);

                        insertCmd.ExecuteNonQuery();
                    }

                    // Success message
                    Response.Write("<script>alert('Staff account created successfully!');</script>");
                    LoadStaffAccounts(); // Refresh the GridView to include the new staff account
                }
                catch (Exception ex)
                {
                    Response.Write($"<script>alert('Database error: {ex.Message}');</script>");
                }
            }
        }





        protected void StaffGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Edit")
            {
                // Redirect to the Edit page with the selected staff ID
                int staffId = Convert.ToInt32(e.CommandArgument);
                Response.Redirect($"EditStaff.aspx?Id={staffId}");
            }
            else if (e.CommandName == "DeleteStaff")
            {
                // Delete the selected staff account
                int staffId = Convert.ToInt32(e.CommandArgument);
                DeleteStaffAccount(staffId);
            }
        }

        protected void StaffGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // Suppress the default RowDeleting event
        }

        private void DeleteStaffAccount(int staffId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    string deleteQuery = "DELETE FROM [Table] WHERE Id = @Id AND Role = 'Staff'";
                    using (SqlCommand cmd = new SqlCommand(deleteQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", staffId);
                        cmd.ExecuteNonQuery();
                    }

                    Response.Write("<script>alert('Staff account deleted successfully!');</script>");
                    LoadStaffAccounts(); // Refresh the GridView
                }
                catch (Exception ex)
                {
                    Response.Write($"<script>alert('Error: {ex.Message}');</script>");
                }
            }
        }



    }
}
