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
                imgHeader.ImageUrl = Page.ResolveUrl("~/Uploads/Coach-Header.jpg");
                BindCoaches();
            }
        //Name : Coach
        //Password: Password
           
        }

        private void BindCoaches()
        {
            Coaches coachManager = new Coaches();
            List<Coaches> approvedCoaches = coachManager.GetApprovedCoaches();

            rptCoaches.DataSource = approvedCoaches;
            rptCoaches.DataBind();
        }

    }
}