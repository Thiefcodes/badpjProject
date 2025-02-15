<%@ Page Title="Our Coaches" Language="C#" MasterPageFile="~/Site1loggedin.Master" AutoEventWireup="true" CodeBehind="Coaches.aspx.cs" Inherits="badpjProject.Coaches" %>

<asp:Content ID="HeaderContent1" ContentPlaceHolderID="HeaderContent" runat="server">
    <div class="coach-header">
        <asp:Image ID="imgHeader" runat="server" CssClass="header-img" AlternateText="Coach Header" />
        <div class="header-overlay">
            <h1 class="header-text">Our Coaches</h1>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Header Section with Background Image and Overlayed Text -->
    

    <!-- Main Container for Coach Cards -->
    <div class="container my-5">
        <div class="row gx-0">
            <asp:Repeater ID="rptCoaches" runat="server">
                <ItemTemplate>
                    <div class="col-sm-6 col-md-4 col-lg-3">
                        <div class="card coach-card">
                            <a href='CoachDetails.aspx?id=<%# Eval("Coach_ID") %>' class="d-block">
                                <div class="image-wrapper position-relative" style="width: 170px; height: 170px; margin: 0 auto;">
                                    <img src='<%# !string.IsNullOrEmpty(Eval("Coach_ProfileImage").ToString()) ? ResolveUrl("~/Uploads/" + Eval("Coach_ProfileImage").ToString()) : ResolveUrl("~/Uploads/default-image.png") %>'
                                        alt="Profile Image" style="width: 170px; height: 170px; object-fit: cover; border-radius: 50%;" />
                                    <div class="overlay">
                                        <span class="overlay-text">View Profile</span>
                                    </div>
                                </div>
                            </a>
                            <div class="card-body text-center">
                                <h5 class="card-title"><%# Eval("Coach_Name") %></h5>
                                <p class="card-text"><%# Eval("Coach_AreaOfExpertise") %></p>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
    <style>
        html, body {
            margin: 0;
            padding: 0;
        }

        /* Header Styles */
        .coach-header {
            position: relative;
            width: 100%;
            height: 400px;
        }

        .header-img {
            width: 100%;
            height: 100%;
            object-fit: cover;
            display: block;
        }

        .header-overlay {
            position: absolute;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background: rgba(0,0,0,0.5);
            display: flex;
            justify-content: center;
            align-items: center;
        }

        .header-text {
            color: #fff;
            font-size: 48px;
            font-weight: bold;
            text-align: center;
        }

        /* Existing styles for cards */
        .coach-card {
            margin-bottom: 10px;
            border: none;
            box-shadow: none;
        }

        .image-wrapper {
            position: relative;
            width: 170px;
            height: 170px;
            margin: 0 auto;
        }

        .image-wrapper img {
            width: 170px;
            height: 170px;
            object-fit: cover;
            border-radius: 50%;
            display: block;
        }

        .image-wrapper .overlay {
            position: absolute;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            border-radius: 50%;
            background-color: rgba(0,0,0,0.5);
            opacity: 0;
            transition: opacity 0.3s ease;
            display: flex;
            justify-content: center;
            align-items: center;
        }

        .image-wrapper:hover .overlay {
            opacity: 1;
        }

        .overlay-text {
            color: #fff;
            font-size: 16px;
            font-weight: bold;
        }
    </style>
</asp:Content>