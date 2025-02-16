using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Diagnostics;
using System.IO;

namespace badpjProject
{
    public partial class Coaches
    {
        string _connStr = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;
        private string _coachID = null;
        private string _coachName = "";
        private string _coachEmail = "";
        private int _coachHp = 0;
        private string _coachDesc = "";
        private string _coachQualification = "";
        private string _coachVideo = "";
        private string _coachStatus = "";
        private string _coachProfileImage = "";
        private string _coachAreaOfExpertise = "";


        public Coaches()
        {
        }

        public Coaches(string coachID, string coachName, string coachEmail,
               int coachHp, string coachDesc, string coachQualification, string coachVideo, string coachStatus,
               string coachProfileImage, string coachAreaOfExpertise)
        {
            _coachID = coachID;
            _coachName = coachName;
            _coachEmail = coachEmail;
            _coachHp = coachHp;
            _coachDesc = coachDesc;
            _coachQualification = coachQualification;
            _coachVideo = coachVideo;
            _coachStatus = coachStatus;
            _coachProfileImage = coachProfileImage;
            _coachAreaOfExpertise = coachAreaOfExpertise;
        }

        public Coaches(string coachName, string coachEmail,
               int coachHp, string coachDesc, string coachQualification, string coachVideo, string coachStatus)
            : this(null, coachName, coachEmail, coachHp, coachDesc, coachQualification, coachVideo, coachStatus, "", "")
        {
        }

        public Coaches(string coachID)
            : this(coachID, "", "", 0, "", "", "", "", "", "")
        {
        }

        public string Coach_ID
        {
            get { return _coachID; }
            set { _coachID = value; }
        }

        public string Coach_Name
        {
            get { return _coachName; }
            set { _coachName = value; }
        }

        public string Coach_Email
        {
            get { return _coachEmail; }
            set { _coachEmail = value; }
        }

        public int Coach_Hp
        {
            get { return _coachHp; }
            set { _coachHp = value; }
        }

        public string Coach_Desc
        {
            get { return _coachDesc; }
            set { _coachDesc = value; }
        }

        public string Coach_Qualification
        {
            get { return _coachQualification; }
            set { _coachQualification = value; }
        }

        public string Coach_Video
        {
            get { return _coachVideo; }
            set { _coachVideo = value; }
        }

        public string Coach_Status
        {
            get { return _coachStatus; }
            set { _coachStatus = value; }
        }

        public string Coach_ProfileImage
        {
            get { return _coachProfileImage; }
            set { _coachProfileImage = value; }
        }

        public string Coach_AreaOfExpertise
        {
            get { return _coachAreaOfExpertise; }
            set { _coachAreaOfExpertise = value; }
        }

        public Coaches getCoaches(string coachID)
        {
            Coaches coachDetail = null;

            string coach_Name, coach_Email, coach_Desc, coach_Qualification, coach_Video, coach_Status, coach_ProfileImage, coach_AreaOfExpertise;
            int coach_Hp;

            string queryStr = "SELECT * FROM Coach WHERE Id = @CoachID";

            SqlConnection conn = new SqlConnection(_connStr);
            SqlCommand cmd = new SqlCommand(queryStr, conn);
            cmd.Parameters.AddWithValue("@CoachID", coachID);

            conn.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                coach_Name = dr["Name"].ToString();
                coach_Email = dr["Email"].ToString();
                coach_Hp = int.Parse(dr["Hp"].ToString());
                coach_Desc = dr["Desc"].ToString();
                coach_Qualification = dr["Qualification"].ToString();
                coach_Video = dr["File_URL"].ToString();
                coach_Status = dr["Status"].ToString();
                coach_ProfileImage = dr["ProfileImage"].ToString();
                coach_AreaOfExpertise = dr["AreaOfExpertise"].ToString();

                coachDetail = new Coaches(coachID, coach_Name, coach_Email, coach_Hp, coach_Desc, coach_Qualification, coach_Video, coach_Status, coach_ProfileImage, coach_AreaOfExpertise);
            }
            else
            {
                coachDetail = null;
            }

