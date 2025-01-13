<%@ Page Title="Visiting Profile" Language="C#" MasterPageFile="~/Site1loggedin.Master" AutoEventWireup="true" CodeBehind="VisitingProfile.aspx.cs" Inherits="badpjProject.VisitingProfile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-5">
        <div class="d-flex justify-content-center align-items-center flex-column">
            <asp:Image ID="ProfilePicture" runat="server" CssClass="rounded-circle mb-3" Width="150px" Height="150px" />
            <h2 class="text-center">
                <asp:Label ID="UsernameLabel" runat="server" Text="User Name"></asp:Label>
            </h2>
            <h5 class="text-center text-muted">
                <asp:Label ID="UserEmailLabel" runat="server" Text="user@example.com"></asp:Label>
            </h5>
        </div>

        <div class="card shadow-sm p-4 mt-3" style="width: 100%; max-width: 600px; border-radius: 10px;">
            <h4>Comments</h4>
            <hr />
            <!-- Comments Section -->
            <asp:Repeater ID="CommentsRepeater" runat="server">
                <HeaderTemplate>
                    <div class="list-group">
                </HeaderTemplate>
                <ItemTemplate>
                    <div class="list-group-item d-flex justify-content-between align-items-center">
                        <div>
                            <strong><%# Eval("CommenterName") %>:</strong> <%# Eval("CommentText") %>
                            <small class="text-muted d-block"><%# Eval("DateCreated", "{0:MMM dd, yyyy HH:mm}") %></small>
                        </div>
                        <!-- Delete button visible only if the visitor is the profile owner -->
                        <asp:LinkButton ID="DeleteCommentButton" runat="server" CommandArgument='<%# Eval("CommentId") %>'
                            CssClass="btn btn-danger btn-sm" OnClick="DeleteCommentButton_Click" Visible='<%# Eval("IsOwner") %>'>
                            Delete
                        </asp:LinkButton>
                    </div>
                </ItemTemplate>
                <FooterTemplate>
                    </div>
                </FooterTemplate>
            </asp:Repeater>

            <!-- Add Comment Section -->
            <asp:TextBox ID="CommentTextBox" runat="server" CssClass="form-control mt-3" placeholder="Leave a comment..." TextMode="MultiLine"></asp:TextBox>
            <asp:Button ID="AddCommentButton" runat="server" Text="Add Comment" CssClass="btn btn-primary mt-2" OnClick="AddCommentButton_Click" />
        </div>
    </div>
</asp:Content>
