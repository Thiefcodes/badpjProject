using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace badpjProject
{
    public partial class Wishlist : System.Web.UI.Page
    {
        private string _connString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;
        private int _currentUserID;

        // Property to track which product is currently being edited.
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
                SELECT p.ProductID, p.ProductName, p.ImageUrl, p.Price, w.Notes
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
                            ImageUrl = rdr.GetString(2),
                            Price = rdr.GetDecimal(3),
                            Notes = rdr["Notes"] == DBNull.Value ? "" : rdr["Notes"].ToString()
                        });
                    }
                }
            }
            rptWishlist.DataSource = items;
            rptWishlist.DataBind();
        }

        protected void rptWishlist_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                // Retrieve current row's product ID.
                dynamic rowData = e.Item.DataItem;
                int productId = rowData.ProductID;

                // Get references to the placeholders and buttons.
                var phView = e.Item.FindControl("phView") as PlaceHolder;
                var phEdit = e.Item.FindControl("phEdit") as PlaceHolder;
                var btnEdit = e.Item.FindControl("btnEdit") as LinkButton;
                var btnUpdate = e.Item.FindControl("btnUpdate") as LinkButton;
                var btnCancel = e.Item.FindControl("btnCancel") as LinkButton;

                // Toggle edit/view mode based on the EditingProductID.
                if (EditingProductID.HasValue && EditingProductID.Value == productId)
                {
                    if (phView != null) phView.Visible = false;
                    if (phEdit != null) phEdit.Visible = true;
                    if (btnEdit != null) btnEdit.Visible = false;
                    if (btnUpdate != null) btnUpdate.Visible = true;
                    if (btnCancel != null) btnCancel.Visible = true;
                }
                else
                {
                    if (phView != null) phView.Visible = true;
                    if (phEdit != null) phEdit.Visible = false;
                    if (btnEdit != null) btnEdit.Visible = true;
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
                    Response.Write("<script>alert('Item added to cart!');</script>");
                    break;
            }
        }

        private void UpdateWishlistNotes(int productId, string newNotes)
        {
            string sql = @"UPDATE dbo.Wishlist SET Notes = @Notes WHERE ProductID = @ProductID AND UserID = @UserID";
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
            string sql = @"DELETE FROM dbo.Wishlist WHERE ProductID = @ProductID AND UserID = @UserID";
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
                            Response.Write("<script>alert('Item added to cart!');</script>");
                        }
                        Session["Cart"] = cart;
                    }
                }
            }
        }
    }
}
