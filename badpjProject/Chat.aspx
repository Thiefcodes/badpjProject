<%@ Page Title="Chat" Language="C#" MasterPageFile="~/Site1loggedin.Master" AutoEventWireup="true" CodeBehind="Chat.aspx.cs" Inherits="badpjProject.Chat" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        function scrollToBottom() {
            var chatBox = document.getElementById("chatBox");
            chatBox.scrollTop = chatBox.scrollHeight;
        }
    </script>

    <div class="chat-container">
        <h2>Chat with <asp:Label ID="lblCoachName" runat="server" /></h2>

        <asp:UpdatePanel ID="UpdatePanelChat" runat="server">
    <ContentTemplate>
        <div class="chat-container">
            <asp:Repeater ID="rptMessages" runat="server">
                <ItemTemplate>
                    <div class='<%# Eval("Sender").ToString() == "Coach" ? "coach-message" : "user-message" %>'>
                        <%# Eval("Message") %> 
                        <span class="message-timestamp">(<%# Eval("Timestamp") %>)</span>
                    </div>
                </ItemTemplate>
            </asp:Repeater>

            <div class="message-input-container">
                <asp:TextBox ID="txtMessage" runat="server" CssClass="message-input"></asp:TextBox>
                <asp:Button ID="btnSend" runat="server" Text="Send" CssClass="btn btn-primary" OnClick="btnSend_Click" />
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>


    </div>

    <style>
    /* Chat Container */
    .chat-container {
        display: flex;
        flex-direction: column;
        max-width: 600px;
        margin: auto;
    }

    /* Message Styles */
    .user-message {
        align-self: flex-start;
        background-color: #f1f1f1;
        color: black;
        padding: 10px;
        border-radius: 10px;
        margin: 5px 0;
        max-width: 70%;
    }

    .coach-message {
        align-self: flex-end;
        background-color: #d4f8c6;
        color: black;
        padding: 10px;
        border-radius: 10px;
        margin: 5px 0;
        max-width: 70%;
        text-align: right;
    }

    /* Timestamp Style */
    .message-timestamp {
        font-size: 12px;
        color: gray;
        display: block;
    }
</style>

</asp:Content>
