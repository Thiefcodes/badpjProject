<%@ Page Title="" Language="C#" MasterPageFile="~/Site1loggedin.Master" AutoEventWireup="true" CodeBehind="Thread.aspx.cs" Inherits="badpjProject.Thread" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4">
    <h1><asp:Label ID="lblThreadTitle" runat="server" Text="Thread Title"></asp:Label></h1>
<asp:GridView ID="gvPosts" runat="server" OnRowDataBound="gvPosts_RowDataBound" AutoGenerateColumns="False" 
    OnRowCommand="gvPosts_RowCommand" DataKeyNames="PostID">
    <Columns>
        <asp:BoundField DataField="Content" HeaderText="Content" />
        <asp:BoundField DataField="CreatedBy" HeaderText="Author" />
        <asp:BoundField DataField="CreatedAt" HeaderText="Date Created" DataFormatString="{0:yyyy-MM-dd}" />
        <asp:BoundField DataField="Likes" HeaderText="Likes" />
        <asp:TemplateField HeaderText="Like">
          <ItemTemplate>
           <asp:Button ID="btnLike" runat="server" 
              CommandName="LikePost" 
              CommandArgument='<%# Eval("PostID") %>' 
              Text="Like" />
          </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="">
        <ItemTemplate>
        <asp:Button ID="btnEdit" runat="server" CommandName="EditPost" Text="Edit" ButtonType="Button" />
        </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="">
        <ItemTemplate>
        <asp:Button ID="btnDelete" runat="server" CommandName="DeletePost" Text="Delete" ButtonType="Button" />
        </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>

<asp:Button ID="btnReply" runat="server" Text="Reply" OnClick="btnReply_Click" />
<asp:Button ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" />
        </div>
</asp:Content>
