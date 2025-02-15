<%@ Page Title="Our Coaches" Language="C#" MasterPageFile="~/Site1loggedin.Master" AutoEventWireup="true" CodeBehind="Coaches.aspx.cs" Inherits="badpjProject.Coaches" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container my-4">
        <h2 class="text-center mb-4">Our Coaches</h2>
        <div class="row">
            <asp:Repeater ID="rptCoaches" runat="server">
                <ItemTemplate>
                    <div class="col-md-4">
                        <div class="card coach-card">
                            <a href='CoachDetails.aspx?id=<%# Eval("Coach_ID") %>'>
                                <div class="image-wrapper position-relative" style="width: 150px; height: 150px; margin: 0 auto;">
                                    <img src='<%# !string.IsNullOrEmpty(Eval("Coach_ProfileImage").ToString()) ? ResolveUrl("~/Uploads/" + Eval("Coach_ProfileImage").ToString()) : ResolveUrl("~/Uploads/default-image.png") %>'
                                        alt="Profile Image" style="width: 150px; height: 150px; object-fit: cover; border-radius: 50%;" />
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
        .coach-card {
            margin-bottom: 20px;
            border: none;
            box-shadow: none;
        }

        .overlay {
            position: absolute;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            border-radius: 50%;
            background-color: rgba(0, 0, 0, 0.5);
            opacity: 0;
            transition: opacity 0.3s ease;
            display: flex;
            align-items: center;
            justify-content: center;
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
