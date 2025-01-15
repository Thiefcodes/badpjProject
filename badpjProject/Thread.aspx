<%@ Page Title="" Language="C#" MasterPageFile="~/Site1loggedin.Master" AutoEventWireup="true" CodeBehind="Thread.aspx.cs" Inherits="badpjProject.Thread" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1><asp:Label ID="lblThreadTitle" runat="server" Text="Thread Title"></asp:Label></h1>
<asp:GridView ID="gvPosts" runat="server" AutoGenerateColumns="False">
    <Columns>
        <asp:BoundField DataField="Content" HeaderText="Content" />
        <asp:BoundField DataField="CreatedBy" HeaderText="Author" />
        <asp:BoundField DataField="CreatedAt" HeaderText="Date Created" DataFormatString="{0:yyyy-MM-dd}" />
    </Columns>
</asp:GridView>
<asp:Button ID="btnReply" runat="server" Text="Reply" OnClick="btnReply_Click" />
</asp:Content>
