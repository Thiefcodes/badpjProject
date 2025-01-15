<%@ Page Title="" Language="C#" MasterPageFile="~/Site1loggedin.Master" AutoEventWireup="true" CodeBehind="UpdateThread.aspx.cs" Inherits="badpjProject.UpdateThread" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4">
    <h1>Update Thread</h1>
    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
    <asp:TextBox ID="txtTitle" runat="server" Width="400px"></asp:TextBox>
    <asp:Button ID="btnUpdate" runat="server" Text="Update" OnClick="btnUpdate_Click" />
    <asp:Button ID="btnCancel" runat="server" Text="Back" OnClick="btnCancel_Click" />
        </div>
</asp:Content>
