using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Diagnostics;

namespace badpjProject
{
    public class Coaches
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

        public Coaches()
        {
        }

        public Coaches(string coachID, string coachName, string coachEmail,
                        int coachHp, string coachDesc, string coachQualification, string coachVideo, string coachStatus)
        {
            _coachID = coachID;
            _coachName = coachName;
            _coachEmail = coachEmail;
            _coachHp = coachHp;
            _coachDesc = coachDesc;
            _coachQualification = coachQualification;
            _coachVideo = coachVideo;
            _coachStatus = coachStatus;
        }

        public Coaches(string coachName, string coachEmail,
               int coachHp, string coachDesc, string coachQualification, string coachVideo, string coachStatus)
            : this(null, coachName, coachEmail, coachHp, coachDesc, coachQualification, coachVideo, coachStatus)
        {
        }

        public Coaches(string coachID)
            : this(coachID, "", "", 0, "", "", "", "")
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

        public Coaches getCoaches(string coachID)
        {

            Coaches coachDetail = null;

            string coach_Name, coach_Email, coach_Desc, coach_Qualification, coach_Video, coach_Status;
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

                coachDetail = new Coaches(coachID, coach_Name, coach_Email, coach_Hp, coach_Desc, coach_Qualification, coach_Video, coach_Status);
            }
            else
            {
                coachDetail = null;
            }

            conn.Close();
            dr.Close();
            dr.Dispose();

            return coachDetail;
        }

        public List<Coaches> getCoachesAll()
        {
            List<Coaches> coachList = new List<Coaches>();

            string coach_ID, coach_Name, coach_Email, coach_Desc, coach_Qualification, coach_Video, coach_Status;
            int coach_Hp;

            string queryStr = "SELECT * FROM Coach Order By Name";

            SqlConnection conn = new SqlConnection(_connStr);
            SqlCommand cmd = new SqlCommand(queryStr, conn);

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

                Coaches a = new Coaches(coach_ID, coach_Name, coach_Email, coach_Hp, coach_Desc, coach_Qualification, coach_Video, coach_Status);
                coachList.Add(a);
            }

            conn.Close();
            dr.Close();
            dr.Dispose();

            return coachList;
        }

        public int CoachesInsert()
        {
            int result = 0;

            string queryStr = "INSERT INTO Coach (Id, [Name], Email, Hp, [Desc], Qualification, File_URL, Status) " +
                              "VALUES (@Coaches_ID, @Coaches_Name, @Coaches_Email, @Coaches_Hp, @Coaches_Desc, @Coaches_Qualification, @Coaches_Video, @Coaches_Status)";

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
}