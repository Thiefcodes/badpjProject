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

            // Ensure _productId is valid
            if (_productId <= 0)
            {
                Response.Write("<script>alert('Invalid product selected for review.');</script>");
                return;
            }

            // Check if a review already exists for this product by this user
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                string checkSql = "SELECT COUNT(*) FROM Reviews WHERE ProductID = @ProductID AND UserID = @UserID";
                using (SqlCommand cmd = new SqlCommand(checkSql, conn))
                {
                    cmd.Parameters.AddWithValue("@ProductID", _productId);
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    conn.Open();
                    int count = (int)cmd.ExecuteScalar();
                    if (count > 0)
                    {
                        Response.Write("<script>alert('You have already left a review for this product.');</script>");
                        return;
                    }
                }
            }

            int starRating = int.Parse(ddlStarRating.SelectedValue);
            string reviewMessage = txtReviewMessage.Text.Trim();

            // Insert the new review
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                string sql = "INSERT INTO Reviews (ProductID, UserID, StarRating, ReviewMessage) " +
                             "VALUES (@ProductID, @UserID, @StarRating, @ReviewMessage)";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ProductID", _productId);
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    cmd.Parameters.AddWithValue("@StarRating", starRating);
                    cmd.Parameters.AddWithValue("@ReviewMessage", reviewMessage);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            Response.Redirect("ProductDetails.aspx?productID=" + _productId);
        }
    }
}
