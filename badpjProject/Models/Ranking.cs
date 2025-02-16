using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.IO;

namespace badpjProject
{
    public class Ranking
    {
        private string _connStr = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;
        private Guid _rankingID;
        private int _userID;
        private int _totalPoints;
        private string _rank;
        private DateTime _lastUpdated;

        public Ranking()
        {
            _rankingID = Guid.NewGuid();
            _totalPoints = 0;
            _rank = "Beginner";
            _lastUpdated = DateTime.Now;
        }

        public Ranking(Guid rankingID, int userID, int totalPoints, string rank, DateTime lastUpdated)
        {
            _rankingID = rankingID;
            _userID = userID;
            _totalPoints = totalPoints;
            _rank = rank;
            _lastUpdated = lastUpdated;
        }

        // Properties (getters and setters)
        public Guid RankingID { get { return _rankingID; } set { _rankingID = value; } }
        public int UserID { get { return _userID; } set { _userID = value; } }
        public int TotalPoints { get { return _totalPoints; } set { _totalPoints = value; } }
        public string Rank { get { return _rank; } set { _rank = value; } }
        public DateTime LastUpdated { get { return _lastUpdated; } set { _lastUpdated = value; } }
        public string Username { get; set; }

        // Inserts a new ranking record for the user.
        public int InsertRanking()
        {
            int result = 0;
            string queryStr = "INSERT INTO Ranking (RankingID, UserID, TotalPoints, Rank, LastUpdated) " +
                              "VALUES (@RankingID, @UserID, @TotalPoints, @Rank, @LastUpdated)";

            using (SqlConnection conn = new SqlConnection(_connStr))
            using (SqlCommand cmd = new SqlCommand(queryStr, conn))
            {
                cmd.Parameters.AddWithValue("@RankingID", _rankingID);
                cmd.Parameters.AddWithValue("@UserID", _userID);
                cmd.Parameters.AddWithValue("@TotalPoints", _totalPoints);
                cmd.Parameters.AddWithValue("@Rank", _rank);
                cmd.Parameters.AddWithValue("@LastUpdated", _lastUpdated);

                try
                {
                    conn.Open();
                    result = cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    Debug.WriteLine("SQL Error in InsertRanking: " + ex.Message);
                    throw;
                }
            }
            return result;
        }

        // Updates the ranking record by adding additional points and recalculates the rank.
        public int UpdateRanking(int additionalPoints)
        {
            _totalPoints += additionalPoints;
            _lastUpdated = DateTime.Now;

            // Example rank thresholds (customize as needed)
            if (_totalPoints >= 1000)
                _rank = "Elite";
            else if (_totalPoints >= 500)
                _rank = "Advanced";
            else if (_totalPoints >= 100)
                _rank = "Intermediate";
            else
                _rank = "Beginner";

            int result = 0;
            string queryStr = "UPDATE Ranking SET TotalPoints = @TotalPoints, Rank = @Rank, LastUpdated = @LastUpdated " +
                              "WHERE UserID = @UserID";

            using (SqlConnection conn = new SqlConnection(_connStr))
            using (SqlCommand cmd = new SqlCommand(queryStr, conn))
            {
                cmd.Parameters.AddWithValue("@TotalPoints", _totalPoints);
                cmd.Parameters.AddWithValue("@Rank", _rank);
                cmd.Parameters.AddWithValue("@LastUpdated", _lastUpdated);
                cmd.Parameters.AddWithValue("@UserID", _userID);

                try
                {
                    conn.Open();
                    result = cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    Debug.WriteLine("SQL Error in UpdateRanking: " + ex.Message);
                    throw;
                }
            }
            return result;
        }

        public static int DeleteRankingByUserId(int userId)
        {
            int result = 0;
            string connStr = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;
            string queryStr = "DELETE FROM Ranking WHERE UserID = @UserID";

            using (SqlConnection conn = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand(queryStr, conn))
            {
                cmd.Parameters.AddWithValue("@UserID", userId);
                try
                {
                    conn.Open();
                    result = cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    Debug.WriteLine("SQL Error in DeleteRankingByUserId: " + ex.Message);
                }
            }
            return result;
        }

        // Getter: Retrieve a Ranking record for a specific user based on UserID.
        public static Ranking GetRankingByUserId(int userId)
        {
            Ranking ranking = null;
            string connStr = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;
            string queryStr = "SELECT RankingID, UserID, TotalPoints, Rank, LastUpdated FROM Ranking WHERE UserID = @UserID";

            using (SqlConnection conn = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand(queryStr, conn))
            {
                cmd.Parameters.AddWithValue("@UserID", userId);

                try
                {
                    conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        ranking = new Ranking(
                            Guid.Parse(dr["RankingID"].ToString()),
                            int.Parse(dr["UserID"].ToString()),
                            int.Parse(dr["TotalPoints"].ToString()),
                            dr["Rank"].ToString(),
                            DateTime.Parse(dr["LastUpdated"].ToString())
                        );
                    }
                    dr.Close();
                }
                catch (SqlException ex)
                {
                    Debug.WriteLine("SQL Error in GetRankingByUserId: " + ex.Message);
                }
            }
            return ranking;
        }

