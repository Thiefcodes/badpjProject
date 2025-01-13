<%@ Page Title="" Language="C#" MasterPageFile="~/Site1loggedin.Master" AutoEventWireup="true" CodeBehind="BecomeACoach.aspx.cs" Inherits="badpjProject.BecomeACoach" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <h1>Apply to Become a Coach</h1><br />
        <p>
            NAME&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:TextBox ID="tb_Name" runat="server" Width="500px"></asp:TextBox>
        </p>
        <p>
            EMAIL&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp; <asp:TextBox ID="tb_Email" runat="server" Width="500px"></asp:TextBox>
        </p>
        <p>
            HP&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; <asp:TextBox ID="tb_Hp" runat="server" Width="500px"></asp:TextBox>
        </p>
        <p>
            ABOUT YOU&nbsp;&nbsp;
            <asp:TextBox ID="tb_AboutYou" runat="server" Height="103px" Width="500px"></asp:TextBox>
            &nbsp;&nbsp;
        </p>
        <asp:Panel ID="Panel1" runat="server" GroupingText="QUALIFICATION">
            <asp:DropDownList ID="ddl_Qualification" runat="server">
                <asp:ListItem Value="Placeholder Rank"></asp:ListItem>
                <asp:ListItem Value="Certified-PT"></asp:ListItem>
            </asp:DropDownList>
            <br />
            <br />
            <h5>Video showcasing your expertise&nbsp;&nbsp;</h5>
            <asp:FileUpload ID="fu_Coach" runat="server" placeholder="Showcase your expertise" Width="500px" />
        </asp:Panel>
        <br />
        <asp:Button ID="btn_Submit" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="btn_Submit_Click" />
    </div>
</asp:Content>
