using System;
using System.Collections.Generic;
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
                    Response.Redirect("ShoppingCart.aspx"); // Redirect if cart is empty
                }
            }
            else
            {
                Response.Redirect("ShoppingCart.aspx"); // Redirect if session expired
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