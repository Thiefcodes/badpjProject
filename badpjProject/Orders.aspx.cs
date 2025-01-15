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