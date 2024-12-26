<%@ Page Title="Sign Up" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SignUp.aspx.cs" Inherits="badpjProject.SignUp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="d-flex justify-content-center align-items-center" style="min-height: 100vh;">
        <div class="card shadow-lg p-4" style="width: 400px; border-radius: 10px;">
            <h3 class="text-center mb-4">Sign Up</h3>
            <div class="form-group mb-3">
                <asp:TextBox ID="TextBoxUsername" runat="server" CssClass="form-control" placeholder="Username"></asp:TextBox>
            </div>
            <div class="form-group mb-3">
                <asp:TextBox ID="TextBoxEmail" runat="server" CssClass="form-control" TextMode="Email" placeholder="Email"></asp:TextBox>
            </div>
            <div class="form-group mb-3">
                <asp:TextBox ID="TextBoxPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Password"></asp:TextBox>
            </div>
            <div class="form-group mb-3">
                <asp:TextBox ID="TextBoxConfirmPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Confirm Password"></asp:TextBox>
            </div>
            <div class="text-center">
                <asp:Button ID="ButtonSignUp" runat="server" Text="Sign Up" CssClass="btn btn-primary w-100" OnClick="ButtonSignUp_Click" />
            </div>
        </div>
    </div>
</asp:Content>
