using System;
using System.Data.SqlClient;
using System.Configuration;

namespace badpjProject
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Optionally add page load logic here
        }
        
        protected void btnFacialLogin_Click(object sender, EventArgs e)
        {
            Response.Redirect("FacialLogin.aspx");
        }

        protected void Button1_Click1(object sender, EventArgs e)
        {
            string username = TextBox1.Text.Trim();
            string password = TextBox2.Text.Trim();

            string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // Query to validate user login
                    string query = "SELECT Id, Role FROM [Table] WHERE Login_Name = @Login_Name AND Password = @Password";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Login_Name", username);
                        cmd.Parameters.AddWithValue("@Password", password);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int userId = Convert.ToInt32(reader["Id"]);
                                string role = reader["Role"].ToString();

                                // Store user information in Session
                                Session["UserId"] = userId;
                                Session["Username"] = username;
                                Session["Role"] = role;

                                // Redirect based on role
                                if (role == "Staff")
                                {
                                    Response.Redirect("StaffPage.aspx");
                                }
                                else
                                {
                                    Response.Redirect("UserPage.aspx");
                                }
                            }
                            else
                            {
                                Response.Write("<script>alert('Invalid username or password.');</script>");
                            }
                        }
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
