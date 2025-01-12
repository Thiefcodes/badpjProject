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

        <div class="text-right">
            <asp:Label ID="lblTotalAmount" runat="server" CssClass="h4 font-weight-bold"></asp:Label>
            <a href="Payment.aspx" class="btn btn-warning mb-3">Payment</a
        </div>
</asp:Content>
