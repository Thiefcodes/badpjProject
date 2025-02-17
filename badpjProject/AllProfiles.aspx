<%@ Page Title="All User Profiles" Language="C#" MasterPageFile="~/Site1loggedin.Master" AutoEventWireup="true" CodeBehind="AllProfiles.aspx.cs" Inherits="badpjProject.AllProfiles" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-5">
        <h2 class="text-center">All User Profiles</h2>
        <div class="row mb-3">
    <div class="col-md-8 offset-md-2">
        <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" Placeholder="Search for users..."></asp:TextBox>
        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary mt-2" OnClick="btnSearch_Click" />
    </div>
</div>

        <div class="card shadow-sm p-4 mt-4" style="width: 100%; max-width: 800px; margin: 0 auto;">
            <asp:Repeater ID="ProfilesRepeater" runat="server">
                <HeaderTemplate>
                    <div class="list-group">
                </HeaderTemplate>
                <ItemTemplate>
                    <div class="list-group-item d-flex justify-content-between align-items-center">
                        <div>
                            <h5 class="mb-1"><%# Eval("Login_Name") %></h5>
                            <p class="mb-1 text-muted">Email: <%# Eval("Email") %></p>
                        </div>
                        <a href="VisitingProfile.aspx?UserId=<%# Eval("Id") %>" class="btn btn-primary">Visit Profile</a>
                    </div>
                </ItemTemplate>
                <FooterTemplate>
                    </div>
                </FooterTemplate>
            </asp:Repeater>
        </div>
    </div>
</asp:Content>
