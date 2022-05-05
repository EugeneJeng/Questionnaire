<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConfirmPage.aspx.cs" Inherits="Questionnaire.FrontDesk.ConfirmPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <script src="../Scripts/jquery.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:HiddenField ID="hfID" runat="server" />
            <h2><asp:Literal ID="ltlTitle" runat="server"></asp:Literal></h2>
            <h4><asp:Literal ID="ltlContent" runat="server"></asp:Literal></h4>
            <table>
                <tr>
                    <td>姓名</td>
                    <td>
                        <asp:TextBox ID="txtName" runat="server" Enabled="false"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>手機</td>
                    <td>
                        <asp:TextBox ID="txtMobile" runat="server" TextMode="Phone" Enabled="false"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>Email</td>
                    <td>
                        <asp:TextBox ID="txtMail" runat="server" TextMode="Email" Enabled="false"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>年齡</td>
                    <td>
                        <asp:TextBox ID="txtAge" runat="server" TextMode="Number" Enabled="false"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </div>
        <asp:PlaceHolder ID="phAnsList" runat="server">

        </asp:PlaceHolder>
        <br />
        <asp:Button ID="btnCancel" runat="server" Text="修改" OnClick="btnCancel_Click" />
        <asp:Button ID="btnSubmit" runat="server" Text="確定送出" OnClick="btnSubmit_Click" />
    </form>
</body>
</html>
