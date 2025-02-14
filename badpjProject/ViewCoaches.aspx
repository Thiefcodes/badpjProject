<%@ Page Title="View Coaches" Language="C#" MasterPageFile="~/Site1loggedin.Master" AutoEventWireup="true" CodeBehind="ViewCoaches.aspx.cs" Inherits="badpjProject.ViewCoaches" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <h2 class="mt-4">Pending Coaches</h2>

        <asp:Repeater ID="rptPendingCoaches" runat="server" OnItemCommand="rptPendingCoaches_ItemCommand">
            <HeaderTemplate>
                <table class="table table-hover" id="pendingCoachesTable">
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
                        <asp:LinkButton ID="btnApprove" runat="server" 
                            CssClass="btn btn-success btn-sm"
                            CommandName="Approve" CommandArgument='<%# Eval("Coach_Id") %>' 
                            Text="Approve" 
                            OnClientClick="return confirm('Do you want to approve this coach?');" />

                        <asp:LinkButton ID="btnReject" runat="server" 
                            CssClass="btn btn-danger btn-sm"
                            CommandName="Reject" CommandArgument='<%# Eval("Coach_Id") %>' 
                            Text="Reject" 
                            OnClientClick="return confirm('Do you want to reject this coach?');" />

                        <asp:LinkButton ID="hlViewDetails" runat="server" 
                            CssClass="btn btn-info btn-sm"
                            CommandName="ViewDetails" CommandArgument='<%# Eval("Coach_Id") %>' 
                            Text="View Details" />
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                    </tbody>
                </table>
            </FooterTemplate>
        </asp:Repeater>

        <asp:Literal ID="litNoPendingCoaches" runat="server" Text="" />

        <h2 class="mt-5">Approved Coaches</h2>

        <asp:Repeater ID="rptApprovedCoaches" runat="server" OnItemCommand="rptApprovedCoaches_ItemCommand">
            <HeaderTemplate>
                <table class="table table-striped" id="approvedCoachesTable">
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
                        <asp:LinkButton ID="hlViewDetails" runat="server" 
                            CssClass="btn btn-info btn-sm"
                            CommandName="ViewDetails" CommandArgument='<%# Eval("Coach_Id") %>' 
                            Text="View Details" />

                        <asp:LinkButton ID="btnRemove" runat="server" 
                            CssClass="btn btn-danger btn-sm"
                            CommandName="Remove" CommandArgument='<%# Eval("Coach_Id") %>' 
                            Text="Remove"
                            OnClientClick="return confirm('Do you want to remove this coach?');" />
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                    </tbody>
                </table>
            </FooterTemplate>
        </asp:Repeater>

        <asp:Literal ID="litNoApprovedCoaches" runat="server" Text="" />

        <!-- Label to show success or error messages -->
        <asp:Label ID="lblMessage" runat="server" CssClass="alert" Visible="false"></asp:Label>
    </div>
</asp:Content>
