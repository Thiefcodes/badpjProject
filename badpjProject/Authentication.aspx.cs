using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using Newtonsoft.Json; // Make sure you've installed the Newtonsoft.Json package

namespace YourNamespace
{
    [ScriptService]
    public partial class Authentication : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Optional initialization
        }

        // WebMethod to verify the face for a given username
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string VerifyUserFace(string username, float[] descriptor)
        {
            string connString = ConfigurationManager.ConnectionStrings["FaceAuthDB"].ConnectionString;
            try
            {
                string storedDescriptorJson = null;
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    string sql = "SELECT FaceDescriptor FROM UserAccounts WHERE Username = @Username";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", username);
                        conn.Open();
                        var result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            storedDescriptorJson = result.ToString();
                        }
                        else
                        {
                            return "No facial data found for this user.";
                        }
                    }
                }

                // Deserialize the stored face descriptor
                float[] storedDescriptor = JsonConvert.DeserializeObject<float[]>(storedDescriptorJson);

                // Compute the Euclidean distance between the new descriptor and the stored one
                float distance = EuclideanDistance(descriptor, storedDescriptor);

                // Define a threshold (commonly around 0.6 for 128D face descriptors)
                float threshold = 0.6f;
                if (distance < threshold)
                {
                    return $"Authentication successful for {username}. Distance: {distance:F4}";
                }
                else
                {
                    return $"Authentication failed for {username}. Distance: {distance:F4}";
                }
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }

        // Helper method to compute Euclidean distance between two descriptors
        private static float EuclideanDistance(float[] descA, float[] descB)
        {
            if (descA.Length != descB.Length)
                return float.MaxValue;
            float sum = 0f;
            for (int i = 0; i < descA.Length; i++)
            {
                float diff = descA[i] - descB[i];
                sum += diff * diff;
            }
            return (float)Math.Sqrt(sum);
        }
    }
}
