using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;

namespace badpjProject
{
	public partial class MyFeed : System.Web.UI.Page
	{
        public string GlobalThreadId { get; set; } // Global variable to store the ThreadID
        protected void Page_Load(object sender, EventArgs e)
        {
            string threadId = Request.QueryString["ThreadID"];
            if (!IsPostBack)
            {
                // Ensure the user is logged in
                if (Session["UserID"] == null)
                {
                    Response.Redirect("Login.aspx"); // Redirect to login page if not logged in
                }
                else if (!string.IsNullOrEmpty(threadId))
                {
                    LoadRandomPosts();
                }
                LoadUserThreadStats();
                LoadRandomThread();
                
                

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
            SELECT t.ThreadID, t.Title, t.CreatedAt, t.Views,
            COALESCE((SELECT COUNT(*) FROM Posts p WHERE p.ThreadID = t.ThreadID AND p.IsDeleted = 0), 0) AS PostCount,
            t.ImagePath
            FROM Threads t
            WHERE t.CreatedBy = @UserID
            ORDER BY t.CreatedAt DESC"; // Order by date created (latest first)

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
            SELECT 
            p.PostID, 
            p.Content, 
            COALESCE(u.Login_Name, 'Unknown') AS CreatedBy, 
            p.CreatedAt, 
            p.ThreadID,
            p.Likes, 
            CASE WHEN pl.UserID IS NOT NULL THEN 'Liked' ELSE 'Like' END AS LikeStatus
            FROM Posts p
            LEFT JOIN [Table] u ON p.CreatedBy = u.Id
            LEFT JOIN PostLikes pl ON p.PostID = pl.PostID AND pl.UserID = @UserID
            WHERE p.CreatedBy = @UserID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserID", userId);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                gvPosts.DataSource = dt;
                gvPosts.DataBind();
            }
        }

        protected void LoadUserThreadStats()
        {
            int userId = Session["UserID"] != null ? Convert.ToInt32(Session["UserID"]) : 0; // Ensure the user is logged in
            string query = @"
        SELECT 
            COALESCE(SUM(t.Views), 0) AS TotalViews,
            COALESCE(COUNT(pl.PostID), 0) AS TotalLikes
        FROM Threads t
        LEFT JOIN Posts p ON t.ThreadID = p.ThreadID
        LEFT JOIN PostLikes pl ON p.PostID = pl.PostID
        WHERE t.CreatedBy = @UserID;
    ";

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        lblTotalLikes.Text = reader["TotalLikes"].ToString();
                    }
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

        private void ToggleLike(int postId, int userId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Check if user already liked the post
                string checkQuery = "SELECT COUNT(*) FROM PostLikes WHERE PostID = @PostID AND UserID = @UserID";
                SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                checkCmd.Parameters.AddWithValue("@PostID", postId);
                checkCmd.Parameters.AddWithValue("@UserID", userId);
                int likeCount = (int)checkCmd.ExecuteScalar();

                if (likeCount > 0)
                {
                    // Unlike: Remove from PostLikes table and decrement Likes count
                    string unlikeQuery = "DELETE FROM PostLikes WHERE PostID = @PostID AND UserID = @UserID; " +
                                         "UPDATE Posts SET Likes = Likes - 1 WHERE PostID = @PostID";
                    SqlCommand unlikeCmd = new SqlCommand(unlikeQuery, conn);
                    unlikeCmd.Parameters.AddWithValue("@PostID", postId);
                    unlikeCmd.Parameters.AddWithValue("@UserID", userId);
                    unlikeCmd.ExecuteNonQuery();
                }
                else
                {
                    // Like: Insert into PostLikes table and increment Likes count
                    string likeQuery = "INSERT INTO PostLikes (PostID, UserID) VALUES (@PostID, @UserID); " +
                                       "UPDATE Posts SET Likes = Likes + 1 WHERE PostID = @PostID";
                    SqlCommand likeCmd = new SqlCommand(likeQuery, conn);
                    likeCmd.Parameters.AddWithValue("@PostID", postId);
                    likeCmd.Parameters.AddWithValue("@UserID", userId);
                    likeCmd.ExecuteNonQuery();
                }
            }

            // Refresh the posts to reflect the updated like status
            LoadRandomPosts();
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

        private void LoadRandomThread()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
              SELECT TOP 3 t.ThreadID, 
              t.Views, 
              t.Title, 
              COALESCE((SELECT COUNT(*) FROM Posts p WHERE p.ThreadID = t.ThreadID AND p.IsDeleted = 0), 0) AS PostCount,
              t.ImagePath
              FROM Threads t
              ORDER BY t.Views DESC, PostCount DESC;"; // Select one random thread

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                gvRandomThreads.DataSource = dt;
                gvRandomThreads.DataBind();
            }
        }
        protected void gvRandomThreads_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "SelectThread")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                string threadId = gvRandomThreads.Rows[index].Cells[0].Text;

                IncrementThreadViews(threadId);
                Response.Redirect($"MyFeed.aspx?ThreadID={threadId}"); // Reload page with ThreadID
            }
        }

        protected void gvRandomPosts_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "LikePost")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                if (!int.TryParse(e.CommandArgument.ToString(), out int postId))
                {
                    Response.Write("Invalid Post ID.");
                    return;
                }

                // Ensure safe conversion of userId from the session
                int userId;
                if (!int.TryParse(Session["UserID"]?.ToString(), out userId))
                {
                    Response.Write("Invalid User ID.");
                    return;
                }

                // Call the Like/Unlike handler and pass the necessary parameters
                ToggleLike(postId, userId);

                // Optionally, refresh the posts to reflect the updated like status
                LoadRandomPosts();
            }
        }

        protected void gvRandomPosts_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int postId = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "PostID"));
                int userId = Convert.ToInt32(Session["UserID"]);

                Button btnLike = (Button)e.Row.FindControl("btnLike");
                if (btnLike != null)
                {
                    // Check if the user has liked this post
                    string checkQuery = "SELECT COUNT(*) FROM PostLikes WHERE PostID = @PostID AND UserID = @UserID";
                    string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;

                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                        checkCmd.Parameters.AddWithValue("@PostID", postId);
                        checkCmd.Parameters.AddWithValue("@UserID", userId);
                        int likeCount = (int)checkCmd.ExecuteScalar();

                        // Update the Like button text based on like status
                        if (likeCount > 0)
                        {
                            btnLike.Text = "Liked";  // User has liked the post
                        }
                        else
                        {
                            btnLike.Text = "Like";   // User has not liked the post
                        }
                    }
                }
            }
                
        }

        private void LoadRandomPosts()
        {
            string threadId = Request.QueryString["ThreadID"]; // Get ThreadID from query string
            if (string.IsNullOrEmpty(threadId))
            {
                lblMessage.Text = "Select a thread to view posts.";
                return;
            }

            string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
        SELECT p.PostID, p.Content, COALESCE(u.Login_Name, 'Unknown') AS CreatedBy, p.CreatedAt, p.Likes
        FROM Posts p
        LEFT JOIN [Table] u ON p.CreatedBy = u.Id
        WHERE p.ThreadID = @ThreadID;";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ThreadID", threadId);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                gvRandomPosts.DataSource = dt;
                gvRandomPosts.DataBind();
            }
        }

        protected void gvRandomThreads_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
