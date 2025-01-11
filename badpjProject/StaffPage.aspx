<%@ Page Title="Management" Language="C#" MasterPageFile="~/Site1loggedin.Master" AutoEventWireup="true" CodeBehind="StaffPage.aspx.cs" Inherits="badpjProject.StaffPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="d-flex justify-content-center align-items-center" style="min-height: 100vh;">
        <div class="card shadow-lg p-4" style="width: 400px; border-radius: 10px;">
            <h3 class="text-center mb-4">Management Portal</h3>
            <div class="text-center mb-3">
                <asp:Button ID="ManageStaffButton" runat="server" Text="Manage Staff" CssClass="btn btn-primary w-100 mb-2" OnClick="ManageStaffButton_Click" />
                <asp:Button ID="ManageUsersButton" runat="server" Text="Manage Users" CssClass="btn btn-secondary w-100" OnClick="ManageUsersButton_Click" />
            </div>
        </div>
    </div>
</asp:Content>
