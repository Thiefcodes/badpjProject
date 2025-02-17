<%@ Page Title="User Chats" Language="C#" MasterPageFile="~/Site1loggedin.Master" AutoEventWireup="true" CodeBehind="ChatList.aspx.cs" Inherits="badpjProject.ChatList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="chat-container">
        <h2>Chats with Users</h2>

        <asp:Repeater ID="rptUsers" runat="server">
            <ItemTemplate>
                <div class="chat-user">
                    <asp:HyperLink ID="lnkUserChat" runat="server"
                        NavigateUrl='<%# "Chat.aspx?userId=" + Eval("UserId") + "&coachId=" + Eval("CoachId") %>'>
                        <%# Eval("UserName") %>
                    </asp:HyperLink>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>

    <style>
     .chat-container {
    width: 80%;
    margin: auto;
    padding: 20px;
    border: 1px solid #ddd;
    border-radius: 10px;
    background: #f9f9f9;
}

.chat-user {
    padding: 10px;
    border-bottom: 1px solid #ddd;
    background-color: #ffffff;
    border-radius: 5px;
    transition: background-color 0.3s ease;
}

.chat-user:hover {
    background-color: #e9f5ff;
}

.chat-user a {
    display: block;
    padding: 10px;
    text-decoration: none;
    font-size: 1.2rem;
    color: #007bff;
    text-align: center;
    width: 100%;
}

.chat-user a:hover {
    text-decoration: none;
    color: #0056b3;
}

    </style>
</asp:Content>
