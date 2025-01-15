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
        private string _connString =
            ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;
        private int _productId;
        private int _currentUserID;
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                this.MasterPageFile = "~/Site.Master";
            }
            else
            {
                this.MasterPageFile = "~/Site1loggedin.Master";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] != null)
                _currentUserID = Convert.ToInt32(Session["UserID"]);
            else
                _currentUserID = -1;

            if (!IsPostBack)
            {
                if (int.TryParse(Request.QueryString["productID"], out _productId))
                {
                    LoadProduct(_productId);
                    UpdateCartCount();

                    bool isInWishlist = IsProductInWishlist(_productId, _currentUserID);
                    lblWishlistIndicator.Visible = isInWishlist;
                }
            }
        }

        private void LoadProduct(int productId)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                string sql = @"SELECT ProductName, Description, ImageUrl, Price 
                               FROM dbo.Products
                               WHERE ProductID=@ProductID";
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
                            decimal price = Convert.ToDecimal(rdr["Price"]);
                            lblPrice.Text = price.ToString("C");
                        }
                    }
                }
            }
        }

        private bool IsProductInWishlist(int productID, int userID)
        {
            if (userID == 0)
                return false;

            bool exists = false;
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                string sql = @"SELECT COUNT(*) FROM dbo.Wishlist
                               WHERE ProductID = @ProductID AND UserID = @UserID";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ProductID", productID);
                    cmd.Parameters.AddWithValue("@UserID", userID);
                    conn.Open();
                    int count = (int)cmd.ExecuteScalar();
                    exists = (count > 0);
                }
            }
            return exists;
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
            string query = "SELECT ProductID, ProductName, Description, ImageUrl, Price FROM Products WHERE ProductID = @ProductID";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ProductID", productId);
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

    }
}

