<%@ Page Title="Login" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="badpjProject.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="d-flex justify-content-center align-items-center" style="min-height: 100vh;">
        <div class="card shadow-lg p-4" style="width: 350px; border-radius: 10px;">
            <h3 class="text-center mb-4">Login</h3>
            <div class="form-group mb-3">
                <asp:TextBox ID="TextBox1" runat="server" CssClass="form-control" placeholder="Username"></asp:TextBox>
            </div>
            <div class="form-group mb-3">
                <asp:TextBox ID="TextBox2" runat="server" CssClass="form-control" placeholder="Password" TextMode="Password"></asp:TextBox>
            </div>
            <div class="form-group text-end">
                <asp:LinkButton ID="LinkButton1" runat="server" CssClass="text-decoration-none text-primary" PostBackUrl="ForgetPassword.aspx">
    Forgot Password?
</asp:LinkButton>

            </div>
            <div class="text-center">
                <asp:Button ID="Button1" runat="server" Text="Login" CssClass="btn btn-primary w-100" OnClick="Button1_Click1" />
            </div>

        </div>
    </div>
    <asp:Button ID="Button2" runat="server" Text="Login" CssClass="btn btn-primary w-100" OnClick="Button2_Click1" />

</asp:Content>
