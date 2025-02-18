<%@ Page Title="" Language="C#" MasterPageFile="~/Site1loggedin.Master" AutoEventWireup="true" CodeBehind="MyFeed.aspx.cs" Inherits="badpjProject.MyFeed" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <link href="ForumStyles.css" rel="stylesheet" type="text/css" />
    <h2>My Feed</h2>
    <br />
    <h3>Top 3 Threads</h3>
    <asp:GridView ID="gvRandomThreads" runat="server" CellPadding="10" AutoGenerateColumns="False" OnRowCommand="gvRandomThreads_RowCommand"
        DataKeyNames="ThreadID" OnSelectedIndexChanged="gvRandomThreads_SelectedIndexChanged">
        
        <Columns>
          <asp:BoundField DataField="ThreadID" HeaderText="ID   " />
          <asp:ImageField DataImageUrlField="ImagePath" HeaderText="">
          <ControlStyle Width="100px" Height="100px" />
          </asp:ImageField>
          <asp:BoundField DataField="Title" HeaderText="Random Thread   " />
          <asp:BoundField DataField="Views" HeaderText="Views   " />
          <asp:BoundField DataField="PostCount" HeaderText="Posts   " />
          <asp:ButtonField Text="Select" ItemStyle-CssClass="btn-custom" CommandName="SelectThread" ButtonType="Button" />
        </Columns>
    </asp:GridView>
    <br />
    <h3>Selected Posts:</h3>
    <asp:GridView ID="gvRandomPosts" runat="server" CellPadding="10" AutoGenerateColumns="False" OnRowCommand="gvRandomPosts_RowCommand"
        OnRowDataBound="gvRandomPosts_RowDataBound"
    DataKeyNames="PostID">
        
        <Columns>
          <asp:BoundField DataField="CreatedBy" HeaderText="User   " />
          <asp:BoundField DataField="Content" HeaderText="Post Content   " />
          <asp:BoundField DataField="CreatedAt" HeaderText="Date   " DataFormatString="{0:yyyy-MM-dd HH:mm}" />
          <asp:BoundField DataField="Likes" HeaderText="Likes   " />
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
    <br />
    <h3>Your Stats</h3>
    <h5>How Many Posts you Liked:</h5>
    <asp:Label ID="lblTotalLikes" runat="server" CssClass="stat-label" Style="font-size: 24px; font-weight: bold;"></asp:Label>
    <h5>Your Thread Views:</h5>
    <asp:Label ID="lblTotalViews" runat="server" CssClass="stat-label" Style="font-size: 24px; font-weight: bold;"></asp:Label>
    <h5>Posts on your threads:</h5>
    <asp:Label ID="lblTotalThreadPosts" runat="server" CssClass="stat-label" Style="font-size: 24px; font-weight: bold;"></asp:Label>
    <br />
    <!-- Threads Section -->
    <h3>Your Threads</h3>
    <asp:GridView ID="gvThreads" runat="server" CellPadding="10" AutoGenerateColumns="False" OnRowCommand="gvThreads_RowCommand">
        <Columns>
           <asp:BoundField DataField="ThreadID" HeaderText="ID   " />
           <asp:ImageField DataImageUrlField="ImagePath" HeaderText="">
           <ControlStyle Width="100px" Height="100px" />
           </asp:ImageField>
           <asp:BoundField DataField="Title" HeaderText="Title   " />
           <asp:BoundField DataField="CreatedAt" HeaderText="Date Created   " DataFormatString="{0:yyyy-MM-dd}" />
           <asp:BoundField DataField="Views" HeaderText="Views   " />
           <asp:BoundField DataField="PostCount" HeaderText="Posts   " />
           <asp:ButtonField CommandName="ViewThread" ItemStyle-CssClass="btn-custom" Text="Select" ButtonType="Button"/>
           <asp:ButtonField CommandName="UpdateThread" ItemStyle-CssClass="btn-custom" Text="Update" ButtonType="Button"/>
           <asp:ButtonField CommandName="DeleteThread" ItemStyle-CssClass="btn-custom" Text="Delete" ButtonType="Button"/>
        </Columns>
    </asp:GridView>

    <!-- Posts Section -->
    <h3>Your Posts</h3>
    <asp:GridView ID="gvPosts" runat="server" CellPadding="10" AutoGenerateColumns="False" OnRowCommand="gvPosts_RowCommand"
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
