<%@ Page Title="My Orders" Language="C#" MasterPageFile="~/Site1loggedin.Master" AutoEventWireup="true" CodeBehind="Orders.aspx.cs" Inherits="badpjProject.Orders" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
  <div class="container mt-4">
    <asp:Repeater ID="rptOrders" runat="server" OnItemCommand="rptOrders_ItemCommand">
      <HeaderTemplate>
        <table class="table table-bordered">
          <thead>
            <tr>
              <th>Order ID</th>
              <th>Date</th>
              <th>Status</th>
              <th>Address</th>
              <th>Items</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
      </HeaderTemplate>
      <ItemTemplate>
        <tr>
          <td>
            <%# Eval("OrderID") %> <%# Eval("Status").ToString() == "Refund" ? "(Refund)" : "" %>
          </td>
          <td>
            <%# Eval("OrderDate", "{0:yyyy-MM-dd}") %>
          </td>
          <td>
            <%# Eval("Status") %>
          </td>
          <td>
            <%# Eval("FullAddress") %>
          </td>
          <td>
            <asp:Repeater ID="rptOrderItems" runat="server" DataSource='<%# Eval("Items") %>'>
              <ItemTemplate>
                <div>
                  <strong><%# Eval("ProductName") %></strong>: <%# Eval("Quantity") %> x <%# Eval("Price", "{0:C}") %>
                </div>
              </ItemTemplate>
            </asp:Repeater>
          </td>
          <td>
            <asp:Button 
              ID="btnRefund" 
              runat="server" 
              Text="Refund" 
              CommandName="Refund" 
              CommandArgument='<%# Eval("OrderID") %>' 
              CssClass="btn btn-warning btn-sm" 
              OnClientClick="return confirm('Are you sure you want to mark this order as refunded?');" />
            &nbsp;
            <asp:Button 
              ID="btnLeaveReview" 
              runat="server" 
              Text="Leave Review" 
              CommandName="LeaveReview" 
              CommandArgument='<%# Eval("OrderID") %>' 
              CssClass="btn btn-info btn-sm"
              Visible='<%# Eval("Status").ToString() == "Shipped" %>' />
          </td>
        </tr>
      </ItemTemplate>
      <FooterTemplate>
          </tbody>
        </table>
      </FooterTemplate>
    </asp:Repeater>
  </div>
    <style>
        .orders-container {
    max-width: 1000px;
    margin: auto;
    padding: 20px;
    background: #fff;
    border-radius: 10px;
    box-shadow: 0px 0px 10px rgba(0, 0, 0, 0.1);
}

.table {
    width: 100%;
    background: white;
    border-radius: 8px;
    overflow: hidden;
    border-collapse: separate;
    border-spacing: 0;
}

.table th {
    background: #f8f9fa;
    font-weight: bold;
    padding: 12px;
    text-align: center;
    border-bottom: 2px solid #ddd;
}

.table td {
    padding: 12px;
    text-align: center;
    border-bottom: 1px solid #ddd;
}

.order-id {
    font-weight: bold;
    color: #343a40;
}

.status-refund {
    font-size: 0.9rem;
    color: #dc3545;
}

.status-pending {
    color: #ff9800;
    font-weight: bold;
}

.status-shipping {
    color: #007bff;
    font-weight: bold;
}

.status-refund {
    color: #dc354

    </style>
</asp:Content>
