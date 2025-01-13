using badpjProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebGrease;

namespace badpjProject
{
    public partial class ViewCoaches : System.Web.UI.Page
    {
        Coaches coachManager = new Coaches();
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
                {
                bind();
                }
        }

        protected void bind()
        {

            // Get pending and approved coaches
            List<Coaches> pendingCoaches = coachManager.GetPendingCoaches();
            List<Coaches> approvedCoaches = coachManager.GetApprovedCoaches();

            // Bind pending coaches to grid view
            gvPendingCoaches.DataSource = pendingCoaches;
            gvPendingCoaches.DataBind();

            // Bind approved coaches to grid view
            gvApprovedCoaches.DataSource = approvedCoaches;
            gvApprovedCoaches.DataBind();
        }

        protected void gvPendingCoaches_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Approve" || e.CommandName == "Reject")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                string coachID = gvPendingCoaches.DataKeys[rowIndex].Value.ToString();

                if (e.CommandName == "Approve")
                {
                    bool success = coachManager.ApproveCoach(coachID);
                    if (success)
                    {
                        Response.Write("<script>alert('Coach approved successfully');</script>");
                    }
                    else
                    {
                        Response.Write("<script>alert('Error approving coach');</script>");
                    }
                }
                else if (e.CommandName == "Reject")
                {
                    bool success = coachManager.RejectCoach(coachID);
                    if (success)
                    {
                        Response.Write("<script>alert('Coach rejected successfully');</script>");
                    }
                    else
                    {
                        Response.Write("<script>alert('Error rejecting coach');</script>");
                    }
                }

                // Rebind data after an action
                bind();
            }
        }
    }
}