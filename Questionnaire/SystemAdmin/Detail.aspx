<%@ Page Title="" Language="C#" MasterPageFile="~/SystemAdmin/Main.Master" AutoEventWireup="true" CodeBehind="Detail.aspx.cs" Inherits="Questionnaire.SystemAdmin.Detail" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <a id="linkQue" href="" runat="server">問卷</a>&nbsp
    <a id="linkQueContent" href="" runat="server">問題</a>&nbsp
    <a id="linkData" href="" runat="server">填寫資料</a>&nbsp
    <a id="linkStatistics" href="" runat="server">統計</a><br />
    <div margin="10px">
        <asp:HiddenField ID="hd1" runat="server" Value="NotOK" />
        <asp:HiddenField ID="hd2" runat="server" />
        <table>
            <tr>
                <th>
                    <asp:Label ID="labQueName" runat="server" Text="問卷名稱"></asp:Label>
                </th>
                <td>
                    <asp:TextBox ID="txtQueName" runat="server" Width="150px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th>
                    <asp:Label ID="labQueContent" runat="server" Text="描述內容"></asp:Label>
                </th>
                <td>
                     <asp:TextBox ID="txtQueContent" runat="server" Width="200px" TextMode="MultiLine" Height="60px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th>
                    <asp:Label ID="labStartDate" runat="server" Text="開始時間"></asp:Label>
                </th>
                <td>
                    <asp:TextBox ID="txtStartDate" runat="server" Width="150px" ToolTip="yyyy/MM/dd"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th>
                    <asp:Label ID="labEndDate" runat="server" Text="結束時間"></asp:Label>
                </th>
                <td>
                    <asp:TextBox ID="txtEndDate" runat="server" Width="150px" ToolTip="yyyy/MM/dd"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:CheckBox ID="ckbEnable" runat="server" Checked="true" />
                    <asp:Label ID="labEnable" runat="server" Text="已啟用"></asp:Label>
                </td>                
            </tr>
            <tr>
                <td></td>
                <td>
                    <asp:Button ID="btnCancel" runat="server" Text="取消" OnClick="btnCancel_Click" />
                </td>
                <td>
                    <asp:Button ID="btnSend" runat="server" Text="送出" BackColor="MediumSeaGreen" OnClick="btnSend_Click" OnClientClick="btnOK()" />
                </td>
            </tr>
        </table>
    </div>
    <script>
        //function btnOK() {
        //    var test = $("id*=hd1").val();
        //    var id = $("id*=hd2").val();
        //    if (test == "OK") {
        //        Response.redirect("Detail.aspx?" + id);
        //    }
        //}
    </script>
</asp:Content>
