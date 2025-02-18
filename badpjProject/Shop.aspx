<%@ Page Title="Shop" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="Shop.aspx.cs" Inherits="badpjProject.Shop" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
  <style type="text/css">
    body { background-color: #fff; color: #000; }
    .container { max-width: 1200px; }
    .search-controls { background-color: #fff; border-bottom: 1px solid #ccc; padding: 15px 0; }
    .search-controls .form-control { border: 1px solid #000; }
    .search-controls .btn-primary { background-color: #000; border-color: #000; }
    a.btn-warning, 
    .btn-warning { background-color: #000; border-color: #000; color: #fff; }
    .btn-info { background-color: #fff; border: 1px solid #000; color: #000; }
    .badge { font-size: 0.8rem; }
    .card { border: 1px solid #000; }
    .card .card-title a { color: #000; }
    .card .card-title a:hover { text-decoration: underline; }
    .card-footer { background-color: #fff; }
    .card-text del { color: #888; }
    .badge.bg-success { background-color: #000; color: #fff; }
    .card-footer .btn-primary { background-color: #000; border-color: #000; }
    .card-footer .btn-warning { background-color: #fff; border: 1px solid #000; color: #000; }
    a { text-decoration: none; }
    a:hover { text-decoration: underline; }
    .badge-discount { background-color: transparent; color: #000; font-size: 0.8rem; padding: 0; border: none; font-style: italic; }
    .discount-indicator { font-weight: bold; color: red; background: transparent; padding: 1px 4px; border-radius: 3px; font-size: 0.8rem; }
  </style>

  <div class="container">

    <div class="row my-4 search-controls">
      <div class="col-md-3">
        <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" placeholder="Search products..."></asp:TextBox>
      </div>
      <div class="col-md-2">
        <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control">
          <asp:ListItem Text="All Categories" Value="" />
        </asp:DropDownList>
      </div>
      <div class="col-md-2">
        <asp:TextBox ID="txtMinPrice" runat="server" CssClass="form-control" placeholder="Min Price"></asp:TextBox>
      </div>
      <div class="col-md-2">
        <asp:TextBox ID="txtMaxPrice" runat="server" CssClass="form-control" placeholder="Max Price"></asp:TextBox>
      </div>
      <div class="col-md-1">
        <asp:DropDownList ID="ddlSort" runat="server" CssClass="form-control">
          <asp:ListItem Text="Sort by" Value="" />
          <asp:ListItem Text="Price: Low to High" Value="asc" />
          <asp:ListItem Text="Price: High to Low" Value="desc" />
        </asp:DropDownList>
      </div>
      <div class="col-md-2">
        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary" OnClick="btnSearch_Click" />
      </div>
    </div>


    <div class="d-flex justify-content-between align-items-center mb-4">
      <a href="Shoppingcart.aspx" class="btn btn-warning position-relative">
        View Cart
        <asp:Label ID="lblCartCount" runat="server" CssClass="badge position-absolute top-0 start-100 translate-middle"></asp:Label>
      </a>
      <div>
        <asp:Button ID="btnViewWishlist" runat="server" CssClass="btn btn-info" Text="View Wishlist" OnClick="btnViewWishlist_Click" />
        <asp:Literal ID="litWishlistBadge" runat="server"></asp:Literal>
      </div>
    </div>

    <div class="row" id="productsContainer">
      <asp:Repeater ID="rptProducts" runat="server">
        <ItemTemplate>
          <div class="col-md-3 col-sm-6 mb-4">
            <div class="card h-100">
              <a href='<%# "ProductDetails.aspx?productID=" + Eval("ProductID") %>'>
              <div style="height:250px; overflow:hidden;">
                <img src='<%# ResolveUrl(Eval("ImageUrl").ToString()) %>' class="card-img-top img-fluid" 
                     alt='<%# Eval("ProductName") %>' 
                     style="width:100%; height:100%; object-fit:cover;">
              </div>
              </a>
              <div class="card-body">
                <h5 class="card-title">
                  <a href='<%# "ProductDetails.aspx?productID=" + Eval("ProductID") %>'>
                    <%# Eval("ProductName") %>
                  </a>
                </h5>
                <p class="card-text small text-muted">
                  <%# Eval("Category") %>
                </p>
                <p class="card-text">
                  <%# Convert.ToInt32(Eval("DiscountPercent")) > 0 
                        ? String.Format("<small><del>{0:C}</del></small> {1:C} <span class='discount-indicator'>{2}% OFF</span>", 
                                        Eval("Price"), 
                                        Convert.ToDecimal(Eval("Price")) * (1 - Convert.ToInt32(Eval("DiscountPercent")) / 100m), 
                                        Eval("DiscountPercent"))
                        : String.Format("{0:C}", Eval("Price")) %>
                </p>
              </div>
              <div class="card-footer">
                <div class="d-flex justify-content-between">
                  <asp:LinkButton ID="btnAddToCart" runat="server" CssClass="btn btn-sm btn-primary" 
                      CommandArgument='<%# Eval("ProductID") %>' OnCommand="AddToCart_Command" Text="Add to Cart" />
                  <asp:LinkButton ID="btnAddToWishlist" runat="server" CssClass="btn btn-sm btn-warning" 
                      CommandArgument='<%# Eval("ProductID") %>' OnCommand="AddToWishlist_Command" Text="Wishlist" />
                </div>
              </div>
            </div>
          </div>
        </ItemTemplate>
      </asp:Repeater>
    </div>
  </div>
</asp:Content>
