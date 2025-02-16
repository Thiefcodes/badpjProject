using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using Newtonsoft.Json; // Ensure Newtonsoft.Json is installed via NuGet

namespace badpjProject
{
    public partial class EnableFacialAuthentication : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           
        }


        // WebMethod to enroll the facial authentication data
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string EnrollFacialAuth(float[] descriptor)
        {
            try
            {
                // Debug: Print session variables to the Output window
                var sessionUserId = HttpContext.Current.Session["UserId"] != null
                                    ? HttpContext.Current.Session["UserId"].ToString()
                                    : "null";
                var sessionUsername = HttpContext.Current.Session["Username"] != null
                                      ? HttpContext.Current.Session["Username"].ToString()
                                      : "null";
                System.Diagnostics.Debug.WriteLine("EnrollFacialAuth - Session UserId: " + sessionUserId);
                System.Diagnostics.Debug.WriteLine("EnrollFacialAuth - Session Username: " + sessionUsername);

                

                int userId = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
                string username = HttpContext.Current.Session["Username"].ToString();

                // Convert the face descriptor array to a JSON string for storage
                string descriptorJson = JsonConvert.SerializeObject(descriptor);
                string connString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;

                // Insert the facial authentication data into the new table
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    string sql = "INSERT INTO UserFacialAuth (UserId, Login_Name, FaceDescriptor) VALUES (@UserId, @Login_Name, @FaceDescriptor)";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserId", userId);
                        cmd.Parameters.AddWithValue("@Login_Name", username);
                        cmd.Parameters.AddWithValue("@FaceDescriptor", descriptorJson);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                return "Facial authentication enabled successfully.";
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }

    }
}
