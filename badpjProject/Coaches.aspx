<%@ Page Title="Our Coaches" Language="C#" MasterPageFile="~/Site1loggedin.Master" AutoEventWireup="true" CodeBehind="Coaches.aspx.cs" Inherits="badpjProject.Coaches" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container my-4">
        <h2 class="text-center mb-4">Our Coaches</h2>
        <div class="row">
            <asp:Repeater ID="rptCoaches" runat="server">
                <ItemTemplate>
                    <div class="col-md-4">
                        <div class="card coach-card">
                            <img src='<%# !string.IsNullOrEmpty(Eval("Coach_ProfileImage").ToString()) ? 
                                ResolveUrl("~/Uploads/" + Eval("Coach_ProfileImage").ToString()) : 
                                ResolveUrl("~/Uploads/default-image.png") %>' class="card-img-top" alt="Profile Image" />
                            <div class="card-body text-center">
                                <h5 class="card-title"><%# Eval("Coach_Name") %></h5>
                                <p class="card-text">
                                    <%# string.IsNullOrEmpty(Eval("Coach_AreaOfExpertise").ToString()) ? "&nbsp;" : Eval("Coach_AreaOfExpertise") %>
                                </p>
                                <a href='CoachDetails.aspx?id=<%# Eval("Coach_ID") %>' class="btn btn-primary">View Profile</a>
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
            border: none; /* Remove card border */
            box-shadow: none; /* Remove card shadow */
        }
        .coach-card img {
            width: 150px;
            height: 150px;
            object-fit: cover;
            border-radius: 50%; /* Circular image */
            display: block;
            margin: 0 auto; /* Center image horizontally */
        }
    </style>
</asp:Content>
