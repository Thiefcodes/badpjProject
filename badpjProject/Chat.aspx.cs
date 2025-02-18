using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace badpjProject
{
    public partial class Chat : System.Web.UI.Page
    {
        protected int UserId;
        protected Guid CoachId;
        protected bool IsCoach = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            {
                if (Session["CoachId"] != null && Request.QueryString["userId"] != null)
                {
                    CoachId = Guid.Parse(Session["CoachId"].ToString());
                    UserId = Convert.ToInt32(Request.QueryString["userId"]);
                    IsCoach = true;
                    LoadUserName();
                }
                else if (Session["UserId"] != null && Request.QueryString["coachId"] != null)
                {
                    UserId = Convert.ToInt32(Session["UserId"]);
                    CoachId = Guid.Parse(Request.QueryString["coachId"]);
                    IsCoach = false;
                    LoadCoachName();
                }
                else
                {
                    Response.Redirect("Login.aspx");
                }

                LoadMessages();
                LoadOfferDetails();
            }
        }

        private void LoadOfferDetails()
        {
            string connStr = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
            SELECT PaymentId, OfferAmount 
            FROM Payments 
            WHERE UserId = @UserId AND CoachId = @CoachId AND Status = 'Pending' 
            ORDER BY CreatedAt DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserId", UserId);
                cmd.Parameters.AddWithValue("@CoachId", CoachId);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    bool isUser = Convert.ToInt32(Session["UserId"]) == UserId;
                    pnlUserResponse.Visible = isUser;

                    if (isUser)
                    {
                        lblOfferAmount.Text = "Offer: $" + reader["OfferAmount"].ToString();
                        btnAcceptOffer.CommandArgument = reader["PaymentId"].ToString();
                        btnRejectOffer.CommandArgument = reader["PaymentId"].ToString();
                    }
                }
                else
                {
                    pnlUserResponse.Visible = false;
                }
                conn.Close();
            }
        }

        private void LoadUserName()
        {
            string connStr = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT Login_Name FROM [Table] WHERE Id = @UserId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserId", UserId);
                conn.Open();
                lblCoachName.Text = cmd.ExecuteScalar()?.ToString(); 
            }
        }

        private void LoadCoachName()
        {
            string connStr = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT Name FROM Coach WHERE Id = @CoachId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CoachId", CoachId);
                conn.Open();
                lblCoachName.Text = cmd.ExecuteScalar()?.ToString();
            }
        }

        private void LoadMessages()
        {
            int userId = Convert.ToInt32(Request.QueryString["userId"]);
            Guid coachId = Guid.Parse(Request.QueryString["coachId"]);

            string connStr = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
        SELECT 
            c.UserId, 
            c.Message, 
            c.Timestamp, 
            c.Sender, 
            p.Status, 
            p.OfferAmount, 
            p.PaymentId, 
            CASE 
                WHEN c.Sender = 'User' THEN (SELECT Login_Name FROM [Table] WHERE Id = c.UserId)
                WHEN c.Sender = 'Coach' THEN (SELECT Name FROM Coach WHERE Id = c.CoachId)
            END AS SenderName
        FROM Chat c
        LEFT JOIN Payments p ON c.UserId = p.UserId 
            AND c.CoachId = p.CoachId 
            AND p.Status IN ('Pending', 'Accepted') 
        WHERE c.UserId = @UserId AND c.CoachId = @CoachId
        ORDER BY c.Timestamp ASC";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@CoachId", coachId);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                conn.Close();

                if (rptMessages.DataSource == null)
                {
                    rptMessages.DataSource = dt;
                    rptMessages.DataBind();
                }
            }
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            string message = txtMessage.Text.Trim();
            string connStr = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;

            if (message.ToLower() == "done")
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    string updateQuery = @"
            UPDATE Payments 
            SET Status = 'Completed', 
                TransferToCoach = PaymentHoldAmount * 0.9, 
                TransferToPlatform = PaymentHoldAmount * 0.1, 
                CompletedAt = GETDATE() 
            WHERE UserId = @UserId AND CoachId = @CoachId AND Status = 'Accepted'";

                    SqlCommand cmd = new SqlCommand(updateQuery, conn);
                    cmd.Parameters.AddWithValue("@UserId", Request.QueryString["userId"]);
                    cmd.Parameters.AddWithValue("@CoachId", Request.QueryString["coachId"]);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    string insertQuery = "INSERT INTO Chat (UserId, CoachId, Message, Timestamp, Sender) VALUES (@UserId, @CoachId, 'Session completed. Payment released.', GETDATE(), 'System')";
                    SqlCommand cmd = new SqlCommand(insertQuery, conn);
                    cmd.Parameters.AddWithValue("@UserId", Request.QueryString["userId"]);
                    cmd.Parameters.AddWithValue("@CoachId", Request.QueryString["coachId"]);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }

                Response.Write("<script>alert('Session complete. Payment released.'); window.location.reload();</script>");

                LoadMessages();
                UpdatePanelChat.Update();
            }
            else
            {
                int sessionUserId = Convert.ToInt32(Session["UserId"]);
                int userId = Convert.ToInt32(Request.QueryString["userId"]);
                Guid coachId = Guid.Parse(Request.QueryString["coachId"]);
                string senderType = (sessionUserId == userId) ? "User" : "Coach";

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    string insertQuery = "INSERT INTO Chat (UserId, CoachId, Message, Timestamp, Sender) VALUES (@UserId, @CoachId, @Message, @Timestamp, @Sender)";
                    SqlCommand cmd = new SqlCommand(insertQuery, conn);
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@CoachId", coachId);
                    cmd.Parameters.AddWithValue("@Message", txtMessage.Text);
                    cmd.Parameters.AddWithValue("@Timestamp", DateTime.Now);
                    cmd.Parameters.AddWithValue("@Sender", senderType);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }

            txtMessage.Text = "";
            LoadMessages();
            UpdatePanelChat.Update(); 
        }

        protected void btnSendOffer_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(Session["UserId"]) == Convert.ToInt32(Request.QueryString["userId"]))
            {
                Response.Write("<script>alert('Only the Coach can send offers!');</script>");
                return;
            }

            TextBox txtOfferPrice = txtOfferPriceField;

            if (txtOfferPrice == null)
            {
                Response.Write("<script>alert('Error: Offer price input not found.');</script>");
                return;
            }

            decimal offerAmount;
            if (!decimal.TryParse(txtOfferPrice.Text, out offerAmount))
            {
                Response.Write("<script>alert('Invalid price entered. Please enter a valid number.');</script>");
                return;
            }

            string connStr = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string checkQuery = "SELECT COUNT(*) FROM Payments WHERE CoachId = @CoachId AND UserId = @UserId AND Status = 'Pending'";
                SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                checkCmd.Parameters.AddWithValue("@CoachId", Guid.Parse(Request.QueryString["coachId"]));
                checkCmd.Parameters.AddWithValue("@UserId", Convert.ToInt32(Request.QueryString["userId"]));

                conn.Open();
                int pendingOffers = (int)checkCmd.ExecuteScalar();
                conn.Close();

                if (pendingOffers > 0)
                {
                    Response.Write("<script>alert('Error: An offer is already pending. Please wait for the user to accept or reject.');</script>");
                    return;
                }

                string insertQuery = "INSERT INTO Payments (CoachId, UserId, OfferAmount, Status) VALUES (@CoachId, @UserId, @OfferAmount, 'Pending')";
                SqlCommand cmd = new SqlCommand(insertQuery, conn);

                cmd.Parameters.AddWithValue("@CoachId", Guid.Parse(Request.QueryString["coachId"]));
                cmd.Parameters.AddWithValue("@UserId", Convert.ToInt32(Request.QueryString["userId"]));
                cmd.Parameters.AddWithValue("@OfferAmount", offerAmount);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string insertChatQuery = "INSERT INTO Chat (UserId, CoachId, Message, Timestamp, Sender) VALUES (@UserId, @CoachId, @Message, GETDATE(), 'System')";
                SqlCommand cmd = new SqlCommand(insertChatQuery, conn);
                cmd.Parameters.AddWithValue("@UserId", Convert.ToInt32(Request.QueryString["userId"]));
                cmd.Parameters.AddWithValue("@CoachId", Guid.Parse(Request.QueryString["coachId"]));
                cmd.Parameters.AddWithValue("@Message", "Coach has made an offer: $" + offerAmount);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }

            txtOfferPriceField.Text = "";

            LoadMessages();
            LoadOfferDetails();
            ScriptManager.RegisterStartupScript(this, GetType(), "scrollToBottom", "scrollToBottom();", true);
        }

        protected void btnAcceptOffer_Click(object sender, EventArgs e)
        {
            try
            {
                string paymentIdStr = ((Button)sender).CommandArgument;
                string connStr = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;

                if (!Guid.TryParse(paymentIdStr, out Guid paymentGuid))
                {
                    Response.Write("<script>alert('Error: Invalid Payment ID format.');</script>");
                    return;
                }

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    string updateQuery = @"
            UPDATE Payments 
            SET Status = 'Accepted', 
                PaymentHoldAmount = OfferAmount 
            WHERE PaymentId = @PaymentId AND Status = 'Pending'";

                    SqlCommand cmd = new SqlCommand(updateQuery, conn);
                    cmd.Parameters.AddWithValue("@PaymentId", paymentGuid);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    conn.Close();

                    if (rowsAffected == 0)
                    {
                        Response.Write("<script>alert('Error: No matching offer found to accept or already accepted.');</script>");
                        return;
                    }
                }

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    string insertQuery = "INSERT INTO Chat (UserId, CoachId, Message, Timestamp, Sender) VALUES (@UserId, @CoachId, 'Offer Accepted! Payment is on hold.', GETDATE(), 'System')";
                    SqlCommand cmd = new SqlCommand(insertQuery, conn);
                    cmd.Parameters.AddWithValue("@UserId", Request.QueryString["userId"]);
                    cmd.Parameters.AddWithValue("@CoachId", Request.QueryString["coachId"]);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }

                string paymentUrl = $"Payment.aspx?paymentId={paymentIdStr}";
                string script = $"window.open('{paymentUrl}', 'PaymentWindow', 'width=500,height=600');";
                ScriptManager.RegisterStartupScript(this, GetType(), "OpenPaymentWindow", script, true);

                LoadMessages();
                LoadOfferDetails();
                UpdatePanelChat.Update();
                ScriptManager.RegisterStartupScript(this, GetType(), "scrollToBottom", "scrollToBottom();", true);
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
            }
        }


        protected void btnRejectOffer_Click(object sender, EventArgs e)
        {
            try
            {
                string paymentId = ((Button)sender).CommandArgument;
                string connStr = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;

                if (!Guid.TryParse(paymentId, out Guid paymentGuid))
                {
                    Response.Write("<script>alert('Error: Invalid Payment ID format.');</script>");
                    return;
                }

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    string updateQuery = "UPDATE Payments SET Status = 'Rejected' WHERE PaymentId = @PaymentId";
                    SqlCommand cmd = new SqlCommand(updateQuery, conn);
                    cmd.Parameters.AddWithValue("@PaymentId", paymentGuid);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    conn.Close();

                    if (rowsAffected == 0)
                    {
                        Response.Write("<script>alert('Error: No matching offer found to reject.');</script>");
                        return;
                    }
                }

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    string insertQuery = "INSERT INTO Chat (UserId, CoachId, Message, Timestamp, Sender) VALUES (@UserId, @CoachId, 'Offer Rejected.', GETDATE(), 'System')";
                    SqlCommand cmd = new SqlCommand(insertQuery, conn);
                    cmd.Parameters.AddWithValue("@UserId", Request.QueryString["userId"]);
                    cmd.Parameters.AddWithValue("@CoachId", Request.QueryString["coachId"]);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }

                Response.Write("<script>alert('Offer rejected.');</script>");
                LoadMessages();
                UpdatePanelChat.Update();
                ScriptManager.RegisterStartupScript(this, GetType(), "scrollToBottom", "scrollToBottom();", true);
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
            }
        }

    }
}