            dr.Close();
            conn.Close();
            dr.Dispose();

            return coachDetail;
        }


        public List<Coaches> getCoachesAll()
        {
            List<Coaches> coachList = new List<Coaches>();

            string coach_ID, coach_Name, coach_Email, coach_Desc, coach_Qualification, coach_Video, coach_Status, coach_ProfileImage, coach_AreaOfExpertise;
            int coach_Hp;

            string queryStr = "SELECT * FROM Coach Order By Name";

            using (SqlConnection conn = new SqlConnection(_connStr))
            using (SqlCommand cmd = new SqlCommand(queryStr, conn))
            {
                conn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        coach_ID = dr["Id"].ToString();
                        coach_Name = dr["Name"].ToString();
                        coach_Email = dr["Email"].ToString();
                        coach_Hp = int.Parse(dr["Hp"].ToString());
                        coach_Desc = dr["Desc"].ToString();
                        coach_Qualification = dr["Qualification"].ToString();
                        coach_Video = dr["File_URL"].ToString();
                        coach_Status = dr["Status"].ToString();
                        coach_ProfileImage = dr["ProfileImage"].ToString();
                        coach_AreaOfExpertise = dr["AreaOfExpertise"].ToString();

                        Coaches a = new Coaches(coach_ID, coach_Name, coach_Email, coach_Hp,
                                                coach_Desc, coach_Qualification, coach_Video, coach_Status,
                                                coach_ProfileImage, coach_AreaOfExpertise);
                        coachList.Add(a);
                    }

                    dr.Close();
                }
            }

            return coachList;
        }


        public List<Coaches> GetPendingCoaches()
        {
            List<Coaches> coachList = new List<Coaches>();

            string coach_ID, coach_Name, coach_Email, coach_Desc, coach_Qualification, coach_Video, coach_Status, coach_ProfileImage, coach_AreaOfExpertise;
            int coach_Hp;

            string queryStr = "SELECT * FROM Coach WHERE Status = 'Pending' ORDER BY Name";

            using (SqlConnection conn = new SqlConnection(_connStr))
            using (SqlCommand cmd = new SqlCommand(queryStr, conn))
            {
                try
                {
                    conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        coach_ID = dr["Id"].ToString();
                        coach_Name = dr["Name"].ToString();
                        coach_Email = dr["Email"].ToString();
                        coach_Hp = int.Parse(dr["Hp"].ToString());
                        coach_Desc = dr["Desc"].ToString();
                        coach_Qualification = dr["Qualification"].ToString();
                        coach_Video = dr["File_URL"].ToString();
                        coach_Status = dr["Status"].ToString();
                        coach_ProfileImage = dr["ProfileImage"].ToString();
                        coach_AreaOfExpertise = dr["AreaOfExpertise"].ToString();

                        Coaches coach = new Coaches(coach_ID, coach_Name, coach_Email, coach_Hp,
                                                    coach_Desc, coach_Qualification, coach_Video, coach_Status,
                                                    coach_ProfileImage, coach_AreaOfExpertise);
                        coachList.Add(coach);
                    }

                    dr.Close();
                }
                catch (SqlException ex)
                {
                    Debug.WriteLine("SQL Error: " + ex.Message);
                    throw;
                }
            }

            return coachList;
        }

        public List<Coaches> GetApprovedCoaches()
        {
            List<Coaches> coachList = new List<Coaches>();

            string coach_ID, coach_Name, coach_Email, coach_Desc, coach_Qualification, coach_Video, coach_Status, coach_ProfileImage, coach_AreaOfExpertise;
            int coach_Hp;

            string queryStr = "SELECT * FROM Coach WHERE Status = 'Approved' ORDER BY Name";

            using (SqlConnection conn = new SqlConnection(_connStr))
            using (SqlCommand cmd = new SqlCommand(queryStr, conn))
            {
                try
                {
                    conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        coach_ID = dr["Id"].ToString();
                        coach_Name = dr["Name"].ToString();
                        coach_Email = dr["Email"].ToString();
                        coach_Hp = int.Parse(dr["Hp"].ToString());
                        coach_Desc = dr["Desc"].ToString();
                        coach_Qualification = dr["Qualification"].ToString();
                        coach_Video = dr["File_URL"].ToString();
                        coach_Status = dr["Status"].ToString();
                        coach_ProfileImage = dr["ProfileImage"].ToString();
                        coach_AreaOfExpertise = dr["AreaOfExpertise"].ToString();

                        Coaches coach = new Coaches(coach_ID, coach_Name, coach_Email, coach_Hp,
                                                    coach_Desc, coach_Qualification, coach_Video, coach_Status,
                                                    coach_ProfileImage, coach_AreaOfExpertise);
                        coachList.Add(coach);
                    }

                    dr.Close();
                }
                catch (SqlException ex)
                {
                    Debug.WriteLine("SQL Error: " + ex.Message);
                    throw;
                }
            }

            return coachList;
        }

        public Coaches GetCoachByUserId(int userId)
        {
            string coachId = null;
            // Query the CoachStatus table to find the coach_id for the given userId.
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                string query = "SELECT TOP 1 coach_id FROM CoachStatus WHERE user_id = @UserId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        coachId = result.ToString();
                    }
                }
            }

            // If a coachId was found, use your existing getCoaches method to retrieve details.
            if (!string.IsNullOrEmpty(coachId))
            {
                return getCoaches(coachId);
            }

            return null;
        }

        public bool UpdateCoachProfile()
        {
            bool success = false;
            string queryStr = "UPDATE Coach SET " +
                              "[Name] = @Name, " +
                              "Email = @Email, " +
                              "Hp = @Hp, " +
                              "[Desc] = @Desc, " +
                              "ProfileImage = @ProfileImage, " +
                              "AreaOfExpertise = @AreaOfExpertise " +
                              "WHERE Id = @Id";

            using (SqlConnection conn = new SqlConnection(_connStr))
            using (SqlCommand cmd = new SqlCommand(queryStr, conn))
            {
                cmd.Parameters.AddWithValue("@Name", this.Coach_Name);
                cmd.Parameters.AddWithValue("@Email", this.Coach_Email);
                cmd.Parameters.AddWithValue("@Hp", this.Coach_Hp);
                cmd.Parameters.AddWithValue("@Desc", this.Coach_Desc);
                cmd.Parameters.AddWithValue("@ProfileImage", this.Coach_ProfileImage);
                cmd.Parameters.AddWithValue("@AreaOfExpertise", this.Coach_AreaOfExpertise);
                cmd.Parameters.AddWithValue("@Id", this.Coach_ID);

                try
                {
                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    success = rowsAffected > 0;
                }
                catch (SqlException ex)
                {
                    Debug.WriteLine("SQL Error in UpdateCoachProfile: " + ex.Message);
                }
            }
            return success;
        }

        public bool ApproveCoach(string coachId)
        {
            bool isApproved = false;

            string queryStr = "UPDATE Coach SET Status = @Status WHERE Id = @Id";

            using (SqlConnection conn = new SqlConnection(_connStr))
            using (SqlCommand cmd = new SqlCommand(queryStr, conn))
            {
                cmd.Parameters.AddWithValue("@Status", "Approved");
                cmd.Parameters.AddWithValue("@Id", coachId);

                try
                {
                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    isApproved = rowsAffected > 0; // True if at least one row is updated
                }
                catch (SqlException ex)
                {
                    Debug.WriteLine("MySQL Error in ApproveCoach: " + ex.Message);
                    throw;
                }
            }

            return isApproved;
        }

        public bool RejectCoach(string coachId)
        {
            string fileUrl = null;
            string profileImageUrl = null;

            // Retrieve File_URL and ProfileImage from the Coach record
            using (SqlConnection conn = new SqlConnection(_connStr))
            using (SqlCommand cmd = new SqlCommand("SELECT File_URL, ProfileImage FROM Coach WHERE Id = @Id", conn))
            {
                cmd.Parameters.AddWithValue("@Id", coachId);
                try
                {
                    conn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            fileUrl = dr["File_URL"]?.ToString();
                            profileImageUrl = dr["ProfileImage"]?.ToString();
                        }
                    }
                }
                catch (SqlException ex)
                {
                    Debug.WriteLine("SQL Error while fetching file URLs: " + ex.Message);
                    return false;
                }
            }

            // Delete the video file if it exists
            if (!string.IsNullOrEmpty(fileUrl))
            {
                try
                {
                    string videoPath = HttpContext.Current.Server.MapPath("~/uploads/" + fileUrl);
                    if (File.Exists(videoPath))
                    {
                        File.Delete(videoPath);
                        Debug.WriteLine($"Video file deleted: {videoPath}");
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error deleting video file: " + ex.Message);
                    return false;
                }
            }

            // Delete the profile image if it exists
            if (!string.IsNullOrEmpty(profileImageUrl))
            {
                try
                {
                    string imagePath = HttpContext.Current.Server.MapPath("~/uploads/" + profileImageUrl);
                    if (File.Exists(imagePath))
                    {
                        File.Delete(imagePath);
                        Debug.WriteLine($"Profile image deleted: {imagePath}");
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error deleting profile image: " + ex.Message);
                    return false;
                }
            }

            // Delete the Coach record from the database
            using (SqlConnection conn = new SqlConnection(_connStr))
            using (SqlCommand cmd = new SqlCommand("DELETE FROM Coach WHERE Id = @Id", conn))
            {
                cmd.Parameters.AddWithValue("@Id", coachId);
                try
                {
                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
                catch (SqlException ex)
                {
                    Debug.WriteLine("SQL Error in RejectCoach: " + ex.Message);
                    return false;
                }
            }
        }

        public int CoachesInsert()
        {
            int result = 0;

            string queryStr = "INSERT INTO Coach (Id, [Name], Email, Hp, [Desc], Qualification, File_URL, Status, ProfileImage, AreaOfExpertise) " +
                              "VALUES (@Coaches_ID, @Coaches_Name, @Coaches_Email, @Coaches_Hp, @Coaches_Desc, @Coaches_Qualification, @Coaches_Video, @Coaches_Status, @ProfileImage, @AreaOfExpertise)";

            using (SqlConnection conn = new SqlConnection(_connStr))
            using (SqlCommand cmd = new SqlCommand(queryStr, conn))
            {
                cmd.Parameters.AddWithValue("@Coaches_ID", this.Coach_ID);
                cmd.Parameters.AddWithValue("@Coaches_Name", this.Coach_Name);
                cmd.Parameters.AddWithValue("@Coaches_Email", this.Coach_Email);
                cmd.Parameters.AddWithValue("@Coaches_Hp", this.Coach_Hp);
                cmd.Parameters.AddWithValue("@Coaches_Desc", this.Coach_Desc);
                cmd.Parameters.AddWithValue("@Coaches_Qualification", this.Coach_Qualification);
                cmd.Parameters.AddWithValue("@Coaches_Video", this.Coach_Video);
                cmd.Parameters.AddWithValue("@Coaches_Status", this.Coach_Status);
                cmd.Parameters.AddWithValue("@ProfileImage", this.Coach_ProfileImage);
                cmd.Parameters.AddWithValue("@AreaOfExpertise", this.Coach_AreaOfExpertise);

                try
                {
                    conn.Open();
                    result = cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    Debug.WriteLine("SQL Error: " + ex.Message);
                    throw;
                }
            }

            return result;
        }
    }

    public partial class CoachStatus
    {
        string _connStr = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;
        private int _id = 0;
        private string _coachId = ""; 
        private int _userId = 0;
        private bool _isCoach = false; 

        public CoachStatus()
        {
        }

        public CoachStatus(int id, string coachId, int userId, bool isCoach)
        {
            _id = id;
            _coachId = coachId;
            _userId = userId;
            _isCoach = isCoach;
        }

        public CoachStatus(string coachId, int userId, bool isCoach)
            : this(0, coachId, userId, isCoach)
        {
        }

        public CoachStatus(int id)
            : this(id, "", 0, false)
        {
        }

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string CoachId
        {
            get { return _coachId; }
            set { _coachId = value; }
        }

        public int UserId
        {
            get { return _userId; }
            set { _userId = value; }
        }

        public bool IsCoach
        {
            get { return _isCoach; }
            set { _isCoach = value; }
        }

        public bool IsUserAlreadyCoach(int userId)
        {
            bool isAlreadyCoach = false;
            // Query to select the BIT column (IsCoach) for the user.
            string queryStr = "SELECT TOP 1 IsCoach FROM [dbo].[CoachStatus] WHERE user_id = @UserId";

            using (SqlConnection conn = new SqlConnection(_connStr))
            using (SqlCommand cmd = new SqlCommand(queryStr, conn))
            {
                cmd.Parameters.AddWithValue("@UserId", userId);

                try
                {
                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    // If a record is found, convert the BIT value to bool.
                    if (result != null)
                    {
                        isAlreadyCoach = Convert.ToBoolean(result);
                    }
                }
                catch (SqlException ex)
                {
                    Debug.WriteLine("SQL Error: " + ex.Message);
                }
            }

            return isAlreadyCoach;
        }


        public bool InsertCoachStatus(int userId, string coachId)
        {
            bool result = false;

            // Check if the user is already in the CoachStatus table
            string checkQuery = "SELECT COUNT(*) FROM [dbo].[CoachStatus] WHERE user_id = @userId";
            using (SqlConnection conn = new SqlConnection(_connStr))
            using (SqlCommand cmd = new SqlCommand(checkQuery, conn))
            {
                cmd.Parameters.AddWithValue("@userId", userId);

                try
                {
                    conn.Open();
                    int count = (int)cmd.ExecuteScalar();

                    // If the user is not already in the CoachStatus table, insert a new record
                    if (count == 0)
                    {
                        string insertQuery = "INSERT INTO [dbo].[CoachStatus] (user_id, coach_id, isCoach) VALUES (@userId, @coachId, 0)";
                        using (SqlCommand insertCmd = new SqlCommand(insertQuery, conn))
                        {
                            insertCmd.Parameters.AddWithValue("@userId", userId);
                            insertCmd.Parameters.AddWithValue("@coachId", coachId);

                            int rowsAffected = insertCmd.ExecuteNonQuery();
                            result = rowsAffected > 0;  // True if the insertion was successful
                        }
                    }
                    else
                    {
                        result = false; // User already exists in CoachStatus table
                    }
                }
                catch (SqlException ex)
                {
                    Debug.WriteLine("SQL Error: " + ex.Message);
                    result = false;
                }
            }

            return result;
        }

        public bool UpdateIsCoachStatus(string coachId)
        {
            bool updated = false;
            string query = @"
                UPDATE cs
                SET cs.isCoach = 1
                FROM dbo.CoachStatus cs
                INNER JOIN dbo.Coach c ON cs.coach_id = c.Id
                WHERE cs.coach_id = @coachId AND c.Status = 'Approved'";

            using (SqlConnection conn = new SqlConnection(_connStr))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@coachId", coachId);

                try
                {
                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    updated = rowsAffected > 0;
                }
                catch (SqlException ex)
                {
                    Debug.WriteLine("SQL Error in UpdateIsCoachStatus: " + ex.Message);
                }
            }

            return updated;
        }

        public bool DeleteCoachStatus(string coachId)
        {
            bool deleted = false;
            string query = "DELETE FROM [dbo].[CoachStatus] WHERE coach_id = @coachId";

            using (SqlConnection conn = new SqlConnection(_connStr))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@coachId", coachId);

                try
                {
                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    deleted = rowsAffected > 0;
                }
                catch (SqlException ex)
                {
                    Debug.WriteLine("SQL Error in DeleteCoachStatus: " + ex.Message);
                }
            }

            return deleted;
        }
    }
}

