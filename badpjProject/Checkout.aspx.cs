using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace badpjProject
{
    public partial class Checkout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadOrderSummary();
            }
        }

        private void LoadOrderSummary()
        {
            if (Session["Cart"] != null)
            {
                List<CartItem> cart = (List<CartItem>)Session["Cart"];

                if (cart.Count > 0)
                {
                    List<OrderSummaryItem> orderSummary = new List<OrderSummaryItem>();
                    decimal totalAmount = 0;

                    foreach (var item in cart)
                    {
                        orderSummary.Add(new OrderSummaryItem
                        {
                            ProductName = item.ProductName,
                            Quantity = item.Quantity,
                            Price = item.Price,
                            TotalPrice = item.Price * item.Quantity
                        });

                        totalAmount += item.Price * item.Quantity;
                    }

                    gvOrderSummary.DataSource = orderSummary;
                    gvOrderSummary.DataBind();

                    lblTotalAmount.Text = "Total Amount: " + totalAmount.ToString("C");
                }
                else
                {
                    Response.Redirect("ShoppingCart.aspx");
                }
            }
            else
            {
                Response.Redirect("ShoppingCart.aspx");
            }
        }
        protected void btnPlaceOrder_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            if (Session["Cart"] == null)
            {
                Response.Write("<script>alert('Your cart is empty.');</script>");
                return;
            }

            List<CartItem> cart = (List<CartItem>)Session["Cart"];
            string connStr = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        int orderId = new Random().Next(1000, 9999);

                        string insertQuery = @"
                    INSERT INTO Orders (OrderID, UserID, ProductName, Quantity, Price, Address, City, PostalCode, OrderDate, Status)
                    VALUES (@OrderID, @UserID, @ProductName, @Quantity, @Price, @Address, @City, @PostalCode, GETDATE(), 'Pending')";

                        foreach (var item in cart)
                        {
                            SqlCommand cmd = new SqlCommand(insertQuery, conn, transaction);
                            cmd.Parameters.AddWithValue("@OrderID", orderId);
                            cmd.Parameters.AddWithValue("@UserID", Session["UserID"]);
                            cmd.Parameters.AddWithValue("@ProductName", item.ProductName);
                            cmd.Parameters.AddWithValue("@Quantity", item.Quantity);
                            cmd.Parameters.AddWithValue("@Price", item.Price);
                            cmd.Parameters.AddWithValue("@Address", txtAddress.Text);
                            cmd.Parameters.AddWithValue("@City", txtCity.Text);
                            cmd.Parameters.AddWithValue("@PostalCode", txtPostalCode.Text);

                            cmd.ExecuteNonQuery();
                        }
                        transaction.Commit();
                        Session["Cart"] = null;
                        Response.Redirect("ThankYou.aspx");
                    }
                    catch (Exception ex)
                    {
                        if (transaction.Connection != null)
                        {
                            transaction.Rollback();
                        }

                        Response.Write("<script>alert('An error occurred while placing the order: " + ex.Message + "');</script>");
                    }
                }
            }
        }



        public class OrderSummaryItem
        {
            public string ProductName { get; set; }
            public int Quantity { get; set; }
            public decimal Price { get; set; }
            public decimal TotalPrice { get; set; }
        }
    }
}