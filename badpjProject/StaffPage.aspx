<%@ Page Title="Management" Language="C#" MasterPageFile="~/Site1loggedin.Master" AutoEventWireup="true" CodeBehind="StaffPage.aspx.cs" Inherits="badpjProject.StaffPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-5">
        <div class="d-flex justify-content-center align-items-center flex-column">
            <asp:Image ID="ProfilePicture" runat="server" CssClass="rounded-circle mb-3" Width="150px" Height="150px" />
            <h2 class="text-center">
                <asp:Label ID="StaffNameLabel" runat="server" Text="Staff Name"></asp:Label>
            </h2>
            <h5 class="text-center text-muted">
                <asp:Label ID="StaffEmailLabel" runat="server" Text="staff@example.com"></asp:Label>
            </h5>
            <asp:Button ID="EditProfileButton" runat="server" Text="Edit Profile" CssClass="btn btn-success mt-3 mb-4" OnClick="EditProfileButton_Click" />
            <div class="card shadow-sm p-4 mt-3" style="width: 100%; max-width: 600px; border-radius: 10px;">
                <h4>Management Portal</h4>
                <hr />
                <p>Welcome to the Management Portal! Use the buttons below to manage staff and users.</p>
                <!-- Add the new button for enabling facial authentication here -->
                <asp:Button ID="EnableFacialAuthButton" runat="server" Text="Enable Facial Authentication" CssClass="btn btn-warning mt-3" OnClick="EnableFacialAuthButton_Click" />
            </div>
        </div>

        <!-- Bottom-left buttons -->
        <div class="position-absolute" style="bottom: 20px; left: 20px;">
            <asp:Button ID="ManageStaffButton" runat="server" Text="Manage Staff" CssClass="btn btn-primary mb-2 w-100" OnClick="ManageStaffButton_Click" />
            <asp:Button ID="ManageUsersButton" runat="server" Text="Manage Users" CssClass="btn btn-secondary mb-2 w-100" OnClick="ManageUsersButton_Click" />
            <asp:Button ID="ConfigureRewardsButton" runat="server" Text="Configure Rewards" CssClass="btn btn-info w-100" OnClick="ConfigureRewardsButton_Click" />
        </div>
    </div>
</asp:Content>
