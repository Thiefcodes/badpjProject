<%@ Page Title="OTP Confirmation" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="OtpConfirmation.aspx.cs" Inherits="badpjProject.OtpConfirmation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="d-flex justify-content-center align-items-center" style="min-height: 100vh;">
        <div class="card shadow-lg p-4" style="width: 400px; border-radius: 10px;">
            <h3 class="text-center mb-4">OTP Confirmation</h3>
            <div class="form-group mb-3">
                <asp:TextBox ID="TextBoxOtp" runat="server" CssClass="form-control" placeholder="Enter OTP"></asp:TextBox>
            </div>
            <div class="text-center">
                <asp:Button ID="ButtonVerify" runat="server" Text="Verify" CssClass="btn btn-primary w-100" OnClick="ButtonVerify_Click" />
            </div>
        </div>
    </div>
</asp:Content>
