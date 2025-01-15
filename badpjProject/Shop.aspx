<%@ Page Title="Shop" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="Shop.aspx.cs" Inherits="badpjProject.Shop" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <h2 class="mt-4">Shop</h2>
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <a href="Shoppingcart.aspx" class="btn btn-warning mb-3 position-relative">View Cart
           
                    <asp:Label ID="lblCartCount" runat="server" CssClass="badge bg-danger position-absolute top-0 start-100 translate-middle rounded-pill"></asp:Label>
                </a>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:Repeater ID="rptProducts" runat="server" OnItemCommand="rptProducts_ItemCommand">
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
