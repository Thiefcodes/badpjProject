using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;

namespace badpjProject
{
    public partial class VisitingProfile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string profileId = Request.QueryString["UserId"];
                if (string.IsNullOrEmpty(profileId))
                {
                    Response.Redirect("ErrorPage.aspx"); // Redirect to an error page if no UserId is provided
                }
                else
                {
                    LoadProfile(profileId);
                    LoadComments(profileId);
                }
            }
        }

        private void LoadProfile(string profileId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT Login_Name, Email, ProfilePicture FROM [Table] WHERE Id = @UserId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", profileId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            UsernameLabel.Text = reader["Login_Name"].ToString();
                            UserEmailLabel.Text = reader["Email"].ToString();
                            ProfilePicture.ImageUrl = reader["ProfilePicture"]?.ToString() ?? "~/Images/default-profile.png";
                        }
                        else
                        {
                            Response.Redirect("ErrorPage.aspx"); // Redirect if user not found
                        }
                    }
                }
            }
        }

        private void LoadComments(string profileId)
        {
            // Get the currently logged-in user's ID from the session
            string currentUserId = Session["UserId"]?.ToString();

            if (string.IsNullOrEmpty(currentUserId))
            {
                Response.Redirect("Login.aspx"); // Redirect if the user is not logged in
                return;
            }

            string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"
            SELECT c.CommentId, 
                   c.CommentText, 
                   c.DateCreated, 
                   u.Login_Name AS CommenterName,
                   CAST(CASE WHEN @CurrentUserId = @ProfileOwnerId THEN 1 ELSE 0 END AS BIT) AS IsOwner
            FROM Comments c
            INNER JOIN [Table] u ON c.CommenterId = u.Id
            WHERE c.UserId = @ProfileOwnerId
            ORDER BY c.DateCreated DESC";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    // Add both @ProfileOwnerId and @CurrentUserId parameters
                    cmd.Parameters.AddWithValue("@ProfileOwnerId", profileId); // The profile being visited
                    cmd.Parameters.AddWithValue("@CurrentUserId", currentUserId); // The logged-in user's ID

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Bind the retrieved data to the Repeater
                        CommentsRepeater.DataSource = reader;
                        CommentsRepeater.DataBind();
                    }
                }
            }
        }


        protected void AddCommentButton_Click(object sender, EventArgs e)
        {
            string profileId = Request.QueryString["UserId"];
            string currentUserId = Session["UserId"]?.ToString();
            string commentText = CommentTextBox.Text.Trim();

            if (!string.IsNullOrEmpty(commentText))
            {
                string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO Comments (UserId, CommenterId, CommentText) VALUES (@UserId, @CommenterId, @CommentText)";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserId", profileId);
                        cmd.Parameters.AddWithValue("@CommenterId", currentUserId);
                        cmd.Parameters.AddWithValue("@CommentText", commentText);

                        cmd.ExecuteNonQuery();
                    }
                }

                // Refresh comments
                LoadComments(profileId);
                CommentTextBox.Text = string.Empty;
            }
        }

        protected void DeleteCommentButton_Click(object sender, EventArgs e)
        {
            string commentId = (sender as LinkButton)?.CommandArgument;
            string profileId = Request.QueryString["UserId"];

            string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "DELETE FROM Comments WHERE CommentId = @CommentId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CommentId", commentId);

                    cmd.ExecuteNonQuery();
                }
            }

            // Refresh comments
            LoadComments(profileId);
        }
    }
}
