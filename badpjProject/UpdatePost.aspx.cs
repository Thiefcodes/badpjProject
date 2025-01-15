using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace badpjProject
{
    public partial class UpdatePost : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadPost();
            }
        }

        private void LoadPost()
        {
            string postId = Request.QueryString["PostID"];
            string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT Content FROM Posts WHERE PostID = @PostID AND IsDeleted = 0";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@PostID", postId);
                conn.Open();
                txtContent.Text = cmd.ExecuteScalar()?.ToString();
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            string postId = Request.QueryString["PostID"];
            string content = txtContent.Text.Trim();
            string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "UPDATE Posts SET Content = @Content WHERE PostID = @PostID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Content", content);
                cmd.Parameters.AddWithValue("@PostID", postId);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            Response.Write($"<script>alert('Post Updated Successfully'); window.location='Thread.aspx?ThreadID={Request.QueryString["ThreadID"]}';</script>");

        }
    }
}