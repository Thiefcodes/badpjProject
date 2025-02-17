<%@ Page Title="Shopping Cart" Language="C#" MasterPageFile="~/Site1loggedin.Master" AutoEventWireup="true" CodeBehind="ShoppingCart.aspx.cs" Inherits="badpjProject.ShoppingCart" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="cart-container">
        <h2 class="text-center">Your Shopping Cart</h2>

        <asp:GridView ID="gvCart" runat="server" AutoGenerateColumns="False" CssClass="table cart-table"
            EmptyDataText="Your cart is empty.">
            <Columns>
                <asp:BoundField DataField="ProductName" HeaderText="Product Name" />
                <asp:BoundField DataField="Description" HeaderText="Description" />
                <asp:ImageField DataImageUrlField="ImageUrl" HeaderText="Image">
                    <ControlStyle Width="80px" Height="80px" CssClass="cart-item-img" />
                </asp:ImageField>
                <asp:BoundField DataField="Price" HeaderText="Price" DataFormatString="{0:C}" />
                <asp:BoundField DataField="Quantity" HeaderText="Quantity" />
                <asp:TemplateField HeaderText="Actions">
                    <ItemTemplate>
                        <div class="cart-actions">
                            <asp:Button ID="btnIncrease" runat="server" Text="+" CssClass="btn btn-success btn-sm cart-btn"
                                CommandArgument='<%# Eval("ProductID") %>' OnClick="IncreaseQuantity_Click" />
                            <asp:Button ID="btnDecrease" runat="server" Text="-" CssClass="btn btn-warning btn-sm cart-btn"
                                CommandArgument='<%# Eval("ProductID") %>' OnClick="DecreaseQuantity_Click" />
                            <asp:Button ID="btnRemove" runat="server" Text="Remove" CssClass="btn btn-danger btn-sm cart-btn"
                                CommandArgument='<%# Eval("ProductID") %>' OnClick="RemoveFromCart_Click" />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>

        <div class="cart-footer">
            <div class="cart-total">
                <span></span>
                <asp:Label ID="lblTotal" runat="server" CssClass="total-amount"></asp:Label>
            </div>
            <asp:Button ID="btnCheckout" runat="server" Text="Proceed to Checkout" CssClass="btn checkout-btn" OnClick="Checkout_Click" />
        </div>
    </div>
    <style>
.cart-container {
    max-width: 900px;
    margin: auto;
    padding: 20px;
    background: #ffffff;
    border-radius: 10px;
    box-shadow: 0px 4px 12px rgba(0, 0, 0, 0.1);
}

.cart-table {
    width: 100%;
    background: white;
    border-radius: 8px;
    overflow: hidden;
    border-collapse: separate;
    border-spacing: 0;
}

.cart-table th {
    background: #333;
    color: white;
    font-weight: bold;
    padding: 14px;
    text-align: center;
    border-bottom: 2px solid #ddd;
}

.cart-table td {
    padding: 14px;
    text-align: center;
    border-bottom: 1px solid #ddd;
}

.cart-item-img {
    display: block;
    margin: auto;
    border-radius: 8px;
}

.cart-actions {
    display: flex;
    justify-content: center;
    gap: 6px;
}

.cart-btn {
    padding: 8px 12px;
    font-size: 14px;
    border-radius: 5px;
}

.cart-btn:hover {
    filter: brightness(90%);
}

.cart-footer {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-top: 20px;
    padding: 15px;
    border-top: 2px solid #ddd;
    font-size: 18px;
    font-weight: bold;
}

.cart-total {
    display: flex;
    align-items: center;
    font-size: 18px;
}

.total-amount {
    margin-left: 8px;
    font-size: 22px;
    color: #28a745;
}

.checkout-btn {
    background-color: #28a745;
    border: none;
    color: white;
    padding: 12px 18px;
    font-size: 16px;
    font-weight: bold;
    border-radius: 5px;
    cursor: pointer;
}

.checkout-btn:hover {
    background-color: #218838;
}

@media (max-width: 768px) {
    .cart-container {
        padding: 15px;
    }

    .cart-table th,
    .cart-table td {
        padding: 10px;
    }

    .cart-actions {
        flex-direction: column;
        gap: 4px;
    }

    .checkout-btn {
        width: 100%;
    }
}

    </style>
</asp:Content>
