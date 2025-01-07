using System;
using System.Data.SqlClient;
using System.Configuration;

namespace badpjProject
{
    public partial class ResetPassword : System.Web.UI.Page
    {
        protected void ResetPasswordButton_Click(object sender, EventArgs e)
        {
            string enteredOtp = OTPTextBox.Text.Trim();
            string newPassword = NewPasswordTextBox.Text.Trim();

            if (enteredOtp == Session["OTP"]?.ToString())
            {
                string loginName = Session["LoginName"]?.ToString();
                string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "UPDATE [Table] SET Password = @Password WHERE Login_Name = @Login_Name";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Password", newPassword);
                        cmd.Parameters.AddWithValue("@Login_Name", loginName);
                        cmd.ExecuteNonQuery();

                        Response.Write("<script>alert('Password reset successfully!');</script>");
                        Response.Redirect("Login.aspx");
                    }
                }
            }
            else
            {
                Response.Write("<script>alert('Invalid OTP.');</script>");
            }
        }
    }
}
