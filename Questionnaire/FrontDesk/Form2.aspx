<%@ Page Title="" Language="C#" MasterPageFile="~/FrontDesk/Main.Master" AutoEventWireup="true" CodeBehind="Form2.aspx.cs" Inherits="Questionnaire.FrontDesk.Form2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">  
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

        <%--<asp:Button ID="btnSubmit" runat="server" Text="送出" OnClick="btnSubmit_Click"  />--%>
    <script>
        $("input[id=btnSubmit]").click(function () {
            alert("Hello");
        }
        $(document).ready(function () {
            alert("Hello");
            $("input[id=btnSubmit]").click(function () {
                var answer = "";
                var questionList = $("input[id*=問題]").get();
                console.log(questionList);
                for (var item of questionList) {
                    if (item.type == "radio" && item.checked) {
                        answer += item.id + " ";
                    }
                    if (item.type == "checkbox" && item.checked) {
                        answer += item.id + " ";
                    }
                    if (item.type == "text") {
                        answer += `${item.id}_${item.value}` + " ";
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
                    url: "/API/AnswerHandler.ashx?ID=" + $("#hfID").val(),
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
        })
    </script>
</asp:Content>
