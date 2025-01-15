<%@ Page Title="Product Details" Language="C#" MasterPageFile="~/Site.Master" 
    AutoEventWireup="true" CodeBehind="ProductDetails.aspx.cs"
    Inherits="badpjProject.ProductDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <h2 class="mt-4">Product Details</h2>
        <div class="row">
            <div class="col-md-6">
                <asp:Image
                    ID="imgProduct"
                    runat="server"
                    Width="200px"
                    CssClass="img-fluid img-thumbnail" />
            </div>
            <div class="col-md-6">
                <asp:Label
                    ID="lblName"
                    runat="server"
                    Font-Bold="true"
                    Font-Size="Large"
                    CssClass="d-block mb-2" />
                <asp:Label
                    ID="lblPrice"
                    runat="server"
                    Font-Size="Medium"
                    CssClass="d-block mb-4" />
                <asp:Label
                    ID="lblDescription"
                    runat="server"
                    CssClass="d-block mb-4" />

                <asp:Label
                    ID="lblWishlistIndicator"
                    runat="server"
                    CssClass="text-success fw-bold"
                    Visible="false"
                    Text="(On your Wishlist!)"/>

                
                <asp:HiddenField ID="hfProductID" runat="server" />

                <asp:Button
                    ID="btnAddToCart"
                    runat="server"
                    CssClass="btn btn-primary btn-sm"
                    OnClick="btnAddToCart_Click"
                    Text="Add to Cart" />

            </div>
        </div>
    </div>
</asp:Content>
