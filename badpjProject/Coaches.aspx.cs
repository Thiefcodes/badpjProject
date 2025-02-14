using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace badpjProject
{
    public partial class Coaches : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindCoaches();
            }
        }

        private void BindCoaches()
        {
            Coaches coachHandler = new Coaches();
            rptCoaches.DataSource = coachHandler.GetApprovedCoaches();
            rptCoaches.DataBind();
        }

    }
}