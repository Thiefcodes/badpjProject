using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
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
                    // 1. Load user profile data into memory, set labels, etc.
                    LoadUserDataInMemory(username);
                    LoadUserRankingForSharing();
                    // 2. Load comments from a DataTable and bind to the Repeater
                    DataTable commentsTable = GetCommentsInMemory();
                    CommentsRepeater.DataSource = commentsTable;
                    CommentsRepeater.DataBind();

                    // 3. Load ranking for sharing (set up share links, etc.)
                    LoadUserRankingForSharingInMemory();
                }
            }
        }
        private void LoadUserDataInMemory(string username)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"
            SELECT Login_Name, Email, Role, ProfilePicture, Description
            FROM [Table]
            WHERE Login_Name = @Login_Name";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Login_Name", username);

                    // Load into a DataTable
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        DataTable dt = new DataTable();
                        dt.Load(reader);

                        if (dt.Rows.Count > 0)
                        {
                            DataRow row = dt.Rows[0];
                            UsernameLabel.Text = row["Login_Name"].ToString();
                            UserEmailLabel.Text = row["Email"].ToString();
                            RoleDisplay.Text = row["Role"].ToString();
                            DescriptionDisplay.Text = row["Description"].ToString();

                            string profilePicturePath = row["ProfilePicture"]?.ToString();
                            ProfilePicture.ImageUrl = string.IsNullOrEmpty(profilePicturePath)
                                ? "~/Uploads/default-profile.png"
                                : profilePicturePath;
                        }
                    }
                }
            }
        }
        private DataTable GetCommentsInMemory()
        {
            string currentUserId = Session["UserId"]?.ToString();
            if (string.IsNullOrEmpty(currentUserId))
            {
                Response.Redirect("Login.aspx");
                return null; // or an empty DataTable
            }

            DataTable dtComments = new DataTable();
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
                    cmd.Parameters.AddWithValue("@ProfileOwnerId", currentUserId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        dtComments.Load(reader); // Read all rows into DataTable
                    }
                }
            }
            return dtComments;
        }

        private void LoadUserRankingForSharingInMemory()
        {
            int userId = Convert.ToInt32(Session["UserId"]);
            string rank = "Unranked";
            int totalPoints = 0;

            string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT Rank, TotalPoints FROM Ranking WHERE UserID = @UserId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        DataTable dt = new DataTable();
                        dt.Load(reader);

                        if (dt.Rows.Count > 0)
                        {
                            rank = dt.Rows[0]["Rank"].ToString();
                            totalPoints = Convert.ToInt32(dt.Rows[0]["TotalPoints"]);
                        }
                    }
                }
            }

            // Now you can build your share URL from the in-memory data (rank & totalPoints).
            // For example:
            string tweetMessage = $"I'm ranked '{rank}' with {totalPoints} points on the Fitness App!";
            // ... Then set hlShareRank.NavigateUrl or whatever you're using for sharing.
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
                            UsernameLabel.Text = reader["Login_Name"].ToString();
                            UserEmailLabel.Text = reader["Email"].ToString();
                            RoleDisplay.Text = reader["Role"].ToString();
                            DescriptionDisplay.Text = reader["Description"].ToString();
                            string profilePicturePath = reader["ProfilePicture"]?.ToString();
                            ProfilePicture.ImageUrl = !string.IsNullOrEmpty(profilePicturePath)
                                ? profilePicturePath
                                : "~/Uploads/default-profile.png";
                        }
                    }
                }
            }
        }

        // Existing methods: LoadComments, AddCommentButton_Click, DeleteCommentButton_Click, etc.

        protected void EditProfileButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("EditProfilePage.aspx");
        }

        protected void EnableFacialAuthButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("EnableFacialAuthentication.aspx");
        }

        /// <summary>
        /// Retrieves the current user's ranking from the Ranking table and sets up a share URL.
        /// </summary>
        private void LoadUserRankingForSharing()
        {
            int userId = Convert.ToInt32(Session["UserId"]);
            string rank = "Unranked";
            int totalPoints = 0;
            string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT Rank, TotalPoints FROM Ranking WHERE UserID = @UserId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            rank = reader["Rank"].ToString();
                            totalPoints = Convert.ToInt32(reader["TotalPoints"]);
                        }
                    }
                }
            }

            // Common message for all share URLs:
            string shareMessage = $"I'm currently ranked '{rank}' with {totalPoints} points on our Fitness App! Check it out:";
            string encodedMessage = HttpUtility.UrlEncode(shareMessage);
            string appUrl = HttpUtility.UrlEncode("https://www.yourfitnessapp.com"); // Replace with your actual URL

            // 1. Twitter
            string twitterShareUrl = $"https://twitter.com/intent/tweet?text={encodedMessage}&url={appUrl}";
            hlShareRank.NavigateUrl = twitterShareUrl; // Example for your Twitter link

            // 2. Facebook
            string facebookShareUrl = $"https://www.facebook.com/sharer/sharer.php?u={appUrl}&quote={encodedMessage}";
            hlShareFacebook.NavigateUrl = facebookShareUrl; // Make sure you have a hlShareFacebook in your markup

            // 3. LinkedIn
            // Note: LinkedIn share can also support "summary" and "title" parameters. 
            string linkedInShareUrl = $"https://www.linkedin.com/sharing/share-offsite/?url={appUrl}";
            hlShareLinkedIn.NavigateUrl = linkedInShareUrl;

            // 4. Instagram - direct linking only (no pre-populated share)
            // Typically, you just link to your Instagram page or app.
            hlShareInstagram.NavigateUrl = "https://www.instagram.com/yourfitnessapp/";
        }


        private void LoadComments()
        {
            string currentUserId = Session["UserId"]?.ToString();
            if (string.IsNullOrEmpty(currentUserId))
            {
                Response.Redirect("Login.aspx");
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
                    cmd.Parameters.AddWithValue("@ProfileOwnerId", currentUserId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        CommentsRepeater.DataSource = reader;
                        CommentsRepeater.DataBind();
                    }
                }
            }
        }
        protected void DeleteCommentButton_Click(object sender, EventArgs e)
        {
            // Your delete comment logic here.
            // For example:
            string commentId = (sender as LinkButton)?.CommandArgument;
            string currentUserId = Session["UserId"]?.ToString();

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
                    cmd.Parameters.AddWithValue("@CommentId", commentId);
                    cmd.Parameters.AddWithValue("@CurrentUserId", currentUserId);
                    cmd.ExecuteNonQuery();
                }
            }

            // Refresh the comments list after deletion.
            LoadComments();
        }
        protected void AddCommentButton_Click(object sender, EventArgs e)
        {
            string currentUserId = Session["UserId"]?.ToString(); // Logged-in user's ID
            string profileOwnerId = currentUserId; // On UserPage, comments are made on the logged-in user's profile
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

        protected string GetTwitterShareUrl()
        {
            // Customize the message with dynamic rank info if available.
            string rank = "Gold"; // Replace with actual rank from your ranking data
            int totalPoints = 1500; // Replace with actual total points

            string message = $"I'm ranked '{rank}' with {totalPoints} points on our Fitness App! Check it out:";
            string encodedMessage = HttpUtility.UrlEncode(message);
            string appUrl = HttpUtility.UrlEncode("https://www.yourfitnessapp.com"); // Replace with your app URL
            return $"https://twitter.com/intent/tweet?text={encodedMessage}&url={appUrl}";
        }

        protected string GetFacebookShareUrl()
        {
            // Facebook only needs the URL and optional quote.
            string rank = "Gold"; // Replace with dynamic rank
            int totalPoints = 1500; // Replace with dynamic points

            string quote = $"I'm ranked '{rank}' with {totalPoints} points on our Fitness App!";
            string encodedQuote = HttpUtility.UrlEncode(quote);
            string appUrl = HttpUtility.UrlEncode("https://www.yourfitnessapp.com");
            return $"https://www.facebook.com/sharer/sharer.php?u={appUrl}&quote={encodedQuote}";
        }

        protected string GetLinkedInShareUrl()
        {
            // LinkedIn share can include the URL and a summary.
            string rank = "Gold"; // Replace dynamically
            int totalPoints = 1500; // Replace dynamically

            string summary = $"I'm ranked '{rank}' with {totalPoints} points on our Fitness App!";
            string encodedSummary = HttpUtility.UrlEncode(summary);
            string appUrl = HttpUtility.UrlEncode("https://www.yourfitnessapp.com");
            return $"https://www.linkedin.com/sharing/share-offsite/?url={appUrl}&summary={encodedSummary}";
        }

        protected string GetInstagramShareUrl()
        {
            // Instagram does not support pre-populated sharing via URL.
            // Instead, provide a link to your Instagram profile.
            return "https://www.instagram.com/yourfitnessapp/"; // Replace with your profile
        }
    }
}
