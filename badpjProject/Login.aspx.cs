using System;
using System.Data.SqlClient;
using System.Configuration;

namespace badpjProject
{
    public partial class Login : System.Web.UI.Page
    {
        

        protected void Button1_Click1(object sender, EventArgs e)
        {
            string username = TextBox1.Text.Trim();
            string password = TextBox2.Text.Trim();

            // Connection string from Web.config
            string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // SQL query to check login credentials
                    string query = "SELECT COUNT(*) FROM [Table] WHERE Login_Name = @Login_Name AND Password = @Password";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Login_Name", username);
                        cmd.Parameters.AddWithValue("@Password", password);

                        int count = Convert.ToInt32(cmd.ExecuteScalar());

                        if (count > 0)
                        {
                            // Successful login
                            Response.Redirect("About.aspx"); // Redirect to homepage or another page
                        }
                        else
                        {
                            // Invalid login
                            Response.Write("<script>alert('Invalid Username or Password');</script>");
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle any errors
                    Response.Write($"<script>alert('Error: {ex.Message}');</script>");
                }
            }
        }
    }
}
