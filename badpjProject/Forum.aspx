<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Forum.aspx.cs" Inherits="badpjProject.Forum" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4">
        <h1>Forum Threads</h1>
    <asp:GridView ID="gvThreads" runat="server" AutoGenerateColumns="False"
    OnRowCommand="gvThreads_RowCommand">
    <Columns>
        <asp:BoundField DataField="ThreadID" HeaderText="ID" />
        <asp:BoundField DataField="Title" HeaderText="Title" />
        <asp:BoundField DataField="CreatedBy" HeaderText="Author" />
        <asp:BoundField DataField="CreatedAt" HeaderText="Date Created" DataFormatString="{0:yyyy-MM-dd}" />
        <asp:BoundField DataField="Views" HeaderText="Views" /> 
        <asp:ButtonField CommandName="ViewThread" Text="Select" />
        <asp:ButtonField CommandName="UpdateThread" Text="Update" ButtonType="Button" />
        <asp:ButtonField CommandName="DeleteThread" Text="Delete" ButtonType="Button" />
    </Columns>
</asp:GridView>
    <asp:Button ID="btnNewThread" runat="server" Text="New Thread" OnClick="btnNewThread_Click" />
       </div>
</asp:Content>
