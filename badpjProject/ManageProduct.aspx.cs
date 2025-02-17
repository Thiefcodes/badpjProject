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
                string sql = "SELECT ProductID, ProductName, Price, DiscountPercent FROM dbo.Products ORDER BY ProductID";
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
                                Price = rdr.GetDecimal(2),
                                DiscountPercent = rdr.IsDBNull(3) ? 0 : rdr.GetInt32(3)
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
                string deleteReviewsQuery = "DELETE FROM Reviews WHERE ProductID = @ProductID";
                using (SqlCommand cmd = new SqlCommand(deleteReviewsQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@ProductID", productId);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
                string deleteWishlistQuery = "DELETE FROM Wishlist WHERE ProductID = @ProductID";
                using (SqlCommand cmd = new SqlCommand(deleteWishlistQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@ProductID", productId);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
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
        protected void gvProducts_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "UpdateDiscount")
            {
                int productId = Convert.ToInt32(e.CommandArgument);

                GridViewRow row = ((LinkButton)e.CommandSource).NamingContainer as GridViewRow;
                TextBox txtDiscount = row.FindControl("txtDiscount") as TextBox;
                int discount;
                if (int.TryParse(txtDiscount.Text, out discount))
                {
                    UpdateProductDiscount(productId, discount);
                    Response.Write("<script>alert('Discount updated successfully!');</script>");
                    LoadProducts(); 
                }
                else
                {
                    Response.Write("<script>alert('Failed to Update!');</script>");
                }
            }
        }
        private void UpdateProductDiscount(int productId, int discount)
        {
            string sql = "UPDATE dbo.Products SET DiscountPercent = @Discount WHERE ProductID = @ProductID";
            using (SqlConnection conn = new SqlConnection(_connString))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Discount", discount);
                cmd.Parameters.AddWithValue("@ProductID", productId);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }


    }
}
