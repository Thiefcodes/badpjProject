using System;
using System.Data.SqlClient;
using System.Configuration;

namespace badpjProject
{
    public partial class SignUp : System.Web.UI.Page
    {
        protected void ButtonSignUp_Click(object sender, EventArgs e)
        {
            string username = TextBoxUsername.Text.Trim();
            string email = TextBoxEmail.Text.Trim();
            string password = TextBoxPassword.Text.Trim();
            string confirmPassword = TextBoxConfirmPassword.Text.Trim();

            if (password != confirmPassword)
            {
                Response.Write("<script>alert('Passwords do not match!');</script>");
                return;
            }

            string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // Check if username already exists
                    string query = "SELECT COUNT(*) FROM [Table] WHERE Login_Name = @Login_Name";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Login_Name", username);
                        int count = Convert.ToInt32(cmd.ExecuteScalar());

                        if (count > 0)
                        {
                            Response.Write("<script>alert('Username already exists!');</script>");
                            return;
                        }
                    }

                    // Insert new user with default role "User"
                    string insertQuery = "INSERT INTO [Table] (Login_Name, Password, Email) VALUES (@Login_Name, @Password, @Email)";
                    using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@Login_Name", username);
                        cmd.Parameters.AddWithValue("@Password", password);
                        cmd.Parameters.AddWithValue("@Email", email);

                        cmd.ExecuteNonQuery();
                        Response.Write("<script>alert('Account created successfully!');</script>");
                        Response.Redirect("Login.aspx");
                    }
                }
                catch (Exception ex)
                {
                    Response.Write($"<script>alert('Error: {ex.Message}');</script>");
                }
            }
        }
    }
}
