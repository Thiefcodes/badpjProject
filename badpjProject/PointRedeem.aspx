<%@ Page Title="Redeem Points" Language="C#" MasterPageFile="~/Site1loggedin.Master" AutoEventWireup="true" CodeBehind="PointRedeem.aspx.cs" Inherits="badpjProject.PointRedeem" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
  <div class="container mt-5">
    <h2 class="text-center">Redeem Your Points</h2>
    <div class="row">
      <div class="col-md-12 text-center">
        <asp:Label ID="lblUserPoints" runat="server" Text="Your Points: 0" CssClass="h4"></asp:Label>
        <br /><br />
      </div>
    </div>
    <hr />
    <div class="row">
      <asp:Repeater ID="ProductsRepeater" runat="server">
        <ItemTemplate>
          <div class="col-md-4">
            <div class="card mb-4 shadow-sm">
              <img src='<%# ResolveUrl(Eval("ProductImage").ToString()) %>' class="card-img-top" alt="Product Image" style="height: 200px; object-fit: cover;">
              <div class="card-body">
                <h5 class="card-title"><%# Eval("ProductName") %></h5>
                <p class="card-text">Cost: <%# Eval("CostPoints") %> points</p>
                <asp:Button ID="btnRedeem" runat="server" Text="Redeem" CssClass="btn btn-primary w-100" 
                            CommandArgument='<%# Eval("ProductId") %>' OnCommand="btnRedeem_Command" />
              </div>
            </div>
          </div>
        </ItemTemplate>
      </asp:Repeater>
    </div>
    <asp:Label ID="lblMessage" runat="server" CssClass="text-success"></asp:Label>
  </div>
</asp:Content>
