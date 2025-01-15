<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="NewThread.aspx.cs" Inherits="badpjProject.NewThread" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4">
    <h1>New Thread</h1>

<!-- Thread Title TextBox -->
<asp:TextBox ID="txtTitle" runat="server" Placeholder="Thread Title"></asp:TextBox>

<!-- Create Button -->
<asp:Button ID="btnCreate" runat="server" Text="Create" OnClick="btnCreate_Click" />
        </div>
</asp:Content>
