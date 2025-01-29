<%@ Page Title="" Language="C#" MasterPageFile="~/Site1loggedin.Master" AutoEventWireup="true" CodeBehind="AllOrders.aspx.cs" Inherits="badpjProject.AllOrders" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     <div class="container mt-4">
        <h2 class="text-center">All Orders</h2>
        <asp:GridView ID="gvAllOrders" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered" OnRowCommand="gvAllOrders_RowCommand" DataKeyNames="OrderID">
    <Columns>
        <asp:BoundField DataField="OrderID" HeaderText="Order ID" />
        <asp:BoundField DataField="UserName" HeaderText="User Name" />
        <asp:BoundField DataField="ProductName" HeaderText="Item" />
        <asp:BoundField DataField="Quantity" HeaderText="Quantity" />
        <asp:BoundField DataField="FullAddress" HeaderText="Address" />
        <asp:BoundField DataField="OrderDate" HeaderText="Date" DataFormatString="{0:yyyy-MM-dd}" />
        <asp:TemplateField HeaderText="Status">
            <ItemTemplate>
                <asp:TextBox ID="txtStatus" runat="server" CssClass="form-control" Text='<%# Eval("Status") %>'></asp:TextBox>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Actions">
            <ItemTemplate>
                <asp:Button ID="btnUpdateStatus" runat="server" Text="Update" CommandName="UpdateStatus" CommandArgument='<%# Container.DataItemIndex %>' CssClass="btn btn-primary btn-sm" />
            </ItemTemplate>
                <ItemTemplate>
        <asp:Button ID="btnUpdateStatus" runat="server" Text="Update" CommandName="UpdateStatus" CommandArgument='<%# Container.DataItemIndex %>' CssClass="btn btn-primary btn-sm" />
        <asp:Button ID="btnDeleteOrder" runat="server" Text="Delete" CommandName="DeleteOrder" CommandArgument='<%# Container.DataItemIndex %>' CssClass="btn btn-danger btn-sm" OnClientClick="return confirm('Are you sure you want to delete this order?');" />
    </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>


    </div>
</asp:Content>
