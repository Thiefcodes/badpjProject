<%@ Page Title="" Language="C#" MasterPageFile="~/Site1loggedin.Master" AutoEventWireup="true" CodeBehind="Checkout.aspx.cs" Inherits="badpjProject.Checkout" %>
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
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="text-danger" HeaderText="Please fix the following errors:" />
        <div class="form-group">
            <label for="txtAddress">Address</label>
            <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" placeholder="Enter your address" ValidationGroup="Checkout"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvAddress" runat="server" ControlToValidate="txtAddress" ErrorMessage="Address is required." CssClass="text-danger" Display="Dynamic" ValidationGroup="Checkout" />
        </div>
        <div class="form-group">
            <label for="txtCity">City</label>
            <asp:TextBox ID="txtCity" runat="server" CssClass="form-control" placeholder="Enter your city" ValidationGroup="Checkout"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvCity" runat="server" ControlToValidate="txtCity" ErrorMessage="City is required." CssClass="text-danger" Display="Dynamic" ValidationGroup="Checkout" />
        </div>
        <div class="form-group">
            <label for="txtPostalCode">Postal Code</label>
            <asp:TextBox ID="txtPostalCode" runat="server" CssClass="form-control" placeholder="Enter your postal code" ValidationGroup="Checkout"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvPostalCode" runat="server" ControlToValidate="txtPostalCode" ErrorMessage="Postal code is required." CssClass="text-danger" Display="Dynamic" ValidationGroup="Checkout" />
            <asp:RegularExpressionValidator ID="revPostalCode" runat="server" ControlToValidate="txtPostalCode" ErrorMessage="Invalid postal code format." CssClass="text-danger" Display="Dynamic" ValidationExpression="^\d{5,6}$" ValidationGroup="Checkout" />
        </div>
    </div>

    <div class="container mt-4">
        <h3>Credit Card Information</h3>
        <div class="form-group">
            <label for="txtCardNumber">Card Number</label>
            <asp:TextBox ID="txtCardNumber" runat="server" CssClass="form-control" placeholder="Enter your card number" ValidationGroup="Checkout"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvCardNumber" runat="server" ControlToValidate="txtCardNumber" ErrorMessage="Card number is required." CssClass="text-danger" Display="Dynamic" ValidationGroup="Checkout" />
            <asp:RegularExpressionValidator ID="revCardNumber" runat="server" ControlToValidate="txtCardNumber" ErrorMessage="Invalid card number. Must be 13-16 digits." CssClass="text-danger" Display="Dynamic" ValidationExpression="^\d{13,16}$" ValidationGroup="Checkout" />
        </div>
        <div class="form-group">
            <label for="txtCardName">Name on Card</label>
            <asp:TextBox ID="txtCardName" runat="server" CssClass="form-control" placeholder="Enter the name on the card" ValidationGroup="Checkout"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvCardName" runat="server" ControlToValidate="txtCardName" ErrorMessage="Name on card is required." CssClass="text-danger" Display="Dynamic" ValidationGroup="Checkout" />
        </div>
        <div class="form-group">
            <label for="txtExpiryDate">Expiry Date (MM/YY)</label>
            <asp:TextBox ID="txtExpiryDate" runat="server" CssClass="form-control" placeholder="MM/YY" ValidationGroup="Checkout"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvExpiryDate" runat="server" ControlToValidate="txtExpiryDate" ErrorMessage="Expiry date is required." CssClass="text-danger" Display="Dynamic" ValidationGroup="Checkout" />
            <asp:RegularExpressionValidator ID="revExpiryDate" runat="server" ControlToValidate="txtExpiryDate" ErrorMessage="Invalid expiry date format. Use MM/YY." CssClass="text-danger" Display="Dynamic" ValidationExpression="^(0[1-9]|1[0-2])\/([0-9]{2})$" ValidationGroup="Checkout" />
        </div>
        <div class="form-group">
            <label for="txtCVV">CVV</label>
            <asp:TextBox ID="txtCVV" runat="server" CssClass="form-control" placeholder="Enter CVV" MaxLength="4" TextMode="Password" ValidationGroup="Checkout"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvCVV" runat="server" ControlToValidate="txtCVV" ErrorMessage="CVV is required." CssClass="text-danger" Display="Dynamic" ValidationGroup="Checkout" />
            <asp:RegularExpressionValidator ID="revCVV" runat="server" ControlToValidate="txtCVV" ErrorMessage="Invalid CVV." CssClass="text-danger" Display="Dynamic" ValidationExpression="^\d{3,4}$" ValidationGroup="Checkout" />
        </div>
    </div>

    <div class="container mt-4">
        <div class="text-right">
            <asp:Label ID="lblTotalAmount" runat="server" CssClass="h4 font-weight-bold"></asp:Label>
            <asp:Button ID="btnPlaceOrder" runat="server" CssClass="btn btn-primary" Text="Place Order" OnClick="btnPlaceOrder_Click" CausesValidation="true" ValidationGroup="Checkout" />
        </div>
    </div>
</asp:Content>
