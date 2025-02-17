<%@ Page Title="Create Product" Language="C#" MasterPageFile="~/Site.Master" 
    AutoEventWireup="true" CodeBehind="CreateProduct.aspx.cs"
    Inherits="badpjProject.CreateProduct" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <!-- Custom styling for the card and black/white theme -->
    <style type="text/css">
        body {
            background-color: #fff;
            color: #000;
        }

        /* Container to center the card both vertically & horizontally */
        .create-product-container {
            min-height: 80vh; /* Height for vertical centering space */
            display: flex;
            align-items: center;
            justify-content: center;
        }

        /* Optional: card styling */
        .create-product-card {
            /* Use either a fixed width or a responsive fraction. For example: */
            width: 50%;            /* 50% of container width on larger screens */
            max-width: 700px;      /* optional max to prevent it from getting too wide */
            box-shadow: 0 2px 6px rgba(0,0,0,0.15); /* a subtle shadow */
            border: none;          /* remove default card border if you want a cleaner look */
        }
        .create-product-card .card-header {
            background-color: #f8f9fa;
            border-bottom: 1px solid #eee;
        }
        .create-product-card .card-body {
            padding: 1.5rem;
        }

        /* Black button style (like your other pages) */
        .btn-black {
            background-color: #000;
            border: 1px solid #000;
            color: #fff;
        }
        .btn-black:hover {
            background-color: #333;
            border-color: #333;
        }

        /* Image preview styling */
        .img-preview {
            max-width: 150px;
            height: auto;
            border: 1px solid #ccc;
            border-radius: 4px;
        }
    </style>

    <div class="container create-product-container">
        
        <!-- Card to hold the form -->
        <div class="card create-product-card">
            <div class="card-header">
                <h4 class="mb-0">Create / Edit Product</h4>
            </div>
            <div class="card-body">
                
                <asp:Label ID="lblMessage" runat="server" ForeColor="Red" CssClass="mb-3 d-block" />
                
                <!-- Product Name -->
                <div class="mb-3">
                    <asp:Label 
                        AssociatedControlID="txtName" 
                        Text="Product Name:" 
                        runat="server" 
                        CssClass="form-label" />
                    <asp:TextBox 
                        ID="txtName" 
                        runat="server" 
                        CssClass="form-control" />
                </div>

                <!-- Description -->
                <div class="mb-3">
                    <asp:Label 
                        AssociatedControlID="txtDescription" 
                        Text="Description:" 
                        runat="server" 
                        CssClass="form-label" />
                    <asp:TextBox 
                        ID="txtDescription" 
                        runat="server" 
                        TextMode="MultiLine" 
                        Rows="3" 
                        CssClass="form-control" />
                </div>
                
                <!-- Category -->
                <div class="mb-3">
                    <asp:Label 
                        AssociatedControlID="txtCategory" 
                        Text="Category:" 
                        runat="server" 
                        CssClass="form-label" />
                    <asp:TextBox 
                        ID="txtCategory" 
                        runat="server" 
                        CssClass="form-control" />
                </div>

                <!-- File Upload + Preview -->
                <div class="mb-3">
                    <asp:Label 
                        AssociatedControlID="fuProductImage" 
                        Text="Upload Image:" 
                        runat="server" 
                        CssClass="form-label" />
                    <asp:FileUpload 
                        ID="fuProductImage" 
                        runat="server" 
                        CssClass="form-control" />
                    <br />
                    <asp:Image 
                        ID="imgPreview" 
                        runat="server" 
                        CssClass="img-preview d-block" 
                        Width="150px" 
                        Visible="false" />
                </div>

                <!-- Price -->
                <div class="mb-3">
                    <asp:Label 
                        AssociatedControlID="txtPrice" 
                        Text="Price:" 
                        runat="server" 
                        CssClass="form-label" />
                    <asp:TextBox 
                        ID="txtPrice" 
                        runat="server" 
                        CssClass="form-control" />
                </div>

                <!-- Save Product button with black styling -->
                <asp:Button 
                    ID="btnSave" 
                    runat="server" 
                    Text="Save Product" 
                    CssClass="btn btn-black" 
                    OnClick="btnSave_Click" />
                
            </div><!-- card-body -->
        </div><!-- card -->
    </div><!-- container -->

</asp:Content>
