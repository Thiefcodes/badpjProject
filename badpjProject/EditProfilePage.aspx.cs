using System;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;

namespace badpjProject
{
    public partial class EditProfilePage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UserId"] == null)
                {
                    Response.Redirect("Login.aspx");
                }
                LoadUserData();
            }
        }

        private void LoadUserData()
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
                            CurrentProfilePicture.ImageUrl = reader["ProfilePicture"]?.ToString() ?? "~/Images/default-profile.png";
                            DescriptionTextBox.Text = reader["Description"]?.ToString();
                        }
                    }
                }
            }
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            string userId = Session["UserId"].ToString();
            string profilePicturePath = CurrentProfilePicture.ImageUrl;

            if (ProfilePictureUpload.HasFile)
            {
                string fileName = Path.GetFileName(ProfilePictureUpload.PostedFile.FileName);
                string filePath = "~/Uploads/" + fileName;
                ProfilePictureUpload.SaveAs(Server.MapPath(filePath));
                profilePicturePath = filePath;
            }

            string description = DescriptionTextBox.Text.Trim();
            string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE [Table] SET ProfilePicture = @ProfilePicture, Description = @Description WHERE Id = @UserId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ProfilePicture", profilePicturePath);
                    cmd.Parameters.AddWithValue("@Description", description);
                    cmd.Parameters.AddWithValue("@UserId", userId);

                    cmd.ExecuteNonQuery();
                }
            }

            // Redirect user based on role
            RedirectBasedOnRole(userId);
        }

        private void RedirectBasedOnRole(string userId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT Role FROM [Table] WHERE Id = @UserId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string role = reader["Role"].ToString();

                            if (role.Equals("Staff", StringComparison.OrdinalIgnoreCase))
                            {
                                Response.Redirect("StaffPage.aspx");
                            }
                            else if (role.Equals("User", StringComparison.OrdinalIgnoreCase))
                            {
                                Response.Redirect("UserPage.aspx");
                            }
                            else
                            {
                                Response.Redirect("ErrorPage.aspx"); // Default for unknown roles
                            }
                        }
                    }
                }
            }
        }
    }
}
