using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace badpjProject
{
    public partial class CoachSubmit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btn_BackToHome_Click(object sender, EventArgs e)
        {
            // Redirect to the home page
            Response.Redirect("~/About.aspx");
        }
    }
}