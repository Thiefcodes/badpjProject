using System;
using System.Data.SqlClient;
using System.Data;
using System.Web.UI.WebControls;
using System.Configuration;

namespace badpjProject
{
    public partial class Forum : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Check the session role
                if (Session["Role"] != null && Session["Role"].ToString() == "Staff")
                {
                    // Make the Delete button visible if the role is "Admin"
                    gvThreads.Columns[5].Visible = true;  // Assuming the Delete button is in the 6th column (index 5)
                    gvThreads.Columns[6].Visible = true; // Assuming the Update button is in the 7th column(index 6)
                }
                else
                {
                    // Hide the Delete button for non-admin roles
                    gvThreads.Columns[5].Visible = false;
                    gvThreads.Columns[6].Visible = false;
                }
                LoadThreads();
            }
        }

        private void LoadThreads()
        {
            // Retrieve the connection string from the configuration file
            string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                // Create and execute the SQL command
                SqlCommand cmd = new SqlCommand("SELECT ThreadID, Title, CreatedBy, CreatedAt FROM Threads", conn);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                // Bind the data to the GridView
                gvThreads.DataSource = dt;
                gvThreads.DataBind();
            }
        }

        protected void gvThreads_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteThread")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                string threadId = gvThreads.Rows[index].Cells[0].Text;

                DeleteThread(threadId);
                LoadThreads();
            }
            else if (e.CommandName == "ViewThread")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                string threadId = gvThreads.Rows[index].Cells[0].Text;
                Response.Redirect($"Thread.aspx?ThreadID={threadId}");
            }
            else if (e.CommandName == "UpdateThread")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                string threadId = gvThreads.Rows[index].Cells[0].Text;

                // Redirect to an update page (or use inline editing as an alternative)
                Response.Redirect($"UpdateThread.aspx?ThreadID={threadId}");
            }
        }

        private void DeleteThread(string threadId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Threads WHERE ThreadID = @ThreadID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ThreadID", threadId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }


        protected void btnNewThread_Click(object sender, EventArgs e)
        {
            // Check if the user is logged in
            if (Session["UserId"] == null || Session["Username"] == null || Session["Role"] == null)
            {
                // Redirect to the login page if user is not logged in
                Response.Redirect("Login.aspx");
            }
            else
            {
                // Proceed with the current page as user is logged in
                // Continue with your page logic here
                Response.Redirect("NewThread.aspx");
            }
        }

    }
}