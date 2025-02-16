<%@ Page Title="My Coach Profile" Language="C#" MasterPageFile="~/Site1loggedin.Master" AutoEventWireup="true" CodeBehind="CoachProfile.aspx.cs" Inherits="badpjProject.CoachProfile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <!-- Header Section for Coach Profile -->
    <div class="profile-header">
        <div class="container my-5">
            <h1 class="text-center">My Coach Profile</h1>
        </div>
    </div>

    <!-- Profile Card -->
    <div class="container my-5">
        <div class="card shadow-sm border-0 rounded">
            <div class="row g-0">
                <!-- Left Column: Profile Image -->
                <div class="col-md-4 text-center p-4">
                    <asp:Image ID="imgProfile" runat="server" CssClass="img-fluid rounded-circle"
                        Style="width: 200px; height: 200px; object-fit: cover;" />
                </div>
                <!-- Right Column: Profile Details -->
                <div class="col-md-8">
                    <div class="card-body">
                        <h2 id="lblName" runat="server" class="card-title"></h2>
                        <p class="card-text"><strong>Email:</strong> <span id="lblEmail" runat="server"></span></p>
                        <p class="card-text"><strong>Phone:</strong> <span id="lblHp" runat="server"></span></p>
                        <p class="card-text"><strong>Description:</strong> <span id="lblDesc" runat="server"></span></p>
                        <p class="card-text"><strong>Qualification:</strong> <span id="lblQualification" runat="server"></span></p>
                        <p class="card-text"><strong>Area of Expertise:</strong> <span id="lblExpertise" runat="server"></span></p>
                        <div class="mt-4 text-center">
                            <asp:Button ID="btnEdit" runat="server" Text="Edit Profile" CssClass="btn btn-primary" OnClick="btnEdit_Click" />
                            <asp:Button ID="btnChatUsers" runat="server" Text="Chat with Users" CssClass="btn btn-success ml-3" OnClick="btnChatUsers_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <style>
        /* Overall Card Styling */
        .card {
            border-radius: 10px;
            background: #fff;
        }

        .card-body {
            padding: 2rem;
        }

        h2.card-title {
            font-size: 2rem;
            margin-bottom: 1rem;
        }

        .card-text {
            font-size: 1rem;
            margin-bottom: 0.5rem;
        }
    </style>
</asp:Content>
