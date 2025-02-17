using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace badpjProject
{
    public partial class UpdateThread : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string threadId = Request.QueryString["ThreadID"];
                string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT Title FROM Threads WHERE ThreadID = @ThreadID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ThreadID", threadId);

                    conn.Open();
                    object title = cmd.ExecuteScalar();
                    conn.Close();

                    if (title != null)
                    {
                        txtTitle.Text = title.ToString();
                    }
                }
            }

        }
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            string threadId = Request.QueryString["ThreadID"];
            string newTitle = txtTitle.Text.Trim();
            string validationMessage = ValidationHelper.ValidateContent(newTitle);

            if (validationMessage != null)
            {
                lblMessage.Text = validationMessage;
                return;
            }

            string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;
            string imagePath = null; // Variable to store the new image path

            if (fuUpdateThreadImage.HasFile)
            {
                // Generate a unique filename to prevent conflicts
                string fileName = Path.GetFileNameWithoutExtension(fuUpdateThreadImage.FileName);
                string fileExtension = Path.GetExtension(fuUpdateThreadImage.FileName);
                string uniqueFileName = fileName + "_" + DateTime.Now.Ticks + fileExtension;

                // Define the upload folder (ensure this folder exists in your project)
                string folderPath = Server.MapPath("~/Uploads/Threads/");
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                // Save the uploaded file to the server
                string filePath = Path.Combine(folderPath, uniqueFileName);
                fuUpdateThreadImage.SaveAs(filePath);

                // Store the relative path in the database (not the absolute server path)
                imagePath = "/Uploads/Threads/" + uniqueFileName;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                // Update query: only update ImagePath if a new image was uploaded
                string query = "UPDATE Threads SET Title = @Title" +
                               (imagePath != null ? ", ImagePath = @ImagePath" : "") +
                               " WHERE ThreadID = @ThreadID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Title", newTitle);
                if (imagePath != null)
                {
                    cmd.Parameters.AddWithValue("@ImagePath", imagePath);
                }
                cmd.Parameters.AddWithValue("@ThreadID", threadId);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                Response.Write("<script>alert('Thread Updated Successfully'); window.location='Thread.aspx?ThreadID=" + threadId + "';</script>");
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            string threadId = Request.QueryString["ThreadID"];
            Response.Redirect($"Forum.aspx?ThreadID={threadId}");
        }
    }
}