        public static List<Ranking> GetAllRankings()
        {
            List<Ranking> list = new List<Ranking>();
            string connStr = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;

            // Join the Ranking table with the user table to fetch the Login_Name.
            string queryStr = @"SELECT r.RankingID, r.UserID, r.TotalPoints, r.Rank, r.LastUpdated, u.Login_Name 
                        FROM Ranking r 
                        INNER JOIN [Table] u ON r.UserID = u.Id 
                        ORDER BY r.TotalPoints DESC";

            using (SqlConnection conn = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand(queryStr, conn))
            {
                try
                {
                    conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        Ranking ranking = new Ranking
                        {
                            RankingID = Guid.Parse(dr["RankingID"].ToString()),
                            UserID = int.Parse(dr["UserID"].ToString()),
                            TotalPoints = int.Parse(dr["TotalPoints"].ToString()),
                            Rank = dr["Rank"].ToString(),
                            LastUpdated = DateTime.Parse(dr["LastUpdated"].ToString()),
                            Username = dr["Login_Name"].ToString() // Using Login_Name as the username
                        };
                        list.Add(ranking);
                    }
                    dr.Close();
                }
                catch (SqlException ex)
                {
                    Debug.WriteLine("SQL Error in GetAllRankings: " + ex.Message);
                }
            }
            return list;
        }
    }

    public class RankingVideoSubmission
    {
        private string _connStr = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;
        private Guid _submissionID;
        private int _userID;
        private string _videoFile;
        private string _comment; // New field for comment
        private string _status;
        private DateTime _submissionDate;
        private int _assignedPoints;

        public RankingVideoSubmission()
        {
            _submissionID = Guid.NewGuid();
            _status = "Pending";
            _submissionDate = DateTime.Now;
            _comment = string.Empty;
            _assignedPoints = 0;
        }

        public RankingVideoSubmission(Guid submissionID, int userID, string videoFile, string comment, string status, DateTime submissionDate, int assignedPoints)
        {
            _submissionID = submissionID;
            _userID = userID;
            _videoFile = videoFile;
            _comment = comment;
            _status = status;
            _submissionDate = submissionDate;
            _assignedPoints = assignedPoints;
        }

        // Properties (getters and setters)
        public Guid SubmissionID { get { return _submissionID; } set { _submissionID = value; } }
        public int UserID { get { return _userID; } set { _userID = value; } }
        public string VideoFile { get { return _videoFile; } set { _videoFile = value; } }
        public string Comment { get { return _comment; } set { _comment = value; } }  // New property
        public string Status { get { return _status; } set { _status = value; } }
        public DateTime SubmissionDate { get { return _submissionDate; } set { _submissionDate = value; } }
        public int AssignedPoints { get { return _assignedPoints; } set { _assignedPoints = value; } }

        // Inserts a new video submission record (now includes the comment).
        public int InsertSubmission()
        {
            int result = 0;
            string queryStr = "INSERT INTO RankingVideoSubmission (SubmissionID, UserID, VideoFile, Comment, Status, SubmissionDate, AssignedPoints) " +
                              "VALUES (@SubmissionID, @UserID, @VideoFile, @Comment, @Status, @SubmissionDate, @AssignedPoints)";
            using (SqlConnection conn = new SqlConnection(_connStr))
            using (SqlCommand cmd = new SqlCommand(queryStr, conn))
            {
                cmd.Parameters.AddWithValue("@SubmissionID", _submissionID);
                cmd.Parameters.AddWithValue("@UserID", _userID);
                cmd.Parameters.AddWithValue("@VideoFile", _videoFile);
                cmd.Parameters.AddWithValue("@Comment", _comment);
                cmd.Parameters.AddWithValue("@Status", _status);
                cmd.Parameters.AddWithValue("@SubmissionDate", _submissionDate);
                cmd.Parameters.AddWithValue("@AssignedPoints", _assignedPoints);
                try
                {
                    conn.Open();
                    result = cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    Debug.WriteLine("SQL Error in InsertSubmission: " + ex.Message);
                    throw;
                }
            }
            return result;
        }

        // Updates the submission status.
        public int UpdateSubmissionStatus(string newStatus)
        {
            int result = 0;
            string queryStr = "UPDATE RankingVideoSubmission SET Status = @Status WHERE SubmissionID = @SubmissionID";

            using (SqlConnection conn = new SqlConnection(_connStr))
            using (SqlCommand cmd = new SqlCommand(queryStr, conn))
            {
                cmd.Parameters.AddWithValue("@Status", newStatus);
                cmd.Parameters.AddWithValue("@SubmissionID", _submissionID);

                try
                {
                    conn.Open();
                    result = cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    Debug.WriteLine("SQL Error in UpdateSubmissionStatus: " + ex.Message);
                    throw;
                }
            }
            return result;
        }

        public static List<RankingVideoSubmission> GetSubmissionsByStatus(string status, string sortField)
        {
            List<RankingVideoSubmission> submissions = new List<RankingVideoSubmission>();
            string connStr = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;
            string queryStr = "SELECT SubmissionID, UserID, VideoFile, Comment, Status, SubmissionDate, AssignedPoints FROM RankingVideoSubmission WHERE Status = @Status";

            // Append ORDER BY clause based on sortField.
            if (sortField == "SubmissionDate")
            {
                queryStr += " ORDER BY SubmissionDate DESC";
            }
            else if (sortField == "UserID")
            {
                queryStr += " ORDER BY UserID ASC";
            }

            using (SqlConnection conn = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand(queryStr, conn))
            {
                cmd.Parameters.AddWithValue("@Status", status);
                try
                {
                    conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        RankingVideoSubmission submission = new RankingVideoSubmission(
                            Guid.Parse(dr["SubmissionID"].ToString()),
                            int.Parse(dr["UserID"].ToString()),
                            dr["VideoFile"].ToString(),
                            dr["Comment"].ToString(),
                            dr["Status"].ToString(),
                            DateTime.Parse(dr["SubmissionDate"].ToString()),
                            int.Parse(dr["AssignedPoints"].ToString())
                        );
                        submissions.Add(submission);
                    }
                    dr.Close();
                }
                catch (SqlException ex)
                {
                    Debug.WriteLine("SQL Error in GetSubmissionsByStatus: " + ex.Message);
                }
            }
            return submissions;
        }


        // Getter: Retrieve a submission record by its SubmissionID.
        public static RankingVideoSubmission GetSubmissionById(Guid submissionId)
        {
            RankingVideoSubmission submission = null;
            string connStr = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;
            string queryStr = "SELECT SubmissionID, UserID, VideoFile, Comment, Status, SubmissionDate, AssignedPoints FROM RankingVideoSubmission WHERE SubmissionID = @SubmissionID";

            using (SqlConnection conn = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand(queryStr, conn))
            {
                cmd.Parameters.AddWithValue("@SubmissionID", submissionId);

                try
                {
                    conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        submission = new RankingVideoSubmission(
                            Guid.Parse(dr["SubmissionID"].ToString()),
                            int.Parse(dr["UserID"].ToString()),
                            dr["VideoFile"].ToString(),
                            dr["Comment"].ToString(),
                            dr["Status"].ToString(),
                            DateTime.Parse(dr["SubmissionDate"].ToString()),
                            int.Parse(dr["AssignedPoints"].ToString())
                        );
                    }
                    dr.Close();
                }
                catch (SqlException ex)
                {
                    Debug.WriteLine("SQL Error in GetSubmissionById: " + ex.Message);
                }
            }
            return submission;
        }

        public int ApproveSubmission(int points)
        {
            int result = 0;
            // Update both the status and the assigned points.
            string queryStr = "UPDATE RankingVideoSubmission SET Status = 'Approved', AssignedPoints = @Points WHERE SubmissionID = @SubmissionID";
            using (SqlConnection conn = new SqlConnection(_connStr))
            using (SqlCommand cmd = new SqlCommand(queryStr, conn))
            {
                cmd.Parameters.AddWithValue("@Points", points);
                cmd.Parameters.AddWithValue("@SubmissionID", _submissionID);
                try
                {
                    conn.Open();
                    result = cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    Debug.WriteLine("SQL Error in ApproveSubmission: " + ex.Message);
                }
            }
            return result;
        }

        public static int DeleteSubmissionById(Guid submissionId)
        {
            int result = 0;
            string videoFile = null;

            // Retrieve the VideoFile from the RankingVideoSubmission record.
            string connStr = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand("SELECT VideoFile FROM RankingVideoSubmission WHERE SubmissionID = @SubmissionID", conn))
            {
                cmd.Parameters.AddWithValue("@SubmissionID", submissionId);
                try
                {
                    conn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            videoFile = dr["VideoFile"]?.ToString();
                        }
                    }
                }
                catch (SqlException ex)
                {
                    Debug.WriteLine("SQL Error while fetching video file: " + ex.Message);
                    return 0;
                }
            }

            // Delete the video file if it exists.
            if (!string.IsNullOrEmpty(videoFile))
            {
                try
                {
                    string videoPath = HttpContext.Current.Server.MapPath("~/Uploads/" + videoFile);
                    if (File.Exists(videoPath))
                    {
                        File.Delete(videoPath);
                        Debug.WriteLine($"Video file deleted: {videoPath}");
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error deleting video file: " + ex.Message);
                    return 0;
                }
            }

            // Delete the submission record from the database.
            using (SqlConnection conn = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand("DELETE FROM RankingVideoSubmission WHERE SubmissionID = @SubmissionID", conn))
            {
                cmd.Parameters.AddWithValue("@SubmissionID", submissionId);
                try
                {
                    conn.Open();
                    result = cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    Debug.WriteLine("SQL Error in DeleteSubmissionById: " + ex.Message);
                    return 0;
                }
            }

            return result;
        }
    }
}