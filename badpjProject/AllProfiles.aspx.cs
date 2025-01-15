using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace badpjProject
{
    public partial class AllProfiles : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadAllProfiles();
            }
        }

        private void LoadAllProfiles()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT Id, Login_Name, Email FROM [Table] ORDER BY Login_Name ASC";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        ProfilesRepeater.DataSource = reader;
                        ProfilesRepeater.DataBind();
                    }
                }
            }
        }
    }
}
