<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Questionnaire.FrontDesk.List" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link rel="stylesheet" href="CSS/bootstrap.css" />
    <style>
        .searchKey{
            
        }
        td,th{
            padding-left:10px;
            padding-right:10px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="frontTitle">
            <h1>前台</h1>
            <a href="../StartPage.aspx">首頁</a>
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
            <div>
                <table border="1">
                    <tr><th>#</th><th>問卷</th><th>狀態</th><th>開始時間</th><th>結束時間</th><th>觀看統計</th></tr>
                        <asp:Repeater ID="repList" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <asp:Label ID="labNumber" runat="server" Text='<%#Eval("Number") %>'></asp:Label>
                                    </td>
                                    <td>
                                        <a href="/FrontDesk/Form.aspx?ID=<%#Eval("QueID") %>">
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
                                        <a href="Statistics.aspx?ID=<%#Eval("QueID") %>">前往</a>
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
        </div>
    </form>
</body>
</html>
