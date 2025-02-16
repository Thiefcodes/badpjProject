<%@ Page Title="Leaderboard" Language="C#" MasterPageFile="~/Site1loggedin.Master" AutoEventWireup="true" CodeBehind="Leaderboard.aspx.cs" Inherits="badpjProject.Leaderboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Submit Your Video Button -->
    <div class="text-center mt-4">
        <asp:Button ID="btnShowModal" runat="server" Text="Submit Your Video" CssClass="btn btn-primary" OnClientClick="showModal(); return false;" />
    </div>

    <!-- Modal Popup Form for Video Submission -->
    <div class="modal fade" id="videoModal" tabindex="-1" aria-labelledby="videoModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <!-- Modal Header -->
                <div class="modal-header">
                    <h5 class="modal-title" id="videoModalLabel">Submit Your Lift Video</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <!-- Modal Body -->
                <div class="modal-body">
                    <asp:Panel ID="pnlVideoForm" runat="server">
                        <div class="mb-3">
                            <label for="fu_Video" class="form-label">Video File</label>
                            <asp:FileUpload ID="fu_Video" runat="server" CssClass="form-control" accept="video/*" />
                            <asp:RequiredFieldValidator ID="rfv_Video" runat="server"
                                ControlToValidate="fu_Video"
                                ErrorMessage="Please upload a video file."
                                Display="Dynamic" CssClass="text-danger" />
                        </div>
                        <div class="mb-3">
                            <label for="tb_Comment" class="form-label">Comment (optional)</label>
                            <asp:TextBox ID="tb_Comment" runat="server" CssClass="form-control"
                                TextMode="MultiLine" Rows="3" placeholder="Add a comment..."></asp:TextBox>
                        </div>
                    </asp:Panel>

                    <!-- Literal or Label for messages -->
                    <asp:Literal ID="litVideoStatus" runat="server"></asp:Literal>
                </div>
                <!-- Modal Footer -->
                <div class="modal-footer">
                    <asp:Button ID="btnSubmitVideo" runat="server" Text="Submit Video"
                        CssClass="btn btn-success" OnClick="btnSubmitVideo_Click" />
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>

    <!-- Leaderboard Display -->
    <div class="mt-5">
        <h2 class="text-center">Leaderboard</h2>
        <asp:Repeater ID="rptLeaderboard" runat="server" OnItemDataBound="rptLeaderboard_ItemDataBound">
            <HeaderTemplate>
                <table class="table table-striped align-middle">
                    <thead>
                        <tr style="vertical-align: middle;">
                            <th>No.</th>
                            <th>Username</th>
                            <th>Total Points</th>
                            <th>Rank</th>
                        </tr>
                    </thead>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td><%# Container.ItemIndex + 1 %></td>
                    <td><%# Eval("Username") %></td>
                    <td><%# Eval("TotalPoints") %></td>
                    <td>
                        <asp:Image ID="imgRank" runat="server" CssClass="rank-icon" />
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </tbody>
                </table>
            </FooterTemplate>
        </asp:Repeater>
    </div>

    <!-- Global Message Label for Notifications -->
    <asp:Literal ID="litMessage" runat="server"></asp:Literal>

    <!-- Include jQuery and Bootstrap JS (if not already in your master page) -->
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.2.3/dist/css/bootstrap.min.css" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.2.0/dist/js/bootstrap.bundle.min.js"></script>
    <style>
        .rank-icon {
            width: 70px;
            height: auto;
        }
    </style>
    <script type="text/javascript">
        function showModal() {
            var videoModal = new bootstrap.Modal(document.getElementById('videoModal'));
            videoModal.show();
        }
    </script>
</asp:Content>
