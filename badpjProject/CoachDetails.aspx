<%@ Page Title="Coach Details" Language="C#" MasterPageFile="~/Site1loggedin.Master" AutoEventWireup="true" CodeBehind="CoachDetails.aspx.cs" Inherits="badpjProject.CoachDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
  <div class="container my-5">
    <div class="row justify-content-center">
      <div class="col-md-8">
        <div class="card shadow">
          <div class="card-body text-center">
            <asp:Image ID="imgProfile" runat="server" CssClass="rounded-circle mb-3" 
                       style="width:200px; height:200px; object-fit:cover;" />
            <h1 id="lblName" runat="server" class="card-title"></h1>
            <p id="lblExpertise" runat="server" class="lead"></p>
            <p id="lblQualification" runat="server" class="mb-3"></p>
            <hr />
            <p id="lblDescription" runat="server" class="text-left"></p>
          </div>
          <div class="card-footer text-center">
            <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="btn btn-secondary mr-2" OnClick="btnBack_Click" />
            <asp:Button ID="btnChat" runat="server" Text="Chat" CssClass="btn btn-primary" OnClick="btnChat_Click" />
          </div>
        </div>
      </div>
    </div>
  </div>
</asp:Content>
