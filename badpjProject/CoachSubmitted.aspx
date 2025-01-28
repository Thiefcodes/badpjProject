<%@ Page Title="Coach Submitted" Language="C#" MasterPageFile="~/Site1loggedin.Master" AutoEventWireup="true" CodeBehind="CoachSubmitted.aspx.cs" Inherits="badpjProject.CoachSubmitted" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="d-flex justify-content-center align-items-center" style="min-height: 100vh;">
        <div class="card shadow-lg p-4 text-center" style="width: 400px; border-radius: 10px;">
            <h3 class="text-success mb-4">Submission Successful!</h3>
            <p>Thank you for submitting your application to become a coach. Your application is under review.</p>
            
            <!-- Centered Button -->
            <div class="text-center mt-3">
                <asp:Button 
                    ID="btn_BackToHome" 
                    runat="server" 
                    Text="Back to Home" 
                    CssClass="btn btn-primary" 
                    OnClick="btn_BackToHome_Click" />
            </div>
        </div>
    </div>
</asp:Content>
