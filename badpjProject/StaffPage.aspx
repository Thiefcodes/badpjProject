<%@ Page Title="Staff Management" Language="C#" MasterPageFile="~/Site1loggedin.Master" AutoEventWireup="true" CodeBehind="StaffPage.aspx.cs" Inherits="badpjProject.StaffPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="d-flex justify-content-center align-items-center" style="min-height: 100vh;">
        <div class="card shadow-lg p-4" style="width: 600px; border-radius: 10px;">
            <h3 class="text-center mb-4">Staff Management</h3>

            <!-- Form to Add a New Staff Account -->
            <h4>Create New Staff</h4>
            <div class="form-group mb-3">
                <asp:TextBox ID="StaffUsernameTextBox" runat="server" CssClass="form-control" placeholder="Username"></asp:TextBox>
            </div>
            <div class="form-group mb-3">
                <asp:TextBox ID="StaffPasswordTextBox" runat="server" CssClass="form-control" TextMode="Password" placeholder="Password"></asp:TextBox>
            </div>
            <div class="text-center mb-4">
                <asp:Button ID="AddStaffButton" runat="server" Text="Add Staff" CssClass="btn btn-primary w-100" OnClick="AddStaffButton_Click" />
            </div>

            <!-- List of Staff Accounts -->
            <h4>Existing Staff Accounts</h4>
            <asp:GridView ID="StaffGridView" runat="server" CssClass="table table-bordered" AutoGenerateColumns="False" OnRowDeleting="StaffGridView_RowDeleting">
                <Columns>
                    <asp:BoundField DataField="Id" HeaderText="Staff ID" ReadOnly="True" />
                    <asp:BoundField DataField="Login_Name" HeaderText="Username" />
                    <asp:CommandField ShowDeleteButton="True" HeaderText="Actions" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
