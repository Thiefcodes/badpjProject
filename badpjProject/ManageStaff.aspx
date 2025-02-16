<%@ Page Title="Manage Staff" Language="C#" MasterPageFile="~/Site1loggedin.Master" AutoEventWireup="true" CodeBehind="ManageStaff.aspx.cs" Inherits="badpjProject.ManageStaff" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="d-flex justify-content-center align-items-center" style="min-height: 100vh;">
        <div class="card shadow-lg p-4" style="width: 600px; border-radius: 10px;">
            <h3 class="text-center mb-4">Manage Staff</h3>

            <!-- Form to Add a New Staff Account -->
            <h4>Create New Staff</h4>
            <div class="form-group mb-3">
                <asp:TextBox ID="StaffUsernameTextBox" runat="server" CssClass="form-control" placeholder="Username"></asp:TextBox>
            </div>
            <div class="form-group mb-3">
                <asp:TextBox ID="StaffPasswordTextBox" runat="server" CssClass="form-control" TextMode="Password" placeholder="Password"></asp:TextBox>
            </div>
            <div class="form-group mb-3">
                <asp:TextBox ID="StaffEmailTextBox" runat="server" CssClass="form-control" placeholder="Email"></asp:TextBox>
            </div>
            <div class="text-center mb-4">
                <asp:Button ID="AddStaffButton" runat="server" Text="Add Staff" CssClass="btn btn-primary w-100" OnClick="AddStaffButton_Click" />
            </div>

            <!-- List of Staff Accounts -->
            <h4>Existing Staff Accounts</h4>
            <asp:GridView ID="StaffGridView" runat="server" CssClass="table table-bordered" AutoGenerateColumns="False" OnRowCommand="StaffGridView_RowCommand">
                <Columns>
                    <asp:BoundField DataField="Id" HeaderText="Staff ID" ReadOnly="True" />
                    <asp:BoundField DataField="Login_Name" HeaderText="Username" />
                    <asp:BoundField DataField="Email" HeaderText="Email" />
                    <asp:TemplateField HeaderText="Actions">
                        <ItemTemplate>
                            <asp:Button ID="EditButton" runat="server" Text="Edit" CssClass="btn btn-warning btn-sm" CommandName="Edit" CommandArgument='<%# Eval("Id") %>' />
                            <asp:Button ID="DeleteButton" runat="server" Text="Delete" CssClass="btn btn-danger btn-sm" CommandName="DeleteStaff" CommandArgument='<%# Eval("Id") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>

            <!-- Back Button to go to staffpage.aspx -->
            <div class="text-center mt-4">
                <asp:Button ID="BackButton" runat="server" Text="Back to Staff Page" CssClass="btn btn-secondary" PostBackUrl="~/staffpage.aspx" />
            </div>
        </div>
    </div>
</asp:Content>
