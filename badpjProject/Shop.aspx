<%@ Page Title="Shop" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="Shop.aspx.cs" Inherits="badpjProject.Shop" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <h2 class="mt-4">Shop</h2>
        <div>
        <a href="Shoppingcart.aspx" class="btn btn-warning mb-3">View Cart</a>
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
                            <th>Price</th>
                            <th>Actions</th>
                        </tr>
                    </thead>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td>
                        <asp:HyperLink
                            NavigateUrl='<%# "ProductDetails.aspx?productID=" + Eval("ProductID") %>'>
                            <asp:Image ID="imgProduct" runat="server"
                                ImageUrl='<%# Eval("ImageUrl") %>'
                                CssClass="img-thumbnail"
                                Height="50px" Width="50px" />
                        </asp:HyperLink>
                    </td>
                    <td><%# Eval("ProductName") %></td>
                     <td>
                        <%# Eval("ProductName") %>
                        <asp:Label ID="lblWishlistIndicator" runat="server"
                            CssClass="text-success fw-bold ms-2"
                            Visible="false"
                            Text="(On your Wishlist!)"></asp:Label>
                    </td>
                    <td><%# Eval("Price", "{0:C}") %></td>
                    <td>
                        <asp:HyperLink
                            ID="hlViewMore"
                            runat="server"
                            NavigateUrl='<%# "ProductDetails.aspx?productID=" + Eval("ProductID") %>'
                            CssClass="btn btn-info btn-sm"
                            Text="View More" />
                        &nbsp;
                        <asp:LinkButton
                            ID="btnAddToCart"
                            runat="server"
                            CssClass="btn btn-primary btn-sm"
                            CommandArgument='<%# Eval("ProductID") %>'
                            OnCommand="AddToCart_Command"
                            Text="Add to Cart" />
                        &nbsp;
                        <asp:LinkButton
                            ID="btnAddToWishlist"
                            runat="server"
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
