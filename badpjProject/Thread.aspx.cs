﻿using System;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Web.UI.WebControls;

namespace badpjProject
{
    public partial class Thread : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadThread();
                LoadPosts();
            }

        }

        protected void gvPosts_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeletePost")
            {
                // Get the row index of the button clicked
                int rowIndex = Convert.ToInt32(e.CommandArgument);

                // Retrieve PostID from the DataKeys collection
                string postId = gvPosts.DataKeys[rowIndex].Value.ToString();

                // Call DeletePost method to delete the record
                DeletePost(postId);

                // Reload the posts to refresh the GridView
                LoadPosts();
            }
            else if (e.CommandName == "EditPost")
            {
                string threadId = Request.QueryString["ThreadID"];
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                string postId = gvPosts.DataKeys[rowIndex].Value.ToString();
                Response.Redirect($"UpdatePost.aspx?PostID={postId}&ThreadID={threadId}");
            }
        }

        private void LoadThread()
        {
            string threadId = Request.QueryString["ThreadID"];
            string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {

                SqlCommand cmd = new SqlCommand("SELECT Title FROM Threads WHERE ThreadID = @ThreadID", conn);
                cmd.Parameters.AddWithValue("@ThreadID", threadId);
                conn.Open();
                lblThreadTitle.Text = (string)cmd.ExecuteScalar();
            }
        }
        private void DeletePost(string postId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Posts WHERE PostID = @PostID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@PostID", postId);

                conn.Open();
                cmd.ExecuteNonQuery();
                Response.Write("<script>alert('Post deleted successfully');</script>");
            }
        }



        private void LoadPosts()
        {
            string threadId = Request.QueryString["ThreadID"];
            string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT PostID, Content, CreatedBy, CreatedAt FROM Posts WHERE ThreadID = @ThreadID", conn);
                cmd.Parameters.AddWithValue("@ThreadID", threadId);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                gvPosts.DataSource = dt;
                gvPosts.DataBind();
            }
        }

        protected void btnReply_Click(object sender, EventArgs e)
        {
            // Check if the user is logged in
            if (Session["UserId"] == null || Session["Username"] == null || Session["Role"] == null)
            {
                // Redirect to the login page if user is not logged in
                Response.Write("<script>alert('Please log in first!'); window.location='Login.aspx';</script>");
            }
            else
            {
                // Proceed with the current page as user is logged in
                // Continue with your page logic here
                string threadId = Request.QueryString["ThreadID"];
                Response.Redirect($"NewPost.aspx?ThreadID={threadId}");
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("Forum.aspx");
        }
    }
}