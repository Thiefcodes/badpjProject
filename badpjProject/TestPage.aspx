<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestPage.aspx.cs" Inherits="badpjProject.TestPage" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Test Page</title>
    <script src="https://code.jquery.com/jquery-3.7.1.min.js"></script>
    <script>
        function callTestMethod() {
            $.ajax({
                type: "POST",
                url: "TestPage.aspx/TestMethod",
                data: "{}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    alert(response.d);
                },
                error: function (err) {
                    console.error(err);
                    alert("Error calling TestMethod");
                }
            });
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <button type="button" onclick="callTestMethod();">Call Test Method</button>
    </form>
</body>
</html>
