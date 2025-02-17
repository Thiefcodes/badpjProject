<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Chatbot.aspx.cs" Inherits="badpjProject.Chatbot" Async="true" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="chat-container">
        <h2>AI Chatbot</h2>
        <div class="chat-box" id="chatBox">
            <asp:Literal ID="litChatbotResponse" runat="server"></asp:Literal>
        </div>
        
        <div class="chat-input-container">
            <asp:TextBox ID="txtChatbotInput" runat="server" CssClass="chat-input" placeholder="Ask me anything..." />
            <asp:Button ID="btnSend" runat="server" Text="Send" CssClass="chat-button" OnClick="btnChatbotSend_Click" />
        </div>
    </div>

    <style>
        .chat-container {
            width: 100%;
            margin: auto;
            padding: 20px;
            border-radius: 10px;
            box-shadow: 0px 0px 10px rgba(0, 0, 0, 0.1);
            background-color: white;
            text-align: center;
        }

        .chat-box {
            width: 100%;
            height: 300px;
            overflow-y: scroll;
            padding: 10px;
            border: 1px solid #ddd;
            background-color: #f9f9f9;
            text-align: left;
        }

        .chat-input-container {
            margin-top: 10px;
            display: flex;
            justify-content: space-between;
        }

        .chat-input {
            width: 80%;
            padding: 10px;
            border: 1px solid #ddd;
            border-radius: 5px;
        }

        .chat-button {
            padding: 10px 15px;
            border: none;
            background-color: #007bff;
            color: white;
            cursor: pointer;
            border-radius: 5px;
        }
    </style>
</asp:Content>
