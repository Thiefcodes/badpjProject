using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using Newtonsoft.Json; // Ensure Newtonsoft.Json is installed via NuGet

namespace YourNamespace
{
    [ScriptService]
    public partial class CameraAccess : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Optional page initialization
        }

        // WebMethod to enroll a user's face descriptor (registration)
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string EnrollFaceDescriptor(string username, float[] descriptor)
        {
            try
            {
                // Convert the float array to a JSON string for storage
                string descriptorJson = JsonConvert.SerializeObject(descriptor);
                string connString = ConfigurationManager.ConnectionStrings["FaceAuthDB"].ConnectionString;

                // Insert or update the user record with the face descriptor
                // (This example assumes the user already exists or that you use INSERT logic accordingly)
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    string sql = "UPDATE UserAccounts SET FaceDescriptor = @FaceDescriptor WHERE Username = @Username";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", username);
                        cmd.Parameters.AddWithValue("@FaceDescriptor", descriptorJson);
                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected == 0)
                        {
                            // If no record was updated, you could INSERT a new record instead
                            sql = "INSERT INTO UserAccounts (Username, FaceDescriptor) VALUES (@Username, @FaceDescriptor)";
                            using (SqlCommand insertCmd = new SqlCommand(sql, conn))
                            {
                                insertCmd.Parameters.AddWithValue("@Username", username);
                                insertCmd.Parameters.AddWithValue("@FaceDescriptor", descriptorJson);
                                insertCmd.ExecuteNonQuery();
                            }
                        }
                    }
                }

                return "Face descriptor enrolled successfully for user: " + username;
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
    }
}
