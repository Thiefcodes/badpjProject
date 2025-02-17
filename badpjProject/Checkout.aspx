<%@ Page Title="" Language="C#" MasterPageFile="~/Site1loggedin.Master" AutoEventWireup="true" CodeBehind="Checkout.aspx.cs" Inherits="badpjProject.Checkout" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="checkout-container">
        <!-- Left: Billing and Payment Form -->
        <div class="billing-form">
            <h3>Billing Information</h3>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="text-danger" HeaderText="Please fix the following errors:" />
            
            <div class="form-group">
                <label for="txtAddress">Address</label>
                <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" placeholder="Enter your address"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvAddress" runat="server" ControlToValidate="txtAddress" ErrorMessage="Address is required." CssClass="text-danger" Display="Dynamic" />
            </div>

            <div class="form-group">
                <label for="txtCity">City</label>
                <asp:TextBox ID="txtCity" runat="server" CssClass="form-control" placeholder="Enter your city"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvCity" runat="server" ControlToValidate="txtCity" ErrorMessage="City is required." CssClass="text-danger" Display="Dynamic" />
            </div>

            <div class="form-group">
                <label for="txtPostalCode">Postal Code</label>
                <asp:TextBox ID="txtPostalCode" runat="server" CssClass="form-control" placeholder="Enter your postal code"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvPostalCode" runat="server" ControlToValidate="txtPostalCode" ErrorMessage="Postal code is required." CssClass="text-danger" Display="Dynamic" />
                <asp:RegularExpressionValidator ID="revPostalCode" runat="server" ControlToValidate="txtPostalCode" ErrorMessage="Invalid postal code format." CssClass="text-danger" Display="Dynamic" ValidationExpression="^\d{5,6}$" />
            </div>

            <h3>Credit Card Information</h3>
            <div class="form-group">
                <label for="txtCardNumber">Card Number</label>
                <asp:TextBox ID="txtCardNumber" runat="server" CssClass="form-control" placeholder="Enter your card number"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvCardNumber" runat="server" ControlToValidate="txtCardNumber" ErrorMessage="Card number is required." CssClass="text-danger" Display="Dynamic" />
            </div>

            <div class="form-group">
                <label for="txtCardName">Name on Card</label>
                <asp:TextBox ID="txtCardName" runat="server" CssClass="form-control" placeholder="Enter the name on the card"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvCardName" runat="server" ControlToValidate="txtCardName" ErrorMessage="Name on card is required." CssClass="text-danger" Display="Dynamic" />
            </div>

            <div class="form-group">
                <label for="txtExpiryDate">Expiry Date (MM/YY)</label>
                <asp:TextBox ID="txtExpiryDate" runat="server" CssClass="form-control" placeholder="MM/YY"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvExpiryDate" runat="server" ControlToValidate="txtExpiryDate" ErrorMessage="Expiry date is required." CssClass="text-danger" Display="Dynamic" />
            </div>

            <div class="form-group">
                <label for="txtCVV">CVV</label>
                <asp:TextBox ID="txtCVV" runat="server" CssClass="form-control" placeholder="Enter CVV" MaxLength="4" TextMode="Password"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvCVV" runat="server" ControlToValidate="txtCVV" ErrorMessage="CVV is required." CssClass="text-danger" Display="Dynamic" />
            </div>

            <div class="text-right">
                <asp:Label ID="lblTotalAmount" runat="server" CssClass="h4 font-weight-bold"></asp:Label>
                <asp:Button ID="btnPlaceOrder" runat="server" CssClass="btn btn-primary" Text="Place Order" OnClick="btnPlaceOrder_Click" />
            </div>
        </div>

        <!-- Right: Order Summary -->
        <div class="order-summary">
            <h3>Order Summary</h3>
            <asp:GridView ID="gvOrderSummary" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered">
                <Columns>
                    <asp:BoundField DataField="ProductName" HeaderText="Product Name" />
                    <asp:BoundField DataField="Quantity" HeaderText="Quantity" />
                    <asp:BoundField DataField="Price" HeaderText="Price" DataFormatString="{0:C}" />
                    <asp:BoundField DataField="TotalPrice" HeaderText="Total" DataFormatString="{0:C}" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
    <style>.checkout-container {
    display: flex;
    justify-content: space-between;
    align-items: flex-start;
    max-width: 1200px;
    margin: auto;
    padding: 20px;
}

.billing-form {
    flex: 1;
    max-width: 600px;
}

.order-summary {
    flex: 0.5;
    max-width: 400px;
    background: #f9f9f9;
    padding: 20px;
    border-radius: 10px;
    border: 1px solid #ddd;
}

.order-summary h3 {
    font-size: 1.2rem;
    font-weight: bold;
    margin-bottom: 15px;
}

.table {
    width: 100%;
    background: white;
    border-radius: 8px;
    overflow: hidden;
}

.table th {
    background: #f1f1f1;
    font-weight: bold;
    padding: 10px;
}

.table td {
    padding: 10px;
}

@media (max-width: 992px) {
    .checkout-container {
        flex-direction: column;
        align-items: center;
    }

    .order-summary {
        max-width: 100%;
        margin-top: 20px;
    }
}
</style>
</asp:Content>
