﻿<%@ Page Title="Product Details" Language="C#" MasterPageFile="~/Site.Master" 
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

                <asp:Button
                    ID="btnAddToCart"
                    runat="server"
                    Text="Add to Cart"
                    CssClass="btn btn-primary"
                    OnClick="btnAddToCart_Click" />
            </div>
        </div>
    </div>
</asp:Content>
