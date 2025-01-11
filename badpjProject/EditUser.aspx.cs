using System;
using System.Configuration;
using System.Data.SqlClient;

namespace badpjProject
{
    public partial class EditUser : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int userId = Convert.ToInt32(Request.QueryString["Id"]);
                LoadUserDetails(userId);
            }
        }

        private void LoadUserDetails(int userId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT Login_Name, Email, Password FROM [Table] WHERE Id = @Id AND Role = 'User'";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", userId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            EditUsernameTextBox.Text = reader["Login_Name"].ToString();
                            EditEmailTextBox.Text = reader["Email"].ToString();
                            EditPasswordTextBox.Text = reader["Password"].ToString();
                        }
                    }
                }
            }
        }

        protected void UpdateUserButton_Click(object sender, EventArgs e)
        {
            int userId = Convert.ToInt32(Request.QueryString["Id"]);
            string username = EditUsernameTextBox.Text.Trim();
            string email = EditEmailTextBox.Text.Trim();
            string password = EditPasswordTextBox.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                Response.Write("<script>alert('Please provide all required fields.');</script>");
                return;
            }

            string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    string updateQuery = "UPDATE [Table] SET Login_Name = @Login_Name, Email = @Email, Password = @Password WHERE Id = @Id AND Role = 'User'";
                    using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", userId);
                        cmd.Parameters.AddWithValue("@Login_Name", username);
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@Password", password);

                        cmd.ExecuteNonQuery();
                    }

                    Response.Write("<script>alert('User account updated successfully!');</script>");
                    Response.Redirect("ManageUsers.aspx");
                }
                catch (Exception ex)
                {
                    Response.Write($"<script>alert('Error: {ex.Message}');</script>");
                }
            }
        }
    }
}
