using System;
using System.Data.SqlClient;
using badpjProject.Models;
using System.Configuration;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Linq;

namespace badpjProject
{
    public partial class ProductDetails : System.Web.UI.Page
    {
        private string _connString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;
        private int _productId;
        private int _currentUserID;

        protected void Page_PreInit(object sender, EventArgs e)
        {
            this.MasterPageFile = Session["UserID"] == null ? "~/Site.Master" : "~/Site1loggedin.Master";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(Request.QueryString["productID"], out _productId))
            {
                Response.Write("<script>alert('Invalid product.'); window.location='Shop.aspx';</script>");
                return;
            }

            _currentUserID = Session["UserID"] != null ? Convert.ToInt32(Session["UserID"]) : -1;

            if (!IsPostBack)
            {
                LoadProduct(_productId);
                hfProductID.Value = _productId.ToString();
                UpdateCartCount();
                bool isInWishlist = IsProductInWishlist(_productId, _currentUserID);
                lblWishlistIndicator.Visible = isInWishlist;
                LoadReviews(_productId);
            }
        }

        private void LoadProduct(int productId)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                string sql = @"SELECT ProductName, Description, ImageUrl, Price, Category, DiscountPercent
                               FROM dbo.Products
                               WHERE ProductID = @ProductID";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ProductID", productId);
                    conn.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read())
                        {
                            lblName.Text = rdr["ProductName"].ToString();
                            lblDescription.Text = rdr["Description"].ToString();
                            imgProduct.ImageUrl = rdr["ImageUrl"].ToString();
                            lblCategory.Text = "Category: " + rdr["Category"].ToString();

                            decimal originalPrice = Convert.ToDecimal(rdr["Price"]);
                            int discount = rdr["DiscountPercent"] != DBNull.Value ? Convert.ToInt32(rdr["DiscountPercent"]) : 0;

                            if (discount > 0)
                            {
                                decimal discountedPrice = originalPrice * (1 - discount / 100m);
                                lblPrice.Text = string.Format("<del>{0:C}</del> {1:C} <span class='text-success'>({2}% OFF)</span>",
                                                              originalPrice, discountedPrice, discount);
                            }
                            else
                            {
                                lblPrice.Text = originalPrice.ToString("C");
                            }
                        }
                    }
                }
            }
        }

        protected bool IsProductInWishlist(int productID, int userID)
        {
            if (userID <= 0)
                return false;

            using (SqlConnection conn = new SqlConnection(_connString))
            {
                string sql = @"SELECT COUNT(*) FROM dbo.Wishlist WHERE ProductID = @ProductID AND UserID = @UserID";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ProductID", productID);
                    cmd.Parameters.AddWithValue("@UserID", userID);
                    conn.Open();
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        protected void btnAddToCart_Click(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                Response.Write("<script>alert('Please log in first!'); window.location='Login.aspx';</script>");
                return;
            }
            int productId;
            if (!int.TryParse(hfProductID.Value, out productId))
            {
                Response.Write("<script>alert('Invalid product selection.');</script>");
                return;
            }

            string connStr = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;
            string query = "SELECT ProductID, ProductName, Description, ImageUrl, Price, DiscountPercent FROM Products WHERE ProductID = @ProductID";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ProductID", productId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        decimal originalPrice = Convert.ToDecimal(reader["Price"]);
                        int discount = reader["DiscountPercent"] != DBNull.Value ? Convert.ToInt32(reader["DiscountPercent"]) : 0;
                        decimal finalPrice = discount > 0 ? originalPrice * (1 - discount / 100m) : originalPrice;

                        CartItem newItem = new CartItem
                        {
                            ProductID = (int)reader["ProductID"],
                            ProductName = reader["ProductName"].ToString(),
                            Description = reader["Description"].ToString(),
                            ImageUrl = reader["ImageUrl"].ToString(),
                            Price = finalPrice,
                            Quantity = 1
                        };

                        List<CartItem> cart = (List<CartItem>)Session["Cart"] ?? new List<CartItem>();

                        CartItem existingItem = cart.Find(item => item.ProductID == newItem.ProductID);
                        if (existingItem != null)
                        {
                            existingItem.Quantity++;
                        }
                        else
                        {
                            cart.Add(newItem);
                        }

                        Session["Cart"] = cart;
                        UpdateCartCount();
                        Response.Write("<script>alert('Item added to cart!');</script>");
                    }
                    else
                    {
                        Response.Write("<script>alert('Product not found.');</script>");
                    }
                }
            }
        }


        private void UpdateCartCount()
        {
            int cartCount = 0;
            if (Session["Cart"] != null)
            {
                List<CartItem> cart = (List<CartItem>)Session["Cart"];
                cartCount = cart.Sum(item => item.Quantity);
            }
        }

        private void LoadReviews(int productId)
        {
            decimal averageRating = 0;
            int totalReviews = 0;
            var reviews = new List<dynamic>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                // Get average rating and count.
                string avgSql = "SELECT AVG(CAST(StarRating AS DECIMAL(10,2))) AS AvgRating, COUNT(*) AS TotalReviews FROM Reviews WHERE ProductID = @ProductID";
                using (SqlCommand cmd = new SqlCommand(avgSql, conn))
                {
                    cmd.Parameters.AddWithValue("@ProductID", productId);
                    conn.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read())
                        {
                            averageRating = rdr["AvgRating"] != DBNull.Value ? Convert.ToDecimal(rdr["AvgRating"]) : 0;
                            totalReviews = Convert.ToInt32(rdr["TotalReviews"]);
                        }
                    }
                    conn.Close();
                }
                // Get individual reviews with customer name.
                string reviewSql = @"
                  SELECT r.StarRating, r.ReviewMessage, r.ReviewDate, u.Login_Name AS CustomerName 
                  FROM Reviews r 
                  INNER JOIN [Table] u ON r.UserID = u.Id 
                  WHERE r.ProductID = @ProductID 
                  ORDER BY r.ReviewDate DESC";
                using (SqlCommand cmd = new SqlCommand(reviewSql, conn))
                {
                    cmd.Parameters.AddWithValue("@ProductID", productId);
                    conn.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            reviews.Add(new
                            {
                                StarRating = rdr["StarRating"],
                                ReviewMessage = rdr["ReviewMessage"],
                                ReviewDate = rdr["ReviewDate"],
                                CustomerName = rdr["CustomerName"]
                            });
                        }
                    }
                }
            }
            lblAverageRating.Text = totalReviews > 0
                ? $"Average Rating: {averageRating:F1} ({totalReviews} reviews)"
                : "No reviews yet.";
            rptReviews.DataSource = reviews;
            rptReviews.DataBind();
        }
    }
}
