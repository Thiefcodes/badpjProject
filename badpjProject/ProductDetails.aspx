<%@ Page Title="Product Details" Language="C#" MasterPageFile="~/Site.Master" 
    AutoEventWireup="true" CodeBehind="ProductDetails.aspx.cs"
    Inherits="badpjProject.ProductDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <!-- Custom styles for the Product Details page -->
    <style type="text/css">
        body {
            background-color: #fff;
            color: #000;
        }

        .product-details-container {
            margin-top: 20px;
            margin-bottom: 40px;
        }

        /* Card styling (subtle shadow, remove default border) */
        .product-details-card {
            box-shadow: 0 2px 6px rgba(0,0,0,0.15);
            border: none;
        }
        .product-details-card .card-body {
            padding: 1.5rem;
            font-size: 1.05rem; /* slightly larger than default */
        }

        /* Make the left column (image) a bit narrower, right column (text) wider */
        .left-col-image {
            text-align: center; /* center the image within its col if you like */
            display: flex;
            align-items: center;    /* vertically center image in the column */
            justify-content: center; 
        }
        /* Ensure image doesn't exceed 300px in width */
        .product-image {
            max-width: 300px;
            height: auto;
            border-radius: 4px;
        }

        /* Black button style (optional, to match your other pages) */
        .btn-black {
            background-color: #000;
            border: 1px solid #000;
            color: #fff;
        }
        .btn-black:hover {
            background-color: #333;
            border-color: #333;
        }

        /* Review card styling */
        .review-card {
            box-shadow: 0 1px 4px rgba(0,0,0,0.12);
            border: none;
        }
        .review-card .card-body {
            padding: 1rem;
        }
    </style>

    <div class="container product-details-container">
        
        <!-- Product Details Card -->
        <div class="card product-details-card mb-4">
            <div class="card-body">
                <!-- Use a row with 2 columns: left for image, right for text -->
                <div class="row align-items-start">
                    
                    <!-- Left Column: Image (narrower) -->
                    <div class="col-md-4 left-col-image">
                        <asp:Image 
                            ID="imgProduct" 
                            runat="server" 
                            CssClass="product-image img-thumbnail" 
                            AlternateText="Product Image" />
                    </div>

                    <!-- Right Column: Product Info (wider) -->
                    <div class="col-md-8">
                        <asp:Label 
                            ID="lblName" 
                            runat="server" 
                            Font-Bold="true" 
                            Font-Size="Large" 
                            CssClass="d-block mb-2" />

                        <asp:Label 
                            ID="lblCategory" 
                            runat="server" 
                            CssClass="d-block mb-2 text-muted" />

                        <asp:Label 
                            ID="lblPrice" 
                            runat="server" 
                            Font-Size="Medium" 
                            CssClass="d-block mb-3" />

                        <asp:Label 
                            ID="lblDescription" 
                            runat="server" 
                            CssClass="d-block mb-4" />

                        <asp:Label 
                            ID="lblWishlistIndicator" 
                            runat="server" 
                            CssClass="text-success fw-bold" 
                            Visible="false" 
                            Text="(On your Wishlist!)" />

                        <asp:HiddenField ID="hfProductID" runat="server" />

                        <!-- If you want black styling to match your site -->
                        <asp:Button 
                            ID="btnAddToCart" 
                            runat="server" 
                            CssClass="btn btn-black btn-sm" 
                            OnClick="btnAddToCart_Click" 
                            Text="Add to Cart" />
                    </div>
                </div>
            </div>
        </div>

        <!-- User Reviews Section -->
        <h3>User Reviews</h3>
        <asp:Label 
            ID="lblAverageRating" 
            runat="server" 
            CssClass="h5 text-muted" />
        <br /><br />

        <!-- Repeater with each review in its own card -->
        <asp:Repeater ID="rptReviews" runat="server">
            <HeaderTemplate>
                <div class="row">
            </HeaderTemplate>
            <ItemTemplate>
                <div class="col-12 mb-3">
                    <div class="card review-card">
                        <div class="card-body">
                            <p class="mb-1">
                                <strong>By:</strong> <%# Eval("CustomerName") %>
                            </p>
                            <p class="mb-1">
                                <strong>Rating:</strong> <%# Eval("StarRating") %> stars
                            </p>
                            <p class="mb-1">
                                <strong>Review:</strong> <%# Eval("ReviewMessage") %>
                            </p>
                            <p class="mb-0 text-muted">
                                <em>Reviewed on <%# Eval("ReviewDate", "{0:yyyy-MM-dd}") %></em>
                            </p>
                        </div>
                    </div>
                </div>
            </ItemTemplate>
            <FooterTemplate>
                </div>
            </FooterTemplate>
        </asp:Repeater>
    </div>
</asp:Content>
