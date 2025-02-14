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
    public partial class Orders : System.Web.UI.Page
    {
        private string _connString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadOrders();
            }
        }

        private void LoadOrders()
        {
            if (Session["UserID"] == null)
            {
                Response.Write("<script>alert('Please log in to view your orders.'); window.location='Login.aspx';</script>");
                return;
            }

            List<Order> orders = new List<Order>();

            using (SqlConnection conn = new SqlConnection(_connString))
            {
                conn.Open();

                string query = @"SELECT OrderID, OrderDate, Status, Address, City, PostalCode, ProductName, Quantity, Price
                                 FROM Orders
                                 WHERE UserID = @UserID
                                 ORDER BY OrderID, OrderDate";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", Session["UserID"]);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int orderId = (int)reader["OrderID"];
                            var existingOrder = orders.FirstOrDefault(o => o.OrderID == orderId);
                            if (existingOrder == null)
                            {
                                existingOrder = new Order
                                {
                                    OrderID = orderId,
                                    OrderDate = (DateTime)reader["OrderDate"],
                                    Status = reader["Status"].ToString(),
                                    FullAddress = $"{reader["Address"]}, {reader["City"]}, {reader["PostalCode"]}",
                                    Items = new List<OrderDetail>()
                                };

                                orders.Add(existingOrder);
                            }
                            existingOrder.Items.Add(new OrderDetail
                            {
                                ProductName = reader["ProductName"].ToString(),
                                Quantity = (int)reader["Quantity"],
                                Price = (decimal)reader["Price"]
                            });
                        }
                    }
                }
            }
            rptOrders.DataSource = orders;
            rptOrders.DataBind();
        }

        protected void rptOrders_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Refund")
            {
                int orderId = Convert.ToInt32(e.CommandArgument);

                using (SqlConnection conn = new SqlConnection(_connString))
                {
                    conn.Open();

                    // Retrieve the current status of the order
                    string query = "SELECT Status FROM Orders WHERE OrderID = @OrderID";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@OrderID", orderId);
                        string currentStatus = cmd.ExecuteScalar()?.ToString();

                        // Check if the order is eligible for a refund
                        if (currentStatus == "Shipping" || currentStatus == "Shipped")
                        {
                            // Inform the user that refund is not allowed for this status
                            Response.Write("<script>alert('Refunds are not allowed for orders with status Shipping or Shipped.');</script>");
                            return;
                        }
                    }
                }

                // If eligible, update the order status to Refund
                UpdateOrderStatusToRefund(orderId);
                LoadOrders();
                Response.Write("<script>alert('Order has been marked as refunded.');</script>");
            }
        }

        private void UpdateOrderStatusToRefund(int orderId)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                conn.Open();

                // Check if the order already has "(Refund)" in the ID
                string checkQuery = "SELECT Status FROM Orders WHERE OrderID = @OrderID";
                using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@OrderID", orderId);
                    string currentStatus = (string)checkCmd.ExecuteScalar();

                    // If already marked as Refund, do nothing
                    if (currentStatus == "Refund")
                    {
                        Response.Write("<script>alert('This order is already refunded.');</script>");
                        return;
                    }
                }

                // Update the order status to "Refund"
                string updateQuery = "UPDATE Orders SET Status = 'Refund' WHERE OrderID = @OrderID";
                using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@OrderID", orderId);
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected == 0)
                    {
                        Response.Write("<script>alert('Failed to update order status.');</script>");
                    }
                }
            }
        }
    }

    public class Order
    {
        public int OrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public string FullAddress { get; set; }
        public List<OrderDetail> Items { get; set; }
    }

    public class OrderDetail
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
