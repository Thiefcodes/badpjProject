﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using badpjProject.Models;

namespace badpjProject
{
    public partial class Shop : System.Web.UI.Page
    {
        private string _connString =
            System.Configuration.ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
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
                string sql = "SELECT ProductID, ProductName, Description, ImageUrl, Price FROM dbo.Products";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            Product p = new Product
                            {
                                ProductID = rdr.GetInt32(0),
                                ProductName = rdr.GetString(1),
                                Description = rdr.GetString(2),
                                ImageUrl = rdr.GetString(3),
                                Price = rdr.GetDecimal(4)
                            };
                            productList.Add(p);
                        }
                    }
                }
            }

            rptProducts.DataSource = productList;
            rptProducts.DataBind();
        }

        protected void AddToCart_Command(object sender, CommandEventArgs e)
        {
            int productId = Convert.ToInt32(e.CommandArgument);
            string connStr = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;
            string query = "SELECT ProductID, ProductName, Description, ImageUrl, Price FROM Products WHERE ProductID = @ProductID";

            using (SqlConnection conn = new SqlConnection(connStr))
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
                        }
                        Session["Cart"] = cart;
                    }
                }
            }
        }
    }
}