using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Web.UI.WebControls;

namespace badpjProject
{
    public partial class StaffPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["Role"]?.ToString() != "Staff")
                {
                    Response.Redirect("Login.aspx");
                }

                LoadStaffAccounts();
            }
        }

        private void LoadStaffAccounts()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT Id, Login_Name FROM [Table] WHERE Role = 'Staff'";
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

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                Response.Write("<script>alert('Please provide a username and password.');</script>");
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
                            Response.Write("<script>alert('Username already exists.');</script>");
                            return;
                        }
                    }

                    // Add new staff account
                    string insertQuery = "INSERT INTO [Table] (Login_Name, Password, Role) VALUES (@Login_Name, @Password, 'Staff')";
                    using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@Login_Name", username);
                        cmd.Parameters.AddWithValue("@Password", password);
                        cmd.ExecuteNonQuery();
                    }

                    Response.Write("<script>alert('Staff account created successfully!');</script>");
                    LoadStaffAccounts(); // Refresh the GridView
                }
                catch (Exception ex)
                {
                    Response.Write($"<script>alert('Error: {ex.Message}');</script>");
                }
            }
        }

        protected void StaffGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int staffId = Convert.ToInt32(StaffGridView.DataKeys[e.RowIndex].Value);

            string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // Delete staff account
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
