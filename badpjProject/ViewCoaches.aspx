<%@ Page Title="" Language="C#" MasterPageFile="~/Site1loggedin.Master" AutoEventWireup="true" CodeBehind="ViewCoaches.aspx.cs" Inherits="badpjProject.ViewCoaches" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Pending Coaches</h2>
    <asp:GridView ID="gvPendingCoaches" runat="server" AutoGenerateColumns="False" OnRowCommand="gvPendingCoaches_RowCommand" DataKeyNames="Coach_Id">
        <Columns>
            <asp:BoundField DataField="Coach_Id" HeaderText="ID" />
            <asp:BoundField DataField="Coach_Name" HeaderText="Name" />
            <asp:BoundField DataField="Coach_Email" HeaderText="Email" />
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Button ID="btnApprove" runat="server" Text="Approve" CommandName="Approve" CommandArgument='<%# Container.DataItemIndex %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Button ID="btnReject" runat="server" Text="Reject" CommandName="Reject" CommandArgument='<%# Container.DataItemIndex %>' />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>

    <h2>Approved Coaches</h2>
    <asp:GridView ID="gvApprovedCoaches" runat="server" AutoGenerateColumns="False">
        <Columns>
            <asp:BoundField DataField="Coach_Id" HeaderText="ID" />
            <asp:BoundField DataField="Coach_Name" HeaderText="Name" />
            <asp:BoundField DataField="Coach_Email" HeaderText="Email" />
        </Columns>
    </asp:GridView>
</asp:Content>

