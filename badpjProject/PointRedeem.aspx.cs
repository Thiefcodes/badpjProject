﻿using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web.UI.WebControls;

namespace badpjProject
{
    public partial class PointRedeem : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Ensure the user is logged in and has the 'User' role.
            if (Session["UserId"] == null || Session["Role"] == null || Session["Role"].ToString() != "User")
            {
                Response.Write("<script>alert('Access Denied: Only users can log workout sessions.');window.location='Login.aspx';</script>");
                Response.End();
            }
            if (!IsPostBack)
            {
                LoadUserPoints();
                LoadRedeemProducts();
            }
        }

        private void LoadUserPoints()
        {
            int userId = Convert.ToInt32(Session["UserId"]);
            string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT ISNULL(RewardPoints, 0) FROM [dbo].[Table] WHERE Id = @UserId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    object pointsObj = cmd.ExecuteScalar();
                    int points = pointsObj != null ? Convert.ToInt32(pointsObj) : 0;
                    lblUserPoints.Text = "Your Points: " + points;
                }
            }
        }

        private void LoadRedeemProducts()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                // Query now references RedeemProduct
                string query = "SELECT ProductId, ProductName, ProductImage, CostPoints FROM RedeemProduct";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        ProductsRepeater.DataSource = reader;
                        ProductsRepeater.DataBind();
                    }
                }
            }
        }

        protected void btnRedeem_Command(object sender, CommandEventArgs e)
        {
            int productId = Convert.ToInt32(e.CommandArgument);
            int userId = Convert.ToInt32(Session["UserId"]);
            int cost = 0;
            string productName = "";

            string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;
            // Retrieve the product's cost and name from RedeemProduct table.
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT CostPoints, ProductName FROM RedeemProduct WHERE ProductId = @ProductId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ProductId", productId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            cost = Convert.ToInt32(reader["CostPoints"]);
                            productName = reader["ProductName"].ToString();
                        }
                    }
                }
            }

            // Check user's points.
            int userPoints = 0;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT ISNULL(RewardPoints,0) FROM [dbo].[Table] WHERE Id = @UserId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    object obj = cmd.ExecuteScalar();
                    if (obj != null)
                    {
                        userPoints = Convert.ToInt32(obj);
                    }
                }
            }

            if (userPoints < cost)
            {
                lblMessage.Text = "Insufficient points to redeem this product.";
                return;
            }

            // Deduct the product cost from the user's points.
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE [dbo].[Table] SET RewardPoints = RewardPoints - @Cost WHERE Id = @UserId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Cost", cost);
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.ExecuteNonQuery();
                }
            }

            // Record the transaction in the blockchain ledger.
            AddTransactionBlock(userId, -cost, "Redeem");

            // Retrieve the user's email.
            string email = "";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT Email FROM [dbo].[Table] WHERE Id = @UserId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    object obj = cmd.ExecuteScalar();
                    if (obj != null)
                    {
                        email = obj.ToString();
                    }
                }
            }

            if (SendRedemptionEmail(email, productName, cost))
            {
                lblMessage.Text = "You have successfully redeemed " + productName + "!";
            }
            else
            {
                lblMessage.Text = "Redemption successful, but failed to send confirmation email.";
            }

            // Reload the user's points.
            LoadUserPoints();
        }

        /// <summary>
        /// Sends an email confirmation for the redemption.
        /// </summary>
        private bool SendRedemptionEmail(string recipientEmail, string productName, int cost)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("ruihernh@gmail.com"); // Replace with your email address
                mail.To.Add(recipientEmail);
                mail.Subject = "Redemption Confirmation";
                mail.Body = $"Dear User,\n\nYou have successfully redeemed '{productName}' for {cost} points.\n\nThank you for using our service!";

                smtpServer.Port = 587;
                smtpServer.Credentials = new NetworkCredential("ruihernh@gmail.com", "yqqh pwcr byeq sseo"); // Use your credentials or App Password
                smtpServer.EnableSsl = true;

                smtpServer.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending redemption email: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Adds a new transaction block to the PointsTransactionLedger table.
        /// </summary>
        private void AddTransactionBlock(int userId, int pointsChanged, string transactionType)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;
            DateTime transactionDate = DateTime.Now;
            string previousHash = GetLastBlockHash(connectionString);
            string blockData = $"{userId}{transactionType}{pointsChanged}{transactionDate}{previousHash}";
            string currentHash = ComputeSHA256Hash(blockData);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"INSERT INTO PointsTransactionLedger (UserId, TransactionType, PointsChanged, TransactionDate, PreviousHash, CurrentHash)
                                 VALUES (@UserId, @TransactionType, @PointsChanged, @TransactionDate, @PreviousHash, @CurrentHash)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@TransactionType", transactionType);
                    cmd.Parameters.AddWithValue("@PointsChanged", pointsChanged);
                    cmd.Parameters.AddWithValue("@TransactionDate", transactionDate);
                    cmd.Parameters.AddWithValue("@PreviousHash", previousHash);
                    cmd.Parameters.AddWithValue("@CurrentHash", currentHash);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Retrieves the CurrentHash of the most recent transaction block.
        /// If none exists, returns "0".
        /// </summary>
        private string GetLastBlockHash(string connectionString)
        {
            string lastHash = "0";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT TOP 1 CurrentHash FROM PointsTransactionLedger ORDER BY TransactionId DESC";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        lastHash = result.ToString();
                    }
                }
            }
            return lastHash;
        }

        /// <summary>
        /// Computes a SHA256 hash of the given input string.
        /// </summary>
        private string ComputeSHA256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                StringBuilder builder = new StringBuilder();
                foreach (var b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
