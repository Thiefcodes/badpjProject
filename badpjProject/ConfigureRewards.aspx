<%@ Page Title="Configure Rewards" Language="C#" MasterPageFile="~/Site1loggedin.Master" AutoEventWireup="true" CodeBehind="ConfigureRewards.aspx.cs" Inherits="badpjProject.ConfigureRewards" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-5">
        <h2 class="text-center">Configure Rewards</h2>
        <div class="card shadow-sm p-4 mt-4" style="max-width: 800px; margin: 0 auto;">
            <!-- Add Reward Form -->
            <h4>Add New Reward</h4>
            <asp:TextBox ID="RewardNameTextBox" runat="server" CssClass="form-control mb-2" Placeholder="Reward Name"></asp:TextBox>
            <asp:FileUpload ID="RewardImageUpload" runat="server" CssClass="form-control mb-2" />
            <asp:TextBox ID="StreakHoursTextBox" runat="server" CssClass="form-control mb-2" Placeholder="Number of Streak Hours"></asp:TextBox>
            <asp:Button ID="AddRewardButton" runat="server" Text="Add Reward" CssClass="btn btn-primary w-100 mb-3" OnClick="AddRewardButton_Click" />

            <!-- Rewards List -->
            <h4>Rewards List</h4>
            <asp:Repeater ID="RewardsRepeater" runat="server">
                <ItemTemplate>
                    <div class="reward-item mb-3">
                        <img src='<%# Eval("RewardImage") %>' alt="Reward Image" class="img-thumbnail mb-2" style="max-width: 150px;" />
                        <h5><%# Eval("RewardName") %></h5>
                        <p>Streak Hours: <%# Eval("StreakHours") %></p>
                        <asp:Button ID="EditRewardButton" runat="server" Text="Edit" CssClass="btn btn-warning btn-sm" CommandArgument='<%# Eval("RewardId") %>' OnClick="EditRewardButton_Click" />
                        <asp:Button ID="DeleteRewardButton" runat="server" Text="Delete" CssClass="btn btn-danger btn-sm" CommandArgument='<%# Eval("RewardId") %>' OnClick="DeleteRewardButton_Click" />
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
</asp:Content>
