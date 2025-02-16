<%@ Page Title="View Coaches" Language="C#" MasterPageFile="~/Site1loggedin.Master" AutoEventWireup="true" CodeBehind="ViewCoaches.aspx.cs" Inherits="badpjProject.ViewCoaches" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <h2 class="mt-4 text-center">Manage Coaches</h2>

        <!-- Filter Section -->
        <div class="row mb-4">
            <div class="col-md-4">
                <asp:Label ID="lblFilter" runat="server" Text="Filter by Status:" CssClass="font-weight-bold"></asp:Label>
                <asp:DropDownList ID="ddlStatusFilter" runat="server" AutoPostBack="true"
                    OnSelectedIndexChanged="ddlStatusFilter_SelectedIndexChanged" CssClass="form-control">
                    <asp:ListItem Value="Pending" Text="Pending" Selected="True" />
                    <asp:ListItem Value="Approved" Text="Approved" />
                </asp:DropDownList>
            </div>
            <div class="col-md-4">
                <asp:Label ID="lblSort" runat="server" Text="Sort by:" CssClass="font-weight-bold"></asp:Label>
                <asp:DropDownList ID="ddlSort" runat="server" AutoPostBack="true"
                    OnSelectedIndexChanged="ddlSort_SelectedIndexChanged" CssClass="form-control">
                    <asp:ListItem Value="Name" Text="Name" />
                    <asp:ListItem Value="Email" Text="Email" />
                </asp:DropDownList>
            </div>
        </div>

        <!-- Single Repeater for Coaches -->
        <asp:Repeater ID="rptCoaches" runat="server" OnItemCommand="rptCoaches_ItemCommand">
            <HeaderTemplate>
                <table class="table table-striped" id="coachesTable">
                    <thead>
                        <tr>
                            <th style="width: 30%;">Name</th>
                            <th style="width: 40%;">Email</th>
                            <th style="width: 30%;">Actions</th>
                        </tr>
                    </thead>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td><%# Eval("Coach_Name") %></td>
                    <td><%# Eval("Coach_Email") %></td>
                    <td>
                        <asp:LinkButton ID="hlViewDetails" runat="server" CssClass="btn btn-info btn-sm"
                            CommandName="ViewDetails" CommandArgument='<%# Eval("Coach_Id") %>' Text="View Details" />
                        &nbsp;
            <!-- Only show Remove action (with confirmation) for approved coaches -->
                        <asp:LinkButton ID="btnRemove" runat="server" CssClass="btn btn-danger btn-sm"
                            CommandName="Remove" CommandArgument='<%# Eval("Coach_Id") %>' Text="Remove"
                            OnClientClick="return confirm('Do you want to remove this coach?');" />
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </tbody>
        </table>
            </FooterTemplate>
        </asp:Repeater>

        <!-- Literal for "No Data" Message -->
        <asp:Literal ID="litNoCoaches" runat="server" Text=""></asp:Literal>

        <!-- Global Message Label -->
        <asp:Label ID="lblMessage" runat="server" CssClass="alert" Visible="false"></asp:Label>
    </div>
</asp:Content>
