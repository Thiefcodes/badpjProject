﻿using badpjProject.Models;
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
            if (!IsPostBack)
            {
                BindCoaches();
            }
        }

        private void BindCoaches()
        {
            // Fetch pending and approved coaches
            List<Coaches> pendingCoaches = coachManager.GetPendingCoaches();
            List<Coaches> approvedCoaches = coachManager.GetApprovedCoaches();

            // Bind data to repeaters
            rptPendingCoaches.DataSource = pendingCoaches;
            rptPendingCoaches.DataBind();

            rptApprovedCoaches.DataSource = approvedCoaches;
            rptApprovedCoaches.DataBind();

            // Check if there is no data for pending coaches
            if (pendingCoaches.Count == 0)
            {
                // Hide the table if there are no pending coaches
                rptPendingCoaches.Visible = false;

                // Display the "No pending coaches" message
                litNoPendingCoaches.Text = "<div class='alert alert-warning text-center' role='alert'>" +
                                            "<strong>Oops!</strong> There are currently no pending coaches to review. Please check back later." +
                                            "</div>";
            }
            else
            {
                // Show the table if there are pending coaches
                rptPendingCoaches.Visible = true;

                // Reset the "No pending coaches" message
                litNoPendingCoaches.Text = "";
            }

            // Handle Approved Coaches (No data case)
            if (approvedCoaches.Count == 0)
            {
                rptApprovedCoaches.Visible = false;
                litNoApprovedCoaches.Text = "<div class='alert alert-warning text-center' role='alert'>" +
                                            "<strong>Oops!</strong> There are currently no approved coaches. Please check back later." +
                                            "</div>";
            }
            else
            {
                rptApprovedCoaches.Visible = true;
                litNoApprovedCoaches.Text = "";
            }
        }

        protected void rptPendingCoaches_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            string coachID = e.CommandArgument.ToString();

            if (e.CommandName == "Approve")
            {
                // Update the Coach table status to "Approved"
                bool coachApproved = coachManager.ApproveCoach(coachID);
                if (coachApproved)
                {
                    // Now update the CoachStatus record to set isCoach = true
                    CoachStatus cs = new CoachStatus();
                    bool statusUpdated = cs.UpdateIsCoachStatus(coachID);
                    if (statusUpdated)
                        ShowAlert("Coach approved successfully!", "success");
                    else
                        ShowAlert("Coach approved, but failed to update coach status record.", "error");
                }
                else
                {
                    ShowAlert("Error approving coach.", "error");
                }
            }
            else if (e.CommandName == "Reject")
            {
                bool coachDeleted = coachManager.RejectCoach(coachID);
                bool coachStatusDeleted = false;

                if (coachDeleted)
                {
                    CoachStatus cs = new CoachStatus();
                    coachStatusDeleted = cs.DeleteCoachStatus(coachID);
                }

                ShowAlert(
                    (coachDeleted && coachStatusDeleted) ? "Coach rejected successfully!" : "Error rejecting coach.",
                    (coachDeleted && coachStatusDeleted) ? "success" : "error"
                );
            }
            else if (e.CommandName == "ViewDetails")
            {
                Response.Redirect($"SignUpCoachesDetails.aspx?id={coachID}");
            }

            BindCoaches();
        }

        protected void rptApprovedCoaches_ItemCommand(object source, RepeaterCommandEventArgs e)
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

                ShowAlert(
                    (coachDeleted && coachStatusDeleted) ? "Coach rejected successfully!" : "Error rejecting coach.",
                    (coachDeleted && coachStatusDeleted) ? "success" : "error"
                );

                BindCoaches();
            }
        }



        private void ShowAlert(string message, string type)
        {
            string colorClass = type == "success" ? "alert-success" : "alert-danger";

            string script = $@"
                var alertBox = document.createElement('div');
                alertBox.className = 'alert {colorClass} alert-dismissible fade show text-center';
                alertBox.role = 'alert';
                alertBox.innerHTML = `{message} <button type='button' class='btn-close' data-bs-dismiss='alert' aria-label='Close'></button>`;

                // Apply custom styles
                alertBox.style.position = 'fixed';
                alertBox.style.top = '80px';
                alertBox.style.left = '50%';
                alertBox.style.transform = 'translateX(-50%)';
                alertBox.style.width = '300px';
                alertBox.style.boxShadow = '0px 4px 6px rgba(0, 0, 0, 0.1)';
                alertBox.style.zIndex = '1050';

                document.body.appendChild(alertBox);

                setTimeout(function() {{
                    alertBox.remove();
                }}, 5000);
            ";

            ClientScript.RegisterStartupScript(this.GetType(), "alert", script, true);
        }


    }
}