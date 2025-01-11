<%@ Page Title="Edit User" Language="C#" MasterPageFile="~/Site1loggedin.Master" AutoEventWireup="true" CodeBehind="EditUser.aspx.cs" Inherits="badpjProject.EditUser" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="d-flex justify-content-center align-items-center" style="min-height: 100vh;">
        <div class="card shadow-lg p-4" style="width: 400px; border-radius: 10px;">
            <h3 class="text-center mb-4">Edit User</h3>
            <div class="form-group mb-3">
                <asp:TextBox ID="EditUsernameTextBox" runat="server" CssClass="form-control" placeholder="Username"></asp:TextBox>
            </div>
            <div class="form-group mb-3">
                <asp:TextBox ID="EditEmailTextBox" runat="server" CssClass="form-control" placeholder="Email"></asp:TextBox>
            </div>
            <div class="form-group mb-3">
                <asp:TextBox ID="EditPasswordTextBox" runat="server" CssClass="form-control" TextMode="Password" placeholder="Password"></asp:TextBox>
            </div>
            <div class="text-center">
                <asp:Button ID="UpdateUserButton" runat="server" Text="Update User" CssClass="btn btn-success w-100" OnClick="UpdateUserButton_Click" />
            </div>
        </div>
    </div>
</asp:Content>
