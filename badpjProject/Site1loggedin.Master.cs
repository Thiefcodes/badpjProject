using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
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
                int userId = int.Parse(Session["UserId"].ToString());

                // Determine if the user is a coach by checking the CoachStatus table
                CoachStatus cs = new CoachStatus();
                bool isCoach = cs.IsUserAlreadyCoach(userId);
                Session["IsCoach"] = isCoach;

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

                if (isCoach)
                {
                    // Hide tabs that should not be visible to coaches
                    HideCoachOnlyTabs();
                }
                else
                {
                    // Hide tabs that are only relevant for coaches (if any)
                    HideNonCoachOnlyTabs();
                }
            }
            else
            {
                // Redirect to login page if not logged in
                Response.Redirect("Login.aspx");
            }
            if (!IsPostBack)
            {
                // Check if the user is logged in and if a profile picture is available
                if (Session["Username"] != null)
                {
                    // Option 1: Use a session variable that holds the profile picture path
                    string profilePicturePath = Session["ProfilePicture"]?.ToString();

                    // Option 2: If not stored in session, you can query the DB to get the path
                    if (string.IsNullOrEmpty(profilePicturePath))
                    {
                        string username = Session["Username"].ToString();
                        string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;
                        using (SqlConnection conn = new SqlConnection(connectionString))
                        {
                            conn.Open();
                            string query = "SELECT ProfilePicture FROM [Table] WHERE Login_Name = @Login_Name";
                            using (SqlCommand cmd = new SqlCommand(query, conn))
                            {
                                cmd.Parameters.AddWithValue("@Login_Name", username);
                                object result = cmd.ExecuteScalar();
                                profilePicturePath = result != null ? result.ToString() : "";
                            }
                        }
                    }

                    // Set the ImageUrl. If no profile picture, use a default image.
                    ProfilePicture.ImageUrl = !string.IsNullOrEmpty(profilePicturePath)
                        ? profilePicturePath
                        : "~/Images/default-profile.png";
                }
                else
                {
                    // If the user is not logged in, you might want to set a default image.
                    ProfilePicture.ImageUrl = "~/Images/default-profile.png";
                }
            }
        }

        private void HideStaffOnlyTabs()
        {
            // List of controls to hide for non-staff users
            List<Control> staffOnlyTabs = new List<Control>
            {
                liStaffManageProduct,
                liStaffViewCoaches,
                liStaffAllorders
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
                liUserBecomeACoach,
                liUserOrders
            };

            foreach (var tab in userOnlyTabs)
            {
                tab.Visible = false;
            }
        }

        private void HideCoachOnlyTabs()
        {
            // List of controls to hide for users who are already coaches (e.g., "Become a Coach" tab, public "Coaches" page)
            List<Control> coachOnlyTabs = new List<Control>
            {
                liUserBecomeACoach,
                liPublicCoaches
            };

            foreach (var tab in coachOnlyTabs)
            {
                tab.Visible = false;
            }
        }

        private void HideNonCoachOnlyTabs()
        {
            // List of controls to hide for users who are not coaches (e.g., coach-specific profile links)
            List<Control> nonCoachOnlyTabs = new List<Control>
            {
                liCoachProfile  // Adjust the control IDs to match your master page
            };

            foreach (var tab in nonCoachOnlyTabs)
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
