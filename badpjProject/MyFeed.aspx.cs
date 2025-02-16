using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace badpjProject
{
	public partial class MyFeed : System.Web.UI.Page
	{
        public string GlobalThreadId { get; set; } // Global variable to store the ThreadID
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Ensure the user is logged in
                if (Session["UserID"] == null)
                {
                    Response.Redirect("Login.aspx"); // Redirect to login page if not logged in
                }

                // Load threads and posts created by the user
                LoadThreads();
                LoadPosts();
            }
        }


        // Load threads created by the logged-in user
        private void LoadThreads()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;
            string query = @"
            SELECT ThreadID, Title, CreatedAt, Views
            FROM Threads
            WHERE CreatedBy = @UserID
            ORDER BY CreatedAt DESC"; // Order by date created (latest first)

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserID", Session["UserID"]);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                gvThreads.DataSource = dt;
                gvThreads.DataBind();
            }
        }

        // Load posts created by the logged-in user
        private void LoadPosts()
        {
            string threadId = GlobalThreadId;
            int userId = Session["UserID"] != null ? Convert.ToInt32(Session["UserID"]) : 0;
            string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
            SELECT p.PostID, p.Content, COALESCE(u.Login_Name, 'Unknown') AS CreatedBy, p.CreatedAt, p.ThreadID, p.Likes
            FROM Posts p
            LEFT JOIN [Table] u ON p.CreatedBy = u.Id
            WHERE u.Id = @UserID;";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserID", userId);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    // Set GlobalThreadId to the first ThreadID from the results
                    GlobalThreadId = dt.Rows[0]["ThreadID"].ToString();
                }

                gvPosts.DataSource = dt;
                gvPosts.DataBind();
            }
        }
        protected void gvThreads_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            string threadId = gvThreads.Rows[index].Cells[0].Text;

            if (e.CommandName == "ViewThread")
            {
                IncrementThreadViews(threadId);
                Response.Redirect($"Thread.aspx?ThreadID={threadId}");
            }
            else if (e.CommandName == "UpdateThread")
            {
                Response.Redirect($"UpdateThread.aspx?ThreadID={threadId}");
            }
            else if (e.CommandName == "DeleteThread")
            {
                DeleteThread(threadId);
                LoadThreads();
            }
        }

        private void IncrementThreadViews(string threadId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "UPDATE Threads SET Views = Views + 1 WHERE ThreadID = @ThreadID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ThreadID", threadId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        protected void gvPosts_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            string postId = gvPosts.Rows[index].Cells[0].Text;
            if (e.CommandName == "DeletePost")
            {
                DeletePost(postId);
                LoadPosts();
            }
        }

        // Delete thread logic
        private void DeleteThread(string threadId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Threads WHERE ThreadID = @ThreadID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ThreadID", threadId);

                conn.Open();
                cmd.ExecuteNonQuery();
                Response.Write("<script>alert('Thread deleted successfully');</script>");

            }
        }

        // Delete post logic
        private void DeletePost(string postId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Posts WHERE PostID = @PostID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@PostID", postId);

                conn.Open();
                cmd.ExecuteNonQuery();
                Response.Write("<script>alert('Post deleted successfully');</script>");
            }
        }
    }
}
