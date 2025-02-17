using System;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace badpjProject
{
    public partial class Thread : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Load the thread and posts after role checks
                LoadThread();
                LoadPosts();

            }
            if (!IsPostBack)
            {
                // Check the session role
                if (Session["Role"] != null && Session["Role"].ToString() == "Staff")
                {
                    // Make the Delete button visible if the role is "Admin"
                    gvPosts.Columns[6].Visible = true;  // Assuming the Delete button is in the 7th column (index 6)
                    gvPosts.Columns[7].Visible = true; // Assuming the Update button is in the 8th column(index 7)
                }
                else
                {
                    // Hide the Delete button for non-admin roles
                    gvPosts.Columns[6].Visible = false;
                    gvPosts.Columns[7].Visible = false;
                }
                LoadPosts();
            }

        }
        protected void gvPosts_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // Check if the row is a data row (not header, footer, or pager)
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Get the PostID from the current row
                int postId = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "PostID"));
                int userId = Convert.ToInt32(Session["UserID"]);

                // Find the Edit, Delete, and Like buttons in the current row
                Button btnEdit = (Button)e.Row.FindControl("btnEdit");
                Button btnDelete = (Button)e.Row.FindControl("btnDelete");
                Button btnLike = (Button)e.Row.FindControl("btnLike");

                // Button visibility based on role (Staff or not)
                if (Session["Role"] != null && Session["Role"].ToString() == "Staff")
                {
                    // Make the Edit and Delete buttons visible for Staff role
                    if (btnEdit != null) btnEdit.Visible = true;
                    if (btnDelete != null) btnDelete.Visible = true;
                }
                else
                {
                    // Hide the Edit and Delete buttons for non-staff roles
                    if (btnEdit != null) btnEdit.Visible = false;
                    if (btnDelete != null) btnDelete.Visible = false;
                }

                // Button text update based on Like status
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

        protected void gvPosts_RowCommand(object sender, GridViewCommandEventArgs e)
        {
             // Get PostID from the first cell (make sure it's the correct column)

            if (e.CommandName == "DeletePost")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                string postId = gvPosts.Rows[index].Cells[0].Text;
                DeletePost(postId);
                LoadPosts();
            }
            else if (e.CommandName == "EditPost")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                string postId = gvPosts.Rows[index].Cells[0].Text;
                string threadId = Request.QueryString["ThreadID"];
                Response.Redirect($"UpdatePost.aspx?PostID={postId}&ThreadID={threadId}");
            }
            else if (e.CommandName == "LikePost")
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
                LoadPosts();
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
            LoadPosts();
        }
        private void LoadThread()
        {
            string threadId = Request.QueryString["ThreadID"];
            string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {

                SqlCommand cmd = new SqlCommand("SELECT Title FROM Threads WHERE ThreadID = @ThreadID", conn);
                cmd.Parameters.AddWithValue("@ThreadID", threadId);
                conn.Open();
                lblThreadTitle.Text = (string)cmd.ExecuteScalar();
            }
        }
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

        private void LoadPosts()
        {
            string threadId = Request.QueryString["ThreadID"];
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
            WHERE p.ThreadID = @ThreadID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ThreadID", threadId);
                cmd.Parameters.AddWithValue("@UserID", userId);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                gvPosts.DataSource = dt;
                gvPosts.DataBind();
            }
        }

        protected void btnReply_Click(object sender, EventArgs e)
        {
            // Check if the user is logged in
            if (Session["UserId"] == null || Session["Username"] == null || Session["Role"] == null)
            {
                // Redirect to the login page if user is not logged in
                Response.Write("<script>alert('Please log in first!'); window.location='Login.aspx';</script>");
            }
            else
            {
                // Proceed with the current page as user is logged in
                // Continue with your page logic here
                string threadId = Request.QueryString["ThreadID"];
                Response.Redirect($"NewPost.aspx?ThreadID={threadId}");
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("Forum.aspx");
        }
    }
}