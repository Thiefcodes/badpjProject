using System;
using System.Configuration;
using System.Data.SqlClient;
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
            string rewardImagePath = "~/Uploads/default-reward.png";

            if (RewardImageUpload.HasFile)
            {
                string fileName = Path.GetFileName(RewardImageUpload.PostedFile.FileName);
                string savePath = Server.MapPath("~/Uploads/" + fileName);
                RewardImageUpload.SaveAs(savePath);
                rewardImagePath = "~/Uploads/" + fileName;
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

            LoadRewards();
            RewardNameTextBox.Text = string.Empty;
            StreakHoursTextBox.Text = string.Empty;
        }

        protected void RewardsRepeater_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "EditReward")
            {
                string rewardId = e.CommandArgument.ToString();
                LoadRewardForEdit(rewardId);
            }
            else if (e.CommandName == "DeleteReward")
            {
                string rewardId = e.CommandArgument.ToString();
                DeleteReward(rewardId);
            }
        }

        private void LoadRewardForEdit(string rewardId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT RewardId, RewardName, RewardImage, StreakHours FROM Rewards WHERE RewardId = @RewardId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@RewardId", rewardId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            EditRewardId.Value = rewardId;
                            EditRewardNameTextBox.Text = reader["RewardName"].ToString();
                            EditStreakHoursTextBox.Text = reader["StreakHours"].ToString();
                        }
                    }
                }
            }

            // Show the edit section
            EditRewardSection.Style["display"] = "block";
        }


        protected void UpdateRewardButton_Click(object sender, EventArgs e)
        {
            string rewardId = EditRewardId.Value;
            string rewardName = EditRewardNameTextBox.Text.Trim();
            string streakHours = EditStreakHoursTextBox.Text.Trim();
            string rewardImagePath = null;

            if (EditRewardImageUpload.HasFile)
            {
                string fileName = Path.GetFileName(EditRewardImageUpload.PostedFile.FileName);
                string savePath = Server.MapPath("~/Uploads/" + fileName);
                EditRewardImageUpload.SaveAs(savePath);
                rewardImagePath = "~/Uploads/" + fileName;
            }

            string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE Rewards SET RewardName = @RewardName, StreakHours = @StreakHours" +
                               (rewardImagePath != null ? ", RewardImage = @RewardImage" : "") +
                               " WHERE RewardId = @RewardId";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@RewardId", rewardId);
                    cmd.Parameters.AddWithValue("@RewardName", rewardName);
                    cmd.Parameters.AddWithValue("@StreakHours", streakHours);

                    if (rewardImagePath != null)
                    {
                        cmd.Parameters.AddWithValue("@RewardImage", rewardImagePath);
                    }

                    cmd.ExecuteNonQuery();
                }
            }

            LoadRewards();
            EditRewardSection.Style["display"] = "none";
        }

        private void DeleteReward(string rewardId)
        {
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

            LoadRewards();
        }
    }
}


