<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TestDisplaySpreadsheets.aspx.vb" Inherits="TestDisplaySpreadsheets" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
    <script src="code.js"></script>
    <script src="https://d3js.org/d3.v4.min.js"></script>
    <link rel="stylesheet" href="StyleSheet.css" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:label ID="jsonLabel" runat="server" Text="" CssClass="hide"/>
            <table class="table"><tr><td><img src="graph.png" /></td><td></td><td>Shiny Ajax Graphs</td><td><img src="graph.png"</td></tr></table><br />
            <p>Download a spreadsheet and watch the graph update</p>
            <asp:Repeater ID="Repeater" runat="server">
                <itemtemplate>
                <asp:LinkButton Id="link" runat = "server" text = '<%#Eval("value")%>' onclick ="downloadFile" CommandArgument ='<%#Eval("value")%>'/><br />
                    </itemtemplate>
            </asp:Repeater>
        </div>
        <div id="graph"></div>
    </form>
</body>
</html>
