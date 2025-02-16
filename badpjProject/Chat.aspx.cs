using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;

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
                    // Coach is viewing messages from a user
                    CoachId = Guid.Parse(Session["CoachId"].ToString());
                    UserId = Convert.ToInt32(Request.QueryString["userId"]);
                    IsCoach = true;
                    LoadUserName();
                }
                else if (Session["UserId"] != null && Request.QueryString["coachId"] != null)
                {
                    // User is messaging a coach
                    UserId = Convert.ToInt32(Session["UserId"]);
                    CoachId = Guid.Parse(Request.QueryString["coachId"]);
                    IsCoach = false;
                    LoadCoachName();
                }
                else
                {
                    Response.Redirect("Login.aspx"); // Redirect to login if not authenticated
                }

                LoadMessages();
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
                lblCoachName.Text = cmd.ExecuteScalar()?.ToString(); // Update UI to show user name instead of coach name
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
            if (Request.QueryString["userId"] == null || Request.QueryString["coachId"] == null)
            {
                Response.Write("<script>alert('Invalid chat parameters. Please try again.');</script>");
                return;
            }

            int userId = Convert.ToInt32(Request.QueryString["userId"]);  // Get UserId from URL
            Guid coachId = Guid.Parse(Request.QueryString["coachId"]);  // Get CoachId from URL

            string connStr = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
            SELECT Message, Timestamp, Sender 
            FROM Chat 
            WHERE UserId = @UserId AND CoachId = @CoachId 
            ORDER BY Timestamp ASC";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@CoachId", coachId);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);

                rptMessages.DataSource = dt;
                rptMessages.DataBind();
            }
        }


        protected void btnSend_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtMessage.Text))
            {
                if ((Session["UserId"] == null && Session["CoachId"] == null) || Request.QueryString["userId"] == null || Request.QueryString["coachId"] == null)
                {
                    Response.Write("<script>alert('Session expired or invalid parameters. Please log in again.');</script>");
                    return;
                }

                int userId = Convert.ToInt32(Request.QueryString["userId"]);  // Get UserId from URL
                Guid coachId = Guid.Parse(Request.QueryString["coachId"]);  // Get CoachId from URL
                string senderType;

                if (Session["UserId"] != null)  // If a User is sending a message
                {
                    senderType = "User";
                }
                else if (Session["CoachId"] != null)  // If a Coach is sending a message
                {
                    senderType = "Coach";
                }
                else
                {
                    Response.Write("<script>alert('Session error. Please log in again.');</script>");
                    return;
                }

                string connStr = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;
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

                txtMessage.Text = "";
                LoadMessages();  // Ensure messages are loaded immediately
                UpdatePanelChat.Update(); // Refresh UI with the new message (if using AJAX)
            }
        }
    }
}
