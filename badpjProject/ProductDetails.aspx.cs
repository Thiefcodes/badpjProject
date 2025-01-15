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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (int.TryParse(Request.QueryString["productID"], out _productId))
                {
                    LoadProduct(_productId);
                    UpdateCartCount();
                }
            }
        }

        private void LoadProduct(int productId)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                string sql = @"SELECT ProductID, ProductName, Description, ImageUrl, Price 
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

                            // ✅ Store ProductID in HiddenField
                            hfProductID.Value = productId.ToString();
                        }
                    }
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

            // ✅ Read ProductID from HiddenField
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

                        // ✅ Ensure cart count updates dynamically
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

