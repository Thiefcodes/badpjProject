using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using badpjProject.Models;

namespace badpjProject
{
    public partial class Shop : System.Web.UI.Page
    {
        private string _connString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;
        private int _currentUserID;

        protected void Page_PreInit(object sender, EventArgs e)
        {
            this.MasterPageFile = Session["UserID"] == null ? "~/Site.Master" : "~/Site1loggedin.Master";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            _currentUserID = Session["UserID"] != null ? Convert.ToInt32(Session["UserID"]) : -1;
            if (!IsPostBack)
            {
                LoadProducts();
                UpdateCartCount();
            }
        }

        private void LoadProducts()
        {
            List<Product> productList = new List<Product>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                string sql = @"
                  SELECT p.ProductID, p.ProductName, p.Description, p.ImageUrl, p.Price,
                  (SELECT AVG(CAST(StarRating AS DECIMAL(10,2))) FROM Reviews r WHERE r.ProductID = p.ProductID) AS AverageRating
                  FROM dbo.Products p";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            Product p = new Product
                            {
                                ProductID = rdr.GetInt32(0),
                                ProductName = rdr.GetString(1),
                                Description = rdr.GetString(2),
                                ImageUrl = rdr.GetString(3),
                                Price = rdr.GetDecimal(4),
                                AverageRating = rdr["AverageRating"] != DBNull.Value ? Convert.ToDecimal(rdr["AverageRating"]) : (decimal?)null
                            };
                            productList.Add(p);
                        }
                    }
                }
            }
            rptProducts.DataSource = productList;
            rptProducts.DataBind();
        }

        protected void btnViewWishlist_Click(object sender, EventArgs e)
        {
            if (Session["Role"]?.ToString() == "Staff" || Session["Role"]?.ToString() == "User")
            {
                Response.Redirect("Wishlist.aspx");
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert",
                  "alert('You need to be logged in to view your wishlist!');", true);
            }
        }

        protected void rptProducts_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var product = (Product)e.Item.DataItem;
                bool isInWishlist = IsProductInWishlist(product.ProductID, _currentUserID);
                var lblWishlistIndicator = (Label)e.Item.FindControl("lblWishlistIndicator");
                if (lblWishlistIndicator != null && isInWishlist)
                {
                    lblWishlistIndicator.Visible = true;
                }
            }
        }

        protected bool IsProductInWishlist(int productID, int userID)
        {
            if (userID <= 0) return false;
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

        protected void AddToCart_Command(object sender, CommandEventArgs e)
        {
            if (Session["UserID"] == null)
            {
                Response.Write("<script>alert('Please log in first!'); window.location='Login.aspx';</script>");
                return;
            }
            if (!int.TryParse(e.CommandArgument.ToString(), out int productId))
            {
                Response.Write("<script>alert('Invalid product selection.');</script>");
                return;
            }
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                string query = "SELECT ProductID, ProductName, Description, ImageUrl, Price FROM Products WHERE ProductID = @ProductID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ProductID", productId);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        CartItem newItem = new CartItem
                        {
                            ProductID = (int)reader["ProductID"],
                            ProductName = reader["ProductName"].ToString(),
                            Description = reader["Description"].ToString(),
                            ImageUrl = reader["ImageUrl"].ToString(),
                            Price = Convert.ToDecimal(reader["Price"]),
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
            lblCartCount.Text = cartCount > 0 ? cartCount.ToString() : "";
        }

        protected void AddToWishlist_Command(object sender, CommandEventArgs e)
        {
            int productId = Convert.ToInt32(e.CommandArgument);
            if (_currentUserID < 1)
            {
                Response.Write("<script>alert('Please log in first!');</script>");
                Response.Redirect("Login.aspx?returnUrl=Shop.aspx");
                return;
            }
            if (!IsProductInWishlist(productId, _currentUserID))
            {
                using (SqlConnection conn = new SqlConnection(_connString))
                {
                    string sql = @"INSERT INTO dbo.Wishlist (UserID, ProductID) VALUES (@UserID, @ProductID)";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserID", _currentUserID);
                        cmd.Parameters.AddWithValue("@ProductID", productId);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}
