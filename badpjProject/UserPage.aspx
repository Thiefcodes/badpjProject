<%@ Page Title="User Profile" Language="C#" MasterPageFile="~/Site1loggedin.Master" AutoEventWireup="true" CodeBehind="UserPage.aspx.cs" Inherits="badpjProject.UserPage" %>

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
            <h5 class="text-center text-muted">
                <asp:Label ID="RoleDisplay" runat="server" Text="User Role"></asp:Label>
            </h5>
            <h5 class="text-center text-muted">
                <asp:Label ID="DescriptionDisplay" runat="server" Text="User Description"></asp:Label>
            </h5>
            <asp:Button ID="EditProfileButton" runat="server" Text="Edit Profile" CssClass="btn btn-success mt-3 mb-2" OnClick="EditProfileButton_Click" />
            <!-- New button for enabling facial authentication -->
            <asp:Button ID="EnableFacialAuthButton" runat="server" Text="Enable Facial Authentication" CssClass="btn btn-warning mt-2 mb-4" OnClick="EnableFacialAuthButton_Click" />
            <!-- New HyperLink for sharing rank -->
      <div class="social-share text-center mt-3">
    <asp:HyperLink ID="hlShareRank" runat="server" CssClass="btn">
    <i class="fab fa-twitter fa-2x"></i>
</asp:HyperLink>

<asp:HyperLink ID="hlShareFacebook" runat="server" CssClass="btn">
    <i class="fab fa-facebook fa-2x"></i>
</asp:HyperLink>

<asp:HyperLink ID="hlShareLinkedIn" runat="server" CssClass="btn">
    <i class="fab fa-linkedin fa-2x"></i>
</asp:HyperLink>

<asp:HyperLink ID="hlShareInstagram" runat="server" CssClass="btn">
    <i class="fab fa-instagram fa-2x"></i>
</asp:HyperLink>

</div>

            </div>

        <div class="card shadow-sm p-4 mt-3" style="width: 100%; max-width: 600px; border-radius: 10px;">
            <h4>Comments</h4>
            <hr />
            <!-- Comments Section -->
            <asp:Repeater ID="CommentsRepeater" runat="server">
                <ItemTemplate>
                    <div class="comment mb-3">
                        <p><strong><%# Eval("CommenterName") %>:</strong> <%# Eval("CommentText") %></p>
                        <small class="text-muted"><%# Eval("DateCreated", "{0:MMM dd, yyyy HH:mm}") %></small>
                        <asp:LinkButton 
                            ID="DeleteCommentButton" 
                            runat="server" 
                            CommandArgument='<%# Eval("CommentId") %>' 
                            CssClass="btn btn-danger btn-sm float-end" 
                            Text="Delete" 
                            OnClick="DeleteCommentButton_Click" 
                            Visible='<%# Convert.ToBoolean(Eval("IsOwner")) %>'>
                        </asp:LinkButton>
                    </div>
                </ItemTemplate>
            </asp:Repeater>

            <!-- Add Comment Form -->
            <asp:TextBox ID="CommentTextBox" runat="server" CssClass="form-control mt-3" placeholder="Leave a comment..." TextMode="MultiLine"></asp:TextBox>
            <asp:Button ID="AddCommentButton" runat="server" Text="Add Comment" CssClass="btn btn-primary mt-2" OnClick="AddCommentButton_Click" />
        </div>
    </div>
</asp:Content>
