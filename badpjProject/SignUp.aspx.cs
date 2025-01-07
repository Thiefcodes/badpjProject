using System.Configuration;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Net;
using System;

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

                    // Generate OTP
                    string otpCode = GenerateOTP();
                    Session["OTP"] = otpCode; // Store OTP in session
                    Session["Username"] = username;
                    Session["Password"] = password;
                    Session["Email"] = email;

                    // Send OTP to email
                    if (SendOTP(email, otpCode))
                    {
                        Response.Redirect("OtpConfirmation.aspx");
                    }
                    else
                    {
                        Response.Write("<script>alert('Failed to send OTP. Please try again.');</script>");
                    }
                }
                catch (Exception ex)
                {
                    Response.Write($"<script>alert('Error: {ex.Message}');</script>");
                }
            }
        }

        private string GenerateOTP()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        private bool SendOTP(string email, string otp)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("ruihernh@gmail.com"); // Replace with your email
                mail.To.Add(email);
                mail.Subject = "Your OTP Code";
                mail.Body = $"Your OTP code is: {otp}";

                smtpServer.Port = 587; // Use 465 for SSL
                smtpServer.Credentials = new NetworkCredential("ruihernh@gmail.com", "yqqh pwcr byeq sseo"); // Replace with your Gmail password or App Password
                smtpServer.EnableSsl = true;

                smtpServer.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                // Log the error to debug the issue
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

    }
}