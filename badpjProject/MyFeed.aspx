<%@ Page Title="" Language="C#" MasterPageFile="~/Site1loggedin.Master" AutoEventWireup="true" CodeBehind="MyFeed.aspx.cs" Inherits="badpjProject.MyFeed" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <link href="ForumStyles.css" rel="stylesheet" type="text/css" />
    <h2>My Feed</h2>
    <asp:GridView ID="gvRandomThreads" runat="server" AutoGenerateColumns="False" OnRowCommand="gvRandomThreads_RowCommand"
        DataKeyNames="ThreadID">
        <Columns>
          <asp:BoundField DataField="ThreadID" HeaderText="ID" />
          <asp:BoundField DataField="Title" HeaderText="Random Thread" />
          <asp:ButtonField Text="Select" CommandName="SelectThread" ButtonType="Button" />
        </Columns>
    </asp:GridView>
    <asp:GridView ID="gvRandomPosts" runat="server" AutoGenerateColumns="False" OnRowCommand="gvRandomPosts_RowCommand"
        OnRowDataBound="gvRandomPosts_RowDataBound"
    DataKeyNames="PostID">
        <Columns>
          <asp:BoundField DataField="CreatedBy" HeaderText="User" />
          <asp:BoundField DataField="Content" HeaderText="Post Content" />
          <asp:BoundField DataField="CreatedAt" HeaderText="Date" DataFormatString="{0:yyyy-MM-dd HH:mm}" />
          <asp:BoundField DataField="Likes" HeaderText="Likes" />
          <asp:TemplateField HeaderText="Like">
             <ItemTemplate>
               <asp:Button ID="btnLike" runat="server" 
                 CommandName="LikePost" 
                 CommandArgument='<%# Eval("PostID") %>'
                 CssClass="btn btn-success btn-sm" Text="Like" />
            </ItemTemplate>
          </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
    <!-- Threads Section -->
    <h3>Threads</h3>
    <asp:GridView ID="gvThreads" runat="server" AutoGenerateColumns="False" OnRowCommand="gvThreads_RowCommand">
        <Columns>
           <asp:BoundField DataField="ThreadID" HeaderText="ID" />
           <asp:BoundField DataField="Title" HeaderText="Title" />
           <asp:BoundField DataField="CreatedAt" HeaderText="Date Created" DataFormatString="{0:yyyy-MM-dd}" />
           <asp:BoundField DataField="Views" HeaderText="Views" /> 
           <asp:ButtonField CommandName="ViewThread" ItemStyle-CssClass="btn-custom" Text="Select" />
           <asp:ButtonField CommandName="UpdateThread" ItemStyle-CssClass="btn-custom" Text="Update" />
           <asp:ButtonField CommandName="DeleteThread" ItemStyle-CssClass="btn-custom" Text="Delete" />
        </Columns>
    </asp:GridView>

    <!-- Posts Section -->
    <h3>Posts</h3>
    <asp:GridView ID="gvPosts" runat="server" AutoGenerateColumns="False" OnRowCommand="gvPosts_RowCommand"
        DataKeyNames="PostID">
        <Columns>
            <asp:BoundField DataField="PostID" HeaderText="ID" />
            <asp:BoundField DataField="Content" HeaderText="Content" />
            <asp:BoundField DataField="CreatedAt" HeaderText="Date Created" DataFormatString="{0:yyyy-MM-dd}" />
            <asp:BoundField DataField="Likes" HeaderText="Likes" />
            <asp:ButtonField CommandName="DeletePost" ItemStyle-CssClass="btn-custom" Text="Delete" ButtonType="Button" />
        </Columns>
    </asp:GridView>
</asp:Content>
