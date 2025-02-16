<%@ Page Title="Manage Products" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="ManageProduct.aspx.cs"
    Inherits="badpjProject.ManageProduct" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
  <div class="container">
    <h2 class="mt-4">Manage Products</h2>
    <asp:HyperLink runat="server" NavigateUrl="CreateProduct.aspx" Text="Create New Product" CssClass="btn btn-success mb-3" />
    <asp:GridView ID="gvProducts" runat="server" AutoGenerateColumns="False" DataKeyNames="ProductID"
      CssClass="table table-bordered table-hover" OnRowEditing="gvProducts_RowEditing" OnRowDeleting="gvProducts_RowDeleting">
      <Columns>
        <asp:BoundField DataField="ProductID" HeaderText="ID" ReadOnly="true" />
        <asp:BoundField DataField="ProductName" HeaderText="Name" />
        <asp:BoundField DataField="Price" HeaderText="Price" DataFormatString="{0:C}" />
        <asp:TemplateField HeaderText="Actions">
          <ItemTemplate>
            <asp:LinkButton ID="btnEdit" runat="server" CommandName="Edit" Text="Edit" CssClass="btn btn-warning btn-sm" />
            &nbsp;
            <asp:LinkButton ID="btnDelete" runat="server" CommandName="Delete" Text="Delete" CssClass="btn btn-danger btn-sm" />
          </ItemTemplate>
        </asp:TemplateField>
      </Columns>
    </asp:GridView>
  </div>
</asp:Content>
