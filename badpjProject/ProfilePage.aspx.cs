using System;
using System.Data.SqlClient;
using System.Configuration;

namespace badpjProject
{
    public partial class ProfilePage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UserId"] == null || Session["Username"] == null)
                {
                    Response.Redirect("Login.aspx");
                }

                LoadUserProfile();
            }
        }

        private void LoadUserProfile()
        {
            string userId = Session["UserId"].ToString();
            string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT ProfilePicture, Description FROM [Table] WHERE Id = @UserId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Set profile picture
                            string profilePicturePath = reader["ProfilePicture"]?.ToString();
                            ProfilePicture.ImageUrl = string.IsNullOrEmpty(profilePicturePath) ? "~/Images/default-profile.png" : profilePicturePath;

                            // Set description
                            lblUsername.Text = $"Welcome, {Session["Username"].ToString()}!";
                            lblDescription.Text = reader["Description"]?.ToString() ?? "No description available.";
                        }
                    }
                }
            }
        }

        protected void EditProfileButton_Click(object sender, EventArgs e)
        {
            // Redirect to Edit Profile Page
            Response.Redirect("EditProfilePage.aspx");
        }
    }
}
