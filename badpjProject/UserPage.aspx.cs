using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;

namespace badpjProject
{
    public partial class UserPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Retrieve UserId and Username from query string
                string userId = Request.QueryString["UserId"];
                string username = Request.QueryString["Username"];

                

            }
        }



        private void LoadUserData(string username)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT ProfilePicture, Description FROM [Table] WHERE Login_Name = @Login_Name";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Login_Name", username);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string profilePicturePath = reader["ProfilePicture"].ToString();
                            string description = reader["Description"].ToString();

                            CurrentProfilePicture.ImageUrl = profilePicturePath;
                            DescriptionTextBox.Text = description;
                        }
                    }
                }
            }
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            string username = Session["Username"]?.ToString();
            if (username == null)
            {
                Response.Redirect("Login.aspx");
            }

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

                string query = "UPDATE [Table] SET ProfilePicture = @ProfilePicture, Description = @Description WHERE Login_Name = @Login_Name";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ProfilePicture", profilePicturePath);
                    cmd.Parameters.AddWithValue("@Description", description);
                    cmd.Parameters.AddWithValue("@Login_Name", username);

                    cmd.ExecuteNonQuery();
                    Response.Write("<script>alert('Profile updated successfully!');</script>");
                }
            }
        }
    }
}
