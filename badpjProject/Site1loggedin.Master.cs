using System;
using System.Collections.Generic;
using System.Web.UI;

namespace badpjProject
{
    public partial class Site1loggedin : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Check if user is logged in
            if (Session["UserId"] != null)
            {
                // Set a default profile picture if none exists
                if (Session["ProfilePicture"] != null)
                {
                    ProfilePicture.ImageUrl = Session["ProfilePicture"].ToString();
                }
                else
                {
                    ProfilePicture.ImageUrl = "~/Images/default-profile.png"; // Default profile picture path
                }

                // Dynamically hide/show tabs based on role
                if (Session["Role"]?.ToString() != "Staff")
                {
                    HideStaffOnlyTabs();
                }
                else
                {
                    HideUserOnlyTabs();
                }
            }
            else
            {
                // Redirect to login page if not logged in
                Response.Redirect("Login.aspx");
            }
        }

        private void HideStaffOnlyTabs()
        {
            // List of controls to hide for non-staff users
            List<Control> staffOnlyTabs = new List<Control>
            {
                liStaffManageProduct,
                liStaffViewCoaches
            };

            foreach (var tab in staffOnlyTabs)
            {
                tab.Visible = false;
            }
        }


        private void HideUserOnlyTabs()
        {
            // List of controls to hide for staff users
            List<Control> userOnlyTabs = new List<Control>
            {
                liUserBecomeACoach
            };

            foreach (var tab in userOnlyTabs)
            {
                tab.Visible = false;
            }
        }

        protected void Logout_Click(object sender, EventArgs e)
        {
            // Clear session and redirect to login page
            Session.Clear();
            Response.Redirect("Login.aspx");
        }
    }
}
