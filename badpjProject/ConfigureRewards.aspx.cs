using System;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Web.UI.WebControls;

namespace badpjProject
{
    public partial class ConfigureRewards : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadRewards();
            }
        }

        private void LoadRewards()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT RewardId, RewardName, RewardImage, StreakHours FROM Rewards";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        RewardsRepeater.DataSource = reader;
                        RewardsRepeater.DataBind();
                    }
                }
            }
        }

        protected void AddRewardButton_Click(object sender, EventArgs e)
        {
            string rewardName = RewardNameTextBox.Text.Trim();
            string streakHours = StreakHoursTextBox.Text.Trim();
            string rewardImagePath = "~/Images/default-reward.png";

            if (RewardImageUpload.HasFile)
            {
                string fileName = Path.GetFileName(RewardImageUpload.PostedFile.FileName);
                rewardImagePath = "~/Uploads/" + fileName;
                RewardImageUpload.SaveAs(Server.MapPath(rewardImagePath));
            }

            string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO Rewards (RewardName, RewardImage, StreakHours) VALUES (@RewardName, @RewardImage, @StreakHours)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@RewardName", rewardName);
                    cmd.Parameters.AddWithValue("@RewardImage", rewardImagePath);
                    cmd.Parameters.AddWithValue("@StreakHours", streakHours);

                    cmd.ExecuteNonQuery();
                }
            }

            // Refresh the rewards list
            LoadRewards();

            // Clear form
            RewardNameTextBox.Text = string.Empty;
            StreakHoursTextBox.Text = string.Empty;
        }

        protected void EditRewardButton_Click(object sender, EventArgs e)
        {
            // Logic to handle editing (not included in this version)
        }

        protected void DeleteRewardButton_Click(object sender, EventArgs e)
        {
            string rewardId = (sender as Button)?.CommandArgument;

            string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "DELETE FROM Rewards WHERE RewardId = @RewardId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@RewardId", rewardId);
                    cmd.ExecuteNonQuery();
                }
            }

            // Refresh the rewards list
            LoadRewards();
        }
    }
}
