using badpjProject.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace badpjProject
{
    public partial class ViewCoaches : System.Web.UI.Page
    {
        Coaches coachManager = new Coaches();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindCoaches();
            }
        }

        /// <summary>
        /// Binds the coach data based on the selected filter and sort criteria.
        /// Only one set (Pending OR Approved) is displayed at a time.
        /// </summary>
        private void BindCoaches()
        {
            string statusFilter = ddlStatusFilter.SelectedValue;
            string sortBy = ddlSort.SelectedValue;
            List<Coaches> coachesList = new List<Coaches>();

            // Retrieve data based on the filter.
            if (statusFilter.Equals("Pending", StringComparison.OrdinalIgnoreCase))
            {
                coachesList = coachManager.GetPendingCoaches();
            }
            else if (statusFilter.Equals("Approved", StringComparison.OrdinalIgnoreCase))
            {
                coachesList = coachManager.GetApprovedCoaches();
            }

            // Apply sorting.
            if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
            {
                coachesList = coachesList.OrderBy(c => c.Coach_Name).ToList();
            }
            else if (sortBy.Equals("Email", StringComparison.OrdinalIgnoreCase))
            {
                coachesList = coachesList.OrderBy(c => c.Coach_Email).ToList();
            }

            // Bind data to repeater or display "no data" message.
            if (coachesList.Count > 0)
            {
                rptCoaches.DataSource = coachesList;
                rptCoaches.DataBind();
                rptCoaches.Visible = true;
                litNoCoaches.Text = "";
            }
            else
            {
                rptCoaches.Visible = false;
                litNoCoaches.Text = $"<div class='alert alert-warning text-center' role='alert'>" +
                                    $"<strong>Oops!</strong> There are currently no {statusFilter.ToLower()} coaches to display. Please check back later." +
                                    $"</div>";
            }
        }

        protected void ddlStatusFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindCoaches();
        }

        protected void ddlSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindCoaches();
        }

        protected void rptCoaches_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            string coachID = e.CommandArgument.ToString();

            if (e.CommandName == "ViewDetails")
            {
                Response.Redirect($"SignUpCoachesDetails.aspx?id={coachID}");
            }
            else if (e.CommandName == "Remove")
            {
                bool coachDeleted = coachManager.RejectCoach(coachID);
                bool coachStatusDeleted = false;
                if (coachDeleted)
                {
                    CoachStatus cs = new CoachStatus();
                    coachStatusDeleted = cs.DeleteCoachStatus(coachID);
                }
                ShowMessage((coachDeleted && coachStatusDeleted) ? "Coach removed successfully!" : "Error removing coach.",
                            (coachDeleted && coachStatusDeleted) ? "success" : "error");
                BindCoaches();
            }
        }

        private void ShowMessage(string message, string type)
        {
            string alertClass = type.Equals("success", StringComparison.OrdinalIgnoreCase) ? "alert-success" : "alert-danger";
            lblMessage.Text = message;
            lblMessage.CssClass = "alert " + alertClass + " text-center";
            lblMessage.Visible = true;
        }
    }
}
