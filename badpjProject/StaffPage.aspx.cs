using System;

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
            // Replace with actual database logic to load staff details
            StaffNameLabel.Text = Session["Username"]?.ToString() ?? "Staff Name";
            StaffEmailLabel.Text = Session["Email"]?.ToString() ?? "staff@example.com";
            ProfilePicture.ImageUrl = "~/Images/default-profile.png"; // Set default or dynamic image
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
