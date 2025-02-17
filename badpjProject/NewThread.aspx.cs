using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;

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

            string imagePath = null;

            if (fuImage.HasFile)
            {
                string fileName = Path.GetFileNameWithoutExtension(fuImage.FileName);
                string fileExtension = Path.GetExtension(fuImage.FileName);

                // Ensure filename is unique (append timestamp)
                string uniqueFileName = fileName + "_" + DateTime.Now.Ticks + fileExtension;

                string folderPath = Server.MapPath("~/Uploads/Threads/");
                Directory.CreateDirectory(folderPath); // Ensure folder exists

                string filePath = Path.Combine(folderPath, uniqueFileName);
                fuImage.SaveAs(filePath);

                imagePath = "Uploads/Threads/" + uniqueFileName; // Relative path for DB
            }

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

                // **FIXED:** Added ImagePath in INSERT statement
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO Threads (ThreadID, Title, CreatedBy, CreatedAt, IsDeleted, ImagePath) " +
                    "VALUES (@ThreadID, @Title, @CreatedBy, @CreatedAt, @IsDeleted, @ImagePath)", conn);
                cmd.Parameters.AddWithValue("@ThreadID", newThreadId);
                cmd.Parameters.AddWithValue("@Title", txtTitle.Text);
                cmd.Parameters.AddWithValue("@CreatedBy", Session["UserId"].ToString()); // Use Session["UserId"]
                cmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now);
                cmd.Parameters.AddWithValue("@IsDeleted", false); // Default value for IsDeleted
                cmd.Parameters.AddWithValue("@ImagePath", (object)imagePath ?? DBNull.Value); // Allow null values

                cmd.ExecuteNonQuery();
            }

            // Redirect to the forum page
            Response.Redirect("Forum.aspx");
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            string threadId = Request.QueryString["ThreadID"];
            Response.Redirect($"Forum.aspx?ThreadID={threadId}");
        }

    }
}