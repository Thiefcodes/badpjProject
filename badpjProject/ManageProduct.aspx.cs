using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using badpjProject.Models;
using System.Configuration;

namespace badpjProject
{
    public partial class ManageProduct : System.Web.UI.Page
    {
        private string _connString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;

        protected void Page_PreInit(object sender, EventArgs e)
        {
            this.MasterPageFile = Session["UserID"] == null ? "~/Site.Master" : "~/Site1loggedin.Master";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Role"]?.ToString() != "Staff")
            {
                Response.Redirect("~/Shop.aspx");
            }
            if (!IsPostBack)
            {
                LoadProducts();
            }
        }

        private void LoadProducts()
        {
            List<Product> productList = new List<Product>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                string sql = "SELECT ProductID, ProductName, Price FROM dbo.Products ORDER BY ProductID";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            productList.Add(new Product
                            {
                                ProductID = rdr.GetInt32(0),
                                ProductName = rdr.GetString(1),
                                Price = rdr.GetDecimal(2)
                            });
                        }
                    }
                }
            }
            gvProducts.DataSource = productList;
            gvProducts.DataBind();
        }

        protected void gvProducts_RowEditing(object sender, GridViewEditEventArgs e)
        {
            int productId = (int)gvProducts.DataKeys[e.NewEditIndex].Value;
            Response.Redirect("CreateProduct.aspx?productID=" + productId);
        }

        protected void gvProducts_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int productId = (int)gvProducts.DataKeys[e.RowIndex].Value;
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                // Delete associated reviews first to avoid FK conflicts.
                string deleteReviewsQuery = "DELETE FROM Reviews WHERE ProductID = @ProductID";
                using (SqlCommand cmd = new SqlCommand(deleteReviewsQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@ProductID", productId);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
                // Delete associated wishlist entries.
                string deleteWishlistQuery = "DELETE FROM Wishlist WHERE ProductID = @ProductID";
                using (SqlCommand cmd = new SqlCommand(deleteWishlistQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@ProductID", productId);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
                // Now delete the product.
                string sql = "DELETE FROM dbo.Products WHERE ProductID = @ProductID";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ProductID", productId);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            LoadProducts();
        }
    }
}
