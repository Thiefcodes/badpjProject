<%@ Page Title="Manage Users" Language="C#" MasterPageFile="~/Site1loggedin.Master" AutoEventWireup="true" CodeBehind="ManageUsers.aspx.cs" Inherits="badpjProject.ManageUsers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="d-flex justify-content-center align-items-center" style="min-height: 100vh;">
        <div class="card shadow-lg p-4" style="width: 600px; border-radius: 10px;">
            <h3 class="text-center mb-4">Manage Users</h3>

            <!-- Form to Add a New User Account -->
            <h4>Create New User</h4>
            <div class="form-group mb-3">
                <asp:TextBox ID="UserUsernameTextBox" runat="server" CssClass="form-control" placeholder="Username"></asp:TextBox>
            </div>
            <div class="form-group mb-3">
                <asp:TextBox ID="UserPasswordTextBox" runat="server" CssClass="form-control" TextMode="Password" placeholder="Password"></asp:TextBox>
            </div>
            <div class="form-group mb-3">
                <asp:TextBox ID="UserEmailTextBox" runat="server" CssClass="form-control" placeholder="Email"></asp:TextBox>
            </div>
            <div class="text-center mb-4">
                <asp:Button ID="AddUserButton" runat="server" Text="Add User" CssClass="btn btn-primary w-100" OnClick="AddUserButton_Click" />
            </div>

            <!-- List of User Accounts -->
            <h4>Existing User Accounts</h4>
            <asp:GridView ID="UserGridView" runat="server" CssClass="table table-bordered" AutoGenerateColumns="False" DataKeyNames="Id" OnRowCommand="UserGridView_RowCommand">
                <Columns>
                    <asp:BoundField DataField="Id" HeaderText="User ID" ReadOnly="True" />
                    <asp:BoundField DataField="Login_Name" HeaderText="Username" />
                    <asp:BoundField DataField="Email" HeaderText="Email" />
                    <asp:TemplateField HeaderText="Actions">
                        <ItemTemplate>
                            <asp:Button ID="EditButton" runat="server" Text="Edit" CssClass="btn btn-warning btn-sm" CommandName="Edit" CommandArgument='<%# Eval("Id") %>' />
                            <asp:Button ID="DeleteButton" runat="server" Text="Delete" CssClass="btn btn-danger btn-sm" CommandName="DeleteUser" CommandArgument='<%# Eval("Id") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
