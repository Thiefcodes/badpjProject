<%@ Page Title="User Profile" Language="C#" MasterPageFile="~/Site1loggedin.Master" AutoEventWireup="true" CodeBehind="ProfilePage.aspx.cs" Inherits="badpjProject.ProfilePage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="d-flex justify-content-center align-items-center" style="min-height: 100vh;">
        <div class="card shadow-lg p-4" style="width: 400px; border-radius: 10px;">
            <h3 class="text-center mb-4">Your Profile</h3>
            <div class="text-center mb-3">
                <asp:Image ID="ProfilePicture" runat="server" CssClass="rounded-circle mb-3" Width="100px" Height="100px" />
            </div>
            <p class="text-center">
                <asp:Label ID="lblUsername" runat="server" CssClass="fw-bold"></asp:Label>
            </p>
            <p class="text-center">
                <asp:Label ID="lblDescription" runat="server"></asp:Label>
            </p>
            <div class="text-center">
                <asp:Button ID="EditProfileButton" runat="server" Text="Edit Profile" CssClass="btn btn-primary" OnClick="EditProfileButton_Click" />
            </div>
        </div>
    </div>
</asp:Content>
