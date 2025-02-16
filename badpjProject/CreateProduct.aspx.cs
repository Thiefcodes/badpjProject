using System;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using badpjProject.Models;

namespace badpjProject
{
    public partial class CreateProduct : System.Web.UI.Page
    {
        private string _connString =
            ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;
        private int _productId = 0;
        private bool _isEditMode = false;

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
            if (Session["Role"]?.ToString() != "Staff")
            {
                Response.Redirect("~/Shop.aspx");
            }

            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["productID"]))
                {
                    if (int.TryParse(Request.QueryString["productID"], out _productId))
                    {
                        _isEditMode = true;
                        LoadProduct(_productId);
                    }
                }
            }
        }

        private void LoadProduct(int productId)
        {
            // Updated query to include Category.
            string sql = @"SELECT ProductName, Description, ImageUrl, Price, Category
                           FROM dbo.Products
                           WHERE ProductID = @ProductID";

            using (SqlConnection conn = new SqlConnection(_connString))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@ProductID", productId);
                conn.Open();

                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    if (rdr.Read())
                    {
                        txtName.Text = rdr["ProductName"].ToString();
                        txtDescription.Text = rdr["Description"].ToString();
                        txtPrice.Text = rdr["Price"].ToString();
                        txtCategory.Text = rdr["Category"] == DBNull.Value ? "" : rdr["Category"].ToString();

                        string imageUrl = rdr["ImageUrl"].ToString();
                        if (!string.IsNullOrEmpty(imageUrl))
                        {
                            imgPreview.ImageUrl = imageUrl;
                            imgPreview.Visible = true;
                        }
                    }
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            string description = txtDescription.Text.Trim();
            string category = txtCategory.Text.Trim();
            decimal price;
            if (!decimal.TryParse(txtPrice.Text, out price))
            {
                lblMessage.Text = "Invalid price. Please enter a valid number.";
                return;
            }

            string finalImageUrl = null;
            if (fuProductImage.HasFile)
            {
                string fileName = Path.GetFileName(fuProductImage.FileName);
                string savePath = Server.MapPath("~/Uploads/" + fileName);
                fuProductImage.SaveAs(savePath);

                finalImageUrl = "~/Uploads/" + fileName;
            }
            else
            {
                if (_isEditMode && imgPreview.Visible)
                {
                    finalImageUrl = imgPreview.ImageUrl;
                }
                else
                {
                    finalImageUrl = "~/Uploads/no-image.png";
                }
            }

            if (!string.IsNullOrEmpty(Request.QueryString["productID"]))
            {
                if (int.TryParse(Request.QueryString["productID"], out _productId))
                {
                    UpdateProduct(_productId, name, description, category, finalImageUrl, price);
                }
            }
            else
            {
                InsertProduct(name, description, category, finalImageUrl, price);
            }

            Response.Redirect("ManageProduct.aspx");
        }

        private void InsertProduct(string name, string description, string category, string imageUrl, decimal price)
        {
            string sql = @"INSERT INTO dbo.Products (ProductName, Description, Category, ImageUrl, Price)
                           VALUES (@Name, @Description, @Category, @ImageUrl, @Price)";

            using (SqlConnection conn = new SqlConnection(_connString))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Description", description);
                cmd.Parameters.AddWithValue("@Category", category);
                cmd.Parameters.AddWithValue("@ImageUrl", imageUrl);
                cmd.Parameters.AddWithValue("@Price", price);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private void UpdateProduct(int productId, string name, string description, string category, string imageUrl, decimal price)
        {
            string sql = @"UPDATE dbo.Products
                           SET ProductName = @Name,
                               Description = @Description,
                               Category = @Category,
                               ImageUrl = @ImageUrl,
                               Price = @Price
                           WHERE ProductID = @ProductID";

            using (SqlConnection conn = new SqlConnection(_connString))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@ProductID", productId);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Description", description);
                cmd.Parameters.AddWithValue("@Category", category);
                cmd.Parameters.AddWithValue("@ImageUrl", imageUrl);
                cmd.Parameters.AddWithValue("@Price", price);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
