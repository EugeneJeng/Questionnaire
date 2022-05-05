<%@ Page Title="" Language="C#" MasterPageFile="~/SystemAdmin/Main.Master" AutoEventWireup="true" CodeBehind="DetailStatistics.aspx.cs" Inherits="Questionnaire.SystemAdmin.DetailStatistics" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">         
        <a id="linkQue" href="" runat="server">問卷</a>&nbsp
        <a id="linkQueContent" href="" runat="server">問題</a>&nbsp
        <a id="linkData" href="" runat="server">填寫資料</a>&nbsp
        <a id="linkStatistics" href="" runat="server">統計</a><br />
        <h2><asp:Label ID="labTitle" runat="server"></asp:Label></h2>
        <h3><asp:Label ID="labContent" runat="server"></asp:Label></h3>
        <asp:PlaceHolder ID="phStatistics" runat="server">

        </asp:PlaceHolder>
    </div>
</asp:Content>
