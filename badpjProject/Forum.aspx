﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Forum.aspx.cs" Inherits="badpjProject.Forum" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="ForumStyles.css" rel="stylesheet" type="text/css" />
    <div class="container mt-4">
        <asp:Button ID="btnMyFeed" runat="server" CssClass="btn-custom" Text="My Feed" OnClick="btnMyFeed_Click" />
        <h1>Forum Threads</h1>
    <asp:GridView ID="gvThreads" runat="server" CellPadding="10" AutoGenerateColumns="False"
    OnRowCommand="gvThreads_RowCommand">
    <Columns>
        <asp:BoundField DataField="ThreadID" HeaderText="ID" />
        <asp:ImageField DataImageUrlField="ImagePath" HeaderText="">
        <ControlStyle Width="100px" Height="100px" />
        </asp:ImageField>
        <asp:BoundField DataField="Title" HeaderText="Title" />
        <asp:BoundField DataField="CreatedBy" HeaderText="Author" />
        <asp:BoundField DataField="CreatedAt" HeaderText="Date Created" DataFormatString="{0:yyyy-MM-dd}" />
        <asp:BoundField DataField="Views" HeaderText="Views" /> 
        <asp:BoundField DataField="PostCount" HeaderText="Posts" />
        <asp:ButtonField CommandName="ViewThread" ItemStyle-CssClass="btn-custom" Text="Select" ButtonType="Button"/>
        <asp:ButtonField CommandName="UpdateThread" ItemStyle-CssClass="btn-custom" Text="Update" ButtonType="Button"/>
        <asp:ButtonField CommandName="DeleteThread" ItemStyle-CssClass="btn-custom" Text="Delete" ButtonType="Button" />
    </Columns>
</asp:GridView>
    <asp:Button ID="btnNewThread" runat="server" CssClass="btn-custom" Text="New Thread" OnClick="btnNewThread_Click" />
       </div>
</asp:Content>
