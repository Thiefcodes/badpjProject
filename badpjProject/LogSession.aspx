<%@ Page Title="Log Workout Session" Language="C#" MasterPageFile="~/Site1loggedin.Master" AutoEventWireup="true" CodeBehind="LogSession.aspx.cs" Inherits="badpjProject.LogSession" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
  <div class="container mt-5">
    <h2>Log Your Workout Session</h2>
    <div class="card p-4">
      <asp:Label ID="lblWorkoutDuration" runat="server" Text="Workout Duration (in hours):" CssClass="form-label"></asp:Label>
      <asp:TextBox ID="txtWorkoutDuration" runat="server" CssClass="form-control" Placeholder="Enter workout hours"></asp:TextBox>
      <asp:Button ID="btnLogWorkout" runat="server" Text="Log Session" CssClass="btn btn-primary mt-3" OnClick="btnLogWorkout_Click" />
      <asp:Label ID="lblMessage" runat="server" CssClass="mt-3 text-success"></asp:Label>
    </div>
  </div>
    <!-- Testing Section: Simulate a future workout session -->
<div class="mt-4">
    <asp:Label ID="lblSimulate" runat="server" Text="Simulate session X days in future:"></asp:Label>
    <asp:TextBox ID="txtDaysToSimulate" runat="server" CssClass="form-control mb-2" Placeholder="Enter number of days"></asp:TextBox>
    <asp:Button ID="btnSimulateForward" runat="server" Text="Simulate Session" CssClass="btn btn-info" OnClick="btnSimulateForward_Click" />
</div>


</asp:Content>
