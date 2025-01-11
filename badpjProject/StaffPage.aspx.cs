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
        }

        protected void ManageStaffButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("ManageStaff.aspx");
        }

        protected void ManageUsersButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("ManageUsers.aspx");
        }
    }
}
