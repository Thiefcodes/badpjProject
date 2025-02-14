<%@ Page Title="Coaches" Language="C#" MasterPageFile="~/Site1loggedin.Master" AutoEventWireup="true" CodeBehind="Coaches.aspx.cs" Inherits="badpjProject.Coaches" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <h2 class="mt-4">Our Coaches</h2>

        <!-- Button for Approved Coaches to Create/Edit Profile -->
        <asp:Panel ID="pnlCreateProfile" runat="server" Visible="false">
            <a href="CreateCoachProfile.aspx" class="btn btn-primary mb-3">Create/Edit Your Profile</a>
        </asp:Panel>

        <div class="coaches-container">
            <asp:Repeater ID="rptCoaches" runat="server">
                <ItemTemplate>
                    <div class="coach-card">
                        <img src='<%# Eval("ProfilePicture") %>' alt='<%# Eval("Name") %>' class="coach-photo" />
                        <h3><%# Eval("Name") %></h3>
                        <p><%# Eval("Specialties") %></p>
                        <a href="CoachProfile.aspx?CoachID=<%# Eval("CoachID") %>" class="btn">View Profile</a>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>

        <asp:Literal ID="litNoCoaches" runat="server" Text="" />
    </div>
</asp:Content>
