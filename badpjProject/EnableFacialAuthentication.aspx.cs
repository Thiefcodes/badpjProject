using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace badpjProject
{
    public partial class EnableFacialAuthentication : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Ensure the user is logged in; if not, redirect to login
            if (Session["UserId"] == null || Session["Username"] == null)
            {
                Response.Redirect("Login.aspx");
            }
        }

        // Button click event to enroll the facial descriptor from the hidden field
        protected void btnEnroll_Click(object sender, EventArgs e)
        {
            string descriptorJson = hfDescriptor.Value;

            if (string.IsNullOrEmpty(descriptorJson))
            {
                lblMessage.Text = "No facial data captured. Please try again.";
                return;
            }

            try
            {
                int userId = Convert.ToInt32(Session["UserId"]);
                string username = Session["Username"].ToString();
                string connString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connString))
                {
                    // Insert the face descriptor (JSON) into the database
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

                lblMessage.Text = "Facial descriptor enrolled successfully for user: " + username;
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error: " + ex.Message;
            }
        }
    }
}
