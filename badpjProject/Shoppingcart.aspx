<%@ Page Title="Shopping Cart" Language="C#" MasterPageFile="~/Site1loggedin.Master" AutoEventWireup="true" CodeBehind="ShoppingCart.aspx.cs" Inherits="badpjProject.ShoppingCart" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4">
        <h2 class="text-center">Your Shopping Cart</h2>

        <asp:GridView ID="gvCart" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered"
            EmptyDataText="Your cart is empty.">
            <Columns>
                <asp:BoundField DataField="ProductName" HeaderText="Product Name" />
                <asp:BoundField DataField="Description" HeaderText="Description" />
                <asp:ImageField DataImageUrlField="ImageUrl" HeaderText="Image">
                    <ControlStyle Width="50px" Height="50px" />
                </asp:ImageField>
                <asp:BoundField DataField="Price" HeaderText="Price" DataFormatString="{0:C}" />
                <asp:BoundField DataField="Quantity" HeaderText="Quantity" />
        <asp:TemplateField HeaderText="Actions">
            <ItemTemplate>
                <asp:Button ID="btnIncrease" runat="server" Text="+" CssClass="btn btn-success btn-sm"
                    CommandArgument='<%# Eval("ProductID") %>' OnClick="IncreaseQuantity_Click" />
                <asp:Button ID="btnDecrease" runat="server" Text="-" CssClass="btn btn-warning btn-sm"
                    CommandArgument='<%# Eval("ProductID") %>' OnClick="DecreaseQuantity_Click" />
                <asp:Button ID="btnRemove" runat="server" Text="Remove" CssClass="btn btn-danger btn-sm"
                    CommandArgument='<%# Eval("ProductID") %>' OnClick="RemoveFromCart_Click" />
            </ItemTemplate>
        </asp:TemplateField>
            </Columns>
        </asp:GridView>

        <div class="text-right">
            <asp:Label ID="lblTotal" runat="server" CssClass="h4 font-weight-bold"></asp:Label>
        </div>

        <div class="text-center mt-3">
            <asp:Button ID="btnCheckout" runat="server" Text="Proceed to Checkout" CssClass="btn btn-success" OnClick="Checkout_Click" />
        </div>
    </div>
</asp:Content>
