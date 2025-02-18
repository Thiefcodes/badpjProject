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
            if (Session["UserID"] != null)
            {
                _currentUserID = Convert.ToInt32(Session["UserID"]);
            }
            else
            {
                _currentUserID = -1;
            }

            if (!IsPostBack)
            {
                LoadCategories();
                LoadProducts();
                UpdateCartCount();

                int discountedCount = GetDiscountedWishlistCount();
                if (discountedCount > 0)
                {
                    litWishlistBadge.Text = $" <span class='badge-discount'>{discountedCount} Discounted</span>";

                    if (Session["DiscountNotified"] == null)
                    {
                        string script = $"Swal.fire({{ icon: 'info', title: 'Discount Alert', text: 'You have {discountedCount} discounted product(s) on your wishlist! Check them out.' }});";
                        ScriptManager.RegisterStartupScript(this, GetType(), "discountAlert", script, true);
                        Session["DiscountNotified"] = true;
                    }
                }
            }
        }


        private void LoadProducts()
        {
            List<Product> productList = new List<Product>();
            string search = txtSearch.Text.Trim();
            string category = ddlCategory.SelectedValue;
            string minPriceStr = txtMinPrice.Text.Trim();
            string maxPriceStr = txtMaxPrice.Text.Trim();
            string sortOrder = ddlSort.SelectedValue;

            using (SqlConnection conn = new SqlConnection(_connString))
            {
                string sql = @"
            SELECT ProductID, ProductName, Description, ImageUrl, Price, Category, DiscountPercent,
                   (SELECT AVG(CAST(StarRating AS DECIMAL(10,2))) FROM Reviews r WHERE r.ProductID = p.ProductID) AS AverageRating
            FROM dbo.Products p
            WHERE 1=1";

                if (!string.IsNullOrEmpty(search))
                {
                    sql += " AND ProductName LIKE @Search";
                }
                if (!string.IsNullOrEmpty(category))
                {
                    sql += " AND Category = @Category";
                }
                if (!string.IsNullOrEmpty(minPriceStr))
                {
                    sql += " AND Price >= @MinPrice";
                }
                if (!string.IsNullOrEmpty(maxPriceStr))
                {
                    sql += " AND Price <= @MaxPrice";
                }

                if (sortOrder == "asc")
                {
                    sql += " ORDER BY Price ASC";
                }
                else if (sortOrder == "desc")
                {
                    sql += " ORDER BY Price DESC";
                }
                else
                {
                    sql += " ORDER BY ProductID";
                }

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    if (!string.IsNullOrEmpty(search))
                    {
                        cmd.Parameters.AddWithValue("@Search", "%" + search + "%");
                    }
                    if (!string.IsNullOrEmpty(category))
                    {
                        cmd.Parameters.AddWithValue("@Category", category);
                    }
                    if (!string.IsNullOrEmpty(minPriceStr))
                    {
                        if (decimal.TryParse(minPriceStr, out decimal minPrice))
                        {
                            cmd.Parameters.AddWithValue("@MinPrice", minPrice);
                        }
                    }
                    if (!string.IsNullOrEmpty(maxPriceStr))
                    {
                        if (decimal.TryParse(maxPriceStr, out decimal maxPrice))
                        {
                            cmd.Parameters.AddWithValue("@MaxPrice", maxPrice);
                        }
                    }

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
                                Category = rdr.IsDBNull(5) ? "" : rdr.GetString(5),
                                DiscountPercent = rdr.IsDBNull(6) ? 0 : rdr.GetInt32(6),
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


        protected void btnSearch_Click(object sender, EventArgs e)
        {
            decimal minPrice, maxPrice;
            bool hasMin = decimal.TryParse(txtMinPrice.Text.Trim(), out minPrice);
            bool hasMax = decimal.TryParse(txtMaxPrice.Text.Trim(), out maxPrice);

            if (hasMin && hasMax && minPrice > maxPrice)
            {
                string script = "Swal.fire({ icon: 'error', title: 'Invalid Price Range', text: 'Minimum price cannot be greater than maximum price!' });";
                ScriptManager.RegisterStartupScript(this, GetType(), "InvalidPriceRange", script, true);
                return;
            }

            LoadProducts();
        }


        protected void btnViewWishlist_Click(object sender, EventArgs e)
        {
            if (Session["Role"]?.ToString() == "Staff" || Session["Role"]?.ToString() == "User")
            {
                Response.Redirect("Wishlist.aspx");
            }
            else
            {
                Response.Redirect("Login.aspx?returnUrl=Shop.aspx");
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
                string script = "Swal.fire({ icon: 'warning', title: 'Not Logged In', text: 'Please log in first!' }).then(() => { window.location='Login.aspx'; });";
                ScriptManager.RegisterStartupScript(this, GetType(), "loginAlert", script, true);
                return;
            }

            int productId;
            if (!int.TryParse(e.CommandArgument.ToString(), out productId))
            {
                string script = "Swal.fire({ icon: 'warning', title: 'Error!', text: 'No product ID found!' }});";
                ScriptManager.RegisterStartupScript(this, GetType(), "noproductIDAlert", script, true);
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
                        string script = "Swal.fire({ icon: 'success', title: 'Added to Cart', text: 'You have successfully added the product to your cart!' });";
                        ScriptManager.RegisterStartupScript(this, GetType(), "discountAlert", script, true);
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
                string script = "Swal.fire({ icon: 'warning', title: 'Not Logged In', text: 'Please log in first!' }).then(() => { window.location='Login.aspx'; });";
                ScriptManager.RegisterStartupScript(this, GetType(), "loginAlert", script, true);
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
        private int GetDiscountedWishlistCount()
        {
            int count = 0;
            if (_currentUserID < 1)
                return count;

            using (SqlConnection conn = new SqlConnection(_connString))
            {
                string sql = @"SELECT COUNT(*) 
                       FROM Wishlist w
                       INNER JOIN Products p ON w.ProductID = p.ProductID
                       WHERE w.UserID = @UserID AND p.DiscountPercent > 0";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", _currentUserID);
                    conn.Open();
                    count = (int)cmd.ExecuteScalar();
                }
            }
            return count;
        }
        private void LoadCategories()
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                string sql = "SELECT DISTINCT Category FROM Products WHERE Category IS NOT NULL AND Category <> ''";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            string category = rdr["Category"].ToString();
                            ddlCategory.Items.Add(new ListItem(category, category));
                        }
                    }
                }
            }
        }

    }
}
