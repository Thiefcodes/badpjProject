using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using badpjProject.Models;
using System.Configuration;
using System.Net.Mail;
using System.Net;
using System.Web.UI;

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
                string sql = @"
                SELECT ProductID, ProductName, Price, DiscountPercent, ImageUrl
                FROM dbo.Products
                ORDER BY ProductID";
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
                                DiscountPercent = rdr.IsDBNull(3) ? 0 : rdr.GetInt32(3),
                                ImageUrl = rdr.IsDBNull(4) ? "" : rdr.GetString(4)
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
                    string script = "Swal.fire({ " +
                        "icon: 'success', " +
                        "title: 'Product Deleted', " +
                        "text: 'Product deleted successfully!' " +
                        "});";
                    ScriptManager.RegisterStartupScript(this, GetType(), "ProductDeletedAlert", script, true);
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
                    string script = "Swal.fire({ " +
                        "icon: 'success', " +
                        "title: 'Discount Applied', " +
                        "text: 'Discount of " + discount + "% applied successfully!' " +
                        "});";
                    ScriptManager.RegisterStartupScript(this, GetType(), "DiscountAlert", script, true);
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

            NotifyUsersOfDiscount(productId, discount);
        }

        private void NotifyUsersOfDiscount(int productId, int discount)
        {
            string productName = GetProductName(productId);

            using (SqlConnection conn = new SqlConnection(_connString))
            {
                string sql = @"SELECT DISTINCT t.Email 
                       FROM Wishlist w 
                       INNER JOIN [Table] t ON w.UserID = t.Id 
                       WHERE w.ProductID = @ProductID";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ProductID", productId);
                    conn.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            string email = rdr["Email"].ToString();
                            SendDiscountEmail(email, productName, discount, productId);
                        }
                    }
                }
            }
        }


        private string GetProductName(int productId)
        {
            string productName = "";
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
                        productName = result.ToString();
                    }
                }
            }
            return productName;
        }

        private void SendDiscountEmail(string email, string productName, int discount, int productId)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("ruihernh@gmail.com"); 
                mail.To.Add(email);
                mail.Subject = "Discount Alert!";

                string productLink = "http://localhost:44300/ProductDetails.aspx?productID=" + productId;
                mail.Body = $"Good news! The product '{productName}' is now on sale with a discount of {discount}%!\n\n" +
                            $"Check it out here: {productLink}";

                smtpServer.Port = 587;
                smtpServer.Credentials = new NetworkCredential("ruihernh@gmail.com", "yqqh pwcr byeq sseo"); 
                smtpServer.EnableSsl = true;

                smtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                
            }
        }


    }
}
