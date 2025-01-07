<%@ Page Title="Forgot Password" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ForgetPassword.aspx.cs" Inherits="badpjProject.ForgetPassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="d-flex justify-content-center align-items-center" style="min-height: 100vh;">
        <div class="card shadow-lg p-4" style="width: 400px; border-radius: 10px;">
            <h3 class="text-center mb-4">Forgot Password</h3>
            <div class="form-group mb-3">
                <asp:TextBox ID="LoginNameTextBox" runat="server" CssClass="form-control" placeholder="Enter your username"></asp:TextBox>
            </div>
            <div class="text-center">
                <asp:Button ID="SendOTPButton" runat="server" Text="Send OTP" CssClass="btn btn-primary w-100" OnClick="SendOTPButton_Click" />
            </div>
        </div>
    </div>
</asp:Content>
