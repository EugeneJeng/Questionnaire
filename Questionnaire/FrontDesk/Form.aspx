<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Form.aspx.cs" Inherits="Questionnaire.FrontDesk.Form" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <script src="../Scripts/jquery.min.js"></script>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <h1>前台</h1>
        <a href="../StartPage.aspx">首頁</a>
        <asp:HiddenField ID="hfID" runat="server" />
        <table>
            <tr>
                <th>姓名</th>
                <td>
                    <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th>手機</th>
                <td>
                    <asp:TextBox ID="txtMobile" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th>Email</th>
                <td>
                    <asp:TextBox ID="txtMail" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th>年齡</th>
                <td>
                    <asp:TextBox ID="txtAge" runat="server"></asp:TextBox>
                </td>
            </tr>
        </table><br />
        <asp:PlaceHolder ID="phQuestion" runat="server">


        </asp:PlaceHolder>
        <asp:Button ID="btnCancel" runat="server" Text="取消" OnClick="btnCancel_Click"  />
        <input type="button" runat="server" id="btnSubmit" value="送出" />
    </form>
    <script src="../Scripts/jquery.min.js"></script>
    <script>
        $(document).ready(function () {
            $("input[id*=btnSubmit]").css("background-color", "yellow")
        });

        $(document).ready(function () {
            $("input[id*=btnSubmit]").click(function () {
                var answer = "";
                var questionList = $("input[id*=Question]").get();
                console.log("QuestionList : " + questionList);
                for (var item of questionList) {
                    if (item.type == "radio" && item.checked) {
                        answer += `${item.id};${item.value}` + " "
                    }
                    if (item.type == "checkbox" && item.checked) {
                        answer += `${item.id};${item.value}` + " "
                    }
                    if(item.type == "text"){
                        answer += `${item.id};${item.value}` + " ";
                    }
                }
                var postData = {
                    "Answer": answer,
                    "Name": $("#txtName").val(),
                    "Mobile": $("#txtMobile").val(),
                    "Email": $("#txtMail").val(),
                    "Age": $("#txtAge").val()
                };

                $.ajax({
                    url: "/API/AnsHandler.ashx?ID=" + $("#hfID").val(),
                    method: "POST",
                    data: postData,
                    success: function (txtMsg) {
                        console.log(txtMsg);
                        if (txtMsg == "success") {
                            window.location = "ConfirmPage.aspx?ID=" + $("#hfID").val();
                        }
                        if (txtMsg == "noAnswer") {
                            alert("請作答");
                        }
                    },
                    error: function (msg) {
                        console.log(msg);
                        alert("通訊失敗，請聯絡管理員。");
                    }
                });
            });
        });
    </script>
</body>
</html>
