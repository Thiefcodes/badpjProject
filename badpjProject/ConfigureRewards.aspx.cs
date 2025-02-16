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
                // Updated query to use StreakDays
                string query = "SELECT RewardId, RewardName, RewardImage, StreakDays FROM Rewards";
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
            // Validate that RewardName (which now represents points) is an integer.
            string pointsText = RewardNameTextBox.Text.Trim();
            if (!int.TryParse(pointsText, out int points))
            {
                // Display an error if the input is not a valid integer.
                Response.Write("<script>alert('Please enter a valid integer for points.');</script>");
                return;
            }

            // Get the streak days value from the textbox.
            string streakDays = StreakDaysTextBox.Text.Trim();  // Consider renaming your textbox ID to StreakDaysTextBox
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
                // Updated query to use StreakDays
                string query = "INSERT INTO Rewards (RewardName, RewardImage, StreakDays) VALUES (@RewardName, @RewardImage, @StreakDays)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    // Store the integer points as a string (if you're keeping the column type as NVARCHAR).
                    cmd.Parameters.AddWithValue("@RewardName", points.ToString());
                    cmd.Parameters.AddWithValue("@RewardImage", rewardImagePath);
                    cmd.Parameters.AddWithValue("@StreakDays", streakDays);

                    cmd.ExecuteNonQuery();
                }
            }

            LoadRewards();
            RewardNameTextBox.Text = string.Empty;
            StreakDaysTextBox.Text = string.Empty;
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
                // Updated query to use StreakDays
                string query = "SELECT RewardId, RewardName, RewardImage, StreakDays FROM Rewards WHERE RewardId = @RewardId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@RewardId", rewardId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            EditRewardId.Value = rewardId;
                            EditRewardNameTextBox.Text = reader["RewardName"].ToString();
                            EditStreakDaysTextBox.Text = reader["StreakDays"].ToString();
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
            string pointsText = EditRewardNameTextBox.Text.Trim();
            if (!int.TryParse(pointsText, out int points))
            {
                Response.Write("<script>alert('Please enter a valid integer for points.');</script>");
                return;
            }

            // Get the streak days value from the edit textbox.
            string streakDays = EditStreakDaysTextBox.Text.Trim();
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
                // Updated query to use StreakDays
                string query = "UPDATE Rewards SET RewardName = @RewardName, StreakDays = @StreakDays" +
                               (rewardImagePath != null ? ", RewardImage = @RewardImage" : "") +
                               " WHERE RewardId = @RewardId";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@RewardId", rewardId);
                    // Store the integer points as a string.
                    cmd.Parameters.AddWithValue("@RewardName", points.ToString());
                    cmd.Parameters.AddWithValue("@StreakDays", streakDays);

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
