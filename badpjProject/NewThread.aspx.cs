using System;
using System.Configuration;
using System.Data.SqlClient;

namespace badpjProject
{
    public partial class NewThread : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnCreate_Click(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO Threads (Title, CreatedBy, CreatedAt) VALUES (@Title, @CreatedBy, @CreatedAt)", conn);
                cmd.Parameters.AddWithValue("@Title", txtTitle.Text);
                cmd.Parameters.AddWithValue("@CreatedBy", Session["UserId"].ToString());  // Use Session["UserId"]
                cmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
            Response.Redirect("Forum.aspx");
        }
    }
}