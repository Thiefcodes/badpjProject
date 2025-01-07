<%@ Page Title="Reset Password" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ResetPassword.aspx.cs" Inherits="badpjProject.ResetPassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="d-flex justify-content-center align-items-center" style="min-height: 100vh;">
        <div class="card shadow-lg p-4" style="width: 400px; border-radius: 10px;">
            <h3 class="text-center mb-4">Reset Password</h3>
            <div class="form-group mb-3">
                <asp:TextBox ID="OTPTextBox" runat="server" CssClass="form-control" placeholder="Enter OTP"></asp:TextBox>
            </div>
            <div class="form-group mb-3">
                <asp:TextBox ID="NewPasswordTextBox" runat="server" CssClass="form-control" TextMode="Password" placeholder="Enter new password"></asp:TextBox>
            </div>
            <div class="text-center">
                <asp:Button ID="ResetPasswordButton" runat="server" Text="Reset Password" CssClass="btn btn-primary w-100" OnClick="ResetPasswordButton_Click" />
            </div>
        </div>
    </div>
</asp:Content>
