using System;
using System.Data.SqlClient;
using System.Configuration;

namespace badpjProject
{
    public partial class AllProfiles : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadAllProfiles(); // Load without search term initially
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            // When the search button is clicked, reload profiles with the search term.
            string searchTerm = txtSearch.Text.Trim();
            LoadAllProfiles(searchTerm);
        }

        /// <summary>
        /// Loads all user profiles, optionally filtering and prioritizing matches based on searchTerm.
        /// </summary>
        /// <param name="searchTerm">The term to search for (optional).</param>
        private void LoadAllProfiles(string searchTerm = "")
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "";

                if (string.IsNullOrEmpty(searchTerm))
                {
                    // No search term provided: simply list all user profiles.
                    query = "SELECT Id, Login_Name, Email FROM [Table] WHERE Role = 'User' ORDER BY Login_Name ASC";
                }
                else
                {
                    // With a search term: include a ranking to prioritize exact or beginning matches.
                    query = @"
                        SELECT Id, Login_Name, Email,
                            CASE 
                                WHEN Login_Name LIKE @SearchExact THEN 1 
                                ELSE 0 
                            END AS ExactMatch,
                            CASE 
                                WHEN Login_Name LIKE @SearchPartial THEN 1 
                                ELSE 0 
                            END AS PartialMatch
                        FROM [Table]
                        WHERE Role = 'User'
                          AND (Login_Name LIKE @SearchPartial OR Email LIKE @SearchPartial)
                        ORDER BY ExactMatch DESC, PartialMatch DESC, Login_Name ASC";
                }

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (!string.IsNullOrEmpty(searchTerm))
                    {
                        // Define parameters to search for users whose names start with the search term (exact match)
                        // or contain the search term anywhere (partial match).
                        cmd.Parameters.AddWithValue("@SearchExact", searchTerm + "%");
                        cmd.Parameters.AddWithValue("@SearchPartial", "%" + searchTerm + "%");
                    }

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
