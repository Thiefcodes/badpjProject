<%@ Page Title="Your Wishlist" Language="C#" MasterPageFile="~/Site.Master" 
    AutoEventWireup="true" CodeBehind="Wishlist.aspx.cs"
    Inherits="badpjProject.Wishlist" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
  <div class="container">
    <h2 class="mt-4">Your Wishlist</h2>
    <a href="Shop.aspx" class="btn btn-primary mb-3">Back to Shop</a>
    <table class="table table-hover">
      <thead>
        <tr>
          <th>Product</th>
          <th>Notes</th>
          <th>Actions</th>
        </tr>
      </thead>
      <tbody>
        <asp:Repeater ID="rptWishlist" runat="server" 
          OnItemDataBound="rptWishlist_ItemDataBound" 
          OnItemCommand="rptWishlist_ItemCommand">
          <ItemTemplate>
            <tr>
              <td>
                <%# Eval("ProductName") %>
              </td>
              <td>
                <asp:PlaceHolder ID="phView" runat="server">
                  <asp:Label ID="lblNotes" runat="server" Text='<%# Eval("Notes") %>' />
                </asp:PlaceHolder>
                <asp:PlaceHolder ID="phEdit" runat="server" Visible="false">
                  <asp:TextBox ID="txtNotes" runat="server" 
                    Text='<%# Eval("Notes") %>' CssClass="form-control" 
                    TextMode="MultiLine" Rows="2" />
                </asp:PlaceHolder>
              </td>
              <td>
                <asp:LinkButton ID="btnEdit" runat="server" CommandName="Edit" 
                  CommandArgument='<%# Eval("ProductID") %>' 
                  CssClass="btn btn-warning btn-sm me-1" Text="Edit" />
                <asp:LinkButton ID="btnUpdate" runat="server" CommandName="Update" 
                  CommandArgument='<%# Eval("ProductID") %>' 
                  CssClass="btn btn-success btn-sm me-1" Text="Update" Visible="false" />
                <asp:LinkButton ID="btnCancel" runat="server" CommandName="Cancel" 
                  CommandArgument='<%# Eval("ProductID") %>' 
                  CssClass="btn btn-secondary btn-sm me-1" Text="Cancel" Visible="false" />
                <asp:LinkButton ID="btnRemove" runat="server" CommandName="Remove" 
                  CommandArgument='<%# Eval("ProductID") %>' 
                  CssClass="btn btn-danger btn-sm me-1" Text="Remove" />
                <asp:LinkButton ID="btnAddToCart" runat="server" CommandName="AddToCart" 
                  CommandArgument='<%# Eval("ProductID") %>' 
                  CssClass="btn btn-primary btn-sm" Text="Add to Cart" />
              </td>
            </tr>
          </ItemTemplate>
        </asp:Repeater>
      </tbody>
    </table>
  </div>
</asp:Content>
