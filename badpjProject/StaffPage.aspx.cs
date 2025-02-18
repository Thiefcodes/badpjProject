using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;
using System.Web.UI;

namespace badpjProject
{
    public partial class StaffPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Role"]?.ToString() != "Staff")
            {
                Response.Redirect("Login.aspx");
            }

            if (!IsPostBack)
            {
                LoadStaffData();
            }
        }

        private void LoadStaffData()
        {
            string username = Session["Username"]?.ToString();

            if (string.IsNullOrEmpty(username))
            {
                Response.Redirect("Login.aspx"); // Redirect to login if session is invalid
                return;
            }

            string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT Login_Name, Email, Role, ProfilePicture FROM [dbo].[Table] WHERE Login_Name = @Login_Name AND Role = 'Staff'";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Login_Name", username);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Set staff profile details
                            StaffNameLabel.Text = reader["Login_Name"].ToString();
                            StaffEmailLabel.Text = reader["Email"].ToString();

                            // Set profile picture
                            string profilePicturePath = reader["ProfilePicture"]?.ToString();
                            if (!string.IsNullOrEmpty(profilePicturePath))
                            {
                                ProfilePicture.ImageUrl = profilePicturePath;
                            }
                            else
                            {
                                ProfilePicture.ImageUrl = "~/Images/default-profile.png"; // Default profile picture
                            }
                        }
                        else
                        {
                            // Handle case where staff data is not found
                            Response.Redirect("ErrorPage.aspx"); // Optional: Redirect to an error page
                        }
                    }
                }
            }
        }

        protected void ManageStaffButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("ManageStaff.aspx");
        }

        protected void ManageUsersButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("ManageUsers.aspx");
        }

        protected void EditProfileButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("EditProfilePage.aspx");
        }

        protected void ConfigureRewardsButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("ConfigureRewards.aspx");
        }

        // Event handler for the new "Enable Facial Authentication" button
        protected void EnableFacialAuthButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("EnableFacialAuthentication.aspx");
        }

        // New event handler to export transactional ledger data to CSV
        protected void ExportTransactionsButton_Click(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;
            StringBuilder csvData = new StringBuilder();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"SELECT TransactionId, UserId, TransactionType, PointsChanged, TransactionDate, PreviousHash, CurrentHash 
                                 FROM PointsTransactionLedger 
                                 ORDER BY TransactionId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Write CSV header
                        csvData.AppendLine("TransactionId,UserId,TransactionType,PointsChanged,TransactionDate,PreviousHash,CurrentHash");

                        while (reader.Read())
                        {
                            // Build a CSV row
                            string transactionId = reader["TransactionId"].ToString();
                            string userId = reader["UserId"].ToString();
                            string transactionType = reader["TransactionType"].ToString();
                            string pointsChanged = reader["PointsChanged"].ToString();
                            string transactionDate = Convert.ToDateTime(reader["TransactionDate"]).ToString("yyyy-MM-dd HH:mm:ss");
                            string previousHash = reader["PreviousHash"].ToString();
                            string currentHash = reader["CurrentHash"].ToString();

                            string row = $"{transactionId},{userId},{transactionType},{pointsChanged},{transactionDate},{previousHash},{currentHash}";
                            csvData.AppendLine(row);
                        }
                    }
                }
            }

            // Set up the response to download the CSV file
            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("Content-Disposition", "attachment; filename=Transactions.csv");
            Response.Write(csvData.ToString());
            Response.End();
        }
    }
}
