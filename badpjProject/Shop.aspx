﻿<%@ Page Title="Shop" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="Shop.aspx.cs" Inherits="badpjProject.Shop" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <h2 class="mt-4">Shop</h2>

        <asp:Repeater ID="rptProducts" runat="server">
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
                        <!-- Clicking the image leads to ProductDetails -->
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
                        <!-- View More button -->
                        <asp:HyperLink
                            ID="hlViewMore"
                            runat="server"
                            NavigateUrl='<%# "ProductDetails.aspx?productID=" + Eval("ProductID") %>'
                            CssClass="btn btn-info btn-sm"
                            Text="View More" />
                        &nbsp;
                        <!-- Add to Cart button -->
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
