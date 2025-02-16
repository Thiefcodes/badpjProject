<%@ Page Title="" Language="C#" MasterPageFile="~/Site1loggedin.Master" AutoEventWireup="true" CodeBehind="Thread.aspx.cs" Inherits="badpjProject.Thread" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="ForumStyles.css" rel="stylesheet" type="text/css" />
    <div class="container mt-4">
    <h1><asp:Label ID="lblThreadTitle" runat="server" Text="Thread Title"></asp:Label></h1>
<asp:GridView ID="gvPosts" runat="server" CellPadding="10" OnRowDataBound="gvPosts_RowDataBound" AutoGenerateColumns="False" 
    OnRowCommand="gvPosts_RowCommand" DataKeyNames="PostID">
    <Columns>
        <asp:BoundField DataField="PostID" HeaderText="ID" />
        <asp:BoundField DataField="Content" HeaderText="Content" />
        <asp:BoundField DataField="CreatedBy" HeaderText="Author" />
        <asp:BoundField DataField="CreatedAt" HeaderText="Date Created" DataFormatString="{0:yyyy-MM-dd}" />
        <asp:BoundField DataField="Likes" HeaderText="Likes" />
        <asp:TemplateField HeaderText="Like">
          <ItemTemplate>
           <asp:Button ID="btnLike" runat="server" 
              CommandName="LikePost" 
              CommandArgument='<%# Eval("PostID") %>'
              CssClass="btn btn-success btn-sm" Text="Like" />
          </ItemTemplate>
        </asp:TemplateField>
        <asp:ButtonField CommandName="EditPost" ItemStyle-CssClass="btn-custom" Text="Edit" ButtonType="Button" />
       <asp:ButtonField CommandName="DeletePost" ItemStyle-CssClass="btn-custom" Text="Delete" ButtonType="Button" />
    </Columns>
</asp:GridView>

<asp:Button ID="btnBack" runat="server" ItemStyle-CssClass="btn-custom" Text="Back" OnClick="btnBack_Click" />
<asp:Button ID="btnReply" runat="server" ItemStyle-CssClass="btn-custom" Text="Reply" OnClick="btnReply_Click" />
        </div>
</asp:Content>
