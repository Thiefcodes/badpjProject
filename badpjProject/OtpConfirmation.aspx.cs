using System;
using System.Configuration;
using System.Data.SqlClient;

namespace badpjProject
{
    public partial class OtpConfirmation : System.Web.UI.Page
    {
        protected void ButtonVerify_Click(object sender, EventArgs e)
        {
            string enteredOtp = TextBoxOtp.Text.Trim();
            string storedOtp = Session["OTP"]?.ToString();

            if (enteredOtp == storedOtp) // Compare entered OTP with the stored session OTP
            {
                // Retrieve user details from the session
                string username = Session["Username"]?.ToString();
                string password = Session["Password"]?.ToString();
                string email = Session["Email"]?.ToString();

                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(email))
                {
                    Response.Write("<script>alert('Session data is missing. Please try signing up again.');</script>");
                    Response.Redirect("SignUp.aspx");
                    return;
                }

                string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();

                        // Determine the next available Id using MAX(Id)
                        int nextId = 1;
                        string maxIdQuery = "SELECT ISNULL(MAX(Id), 0) + 1 FROM [Table]";
                        using (SqlCommand maxIdCmd = new SqlCommand(maxIdQuery, conn))
                        {
                            nextId = Convert.ToInt32(maxIdCmd.ExecuteScalar());
                        }

                        // Insert new user into the database
                        string insertQuery = "INSERT INTO [Table] (Id, Login_Name, Password, Email, Role) VALUES (@Id, @Login_Name, @Password, @Email, 'User')";
                        using (SqlCommand insertCmd = new SqlCommand(insertQuery, conn))
                        {
                            insertCmd.Parameters.AddWithValue("@Id", nextId);
                            insertCmd.Parameters.AddWithValue("@Login_Name", username);
                            insertCmd.Parameters.AddWithValue("@Password", password);
                            insertCmd.Parameters.AddWithValue("@Email", email);

                            insertCmd.ExecuteNonQuery();
                        }

                        // Account creation success
                        Response.Write("<script>alert('Account created successfully!');</script>");
                        Response.Redirect("Login.aspx");
                    }
                    catch (Exception ex)
                    {
                        Response.Write($"<script>alert('Database error: {ex.Message}');</script>");
                    }
                }
            }
            else
            {
                // Invalid OTP case
                Response.Write("<script>alert('Invalid OTP. Please try again.');</script>");
            }
        }
    }
}
