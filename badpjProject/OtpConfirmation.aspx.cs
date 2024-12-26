using System.Configuration;
using System.Data.SqlClient;
using System;

namespace badpjProject
{
    public partial class OtpConfirmation : System.Web.UI.Page
    {
        protected void ButtonVerify_Click(object sender, EventArgs e)
        {
            string enteredOtp = TextBoxOtp.Text.Trim();
            string storedOtp = Session["OTP"]?.ToString();

            if (enteredOtp == storedOtp) // Compare the entered OTP with the session OTP
            {
                string username = Session["Username"]?.ToString();
                string password = Session["Password"]?.ToString();
                string email = Session["Email"]?.ToString();

                string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();

                        // Determine the next Id value by counting rows
                        int nextId = 1; // Default Id if no rows exist
                        string countQuery = "SELECT COUNT(*) FROM [Table]";
                        using (SqlCommand countCmd = new SqlCommand(countQuery, conn))
                        {
                            int rowCount = Convert.ToInt32(countCmd.ExecuteScalar());
                            nextId = rowCount + 1;
                        }

                        // Insert new user into the database
                        string query = "INSERT INTO [Table] (Id, Login_Name, Password) VALUES (@Id, @Login_Name, @Password)";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@Id", nextId);
                            cmd.Parameters.AddWithValue("@Login_Name", username);
                            cmd.Parameters.AddWithValue("@Password", password);

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
            else
            {
                Response.Write("<script>alert('Invalid OTP. Please try again.');</script>");
            }
        }
    }
}
