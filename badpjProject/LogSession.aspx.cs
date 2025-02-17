using System;
using System.Configuration;
using System.Data.SqlClient;

namespace badpjProject
{
    public partial class LogSession : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Ensure the user is logged in and has the 'User' role.
            if (Session["UserId"] == null || Session["Role"] == null || Session["Role"].ToString() != "User")
            {
                Response.Write("<script>alert('Access Denied: Only users can log workout sessions.');window.location='Login.aspx';</script>");
                Response.End();
            }
        }

        protected void btnLogWorkout_Click(object sender, EventArgs e)
        {
            int userId = Convert.ToInt32(Session["UserId"]);
            ProcessWorkoutSession(userId);
            lblMessage.Text = "Workout session logged successfully!";
        }

        /// <summary>
        /// Processes a workout session using the current day (DateTime.Today).
        /// </summary>
        private void ProcessWorkoutSession(int userId)
        {
            DateTime today = DateTime.Today;
            ProcessWorkoutSessionForDate(userId, today);
        }

        /// <summary>
        /// Processes a workout session for a specified date.
        /// This method contains the common logic used for both normal and simulated sessions.
        /// </summary>
        private void ProcessWorkoutSessionForDate(int userId, DateTime sessionDate)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Retrieve current workout progress for the user.
                string selectProgress = @"SELECT CurrentStreak, LastWorkoutDate, TotalWorkouts 
                                          FROM UserWorkoutProgress 
                                          WHERE UserId = @UserId";
                int currentStreak = 0;
                DateTime? lastWorkoutDate = null;
                int totalWorkouts = 0;

                using (SqlCommand cmd = new SqlCommand(selectProgress, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            currentStreak = Convert.ToInt32(reader["CurrentStreak"]);
                            lastWorkoutDate = reader["LastWorkoutDate"] as DateTime?;
                            totalWorkouts = Convert.ToInt32(reader["TotalWorkouts"]);
                        }
                    }
                }

                // Use the provided sessionDate (which is either DateTime.Today or a simulated date)
                // Check if the previous workout was the day before (or on the same day, if multiple sessions are allowed).
                if (lastWorkoutDate.HasValue && lastWorkoutDate.Value.Date >= sessionDate.AddDays(-1))
                {
                    currentStreak += 1;
                }
                else
                {
                    currentStreak = 1;
                }
                totalWorkouts += 1;

                // Update or insert the user's progress.
                string updateProgress = @"
                    IF EXISTS(SELECT 1 FROM UserWorkoutProgress WHERE UserId = @UserId)
                        UPDATE UserWorkoutProgress
                        SET CurrentStreak = @CurrentStreak,
                            LastWorkoutDate = @SessionDate,
                            TotalWorkouts = @TotalWorkouts
                        WHERE UserId = @UserId
                    ELSE
                        INSERT INTO UserWorkoutProgress (UserId, CurrentStreak, LastWorkoutDate, TotalWorkouts)
                        VALUES (@UserId, @CurrentStreak, @SessionDate, @TotalWorkouts)";
                using (SqlCommand cmd = new SqlCommand(updateProgress, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@CurrentStreak", currentStreak);
                    cmd.Parameters.AddWithValue("@SessionDate", sessionDate);
                    cmd.Parameters.AddWithValue("@TotalWorkouts", totalWorkouts);
                    cmd.ExecuteNonQuery();
                }

                // Check if the updated streak meets a reward milestone.
                // Now using the StreakDays field from Rewards (assuming that's set up to match streak days).
                string selectReward = "SELECT RewardName FROM Rewards WHERE StreakDays = @CurrentStreak";
                string rewardPointsText = "";
                using (SqlCommand cmd = new SqlCommand(selectReward, conn))
                {
                    cmd.Parameters.AddWithValue("@CurrentStreak", currentStreak);
                    object obj = cmd.ExecuteScalar();
                    if (obj != null)
                    {
                        rewardPointsText = obj.ToString();
                    }
                }

                // If a milestone is reached, award points.
                if (!string.IsNullOrEmpty(rewardPointsText))
                {
                    int pointsToAward = 0;
                    if (!int.TryParse(rewardPointsText, out pointsToAward))
                    {
                        // Use a default value if RewardName cannot be parsed as an integer.
                        pointsToAward = 100;
                    }
                    string updateUser = @"UPDATE [dbo].[Table]
                                          SET RewardPoints = ISNULL(RewardPoints, 0) + @Points
                                          WHERE Id = @UserId";
                    using (SqlCommand cmd = new SqlCommand(updateUser, conn))
                    {
                        cmd.Parameters.AddWithValue("@Points", pointsToAward);
                        cmd.Parameters.AddWithValue("@UserId", userId);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        /// <summary>
        /// Event handler for the simulation button to log a session for a future date.
        /// </summary>
        protected void btnSimulateForward_Click(object sender, EventArgs e)
        {
            int daysToAdd = 0;
            if (int.TryParse(txtDaysToSimulate.Text.Trim(), out daysToAdd) && daysToAdd > 0)
            {
                int userId = Convert.ToInt32(Session["UserId"]);
                DateTime simulatedDate = DateTime.Today.AddDays(daysToAdd);
                ProcessWorkoutSessionForDate(userId, simulatedDate);
                lblMessage.Text = $"Workout session simulated for {simulatedDate.ToShortDateString()}!";
            }
            else
            {
                lblMessage.Text = "Please enter a valid number of days (greater than 0) to simulate.";
            }
        }
    }
}
