using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace badpjProject
{
    public partial class UserPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string username = Session["Username"]?.ToString();
                if (string.IsNullOrEmpty(username))
                {
                    Response.Redirect("Login.aspx");
                }
                else
                {
                    LoadUserData(username);
                    LoadComments();
                }
            }
        }

        private void LoadUserData(string username)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT Login_Name, Email, Role, ProfilePicture, Description FROM [Table] WHERE Login_Name = @Login_Name";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Login_Name", username);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Set profile details
                            UsernameLabel.Text = reader["Login_Name"].ToString();
                            UserEmailLabel.Text = reader["Email"].ToString();
                            RoleDisplay.Text = reader["Role"].ToString();
                            DescriptionDisplay.Text = reader["Description"].ToString();

                            // Set login name and email for header
                            UsernameLabel.Text = reader["Login_Name"].ToString();
                            UserEmailLabel.Text = reader["Email"].ToString();

                            // Set profile picture
                            string profilePicturePath = reader["ProfilePicture"]?.ToString();
                            if (!string.IsNullOrEmpty(profilePicturePath))
                            {
                                ProfilePicture.ImageUrl = profilePicturePath;
                            }
                            else
                            {
                                ProfilePicture.ImageUrl = "~/default-profile.png"; // Default profile picture
                            }
                        }
                    }
                }
            }
        }

        protected void UpdateButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("EditProfilePage.aspx");
        }
        private void LoadComments()
        {
            string currentUserId = Session["UserId"]?.ToString(); // Logged-in user's ID

            if (string.IsNullOrEmpty(currentUserId))
            {
                Response.Redirect("Login.aspx"); // Redirect if not authenticated
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
                   CAST(CASE WHEN c.UserId = @ProfileOwnerId THEN 1 ELSE 0 END AS BIT) AS IsOwner
            FROM Comments c
            INNER JOIN [Table] u ON c.CommenterId = u.Id
            WHERE c.UserId = @ProfileOwnerId
            ORDER BY c.DateCreated DESC";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    // Pass the current user's ID as @ProfileOwnerId
                    cmd.Parameters.AddWithValue("@ProfileOwnerId", currentUserId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        CommentsRepeater.DataSource = reader;
                        CommentsRepeater.DataBind();
                    }
                }
            }
        }



        protected void AddCommentButton_Click(object sender, EventArgs e)
        {
            string currentUserId = Session["UserId"]?.ToString(); // Logged-in user's ID
            string profileOwnerId = currentUserId; // On `UserPage`, comments are made on the logged-in user's profile
            string commentText = CommentTextBox.Text.Trim();

            if (string.IsNullOrEmpty(commentText))
            {
                return;
            }

            string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO Comments (UserId, CommenterId, CommentText) VALUES (@UserId, @CommenterId, @CommentText)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", profileOwnerId); // Profile owner
                    cmd.Parameters.AddWithValue("@CommenterId", currentUserId); // Commenter
                    cmd.Parameters.AddWithValue("@CommentText", commentText);

                    cmd.ExecuteNonQuery();
                }
            }

            // Refresh comments
            LoadComments();
            CommentTextBox.Text = string.Empty;
        }


        protected void DeleteCommentButton_Click(object sender, EventArgs e)
        {
            string commentId = (sender as LinkButton)?.CommandArgument; // Comment ID
            string currentUserId = Session["UserId"]?.ToString(); // Logged-in user's ID

            if (string.IsNullOrEmpty(commentId) || string.IsNullOrEmpty(currentUserId))
            {
                return;
            }

            string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "DELETE FROM Comments WHERE CommentId = @CommentId AND UserId = @CurrentUserId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CommentId", commentId); // ID of the comment to delete
                    cmd.Parameters.AddWithValue("@CurrentUserId", currentUserId); // Ensure only the profile owner can delete

                    cmd.ExecuteNonQuery();
                }
            }

            // Refresh comments
            LoadComments();
        }
        protected void EditProfileButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("EditProfilePage.aspx");
        }


    }
}




