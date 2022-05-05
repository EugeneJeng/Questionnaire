<%@ Page Title="" Language="C#" MasterPageFile="~/SystemAdmin/Main.Master" AutoEventWireup="true" CodeBehind="DetailAns.aspx.cs" Inherits="Questionnaire.SystemAdmin.DetailAns" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <a id="linkQue" href="" runat="server">問卷</a>&nbsp
    <a id="linkQueContent" href="" runat="server">問題</a>&nbsp
    <a id="linkData" href="" runat="server">填寫資料</a>&nbsp
    <a id="linkStatistics" href="" runat="server">統計</a><br /><br />
    <asp:Button ID="btnExport" runat="server" Text="匯出" OnClick="btnExport_Click" />
    <table border="1">
        <tr>
            <th style="width:40px">#</th><th>姓名</th><th>填寫時間</th><th>觀看細節</th>
        </tr>
        <asp:Repeater ID="rptList" runat="server">
            <ItemTemplate>
                <tr>
                    <td align="center" style="width:40px">
                        <asp:Label ID="lblNumber" runat="server"></asp:Label>
                    </td>
                    <td align="center">
                        <asp:Literal ID="ltlName" runat="server" Text='<%#Eval("UserName") %>'></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal ID="ltlTime" runat="server" Text='<%#((DateTime)Eval("CreateDate")).ToString("yyyy/MM/dd HH:mm") %>'></asp:Literal>
                    </td>
                    <td align="center">
                        <a href="UserInfo.aspx?ID=<%#Eval("QueID") %>&UserID=<%#Eval("UserID") %>">前往</a>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
        </table>
</asp:Content>
