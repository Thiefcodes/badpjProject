<%@ Page Title="" Language="C#" MasterPageFile="~/Site1loggedin.Master" AutoEventWireup="true" CodeBehind="ThankYou.aspx.cs" Inherits="badpjProject.ThankYou" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">]
    <div class="container text-center mt-5">
        <h2>Thank You for Your Order!</h2>
        <p class="mt-3">Your order has been placed successfully. You will receive an email confirmation shortly.</p>
        <div class="mt-4">
            <a href="Shop.aspx" class="btn btn-primary">Continue Shopping</a>
            <a href="Orders.aspx" class="btn btn-secondary">View Your Orders</a>
        </div>
    </div>
</asp:Content>
