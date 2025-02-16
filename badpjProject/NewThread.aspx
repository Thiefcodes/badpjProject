<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="NewThread.aspx.cs" Inherits="badpjProject.NewThread" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4">
    <h1>New Thread</h1>

<!-- Thread Title TextBox -->
<asp:TextBox ID="txtTitle" runat="server" Placeholder="Thread Title"></asp:TextBox>
<asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
<asp:Button ID="btnCancel" runat="server" CssClass="btn btn-danger" Text="Cancel" OnClick="btnCancel_Click" />
<!-- Create Button -->
<asp:Button ID="btnCreate" runat="server" CssClass="btn btn-primary btn-sm" Text="Create" OnClick="btnCreate_Click" />

        </div>
</asp:Content>
