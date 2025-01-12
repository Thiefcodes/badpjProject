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
            if (e.CommandName == "ViewThread")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                string threadId = gvThreads.Rows[index].Cells[0].Text;
                Response.Redirect($"Thread.aspx?ThreadID={threadId}");
            }
        }

        protected void btnNewThread_Click(object sender, EventArgs e)
        {
            Response.Redirect("NewThread.aspx");
        }

        protected void gvThreads_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Get the selected row
            GridViewRow selectedRow = gvThreads.SelectedRow;

            // Retrieve the ThreadID from the first cell (adjust index as needed)
            int threadId = Convert.ToInt32(selectedRow.Cells[0].Text);

            // Redirect to the Thread page with the selected ThreadID
            Response.Redirect($"Thread.aspx?ThreadID={threadId}");
        }
    }
}