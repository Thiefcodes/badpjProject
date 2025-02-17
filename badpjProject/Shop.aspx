<%@ Page Title="Shop" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="Shop.aspx.cs" Inherits="badpjProject.Shop" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
  <div class="container">
    <h2 class="mt-4">Shop</h2>
    <div>
      <asp:UpdatePanel runat="server">
        <ContentTemplate>
          <a href="Shoppingcart.aspx" class="btn btn-warning mb-3 position-relative">
            View Cart
            <asp:Label ID="lblCartCount" runat="server" CssClass="badge bg-danger position-absolute top-0 start-100 translate-middle rounded-pill"></asp:Label>
          </a>
        </ContentTemplate>
      </asp:UpdatePanel>
      <asp:Button ID="btnViewWishlist" runat="server"
          CssClass="btn btn-info mb-3"
          Text="View Wishlist"
          OnClick="btnViewWishlist_Click"/>
    </div>
    <asp:Repeater ID="rptProducts" runat="server" OnItemDataBound="rptProducts_ItemDataBound">
      <HeaderTemplate>
        <table class="table table-hover">
          <thead>
            <tr>
              <th>Image</th>
              <th>Name</th>
              <th>Category</th>
              <th>Price</th>
              <th>Rating</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
      </HeaderTemplate>
      <ItemTemplate>
        <tr>
          <td>
            <asp:HyperLink NavigateUrl='<%# "ProductDetails.aspx?productID=" + Eval("ProductID") %>'>
              <asp:Image ID="imgProduct" runat="server"
                ImageUrl='<%# Eval("ImageUrl") %>'
                CssClass="img-thumbnail"
                Height="50px" Width="50px" />
            </asp:HyperLink>
          </td>
          <td>
            <%# Eval("ProductName") %>
            <asp:Label ID="lblWishlistIndicator" runat="server"
              CssClass="text-success fw-bold ms-2"
              Visible="false"
              Text="(On your Wishlist!)"></asp:Label>
          </td>
          <td>
              <%# Eval("Category") %>
           </td>
           <td>
            <%# Convert.ToInt32(Eval("DiscountPercent")) > 0 
                ? String.Format("<del>{0:C}</del> {1:C} <span class='text-success'>({2}% OFF)</span>", 
                    Eval("Price"), 
                    Convert.ToDecimal(Eval("Price")) * (1 - Convert.ToInt32(Eval("DiscountPercent")) / 100m), 
                    Eval("DiscountPercent"))
                : String.Format("{0:C}", Eval("Price")) %>
          </td>
          <td>
            <%# Eval("AverageRating") != DBNull.Value ? String.Format("{0:F1}", Eval("AverageRating")) : "N/A" %>
          </td>
          <td>
            <asp:HyperLink ID="hlViewMore" runat="server"
              NavigateUrl='<%# "ProductDetails.aspx?productID=" + Eval("ProductID") %>'
              CssClass="btn btn-info btn-sm"
              Text="View More" />
            &nbsp;
            <asp:LinkButton ID="btnAddToCart" runat="server"
              CssClass="btn btn-primary btn-sm"
              CommandArgument='<%# Eval("ProductID") %>'
              OnCommand="AddToCart_Command"
              Text="Add to Cart" />
            &nbsp;
            <asp:LinkButton ID="btnAddToWishlist" runat="server"
              CssClass="btn btn-warning btn-sm"
              CommandArgument='<%# Eval("ProductID") %>'
              OnCommand="AddToWishlist_Command"
              Text="Add to Wishlist" />
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
