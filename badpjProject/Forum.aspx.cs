using System;
using System.Data.SqlClient;
using System.Data;
using System.Web.UI.WebControls;
using System.Configuration;

namespace badpjProject
{
    public partial class Forum : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                this.MasterPageFile = "~/Site.Master";
            }
            else
            {
                this.MasterPageFile = "~/Site1loggedin.Master";
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Check the session role
                if (Session["Role"] != null && Session["Role"].ToString() == "Staff")
                {
                    // Make the Delete button visible if the role is "Admin"
                    gvThreads.Columns[6].Visible = true;  // Assuming the Delete button is in the 7th column (index 6)
                    gvThreads.Columns[7].Visible = true; // Assuming the Update button is in the 8th column(index 7)
                }
                else
                {
                    // Hide the Delete button for non-admin roles
                    gvThreads.Columns[6].Visible = false;
                    gvThreads.Columns[7].Visible = false;
                }
                LoadThreads();
            }
        }


        private void LoadThreads()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                // Fix syntax error by enclosing "Table" in square brackets
                string query = @"
                 SELECT t.ThreadID, t.Title, 
                 COALESCE(u.Login_Name, 'Unknown') AS CreatedBy, 
                 t.CreatedAt,
                 t.Views   -- Added Views column
                 FROM Threads t
                 LEFT JOIN [Table] u ON t.CreatedBy = u.Id";

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                gvThreads.DataSource = dt;
                gvThreads.DataBind();
            }
        }

        protected void gvThreads_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            string threadId = gvThreads.Rows[index].Cells[0].Text;

            if (e.CommandName == "ViewThread")
            {
                IncrementThreadViews(threadId);
                Response.Redirect($"Thread.aspx?ThreadID={threadId}");
            }
            else if (e.CommandName == "UpdateThread")
            {
                Response.Redirect($"UpdateThread.aspx?ThreadID={threadId}");
            }
            else if (e.CommandName == "DeleteThread")
            {
                DeleteThread(threadId);
                LoadThreads();
            }
        }

        private void IncrementThreadViews(string threadId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "UPDATE Threads SET Views = Views + 1 WHERE ThreadID = @ThreadID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ThreadID", threadId);

                conn.Open();
                cmd.ExecuteNonQuery();
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
                Response.Write("<script>alert('Thread deleted successfully');</script>");

            }
        }


        protected void btnNewThread_Click(object sender, EventArgs e)
        {
            // Check if the user is logged in
            if (Session["UserId"] == null || Session["Username"] == null || Session["Role"] == null)
            {
                // Redirect to the login page if user is not logged in
                Response.Write("<script>alert('Please log in first!'); window.location='Login.aspx';</script>");
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