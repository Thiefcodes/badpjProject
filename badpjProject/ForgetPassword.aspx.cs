using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Net.Mail;
using System.Net;

namespace badpjProject
{
    public partial class ForgetPassword : System.Web.UI.Page
    {
        protected void SendOTPButton_Click(object sender, EventArgs e)
        {
            string loginName = LoginNameTextBox.Text.Trim();
            string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // Retrieve email for the given login name
                    string query = "SELECT Email FROM [Table] WHERE Login_Name = @Login_Name";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Login_Name", loginName);

                        object result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            string email = result.ToString();
                            string otpCode = GenerateOTP();

                            // Send OTP to the email
                            if (SendOTP(email, otpCode))
                            {
                                // Store OTP and Login_Name in Session for verification
                                Session["OTP"] = otpCode;
                                Session["LoginName"] = loginName;
                                Response.Redirect("ResetPassword.aspx");
                            }
                            else
                            {
                                Response.Write("<script>alert('Failed to send OTP. Please try again.');</script>");
                            }
                        }
                        else
                        {
                            Response.Write("<script>alert('Username not found.');</script>");
                        }
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
                mail.Subject = "Password Reset OTP";
                mail.Body = $"Your OTP code is: {otp}";

                smtpServer.Port = 587;
                smtpServer.Credentials = new NetworkCredential("ruihernh@gmail.com", "yqqh pwcr byeq sseo"); // Replace with your credentials
                smtpServer.EnableSsl = true;

                smtpServer.Send(mail);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
