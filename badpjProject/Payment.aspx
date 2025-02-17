<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Payment.aspx.cs" Inherits="badpjProject.Payment" %>

<!DOCTYPE html>
<html>
<head>
    <title>Payment Page</title>
    <script type="text/javascript">
        function closePaymentWindow() {
            alert("Payment Submitted!");
            window.close();
        }
    </script>
    <style>
        body {
            font-family: Arial, sans-serif;
            text-align: center;
            padding: 20px;
        }
        .payment-container {
            width: 100%;
            max-width: 400px;
            margin: auto;
            padding: 20px;
            border: 1px solid #ddd;
            border-radius: 10px;
            background-color: #f9f9f9;
        }
        .payment-input {
            width: 90%;
            padding: 10px;
            margin: 5px 0;
            border: 1px solid #ddd;
            border-radius: 5px;
        }
        .payment-button {
            padding: 10px 15px;
            border: none;
            background-color: #28a745;
            color: white;
            cursor: pointer;
            border-radius: 5px;
            margin-top: 15px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="payment-container">
            <h2>Payment Processing</h2>
            <p>Enter your payment details below.</p>

            <asp:Panel ID="pnlPaymentForm" runat="server">
                <asp:TextBox ID="txtCardNumber" runat="server" CssClass="payment-input" Placeholder="Card Number"></asp:TextBox><br>
                <asp:TextBox ID="txtCardholderName" runat="server" CssClass="payment-input" Placeholder="Cardholder Name"></asp:TextBox><br>
                <asp:TextBox ID="txtExpiryDate" runat="server" CssClass="payment-input" Placeholder="Expiry Date (MM/YY)"></asp:TextBox><br>
                <asp:TextBox ID="txtCVV" runat="server" CssClass="payment-input" Placeholder="CVV"></asp:TextBox><br>

                <asp:Button ID="btnSubmitPayment" runat="server" Text="Submit Payment" CssClass="payment-button" OnClientClick="closePaymentWindow(); return false;" />
            </asp:Panel>
        </div>
    </form>
</body>
</html>
