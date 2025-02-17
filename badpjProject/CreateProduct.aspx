<%@ Page Title="Create Product" Language="C#" MasterPageFile="~/Site.Master" 
    AutoEventWireup="true" CodeBehind="CreateProduct.aspx.cs"
    Inherits="badpjProject.CreateProduct" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <h2 class="mt-4">Create / Edit Product</h2>

        <asp:Label ID="lblMessage" runat="server" ForeColor="Red" />
        <br />

        <div class="mb-3">
            <asp:Label AssociatedControlID="txtName" Text="Product Name:" runat="server" CssClass="form-label" />
            <asp:TextBox ID="txtName" runat="server" CssClass="form-control" />
        </div>

        <div class="mb-3">
            <asp:Label AssociatedControlID="txtDescription" Text="Description:" runat="server" CssClass="form-label" />
            <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" Rows="3" Columns="40" CssClass="form-control" />
        </div>
        
        <div class="mb-3">
            <asp:Label AssociatedControlID="txtCategory" Text="Category:" runat="server" CssClass="form-label" />
            <asp:TextBox ID="txtCategory" runat="server" CssClass="form-control" />
        </div>

        <div class="mb-3">
            <asp:Label AssociatedControlID="fuProductImage" Text="Upload Image:" runat="server" CssClass="form-label" />
            <asp:FileUpload ID="fuProductImage" runat="server" CssClass="form-control" />
            <br />
            <asp:Image ID="imgPreview" runat="server" CssClass="img-thumbnail d-block" Width="150px" Visible="false" />
        </div>

        <div class="mb-3">
            <asp:Label AssociatedControlID="txtPrice" Text="Price:" runat="server" CssClass="form-label" />
            <asp:TextBox ID="txtPrice" runat="server" CssClass="form-control" />
        </div>

        <asp:Button ID="btnSave" runat="server" Text="Save Product" CssClass="btn btn-primary" OnClick="btnSave_Click" />
    </div>
</asp:Content>
