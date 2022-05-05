<%@ Page Title="" Language="C#" MasterPageFile="~/SystemAdmin/Main.Master" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Questionnaire.SystemAdmin.List" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="searchDiv">
                <table style="text-align:left">
                    <tr>
                        <th>
                            問卷標題
                        </th>
                        <td>
                            <asp:TextBox ID="txtSearch" runat="server" Width="215px" ></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            開始 &nbsp/&nbsp 結束
                        </th>
                        <td>
                            <asp:TextBox ID="txtStartTime" runat="server" Width="100px"></asp:TextBox>&nbsp
                            <asp:TextBox ID="txtEndTime" runat="server" Width="100px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Button ID="btnSearch" runat="server" Text="搜尋" OnClick="btnSearch_Click" /><br />
                            <asp:Label ID="labMsg" runat="server" ForeColor="Red" ></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
    <asp:Button ID="btnDelete" runat="server" Text="刪除" OnClick="btnDelete_Click" />&nbsp
    <asp:Button ID="btnCreate" runat="server" Text="新增" OnClick="btnCreate_Click" /><br />
    <div>
        <table border="1">
            <tr><th></th><th>#</th><th>問卷</th><th>狀態</th><th>開始時間</th><th>結束時間</th><th>觀看統計</th></tr>
                <asp:Repeater ID="repList" runat="server">
                    <ItemTemplate>
                        <asp:HiddenField ID="hfID" runat="server" Value='<%#Eval("QueID") %>' />
                        <tr>
                            <td>
                                <asp:CheckBox ID="ckbDel" runat="server" />
                            </td>
                            <td align="center" style="width:40px">
                                <asp:Label ID="labNumber" runat="server" Text='<%#Eval("Number") %>'></asp:Label>
                            </td>
                            <td>
                                <a href="Detail.aspx?ID=<%#Eval("QueID") %>">
                                    <asp:Label ID="labTitle" runat="server" Text='<%#Eval("Title") %>'></asp:Label>
                                </a>
                            </td>
                            <td>
                                <asp:Label ID="labState" runat="server" Text='<%#Eval("State") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labStart" runat="server" Text='<%#((DateTime)Eval("StartTime")).ToString("yyyy/MM/dd") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labEnd" runat="server" Text='<%#((DateTime)Eval("EndTime")).ToString("yyyy/MM/dd") %>'></asp:Label>
                            </td>
                            <td width="80px">
                                <a href="DetailStatistics.aspx?ID=<%#Eval("QueID") %>">前往</a>
                            </td>
                        </tr>                                
                    </ItemTemplate>
                </asp:Repeater>
            </tr>
        </table>
    </div>
    <div>
        <a href="" runat="server" id="linkFirstPage"><<</a>
        <a href="" runat="server" id="linkPrevPage"><</a>
        <a href="" runat="server" id="linkPage1">1</a>
        <a href="" runat="server" id="linkPage2">2</a>
        <a href="" runat="server" id="linkPage3">3</a>
        <a href="" runat="server" id="linkPage4">4</a>
        <a href="" runat="server" id="linkPage5">5</a>
        <a href="" runat="server" id="linkNextPage">></a>
        <a href="" runat="server" id="linkButtomPage">>></a>
    </div>
</asp:Content>
