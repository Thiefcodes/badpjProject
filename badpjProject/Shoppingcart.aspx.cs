using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace badpjProject
{
    public partial class ShoppingCart : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindCart();
            }
        }

        private void BindCart()
        {
            if (Session["Cart"] != null)
            {
                List<CartItem> cart = (List<CartItem>)Session["Cart"];

                if (cart.Count > 0)
                {
                    gvCart.DataSource = cart;
                    gvCart.DataBind();
                }
                else
                {
                    gvCart.DataSource = null;
                    gvCart.DataBind();
                }
                decimal total = 0;
                foreach (var item in cart)
                {
                    total += item.Price * item.Quantity;
                }
                lblTotal.Text = "Total: " + total.ToString("C");
            }
        }

        protected void RemoveFromCart_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int productId = Convert.ToInt32(btn.CommandArgument);

            if (Session["Cart"] != null)
            {
                List<CartItem> cart = (List<CartItem>)Session["Cart"];
                CartItem itemToRemove = cart.Find(item => item.ProductID == productId);

                if (itemToRemove != null)
                {
                    cart.Remove(itemToRemove);
                }

                Session["Cart"] = cart;
                BindCart();
            }
        }

        protected void Checkout_Click(object sender, EventArgs e)
        {
            if (Session["Cart"] != null && ((List<CartItem>)Session["Cart"]).Count > 0)
            {
                Response.Redirect("Checkout.aspx");
            }
            else
            {
                lblTotal.Text = "Your cart is empty!";
            }
        }
        protected void IncreaseQuantity_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int productId = Convert.ToInt32(btn.CommandArgument);

            if (Session["Cart"] != null)
            {
                List<CartItem> cart = (List<CartItem>)Session["Cart"];
                CartItem existingItem = cart.Find(item => item.ProductID == productId);

                if (existingItem != null)
                {
                    existingItem.Quantity++;
                }

                Session["Cart"] = cart;
                BindCart();
            }
        }

        protected void DecreaseQuantity_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int productId = Convert.ToInt32(btn.CommandArgument);

            if (Session["Cart"] != null)
            {
                List<CartItem> cart = (List<CartItem>)Session["Cart"];
                CartItem existingItem = cart.Find(item => item.ProductID == productId);

                if (existingItem != null)
                {
                    if (existingItem.Quantity > 1)
                    {
                        existingItem.Quantity--;
                    }
                    else
                    {
                        cart.Remove(existingItem);
                    }
                }

                Session["Cart"] = cart;
                BindCart();
            }
        }
    }


    [Serializable]
    public class CartItem
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}