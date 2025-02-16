using System;
using System.Configuration;
using System.Data.SqlClient;

namespace badpjProject
{
    public partial class NewThread : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnCreate_Click(object sender, EventArgs e)
        {
            string content = txtTitle.Text.Trim();
            string validationMessage = ValidationHelper.ValidateContent(content);

            if (validationMessage != null) // If there is an error
            {
                lblMessage.Text = validationMessage;
                return;
            }
            string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Retrieve the next available ThreadID
                SqlCommand getMaxIdCmd = new SqlCommand("SELECT ISNULL(MAX(ThreadID), 0) + 1 FROM Threads", conn);
                int newThreadId = Convert.ToInt32(getMaxIdCmd.ExecuteScalar());

                // Insert the new thread
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO Threads (ThreadID, Title, CreatedBy, CreatedAt, IsDeleted) VALUES (@ThreadID, @Title, @CreatedBy, @CreatedAt, @IsDeleted)", conn);
                cmd.Parameters.AddWithValue("@ThreadID", newThreadId);
                cmd.Parameters.AddWithValue("@Title", txtTitle.Text);
                cmd.Parameters.AddWithValue("@CreatedBy", Session["UserId"].ToString()); // Use Session["UserId"]
                cmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now);
                cmd.Parameters.AddWithValue("@IsDeleted", false); // Default value for IsDeleted

                cmd.ExecuteNonQuery();
            }

            // Redirect to the forum page
            Response.Redirect("Forum.aspx");
        }

    }
}