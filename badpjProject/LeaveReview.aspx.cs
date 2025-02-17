using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;

namespace badpjProject
{
    public partial class LeaveReview : System.Web.UI.Page
    {
        private string _connString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;
        private int _productId;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Always parse the productID from the query string, even on postback.
            if (!int.TryParse(Request.QueryString["productID"], out _productId))
            {
                Response.Write("<script>alert('Invalid product.'); window.location='Orders.aspx';</script>");
                return;
            }

            if (!IsPostBack)
            {
                LoadProductName(_productId);
            }
        }

        private void LoadProductName(int productId)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                string sql = "SELECT ProductName FROM Products WHERE ProductID = @ProductID";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ProductID", productId);
                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        lblProductName.Text = result.ToString();
                    }
                }
            }
        }

        protected void btnSubmitReview_Click(object sender, EventArgs e)
        {
            int userId = Convert.ToInt32(Session["UserID"]);

            // Ensure _productId is valid.
            if (_productId <= 0)
            {
                Response.Write("<script>alert('Invalid product selected for review.');</script>");
                return;
            }

            int starRating = int.Parse(ddlStarRating.SelectedValue);
            string reviewMessage = txtReviewMessage.Text.Trim();

            using (SqlConnection conn = new SqlConnection(_connString))
            {
                conn.Open();
                // Check if a review already exists for this product by this user.
                string checkSql = "SELECT COUNT(*) FROM Reviews WHERE ProductID = @ProductID AND UserID = @UserID";
                using (SqlCommand cmd = new SqlCommand(checkSql, conn))
                {
                    cmd.Parameters.AddWithValue("@ProductID", _productId);
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    int count = (int)cmd.ExecuteScalar();

                    if (count > 0)
                    {
                        // If a review exists, update it.
                        string updateSql = "UPDATE Reviews SET StarRating = @StarRating, ReviewMessage = @ReviewMessage, ReviewDate = GETDATE() " +
                                           "WHERE ProductID = @ProductID AND UserID = @UserID";
                        using (SqlCommand updateCmd = new SqlCommand(updateSql, conn))
                        {
                            updateCmd.Parameters.AddWithValue("@ProductID", _productId);
                            updateCmd.Parameters.AddWithValue("@UserID", userId);
                            updateCmd.Parameters.AddWithValue("@StarRating", starRating);
                            updateCmd.Parameters.AddWithValue("@ReviewMessage", reviewMessage);
                            updateCmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        // Otherwise, insert a new review.
                        string insertSql = "INSERT INTO Reviews (ProductID, UserID, StarRating, ReviewMessage) " +
                                           "VALUES (@ProductID, @UserID, @StarRating, @ReviewMessage)";
                        using (SqlCommand insertCmd = new SqlCommand(insertSql, conn))
                        {
                            insertCmd.Parameters.AddWithValue("@ProductID", _productId);
                            insertCmd.Parameters.AddWithValue("@UserID", userId);
                            insertCmd.Parameters.AddWithValue("@StarRating", starRating);
                            insertCmd.Parameters.AddWithValue("@ReviewMessage", reviewMessage);
                            insertCmd.ExecuteNonQuery();
                        }
                    }
                }
            }

            Response.Redirect("ProductDetails.aspx?productID=" + _productId);
        }
    }
}
