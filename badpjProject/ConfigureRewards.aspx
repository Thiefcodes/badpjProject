<%@ Page Title="Configure Rewards" Language="C#" MasterPageFile="~/Site1loggedin.Master" AutoEventWireup="true" CodeBehind="ConfigureRewards.aspx.cs" Inherits="badpjProject.ConfigureRewards" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-5">
        <h2 class="text-center">Configure Rewards</h2>
        <div class="card shadow-sm p-4 mt-4" style="max-width: 800px; margin: 0 auto;">
             <div class="text-center mt-4">
     <asp:Button ID="BackButton" runat="server" Text="Back to Staff Page" CssClass="btn btn-secondary" PostBackUrl="~/staffpage.aspx" />
 </div>
            <!-- Add Reward Form -->
            <h4>Add New Reward</h4>
            <asp:TextBox ID="RewardNameTextBox" runat="server" CssClass="form-control mb-2" Placeholder="Reward Name"></asp:TextBox>
            <asp:FileUpload ID="RewardImageUpload" runat="server" CssClass="form-control mb-2" />
            <asp:TextBox ID="StreakHoursTextBox" runat="server" CssClass="form-control mb-2" Placeholder="Number of Streak Hours"></asp:TextBox>
            <asp:Button ID="AddRewardButton" runat="server" Text="Add Reward" CssClass="btn btn-primary w-100 mb-3" OnClick="AddRewardButton_Click" />

            <!-- Rewards List -->
            <h4>Rewards List</h4>
            <asp:Repeater ID="RewardsRepeater" runat="server" OnItemCommand="RewardsRepeater_ItemCommand">
                <ItemTemplate>
                    <div class="reward-item mb-3">
                   <img src='<%# ResolveUrl(Eval("RewardImage").ToString()) %>' alt="Reward Image" 
     class="img-thumbnail mb-2" style="max-width: 150px;" />
     <h5><%# Eval("RewardName") %></h5>
                        <p>Streak Hours: <%# Eval("StreakHours") %></p>
                        <asp:Button ID="EditRewardButton" runat="server" Text="Edit" CssClass="btn btn-warning btn-sm" CommandArgument='<%# Eval("RewardId") %>' CommandName="EditReward" />
                        <asp:Button ID="DeleteRewardButton" runat="server" Text="Delete" CssClass="btn btn-danger btn-sm" CommandArgument='<%# Eval("RewardId") %>' CommandName="DeleteReward" />
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>

        <!-- Edit Reward Section -->
        <div id="EditRewardSection" runat="server" class="card shadow-sm p-4 mt-4" style="max-width: 800px; margin: 0 auto; display: none;">
            <h4>Edit Reward</h4>
            <asp:HiddenField ID="EditRewardId" runat="server" />
            <asp:TextBox ID="EditRewardNameTextBox" runat="server" CssClass="form-control mb-2" Placeholder="Reward Name"></asp:TextBox>
            <asp:FileUpload ID="EditRewardImageUpload" runat="server" CssClass="form-control mb-2" />
            <asp:TextBox ID="EditStreakHoursTextBox" runat="server" CssClass="form-control mb-2" Placeholder="Number of Streak Hours"></asp:TextBox>
            <asp:Button ID="UpdateRewardButton" runat="server" Text="Update Reward" CssClass="btn btn-warning w-100" OnClick="UpdateRewardButton_Click" />
            <asp:Button ID="CancelEditButton" runat="server" Text="Cancel" CssClass="btn btn-secondary w-100 mt-2" OnClientClick="document.getElementById('EditRewardSection').style.display='none'; return false;" />
        </div>
    </div>
</asp:Content>

