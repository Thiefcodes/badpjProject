using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

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
            }
            else
            {
                // Redirect to login page if not logged in
                Response.Redirect("Login.aspx");
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
