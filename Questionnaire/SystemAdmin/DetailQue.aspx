<%@ Page Title="" Language="C#" MasterPageFile="~/SystemAdmin/Main.Master" AutoEventWireup="true" CodeBehind="DetailQue.aspx.cs" Inherits="Questionnaire.SystemAdmin.DetailQue" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <a id="linkQue" href="" runat="server">問卷</a>&nbsp
    <a id="linkQueContent" href="" runat="server">問題</a>&nbsp
    <a id="linkData" href="" runat="server">填寫資料</a>&nbsp
    <a id="linkStatistics" href="" runat="server">統計</a><br />
    <br />
    <table>
        <tr>
            <th>種類</th>
            <td>
                <asp:DropDownList ID="dpQue" runat="server" AutoPostBack="True" OnSelectedIndexChanged="dpQue_SelectedIndexChanged">
                    <asp:ListItem Value="0" >自訂問題</asp:ListItem>
                    <asp:ListItem Value="1">常用問題</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <th>問題</th>
            <td>
                <asp:TextBox ID="txtxQueTitle" runat="server"></asp:TextBox>
            </td>
            <td>
                <asp:DropDownList ID="dpQueType" runat="server">
                    <asp:ListItem Value="0">文字</asp:ListItem>
                    <asp:ListItem Value="1">單選方塊</asp:ListItem>
                    <asp:ListItem Value="2">複選方塊</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
                <asp:CheckBox ID="ckbNecessaryFirst" runat="server" />
                <asp:Label ID="labMsg" runat="server" Text="必填"></asp:Label>
            </td>
        </tr>
        <tr>
            <th>回答</th>
            <td>
                <asp:TextBox ID="txtAns" runat="server"></asp:TextBox>
            </td>
            <td>
                <asp:Label ID="labMag" runat="server" Text="(多個答案以 ; 分隔)"></asp:Label>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <th></th>
            <td></td>
            <td></td>
            <td>
                <asp:Button ID="Button1" runat="server" Text="加入修改" OnClick="btnAdd_Click" />
            </td>
        </tr>
    </table>
    <br />
    <asp:Button ID="btnDelete" runat="server" Text="刪除" OnClick="btnDelete_Click" />
    <table border="1">
        <tr>
            <th style="width:40px"></th><th style="width:40px">#</th><th style="width:150px">問題</th><th style="width:70px">種類</th><th style="width:60px">必填</th><th></th>
        </tr>
        <asp:Repeater ID="repDown" runat="server">
            <ItemTemplate>
                <asp:HiddenField ID="hf1" runat="server" Value='<%#Eval("QueID") %>' />
                <asp:HiddenField ID="hf2" runat="server" Value='<%#Eval("QuestionID") %>' />
                <tr>
                    <td align="center">
                        <asp:CheckBox ID="ckbDelet" runat="server" />
                    </td>
                    <td align="center">
                     <%--   <asp:Label ID="labQueNumber" runat="server" Text='<%#Eval("QuestionNumber") %>'></asp:Label>--%>
                        <asp:Label ID="labQueNumber" runat="server" ></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="labQueTitle" runat="server" Text='<%#Eval("QueTitle") %>'></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="labQueType" runat="server" Text='<%#Eval("Type").ToString() %>'></asp:Label>
                    </td>
                    <td align="center">
                        <asp:CheckBox ID="ckbNecessary" runat="server" Checked='<%#Eval("Necessary") %>' />
                    </td>
                    <td align="center">
                        <a href="DetailQue.aspx?ID=<%#Eval("QueID") %>&Ques=<%#Eval("QuestionID") %>">編輯</a>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
        <asp:Repeater ID="repCommonly" runat="server">
            <ItemTemplate>
                <asp:HiddenField ID="hf2" runat="server" Value='<%#Eval("QuestionID") %>' />
                <tr>
                    <td align="center">
                        <asp:CheckBox ID="ckbDelet" runat="server" />
                    </td>
                    <td align="center">
                        <asp:Label ID="labQueNumber" runat="server" ></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="labQueTitle" runat="server" Text='<%#Eval("QueTitle") %>'></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="labQueType" runat="server" Text='<%#Eval("Type").ToString() %>'></asp:Label>
                    </td>
                    <td align="center">
                        
                    </td>
                    <td align="center">
                        <a href="<%=Request.RawUrl %>&CommID=<%#Eval("QuestionID") %>">選擇</a>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
    <table>
        <tr>
            <td></td><td></td>
            <td>
                <asp:Button ID="btnCancel" runat="server" Text="取消" OnClick="btnCancel_Click" />
            </td>
            <td></td><td></td>
            <td>
                <asp:Button ID="btnSend" runat="server" Text="儲存修改內容" BackColor="MediumSeaGreen" OnClick="btnSend_Click" />
            </td>
            <td>
                <asp:Label ID="labWorring" runat="server"></asp:Label>
            </td>
        </tr>
    </table>
</asp:Content>
