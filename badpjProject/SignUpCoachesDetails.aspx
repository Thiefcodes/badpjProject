<%@ Page Title="Sign Up Coaches Details" Language="C#" MasterPageFile="~/Site1loggedin.Master" AutoEventWireup="true" CodeBehind="SignUpCoachesDetails.aspx.cs" Inherits="badpjProject.SignUpCoachesDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>COACH DETAILS</h2>
    <div class="details-container">
        <!-- Video Section -->
        <div class="video-container" ID="videoContainer" runat="server">
            <video ID="videoPlayer" runat="server" controls>
                <source ID="videoSource" runat="server" type="video/mp4" />
                Your browser does not support the video tag.
            </video>
        </div>
        <!-- Details Section -->
        <table class="details-table">
            <tr>
                <td><b>Name:</b></td>
                <td><asp:Label ID="lbl_CoachName" runat="server" Text="[lbl_CoachName]"></asp:Label></td>
            </tr>
            <tr>
                <td><b>Email:</b></td>
                <td><asp:Label ID="lbl_CoachEmail" runat="server" Text="[lbl_CoachEmail]"></asp:Label></td>
            </tr>
            <tr>
                <td><b>Phone:</b></td>
                <td><asp:Label ID="lbl_CoachHp" runat="server" Text="[lbl_CoachHp]"></asp:Label></td>
            </tr>
            <tr>
                <td><b>Description:</b></td>
                <td><asp:Label ID="lbl_CoachDesc" runat="server" Text="[lbl_CoachDesc]"></asp:Label></td>
            </tr>
            <tr>
                <td><b>Qualification:</b></td>
                <td><asp:Label ID="lbl_CoachQualification" runat="server" Text="[lbl_CoachQualification]"></asp:Label></td>
            </tr>
            <tr>
                <td><b>Status:</b></td>
                <td><asp:Label ID="lbl_CoachStatus" runat="server" Text="[lbl_CoachStatus]"></asp:Label></td>
            </tr>
        </table>

        <!-- Back Button -->
        <asp:Button ID="Btn_Back" runat="server" CssClass="btn-back" Text="Back to Coaches" OnClick="Btn_Back_Click" />
    </div>
</asp:Content>