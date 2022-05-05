<%@ Page Title="" Language="C#" MasterPageFile="~/SystemAdmin/Main.Master" AutoEventWireup="true" CodeBehind="UserInfo.aspx.cs" Inherits="Questionnaire.SystemAdmin.UserInfo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <a id="linkQue" href="" runat="server">問卷</a>&nbsp
        <a id="linkQueContent" href="" runat="server">問題</a>&nbsp
        <a id="linkData" href="" runat="server">填寫資料</a>&nbsp
        <a id="linkStatistics" href="" runat="server">統計</a><br />
    </div>
    
    <table>
        <tr>
            <td>
                <h2><asp:Literal ID="ltlTitle" runat="server"></asp:Literal></h2>
            </td>
        </tr>
        <tr>
            <td>
                <h4><asp:Literal ID="ltlContent" runat="server"></asp:Literal></h4>
            </td>
        </tr>
        <tr>
            <th>姓名&nbsp</th>
            <td>
                <asp:TextBox ID="txtUserName" runat="server" Enabled="false"></asp:TextBox>
            </td>
            <td style="width:40px"></td>
            <th>手機&nbsp</th>
            <td>
                <asp:TextBox ID="txtUserPhone" runat="server" Enabled="false"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <th>Email&nbsp</th>
            <td>
                <asp:TextBox ID="txtUserMail" runat="server" Enabled="false"></asp:TextBox>
            </td>
            <td style="width:40px"></td>
            <th>年齡&nbsp</th>
            <td>
                <asp:TextBox ID="txtUserAge" runat="server" Enabled="false"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td></td><td></td><td></td>
            <th>填寫時間</th>
            <td>
                <asp:Label ID="labCreateDate" runat="server"></asp:Label>
            </td>
        </tr>
    </table><br />
    <asp:PlaceHolder ID="phList" runat="server">

    </asp:PlaceHolder>
</asp:Content>
