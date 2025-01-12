using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using badpjProject.Models;

namespace badpjProject
{
    public partial class Shop : System.Web.UI.Page
    {
        private string _connString =
            System.Configuration.ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
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
                string sql = "SELECT ProductID, ProductName, Description, ImageUrl, Price FROM dbo.Products";
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
                                Price = rdr.GetDecimal(4)
                            };
                            productList.Add(p);
                        }
                    }
                }
            }

            rptProducts.DataSource = productList;
            rptProducts.DataBind();
        }

        protected void AddToCart_Command(object sender, System.Web.UI.WebControls.CommandEventArgs e)
        {
            int productId = Convert.ToInt32(e.CommandArgument);
            // For now, just redirect to cart. In practice, you'd store productId in session or DB.
            Response.Redirect("Shoppingcart.aspx");
        }
    }
}