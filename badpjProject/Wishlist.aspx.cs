using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace badpjProject
{
    public partial class Wishlist : System.Web.UI.Page
    {
        private string _connString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;
        private int _currentUserID;

        protected int? EditingProductID
        {
            get { return (int?)ViewState["EditingProductID"]; }
            set { ViewState["EditingProductID"] = value; }
        }

        protected void Page_PreInit(object sender, EventArgs e)
        {
            this.MasterPageFile = Session["UserID"] == null ? "~/Site.Master" : "~/Site1loggedin.Master";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] != null)
                _currentUserID = Convert.ToInt32(Session["UserID"]);
            else
                _currentUserID = -1;

            if (_currentUserID < 1)
            {
                Response.Write("<script>alert('You need to be logged in to view your wishlist!');</script>");
                Response.Redirect("Shop.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadWishlist();
            }
        }

        private void LoadWishlist()
        {
            string sql = @"
                SELECT p.ProductID, 
                       p.ProductName, 
                       p.Description, 
                       p.ImageUrl, 
                       p.Price, 
                       p.DiscountPercent, 
                       p.Category, 
                       (SELECT AVG(CAST(StarRating AS DECIMAL(10,2))) 
                        FROM Reviews r 
                        WHERE r.ProductID = p.ProductID) AS AverageRating,
                       w.Notes
                FROM dbo.Wishlist w
                INNER JOIN dbo.Products p ON w.ProductID = p.ProductID
                WHERE w.UserID = @UserID";

            var items = new List<dynamic>();

            using (SqlConnection conn = new SqlConnection(_connString))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@UserID", _currentUserID);
                conn.Open();

                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        items.Add(new
                        {
                            ProductID = rdr.GetInt32(0),
                            ProductName = rdr.GetString(1),
                            Description = rdr.IsDBNull(2) ? "" : rdr.GetString(2),
                            ImageUrl = rdr.GetString(3),
                            Price = rdr.GetDecimal(4),
                            DiscountPercent = rdr.IsDBNull(5) ? 0 : rdr.GetInt32(5),
                            Category = rdr.IsDBNull(6) ? "" : rdr.GetString(6),
                            AverageRating = rdr["AverageRating"] != DBNull.Value
                                            ? Convert.ToDecimal(rdr["AverageRating"])
                                            : (decimal?)null,
                            Notes = rdr["Notes"] == DBNull.Value ? "" : rdr["Notes"].ToString()
                        });
                    }
                }
            }

            rptWishlist.DataSource = items;
            rptWishlist.DataBind();
        }

        /// <summary>
        /// Now that OnItemDataBound is assigned in .aspx, this will be called for each item
        /// so we can show/hide the placeholders and link button.
        /// </summary>
        protected void rptWishlist_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            // Only process data rows
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                dynamic rowData = e.Item.DataItem;
                int productId = rowData.ProductID;

                var phView = e.Item.FindControl("phView") as PlaceHolder;
                var phEdit = e.Item.FindControl("phEdit") as PlaceHolder;
                var linkEdit = e.Item.FindControl("LinkButton1") as LinkButton;  // Single "Edit Note" link
                var btnUpdate = e.Item.FindControl("btnUpdate") as LinkButton;
                var btnCancel = e.Item.FindControl("btnCancel") as LinkButton;

                // Toggle edit/view mode based on the EditingProductID.
                if (EditingProductID.HasValue && EditingProductID.Value == productId)
                {
                    if (phView != null) phView.Visible = false;
                    if (phEdit != null) phEdit.Visible = true;
                    if (linkEdit != null) linkEdit.Visible = false;
                    if (btnUpdate != null) btnUpdate.Visible = true;
                    if (btnCancel != null) btnCancel.Visible = true;
                }
                else
                {
                    if (phView != null) phView.Visible = true;
                    if (phEdit != null) phEdit.Visible = false;
                    if (linkEdit != null) linkEdit.Visible = true;
                    if (btnUpdate != null) btnUpdate.Visible = false;
                    if (btnCancel != null) btnCancel.Visible = false;
                }
            }
        }

        protected void rptWishlist_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int productId = Convert.ToInt32(e.CommandArgument);

            switch (e.CommandName)
            {
                case "Edit":
                    EditingProductID = productId;
                    LoadWishlist();
                    break;
                case "Cancel":
                    EditingProductID = null;
                    LoadWishlist();
                    break;
                case "Update":
                    TextBox txtNotes = (TextBox)e.Item.FindControl("txtNotes");
                    string newNotes = txtNotes.Text.Trim();
                    UpdateWishlistNotes(productId, newNotes);
                    EditingProductID = null;
                    LoadWishlist();
                    break;
                case "Remove":
                    RemoveFromWishlist(productId);
                    LoadWishlist();
                    break;
                case "AddToCart":
                    AddToCart(productId);
                    break;
            }
        }

        private void UpdateWishlistNotes(int productId, string newNotes)
        {
            string sql = @"UPDATE dbo.Wishlist 
                           SET Notes = @Notes 
                           WHERE ProductID = @ProductID AND UserID = @UserID";
            using (SqlConnection conn = new SqlConnection(_connString))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Notes", newNotes);
                cmd.Parameters.AddWithValue("@ProductID", productId);
                cmd.Parameters.AddWithValue("@UserID", _currentUserID);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private void RemoveFromWishlist(int productId)
        {
            string sql = @"DELETE FROM dbo.Wishlist 
                           WHERE ProductID = @ProductID AND UserID = @UserID";
            using (SqlConnection conn = new SqlConnection(_connString))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@ProductID", productId);
                cmd.Parameters.AddWithValue("@UserID", _currentUserID);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private void AddToCart(int productId)
        {
            if (_currentUserID < 1)
            {
                Response.Write("<script>alert('Please log in first!');</script>");
                return;
            }

            string query = "SELECT ProductID, ProductName, Description, ImageUrl, Price FROM Products WHERE ProductID = @ProductID";
            using (SqlConnection conn = new SqlConnection(_connString))
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

                        List<CartItem> cart = (List<CartItem>)Session["Cart"];
                        if (cart == null)
                        {
                            cart = new List<CartItem>();
                        }
                        CartItem existingItem = cart.Find(item => item.ProductID == newItem.ProductID);
                        if (existingItem != null)
                        {
                            existingItem.Quantity++;
                        }
                        else
                        {
                            cart.Add(newItem);
                            string script = "Swal.fire({ icon: 'success', title: 'Added to Cart', text: 'You have successfully added the product to your cart!' });";
                            ScriptManager.RegisterStartupScript(this, GetType(), "discountAlert", script, true);
                        }
                        Session["Cart"] = cart;
                    }
                }
            }
        }

        protected void btnShareWishlist_Click(object sender, EventArgs e)
        {
            string baseUrl = Request.Url.GetLeftPart(UriPartial.Authority);
            string shareUrl = baseUrl + "/Wishlist.aspx?userId=" + _currentUserID;

            string shareHtml = @"
                <div style='text-align: left;'>
                    <p><strong>Email:</strong> <a href='mailto:?subject=My Wishlist&body=Check out my wishlist here: " + shareUrl + @"' target='_blank'>Share via Email</a></p>
                    <p><strong>X:</strong> <a href='https://twitter.com/intent/tweet?text=Check out my wishlist: " + shareUrl + @"' target='_blank'>Share on X (Twitter)</a></p>
                    <p><strong>Facebook:</strong> <a href='https://www.facebook.com/sharer/sharer.php?u=" + shareUrl + @"' target='_blank'>Share on Facebook</a></p>
                </div>";

            string script = "Swal.fire({ " +
                            "title: 'Share Your Wishlist', " +
                            "html: \"" + shareHtml.Replace("\"", "\\\"").Replace("\r", "").Replace("\n", "") + "\", " +
                            "showCloseButton: true" +
                            "});";

            ScriptManager.RegisterStartupScript(this, GetType(), "shareWishlist", script, true);
        }
    }
}
