<%@ Page Title="Leave a Review" Language="C#" MasterPageFile="~/Site1loggedin.Master"
    AutoEventWireup="true" CodeBehind="LeaveReview.aspx.cs" Inherits="badpjProject.LeaveReview" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4">
        <h2>Leave a Review</h2>
        <asp:Label ID="lblProductName" runat="server" Font-Bold="true" Font-Size="Large" />
        <br /><br />
        <asp:Label ID="lblRating" runat="server" Text="Rating:" />
        <asp:DropDownList ID="ddlStarRating" runat="server">
            <asp:ListItem Text="1" Value="1" />
            <asp:ListItem Text="2" Value="2" />
            <asp:ListItem Text="3" Value="3" />
            <asp:ListItem Text="4" Value="4" />
            <asp:ListItem Text="5" Value="5" />
        </asp:DropDownList>
        <br /><br />
        <asp:Label ID="lblReview" runat="server" Text="Your Review:" />
        <asp:TextBox ID="txtReviewMessage" runat="server" TextMode="MultiLine" Rows="4" CssClass="form-control" />
        <br />
        <asp:Button ID="btnSubmitReview" runat="server" Text="Submit Review" CssClass="btn btn-primary" OnClick="btnSubmitReview_Click" />
    </div>
</asp:Content>
