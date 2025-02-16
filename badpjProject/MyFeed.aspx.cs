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
            string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;
            string query = @"
    SELECT DISTINCT p.PostID, p.Content, p.CreatedAt, p.Likes, p.ThreadID
    FROM Posts p
    WHERE p.CreatedBy = @UserID AND p.IsDeleted = 0";  // Assuming you only want non-deleted posts

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserID", Session["UserID"]);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    // Assuming you're getting the ThreadID from the first post, as a global value
                    GlobalThreadId = dt.Rows[0]["ThreadID"].ToString();  // Store the ThreadID from the first row

                    gvPosts.DataSource = dt;
                    gvPosts.DataBind();
                }
                else
                {
                    gvPosts.DataSource = null;
                    gvPosts.DataBind();
                }
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
            if (e.CommandName == "DeletePost")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                string postId = gvPosts.DataKeys[rowIndex].Value.ToString();
                DeletePost(postId);
                LoadPosts();
            }
            else if (e.CommandName == "EditPost")
            {
                string threadId = Request.QueryString["ThreadID"];
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                string postId = gvPosts.DataKeys[rowIndex].Value.ToString();
                Response.Redirect($"UpdatePost.aspx?PostID={postId}&ThreadID={threadId}");
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