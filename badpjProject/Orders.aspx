<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Orders.aspx.cs" Inherits="badpjProject.Orders" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4">
        <h2 class="text-center">My Orders</h2>
        <asp:Repeater ID="rptOrders" runat="server">
            <HeaderTemplate>
                <table class="table table-bordered">
                    <thead>
                        <tr>
                            <th>Order ID</th>
                            <th>Date</th>
                            <th>Status</th>
                            <th>Address</th>
                            <th>Items</th>
                        </tr>
                    </thead>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td><%# Eval("OrderID") %></td>
                    <td><%# Eval("OrderDate", "{0:yyyy-MM-dd}") %></td>
                    <td><%# Eval("Status") %></td>
                    <td><%# Eval("FullAddress") %></td>
                    <td>
                        <asp:Repeater ID="rptOrderItems" runat="server" DataSource='<%# Eval("Items") %>'>
                            <ItemTemplate>
                                <div>
                                    <strong><%# Eval("ProductName") %></strong>: <%# Eval("Quantity") %> x <%# Eval("Price", "{0:C}") %>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </tbody>
                </table>
            </FooterTemplate>
        </asp:Repeater>
    </div>
</asp:Content>
