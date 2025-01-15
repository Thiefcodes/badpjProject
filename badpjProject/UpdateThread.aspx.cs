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
    public partial class UpdateThread : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string threadId = Request.QueryString["ThreadID"];
                string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT Title FROM Threads WHERE ThreadID = @ThreadID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ThreadID", threadId);

                    conn.Open();
                    object title = cmd.ExecuteScalar();
                    conn.Close();

                    if (title != null)
                    {
                        txtTitle.Text = title.ToString();
                    }
                }
            }

        }
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            string threadId = Request.QueryString["ThreadID"];
            string newTitle = txtTitle.Text.Trim();
            string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "UPDATE Threads SET Title = @Title WHERE ThreadID = @ThreadID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Title", newTitle);
                cmd.Parameters.AddWithValue("@ThreadID", threadId);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                Response.Write("<script>alert('Thread Updated Successfully'); window.location='Thread.aspx?ThreadID=" + threadId + "';</script>");

            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            string threadId = Request.QueryString["ThreadID"];
            Response.Redirect($"Thread.aspx?ThreadID={threadId}");
        }
    }
}