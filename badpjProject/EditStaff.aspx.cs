using System;
using System.Configuration;
using System.Data.SqlClient;

namespace badpjProject
{
    public partial class EditStaff : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int staffId = Convert.ToInt32(Request.QueryString["Id"]);
                LoadStaffDetails(staffId);
            }
        }

        private void LoadStaffDetails(int staffId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT Login_Name, Email FROM [Table] WHERE Id = @Id AND Role = 'Staff'";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", staffId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            EditUsernameTextBox.Text = reader["Login_Name"].ToString();
                            EditEmailTextBox.Text = reader["Email"].ToString();
                        }
                    }
                }
            }
        }

        protected void UpdateStaffButton_Click(object sender, EventArgs e)
        {
            int staffId = Convert.ToInt32(Request.QueryString["Id"]);
            string username = EditUsernameTextBox.Text.Trim();
            string email = EditEmailTextBox.Text.Trim();
            string password = EditPasswordTextBox.Text.Trim();

            string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    string updateQuery = "UPDATE [Table] SET Login_Name = @Login_Name, Email = @Email, Password = @Password WHERE Id = @Id AND Role = 'Staff'";
                    using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", staffId);
                        cmd.Parameters.AddWithValue("@Login_Name", username);
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@Password", password);

                        cmd.ExecuteNonQuery();
                    }

                    Response.Write("<script>alert('Staff account updated successfully!');</script>");
                    Response.Redirect("ManageStaff.aspx");
                }
                catch (Exception ex)
                {
                    Response.Write($"<script>alert('Error: {ex.Message}');</script>");
                }
            }
        }
    }
}
