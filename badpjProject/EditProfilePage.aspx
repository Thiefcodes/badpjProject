<%@ Page Title="Edit Profile" Language="C#" MasterPageFile="~/Site1loggedin.Master" AutoEventWireup="true" CodeBehind="EditProfilePage.aspx.cs" Inherits="badpjProject.EditProfilePage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="d-flex justify-content-center align-items-center" style="min-height: 100vh;">
        <div class="card shadow-lg p-4" style="width: 400px; border-radius: 10px;">
            <h3 class="text-center mb-4">Edit Profile</h3>
            <div class="form-group mb-3 text-center">
                <asp:Image ID="CurrentProfilePicture" runat="server" CssClass="rounded-circle mb-3" Width="100px" Height="100px" />
                <asp:FileUpload ID="ProfilePictureUpload" runat="server" CssClass="form-control" />
            </div>
            <div class="form-group mb-3">
                <asp:TextBox ID="DescriptionTextBox" runat="server" CssClass="form-control" TextMode="MultiLine" placeholder="Update your description"></asp:TextBox>
            </div>
            <div class="text-center">
                <asp:Button ID="SaveButton" runat="server" Text="Save Changes" CssClass="btn btn-primary w-100" OnClick="SaveButton_Click" />
            </div>
        </div>
    </div>
</asp:Content>
