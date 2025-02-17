<%@ Page Title="" Language="C#" MasterPageFile="~/Site1loggedin.Master" AutoEventWireup="true" CodeBehind="Payment.aspx.cs" Inherits="badpjProject.Payment" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeaderContent" runat="server">
     <div class="container">
        <h2>Complete Your Payment</h2>
        <form method="post" action="Payment.aspx">
            <input type="hidden" name="paymentId" value="<%= Request.QueryString["paymentId"] %>" />
            <input type="text" name="cardNumber" placeholder="Card Number" required />
            <input type="text" name="cardName" placeholder="Name on Card" required />
            <input type="text" name="expiryDate" placeholder="Expiry Date (MM/YY)" required />
            <input type="text" name="cvv" placeholder="CVV" required />
            <button type="submit">Pay Now</button>
        </form>
    </div>
        <style>
        body { font-family: Arial, sans-serif; text-align: center; padding: 20px; }
        .container { max-width: 400px; margin: auto; padding: 20px; border: 1px solid #ccc; border-radius: 10px; background: #f9f9f9; }
        input { width: 100%; padding: 10px; margin: 10px 0; }
        button { width: 100%; padding: 10px; background: green; color: white; border: none; cursor: pointer; }
    </style>
</asp:Content>

