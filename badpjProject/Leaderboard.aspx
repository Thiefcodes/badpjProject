<%@ Page Title="Leaderboard" Language="C#" MasterPageFile="~/Site1loggedin.Master" AutoEventWireup="true"
    CodeBehind="Leaderboard.aspx.cs" Inherits="badpjProject.Leaderboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Submit Video & Rank Info Button -->
    <div class="row justify-content-center mt-4 mb-3">
        <div class="col-auto">
            <asp:Button ID="btnShowModal" runat="server" Text="Submit Your Video" CssClass="btn btn-primary me-2"
                OnClientClick="showModal(); return false;" />
            <asp:Button ID="Button1" runat="server" Text="Rank Info" CssClass="btn btn-info"
                OnClientClick="showRankInfoModal(); return false;" />
        </div>
    </div>

    <!-- Video Submission Modal (Bootstrap 5) -->
    <div class="modal fade" id="videoModal" tabindex="-1" role="dialog" aria-labelledby="videoModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <!-- Modal Header -->
                <div class="modal-header">
                    <h5 class="modal-title" id="videoModalLabel">Submit Your Lift Video</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <!-- Modal Body -->
                <div class="modal-body">
                    <!-- Panel containing file upload & comment fields -->
                    <asp:Panel ID="pnlVideoForm" runat="server">
                        <div class="mb-3">
                            <label for="fu_Video" class="form-label">Video File</label>
                            <asp:FileUpload ID="fu_Video" runat="server" CssClass="form-control" accept="video/*" />
                            <asp:RequiredFieldValidator ID="rfv_Video" runat="server"
                                ControlToValidate="fu_Video" ErrorMessage="Please upload a video file."
                                Display="Dynamic" CssClass="text-danger" />
                        </div>
                        <div class="mb-3">
                            <label for="tb_Comment" class="form-label">Comment (optional)</label>
                            <asp:TextBox ID="tb_Comment" runat="server" CssClass="form-control"
                                TextMode="MultiLine" Rows="3" placeholder="Add a comment..."></asp:TextBox>
                        </div>
                    </asp:Panel>

                    <!-- Literal for any submission messages -->
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

    <!-- Rank Info Modal -->
    <div class="modal fade" id="rankInfoModal" tabindex="-1" aria-labelledby="rankInfoModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <!-- Modal Header -->
                <div class="modal-header">
                    <h5 class="modal-title" id="rankInfoModalLabel">Rank Information</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <!-- Modal Body -->
                <div class="modal-body">
                    <asp:Repeater ID="rptRankInfo" runat="server" OnItemDataBound="rptRankInfo_ItemDataBound">
                        <ItemTemplate>
                            <div class="row mb-3">
                                <div class="col-3 text-center">
                                    <!-- We'll set this icon's URL in ItemDataBound -->
                                    <asp:Image ID="imgRankIcon" runat="server" CssClass="rank-icon" />
                                </div>
                                <div class="col-9 d-flex align-items-center">
                                    <!-- We'll set the text here: rank name & points -->
                                    <asp:Label ID="lblRankText" runat="server"></asp:Label>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
                <!-- Modal Footer -->
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>


    <!-- Leaderboard Display -->
    <div class="mt-5 text-center">
        <h2 class="fw-bold">Leaderboard</h2>
        <asp:Repeater ID="rptLeaderboard" runat="server" OnItemDataBound="rptLeaderboard_ItemDataBound">
            <HeaderTemplate>
                <table class="table table-striped align-middle w-75 mx-auto">
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

    <!-- Global Message Literal if needed -->
    <asp:Literal ID="litMessage" runat="server"></asp:Literal>

    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.2.3/dist/css/bootstrap.min.css" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.2.3/dist/js/bootstrap.bundle.min.js"></script>

    <style>
        .rank-icon {
            width: 70px;
            height: auto;
            filter: drop-shadow(0 0 5px rgba(255, 255, 255, 0.6));
        }
    </style>


    <script>
        // Show the Rank Info modal
        function showRankInfoModal() {
            var rankModal = new bootstrap.Modal(document.getElementById('rankInfoModal'));
            rankModal.show();
        }

        // Show the Submit Video modal
        function showModal() {
            var videoModal = new bootstrap.Modal(document.getElementById('videoModal'));
            videoModal.show();
        }
    </script>
</asp:Content>
