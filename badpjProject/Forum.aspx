<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Forum.aspx.cs" Inherits="badpjProject.Forum" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
        <h1>Forum Threads</h1>
    <asp:GridView ID="gvThreads" runat="server" AutoGenerateColumns="False" 
    OnSelectedIndexChanged="gvThreads_SelectedIndexChanged">
    <Columns>
        <asp:BoundField DataField="ThreadID" HeaderText="ID" />
        <asp:BoundField DataField="Title" HeaderText="Title" />
        <asp:BoundField DataField="CreatedBy" HeaderText="Author" />
        <asp:BoundField DataField="CreatedAt" HeaderText="Date Created" DataFormatString="{0:yyyy-MM-dd}" />
        <asp:CommandField ShowSelectButton="True" SelectText="Select" />
    </Columns>
</asp:GridView>
    <asp:Button ID="btnNewThread" runat="server" Text="New Thread" OnClick="btnNewThread_Click" />
</asp:Content>
