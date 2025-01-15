<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Checkout.aspx.cs" Inherits="badpjProject.Checkout" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4">
        <h2 class="text-center">Checkout</h2>
        
        <asp:GridView ID="gvOrderSummary" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered">
            <Columns>
                <asp:BoundField DataField="ProductName" HeaderText="Product Name" />
                <asp:BoundField DataField="Quantity" HeaderText="Quantity" />
                <asp:BoundField DataField="Price" HeaderText="Price" DataFormatString="{0:C}" />
                <asp:BoundField DataField="TotalPrice" HeaderText="Total" DataFormatString="{0:C}" />
            </Columns>
        </asp:GridView>
    </div>

    <div class="container mt-4">
        <h3>Billing Information</h3>
        <div class="form-group">
            <label for="txtAddress">Address</label>
            <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" placeholder="Enter your address"></asp:TextBox>
        </div>
        <div class="form-group">
            <label for="txtCity">City</label>
            <asp:TextBox ID="txtCity" runat="server" CssClass="form-control" placeholder="Enter your city"></asp:TextBox>
        </div>
        <div class="form-group">
            <label for="txtPostalCode">Postal Code</label>
            <asp:TextBox ID="txtPostalCode" runat="server" CssClass="form-control" placeholder="Enter your postal code"></asp:TextBox>
        </div>
    </div>
    <div class="container mt-4">
        <h3>Credit Card Information</h3>
        <div class="form-group">
            <label for="txtCardNumber">Card Number</label>
            <asp:TextBox ID="txtCardNumber" runat="server" CssClass="form-control" placeholder="Enter your card number"></asp:TextBox>
        </div>
        <div class="form-group">
            <label for="txtCardName">Name on Card</label>
            <asp:TextBox ID="txtCardName" runat="server" CssClass="form-control" placeholder="Enter the name on the card"></asp:TextBox>
        </div>
        <div class="form-group">
            <label for="txtExpiryDate">Expiry Date (MM/YY)</label>
            <asp:TextBox ID="txtExpiryDate" runat="server" CssClass="form-control" placeholder="MM/YY"></asp:TextBox>
        </div>
        <div class="form-group">
            <label for="txtCVV">CVV</label>
            <asp:TextBox ID="txtCVV" runat="server" CssClass="form-control" placeholder="Enter CVV" MaxLength="4" TextMode="Password"></asp:TextBox>
        </div>
    </div>
    <div class="container mt-4">
        <div class="text-right">
            <asp:Label ID="lblTotalAmount" runat="server" CssClass="h4 font-weight-bold"></asp:Label>
            <asp:Button ID="btnPlaceOrder" runat="server" CssClass="btn btn-primary" Text="Place Order" OnClick="btnPlaceOrder_Click" />
        </div>
    </div>
</asp:Content>
