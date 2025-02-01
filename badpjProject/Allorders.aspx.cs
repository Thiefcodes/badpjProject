using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace badpjProject
{
    public partial class AllOrders : System.Web.UI.Page
    {
        private string _connString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;

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
                LoadAllOrders();
            }
        }

        private void LoadAllOrders()
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                conn.Open();

                string query = @"
                    SELECT o.OrderID, u.Login_Name AS UserName, o.ProductName, o.Quantity, 
                           CONCAT(o.Address, ', ', o.City, ', ', o.PostalCode) AS FullAddress,
                           o.OrderDate, o.Status
                    FROM Orders o
                    INNER JOIN [Table] u ON o.UserID = u.Id
                    ORDER BY o.OrderDate DESC";

                SqlCommand cmd = new SqlCommand(query, conn);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    List<AllOrdersModel> orders = new List<AllOrdersModel>();
                    while (reader.Read())
                    {
                        orders.Add(new AllOrdersModel
                        {
                            OrderID = (int)reader["OrderID"],
                            UserName = reader["UserName"].ToString(),
                            ProductName = reader["ProductName"].ToString(),
                            Quantity = (int)reader["Quantity"],
                            FullAddress = reader["FullAddress"].ToString(),
                            OrderDate = (DateTime)reader["OrderDate"],
                            Status = reader["Status"].ToString()
                        });
                    }

                    gvAllOrders.DataSource = orders;
                    gvAllOrders.DataBind();
                }
            }
        }

        protected void gvAllOrders_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "UpdateStatus")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gvAllOrders.Rows[rowIndex];
                DropDownList ddlStatus = (DropDownList)row.FindControl("ddlStatus");
                string newStatus = ddlStatus.SelectedValue;

                if (string.IsNullOrEmpty(newStatus))
                {
                    Response.Write("<script>alert('Status cannot be empty.');</script>");
                    return;
                }

                int orderId = Convert.ToInt32(gvAllOrders.DataKeys[row.RowIndex].Value);
                UpdateOrderStatus(orderId, newStatus);
                LoadAllOrders();
                Response.Write("<script>alert('Order status updated successfully!');</script>");
            }
            else if (e.CommandName == "DeleteOrder")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                int orderId = Convert.ToInt32(gvAllOrders.DataKeys[rowIndex].Value);
                DeleteOrder(orderId);
                LoadAllOrders();
                Response.Write("<script>alert('Order deleted successfully!');</script>");
            }
        }

        private void DeleteOrder(int orderId)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                conn.Open();

                string query = "DELETE FROM Orders WHERE OrderID = @OrderID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@OrderID", orderId);

                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected == 0)
                {
                    Response.Write("<script>alert('Failed to delete the order.');</script>");
                }
            }
        }

        private void UpdateOrderStatus(int orderId, string newStatus)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                conn.Open();

                string query = "UPDATE Orders SET Status = @Status WHERE OrderID = @OrderID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Status", newStatus);
                cmd.Parameters.AddWithValue("@OrderID", orderId);

                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected == 0)
                {
                    Response.Write("<script>alert('Failed to update order status.');</script>");
                }
            }
        }
    }

    public class AllOrdersModel
    {
        public int OrderID { get; set; }
        public string UserName { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public string FullAddress { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
    }
}