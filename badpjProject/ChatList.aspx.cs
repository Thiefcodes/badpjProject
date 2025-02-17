using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;

namespace badpjProject
{
    public partial class ChatList : System.Web.UI.Page
    {
        protected Guid CoachId;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["coachId"] != null)
                {
                    CoachId = Guid.Parse(Request.QueryString["coachId"]);
                    Session["CoachId"] = CoachId.ToString();
                    LoadUserChats();
                }
                else if (Session["CoachId"] != null)
                {
                    CoachId = Guid.Parse(Session["CoachId"].ToString());
                    LoadUserChats();
                }
                else
                {
                    Response.Redirect("Login.aspx"); 
                }
            }
        }


        private void LoadUserChats()
        {
            string connStr = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                    SELECT DISTINCT u.Id AS UserId, u.Login_Name AS UserName, c.CoachId
                    FROM Chat c
                    INNER JOIN [Table] u ON c.UserId = u.Id
                    WHERE c.CoachId = @CoachId
                    ORDER BY u.Login_Name ASC";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CoachId", CoachId);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                DataTable dt = new DataTable();
                dt.Load(reader);

                rptUsers.DataSource = dt;
                rptUsers.DataBind();
            }
        }
    }
}
