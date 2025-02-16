<%@ Page Title="" Language="C#" MasterPageFile="~/Site1loggedin.Master" AutoEventWireup="true" CodeBehind="UpdatePost.aspx.cs" Inherits="badpjProject.UpdatePost" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4">
    <h1>Edit Post</h1>
    <asp:Label ID="lblPostId" runat="server" Visible="false"></asp:Label>
    <asp:TextBox ID="txtContent" runat="server" TextMode="MultiLine" Rows="5"></asp:TextBox><br />
        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
    <asp:Button ID="btnUpdate" runat="server" Text="Edit" OnClick="btnUpdate_Click" />\
        </div>
</asp:Content>
