<%@ Page Title="" Language="C#" MasterPageFile="~/Site1loggedin.Master" AutoEventWireup="true" CodeBehind="MyFeed.aspx.cs" Inherits="badpjProject.MyFeed" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>My Feed</h2>

    <!-- Threads Section -->
    <h3>Threads</h3>
    <asp:GridView ID="gvThreads" runat="server" AutoGenerateColumns="False" OnRowCommand="gvThreads_RowCommand">
        <Columns>
           <asp:BoundField DataField="ThreadID" HeaderText="ID" />
           <asp:BoundField DataField="Title" HeaderText="Title" />
           <asp:BoundField DataField="CreatedAt" HeaderText="Date Created" DataFormatString="{0:yyyy-MM-dd}" />
           <asp:BoundField DataField="Views" HeaderText="Views" /> 
           <asp:ButtonField CommandName="ViewThread" Text="Select" />
           <asp:ButtonField CommandName="UpdateThread" Text="Update" />
           <asp:ButtonField CommandName="DeleteThread" Text="Delete" />
        </Columns>
    </asp:GridView>

    <!-- Posts Section -->
    <h3>Posts</h3>
    <asp:GridView ID="gvPosts" runat="server" AutoGenerateColumns="False" OnRowCommand="gvPosts_RowCommand"
        DataKeyNames="PostID">
        <Columns>
            <asp:BoundField DataField="Content" HeaderText="Content" />
            <asp:BoundField DataField="CreatedAt" HeaderText="Date Created" DataFormatString="{0:yyyy-MM-dd}" />
            <asp:BoundField DataField="Likes" HeaderText="Likes" />
            
            <asp:TemplateField HeaderText="">
              <ItemTemplate>
                <asp:Button ID="btnEdit" runat="server" CommandName="EditPost" Text="Edit" ButtonType="Button" 
                            CommandArgument='<%# Eval("PostID") %>' />
              </ItemTemplate>
            </asp:TemplateField>
        
            <asp:TemplateField HeaderText="">
              <ItemTemplate>
                <asp:Button ID="btnDelete" runat="server" CommandName="DeletePost" Text="Delete" ButtonType="Button" 
                            CommandArgument='<%# Eval("PostID") %>' />
              </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</asp:Content>
