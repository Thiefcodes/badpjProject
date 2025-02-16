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
<asp:UpdatePanel ID="UpdatePanelChat" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div class="chat-container">
<asp:Repeater ID="rptMessages" runat="server">
    <ItemTemplate>
        <div class="chat-message <%# Convert.ToInt32(Session["UserId"]) == Convert.ToInt32(Eval("UserId")) ? "my-message" : "other-message" %>">
            <div class="message-sender">
                <strong>
                    <%# Convert.ToInt32(Session["UserId"]) == Convert.ToInt32(Eval("UserId")) ? Session["Name"] : Eval("SenderName") %>
                </strong> 
            </div>
            <div class="message-text"><%# Eval("Message") %></div>
            <div class="message-timestamp"><%# Eval("Timestamp", "{0:dd/MM/yyyy HH:mm}") %></div>
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
    .message-sender {
    font-size: 14px;
    font-weight: bold;
    margin-bottom: 3px;
    color: #555;
}

    /* Chat Container */
    .chat-container {
        display: flex;
        flex-direction: column;
        max-width: 600px;
        margin: auto;
    }

    /* Message Wrapper */
    .message-container {
        display: flex;
        max-width: 70%;
        padding: 8px 12px;
        border-radius: 10px;
        margin: 5px 0;
        word-wrap: break-word;
    }

    /* User Messages (Align Left) */
    .user-message {
        align-self: flex-start;
        background-color: #f1f1f1;
        color: black;
    }

    /* Coach Messages (Align Right) */
    .coach-message {
        align-self: flex-end;
        background-color: #d4f8c6;
        color: black;
        text-align: right;
    }

    /* Message Text */
    .message-text {
        font-size: 16px;
    }

    /* Timestamp */
    .message-timestamp {
        font-size: 12px;
        color: gray;
        margin-top: 5px;
        text-align: right;
    }
</style>


</asp:Content>
