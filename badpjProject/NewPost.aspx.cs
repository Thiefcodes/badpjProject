using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace badpjProject
{
    public partial class NewPost : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string threadId = Request.QueryString["ThreadID"];
            string content = txtContent.Text.Trim();
            int userId = Convert.ToInt32(Session["UserId"]); // Replace with actual logged-in user ID
            string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Retrieve the next available PostID
                SqlCommand getMaxIdCmd = new SqlCommand("SELECT ISNULL(MAX(PostID), 0) + 1 FROM Posts", conn);
                int newPostId = Convert.ToInt32(getMaxIdCmd.ExecuteScalar());

                // Insert the new post
                string query = "INSERT INTO Posts (PostID, ThreadID, Content, CreatedBy, CreatedAt, IsDeleted) " +
                               "VALUES (@PostID, @ThreadID, @Content, @CreatedBy, @CreatedAt, @IsDeleted)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@PostID", newPostId);
                cmd.Parameters.AddWithValue("@ThreadID", threadId);
                cmd.Parameters.AddWithValue("@Content", content);
                cmd.Parameters.AddWithValue("@CreatedBy", userId);
                cmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now);
                cmd.Parameters.AddWithValue("@IsDeleted", false); // Default value for IsDeleted

                cmd.ExecuteNonQuery();
                conn.Close();

                Response.Write("<script>alert('Post submitted successfully.'); window.location='Thread.aspx?ThreadID=" + threadId + "';</script>");
            }
        }


        protected void btnCancel_Click(object sender, EventArgs e)
        {
            string threadId = Request.QueryString["ThreadID"];
            Response.Redirect($"Thread.aspx?ThreadID={threadId}");
        }
    }
}