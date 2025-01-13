using System;
using System.Data.SqlClient;
using badpjProject.Models;
using System.Configuration;

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

        protected void btnAddToCart_Click(object sender, EventArgs e)
        {
            // Add to cart code!
            Response.Redirect("Shoppingcart.aspx");
        }
    }
}
