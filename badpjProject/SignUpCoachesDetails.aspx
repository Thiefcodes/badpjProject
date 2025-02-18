<%@ Page Title="Sign Up Coaches Details" Language="C#" MasterPageFile="~/Site1loggedin.Master" AutoEventWireup="true" CodeBehind="SignUpCoachesDetails.aspx.cs" Inherits="badpjProject.SignUpCoachesDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container my-5">
        <div class="card shadow-sm">
            <div class="card-header text-center">
                <h2>Coach Details</h2>
            </div>
            <div class="card-body">
                <div class="row">
                    <!-- Video Section -->
                    <div class="ratio ratio-16x9 mb-3" id="videoContainer" runat="server">
                        <video id="videoPlayer" runat="server" class="w-100" controls>
                            <source id="videoSource" runat="server" type="video/mp4" />
                            Your browser does not support the video tag.
                        </video>
                    </div>

                    <!-- Details Section -->
                    <div class="col-md-6">
                        <dl class="row">
                            <dt class="col-sm-4">Name:</dt>
                            <dd class="col-sm-8">
                                <asp:Label ID="lbl_CoachName" runat="server" Text=""></asp:Label>
                            </dd>
                            <dt class="col-sm-4">Email:</dt>
                            <dd class="col-sm-8">
                                <asp:Label ID="lbl_CoachEmail" runat="server" Text=""></asp:Label>
                            </dd>
                            <dt class="col-sm-4">Phone:</dt>
                            <dd class="col-sm-8">
                                <asp:Label ID="lbl_CoachHp" runat="server" Text=""></asp:Label>
                            </dd>
                            <dt class="col-sm-4">Description:</dt>
                            <dd class="col-sm-8">
                                <asp:Label ID="lbl_CoachDesc" runat="server" Text=""></asp:Label>
                            </dd>
                            <dt class="col-sm-4">Qualification:</dt>
                            <dd class="col-sm-8">
                                <asp:Label ID="lbl_CoachQualification" runat="server" Text=""></asp:Label>

                                <!-- Certification Link (Only Visible if Exists) -->
                                <asp:Panel ID="pnlCertDoc" runat="server" visible="false" CssClass="ms-2">
                                    <asp:HyperLink ID="lnkCertDoc" runat="server" CssClass="text-primary" Target="_blank">View Certificate</asp:HyperLink>
                                </asp:Panel>
                            </dd>
                            <dt class="col-sm-4">Status:</dt>
                            <dd class="col-sm-8">
                                <asp:Label ID="lbl_CoachStatus" runat="server" Text=""></asp:Label>
                            </dd>
                        </dl>
                    </div>
                </div>
            </div>
            <div class="card-footer text-center">
                <asp:Button ID="Btn_Back" runat="server"
                    CssClass="btn btn-primary" Text="Back to Coaches"
                    OnClick="Btn_Back_Click" />
            </div>
        </div>
    </div>
</asp:Content>
