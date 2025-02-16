<%@ Page Title="Leaderboard" Language="C#" MasterPageFile="~/Site1loggedin.Master" AutoEventWireup="true" CodeBehind="Leaderboard.aspx.cs" Inherits="badpjProject.Leaderboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Submit Your Video Button -->
    <div class="text-center mt-4">
        <asp:Button ID="btnShowModal" runat="server" Text="Submit Your Video" CssClass="btn btn-primary" OnClientClick="showModal(); return false;" />
    </div>

    <!-- Modal Popup Form for Video Submission -->
    <div class="modal fade" id="videoModal" tabindex="-1" role="dialog" aria-labelledby="videoModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="videoModalLabel">Submit Your Lift Video</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <asp:Panel ID="pnlVideoForm" runat="server">
                        <div class="form-group">
                            <asp:FileUpload ID="fu_Video" runat="server" CssClass="form-control" accept="video/*" />
                            <asp:RequiredFieldValidator ID="rfv_Video" runat="server" ControlToValidate="fu_Video" ErrorMessage="Please upload a video file." Display="Dynamic" CssClass="text-danger" />
                        </div>
                        <div class="form-group">
                            <asp:TextBox ID="tb_Comment" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" placeholder="Add a comment (optional)..."></asp:TextBox>
                        </div>
                        <asp:Label ID="lblVideoStatus" runat="server" CssClass="text-danger" Visible="false"></asp:Label>
                    </asp:Panel>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="btnSubmitVideo" runat="server" Text="Submit Video" CssClass="btn btn-success" OnClick="btnSubmitVideo_Click" />
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>

    <!-- Leaderboard Display -->
    <div class="mt-5">
        <h2 class="text-center">Leaderboard</h2>
        <asp:Repeater ID="rptLeaderboard" runat="server">
            <HeaderTemplate>
                <table class="table table-striped">
                    <thead>
                        <tr>
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
                    <td><%# Eval("Rank") %></td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </tbody>
                </table>
            </FooterTemplate>
        </asp:Repeater>
    </div>

    <!-- Include jQuery and Bootstrap JS if not already included in your master page -->
    <script src="https://code.jquery.com/jquery-3.5.1.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>
    <script type="text/javascript">
        function showModal() {
            $('#videoModal').modal('show');
        }
    </script>
</asp:Content>
