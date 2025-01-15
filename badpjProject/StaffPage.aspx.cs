using System;
using System.Configuration;
using System.Data.SqlClient;

namespace badpjProject
{
    public partial class StaffPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Role"]?.ToString() != "Staff")
            {
                Response.Redirect("Login.aspx");
            }

            if (!IsPostBack)
            {
                LoadStaffData();
            }
        }

        private void LoadStaffData()
        {
            string username = Session["Username"]?.ToString();

            if (string.IsNullOrEmpty(username))
            {
                Response.Redirect("Login.aspx"); // Redirect to login if session is invalid
                return;
            }

            string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT Login_Name, Email, Role, ProfilePicture FROM [Table] WHERE Login_Name = @Login_Name AND Role = 'Staff'";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Login_Name", username);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Set staff profile details
                            StaffNameLabel.Text = reader["Login_Name"].ToString();
                            StaffEmailLabel.Text = reader["Email"].ToString();

                            // Set profile picture
                            string profilePicturePath = reader["ProfilePicture"]?.ToString();
                            if (!string.IsNullOrEmpty(profilePicturePath))
                            {
                                ProfilePicture.ImageUrl = profilePicturePath;
                            }
                            else
                            {
                                ProfilePicture.ImageUrl = "~/Images/default-profile.png"; // Default profile picture
                            }
                        }
                        else
                        {
                            // Handle case where staff data is not found
                            Response.Redirect("ErrorPage.aspx"); // Optional: Redirect to an error page
                        }
                    }
                }
            }
        }


        protected void ManageStaffButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("ManageStaff.aspx");
        }

        protected void ManageUsersButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("ManageUsers.aspx");
        }

        protected void EditProfileButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("EditProfilePage.aspx");
        }
        protected void ConfigureRewardsButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("ConfigureRewards.aspx");
        }

    }
}
