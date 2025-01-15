<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Forum.aspx.cs" Inherits="badpjProject.Forum" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
        <h1>Forum Threads</h1>
    <asp:GridView ID="gvThreads" runat="server" AutoGenerateColumns="False"
    OnRowCommand="gvThreads_RowCommand" OnRowEditing="gvThreads_RowEditing"
    OnRowUpdating="gvThreads_RowUpdating" OnRowCancelingEdit="gvThreads_RowCancelingEdit"
    >
    <Columns>
        <asp:BoundField DataField="ThreadID" HeaderText="ID" />
        <asp:BoundField DataField="Title" HeaderText="Title" />
        <asp:BoundField DataField="CreatedBy" HeaderText="Author" />
        <asp:BoundField DataField="CreatedAt" HeaderText="Date Created" DataFormatString="{0:yyyy-MM-dd}" />
        <asp:CommandField ShowEditButton="True" />
        <asp:ButtonField CommandName="ViewThread" Text="Select" />
        <asp:ButtonField CommandName="DeleteThread" Text="Delete" />
    </Columns>
</asp:GridView>
    <asp:Button ID="btnNewThread" runat="server" Text="New Thread" OnClick="btnNewThread_Click" />
</asp:Content>
