﻿<%@ Page Title="Login" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="badpjProject.Login" %>

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
                            <div class="text-center mt-3">
    <asp:Button ID="btnFacialLogin" runat="server" Text="Login via Facial Authentication" CssClass="btn btn-info w-100" OnClick="btnFacialLogin_Click" />
</div>
        </div>
    </div>
  
</asp:Content>
 <!-- User:test1 Password:1
      admin: admin1 Password:1
     for more accounts check the data in table [Table]-->
                