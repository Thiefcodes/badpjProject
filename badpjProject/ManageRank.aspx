<%@ Page Title="Manage Rank" Language="C#" MasterPageFile="~/Site1loggedin.Master" AutoEventWireup="true" CodeBehind="ManageRank.aspx.cs" Inherits="badpjProject.ManageRank" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2 class="mt-4 text-center">Manage Video Submissions</h2>

    <!-- Filtering and Sorting Options -->
    <div class="row mb-4">
        <div class="col-md-4">
            <asp:Label ID="lblFilter" runat="server" Text="Filter by Status:" CssClass="font-weight-bold"></asp:Label>
            <asp:DropDownList ID="ddlStatusFilter" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlStatusFilter_SelectedIndexChanged" CssClass="form-control">
                <asp:ListItem Value="Pending" Text="Pending" Selected="True"></asp:ListItem>
                <asp:ListItem Value="Approved" Text="Approved"></asp:ListItem>
                <asp:ListItem Value="Rejected" Text="Rejected"></asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="col-md-4">
            <asp:Label ID="lblSort" runat="server" Text="Sort by:" CssClass="font-weight-bold"></asp:Label>
            <asp:DropDownList ID="ddlSort" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSort_SelectedIndexChanged" CssClass="form-control">
                <asp:ListItem Value="SubmissionDate" Text="Submission Date"></asp:ListItem>
                <asp:ListItem Value="UserID" Text="User ID"></asp:ListItem>
            </asp:DropDownList>
        </div>
    </div>

    <!-- Repeater to display submissions -->
    <asp:Repeater
        ID="rptSubmissions"
        runat="server"
        OnItemDataBound="rptSubmissions_ItemDataBound"
        OnItemCommand="rptSubmissions_ItemCommand">
        <HeaderTemplate>
            <table class="table table-bordered">
                <thead>
                    <tr>
                        <th>Video</th>
                        <th>User ID</th>
                        <th>Comment</th>
                        <th>Submission Date</th>
                        <th>Assign Points</th>
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td>
                    <video width="200" controls>
                        <source src='<%# "/Uploads/" + Eval("VideoFile") %>' type="video/mp4">
                        Your browser does not support the video tag.
                    </video>
                </td>
                <td><%# Eval("UserID") %></td>
                <td><%# Eval("Comment") %></td>
                <td><%# Eval("SubmissionDate", "{0:MM/dd/yyyy HH:mm}") %></td>
                <td>
                    <asp:TextBox
                        ID="tbPoints"
                        runat="server"
                        CssClass="form-control"
                        Width="80px"></asp:TextBox>
                </td>
                <td>
                    <!-- Panel for Pending actions (Approve/Reject) -->
                    <asp:Panel ID="pnlApproveReject" runat="server">
                        <asp:LinkButton
                            ID="btnApprove"
                            runat="server"
                            CommandName="Approve"
                            CommandArgument='<%# Eval("SubmissionID") %>'
                            Text="Approve"
                            CssClass="btn btn-success btn-sm" />
                        &nbsp;
                        <asp:LinkButton
                            ID="btnReject"
                            runat="server"
                            CommandName="Reject"
                            CommandArgument='<%# Eval("SubmissionID") %>'
                            Text="Reject"
                            CssClass="btn btn-danger btn-sm" />
                    </asp:Panel>
                    <!-- Panel for Approved/Rejected actions (Delete) -->
                    <asp:Panel ID="pnlDelete" runat="server">
                        <asp:LinkButton
                            ID="btnDelete"
                            runat="server"
                            CommandName="DeleteSubmission"
                            CommandArgument='<%# Eval("SubmissionID") %>'
                            Text="Delete"
                            CssClass="btn btn-danger btn-sm"
                            OnClientClick="return confirm('Are you sure you want to delete this submission?');" />
                    </asp:Panel>
                </td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </tbody>
        </table>
        </FooterTemplate>
    </asp:Repeater>



    <!-- Message Label for notifications -->
    <asp:Label ID="lblMessage" runat="server" CssClass="alert" Visible="false"></asp:Label>
</asp:Content>
