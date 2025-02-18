using System;
using System.Data.SqlClient;
using System.Configuration;

namespace badpjProject
{
    public partial class Payment : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["paymentId"] == null)
                {
                    Response.Write("<script>alert('Invalid payment request.'); window.close();</script>");
                    return;
                }
            }
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            if (Request.HttpMethod == "POST")
            {
                string paymentIdStr = Request.Form["paymentId"];

                if (string.IsNullOrEmpty(paymentIdStr))
                {
                    Response.Write("<script>alert('Error: Missing payment ID.');</script>");
                    return;
                }

                string connStr = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    string updateQuery = "UPDATE Payments SET Status = 'Accepted' WHERE PaymentId = @PaymentId AND Status = 'Pending'";
                    SqlCommand cmd = new SqlCommand(updateQuery, conn);
                    cmd.Parameters.AddWithValue("@PaymentId", Guid.Parse(paymentIdStr));

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    conn.Close();

                    if (rowsAffected > 0)
                    {
                        using (SqlConnection chatConn = new SqlConnection(connStr))
                        {
                            string chatQuery = "INSERT INTO Chat (UserId, CoachId, Message, Timestamp, Sender) VALUES " +
                                               "((SELECT UserId FROM Payments WHERE PaymentId = @PaymentId), " +
                                               "(SELECT CoachId FROM Payments WHERE PaymentId = @PaymentId), " +
                                               "'Payment Accepted! You may now proceed with the session.', GETDATE(), 'System')";

                            SqlCommand chatCmd = new SqlCommand(chatQuery, chatConn);
                            chatCmd.Parameters.AddWithValue("@PaymentId", Guid.Parse(paymentIdStr));

                            chatConn.Open();
                            chatCmd.ExecuteNonQuery();
                            chatConn.Close();
                        }

                        Response.Write("<script>alert('Payment successful!'); window.close();</script>");
                    }
                    else
                    {
                        Response.Write("<script>alert('Error: Payment could not be accepted. Please try again.');</script>");
                    }
                }
            }
        }
    }
}
