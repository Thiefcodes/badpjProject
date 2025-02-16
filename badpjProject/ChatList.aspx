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
        }

        .chat-user a {
            text-decoration: none;
            font-size: 1.2rem;
            color: #007bff;
        }

        .chat-user a:hover {
            text-decoration: underline;
        }
    </style>
</asp:Content>
