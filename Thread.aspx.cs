using System;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace badpjProject
{
    public partial class Thread : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadThread();
                LoadPosts();
            }

        }


        private void LoadThread()
        {
            string threadId = Request.QueryString["ThreadID"];
            string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {

                SqlCommand cmd = new SqlCommand("SELECT Title FROM Threads WHERE ThreadID = @ThreadID", conn);
                cmd.Parameters.AddWithValue("@ThreadID", threadId);
                conn.Open();
                lblThreadTitle.Text = (string)cmd.ExecuteScalar();
            }
        }

        private void LoadPosts()
        {
            string threadId = Request.QueryString["ThreadID"];
            string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT Content, CreatedBy, CreatedAt FROM Posts WHERE ThreadID = @ThreadID", conn);
                cmd.Parameters.AddWithValue("@ThreadID", threadId);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                gvPosts.DataSource = dt;
                gvPosts.DataBind();
            }
        }

        protected void btnReply_Click(object sender, EventArgs e)
        {
            string threadId = Request.QueryString["ThreadID"];
            Response.Redirect($"NewPost.aspx?ThreadID={threadId}");
        }
    }
}