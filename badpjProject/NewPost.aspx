<%@ Page Title="" Language="C#" MasterPageFile="~/Site1loggedin.Master" AutoEventWireup="true" CodeBehind="NewPost.aspx.cs" Inherits="badpjProject.NewPost" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4">
    <h1>New Post</h1>
    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
    <asp:TextBox ID="txtContent" runat="server" TextMode="MultiLine" Rows="10" Width="500px"></asp:TextBox>
    <br />
    <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-primary btn-sm" Text="Submit" OnClick="btnSubmit_Click" />
    <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-warning" Text="Back" OnClick="btnCancel_Click" />
        </div>
</asp:Content>
