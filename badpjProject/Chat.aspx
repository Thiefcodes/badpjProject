<%@ Page Title="Chat" Language="C#" MasterPageFile="~/Site1loggedin.Master" AutoEventWireup="true" CodeBehind="Chat.aspx.cs" Inherits="badpjProject.Chat" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        function scrollToBottom() {
            setTimeout(function () {
                var chatBox = document.getElementById("chatBox");
                if (chatBox) {
                    chatBox.scrollTop = chatBox.scrollHeight;
                }
            }, 500); // Small delay ensures the messages load first
        }

        // ✅ Ensure chat scrolls to bottom after AJAX updates
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            scrollToBottom();
        });

        // ✅ Call scrollToBottom when the page loads
        window.onload = function () {
            scrollToBottom();
        };
    </script>


    <div class="chat-container">
        <h2>Chat with <asp:Label ID="lblCoachName" runat="server" /></h2>

        <asp:UpdatePanel ID="UpdatePanelChat" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="chat-box" id="chatBox">
                   <asp:Repeater ID="rptMessages" runat="server">
                        <ItemTemplate>
                            <div class="message-container">
                                <div class="chat-message">
                                    <div class="message-sender">
                                        <strong><%# Eval("SenderName") %></strong>
                                    </div>
                                    <div class="message-text">
                                        <%# Eval("Message") %>
                                    </div>
                                    <div class="message-timestamp">
                                        <%# Eval("Timestamp", "{0:dd/MM/yyyy HH:mm}") %>
                                    </div>
                                </div>
                            </div>
                        </ItemTemplate>
                   </asp:Repeater>
                </div>
                <asp:Panel ID="pnlUserResponse" runat="server"
    Visible='<%# Convert.ToInt32(Session["UserId"]) == Convert.ToInt32(Request.QueryString["userId"]) && Eval("Status") != DBNull.Value && Eval("Status").ToString() == "Pending" %>'>
    <strong>Offer: $<%# Eval("OfferAmount") %></strong>
    <asp:Button ID="btnAcceptOffer" runat="server" Text="Accept" CssClass="btn btn-primary"
        CommandArgument='<%# Eval("PaymentId") %>' OnClick="btnAcceptOffer_Click" />
    <asp:Button ID="btnRejectOffer" runat="server" Text="Reject" CssClass="btn btn-danger"
        CommandArgument='<%# Eval("PaymentId") %>' OnClick="btnRejectOffer_Click" />
</asp:Panel>
                <div class="message-input-container">
                    <asp:TextBox ID="txtMessage" runat="server" CssClass="message-input" placeholder="Type a message..."></asp:TextBox>
                    <asp:Button ID="btnSend" runat="server" Text="Send" CssClass="chat-button" OnClick="btnSend_Click" />

                    <%-- ✅ Move the Offer Section OUTSIDE the Repeater, so it only shows once for Coaches --%>
                    <% if (Convert.ToInt32(Session["UserId"]) != Convert.ToInt32(Request.QueryString["userId"])) { %>
                        <asp:Panel ID="pnlOfferSection" runat="server">
                            <asp:TextBox ID="txtOfferPriceField" runat="server" CssClass="offer-input" placeholder="Enter Offer Price"></asp:TextBox>
                            <asp:Button ID="btnSendOffer" runat="server" Text="Send Offer" CssClass="btn btn-warning" OnClick="btnSendOffer_Click" />
                        </asp:Panel>
                    <%-- ✅ Show Accept/Reject Offer buttons ONLY if User is logged in and an offer is pending --%>
                    <% } %>
                </div>

            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <style>
        .chat-container {
            width: 50%;
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
            display: flex;
            flex-direction: column;
        }

        .message-container {
            display: flex;
            flex-direction: column;
            align-items: flex-start;
            margin-bottom: 10px;
            width: 100%;
        }

        .chat-message {
            max-width: 70%;
            padding: 8px 12px;
            border-radius: 10px;
            word-wrap: break-word;
            font-size: 16px;
            background-color: #d4f8c6;
        }

        .message-sender {
            font-weight: bold;
            font-size: 14px;
            color: #555;
            margin-bottom: 5px;
        }

        .message-timestamp {
            font-size: 12px;
            color: gray;
            margin-top: 5px;
            text-align: right;
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
