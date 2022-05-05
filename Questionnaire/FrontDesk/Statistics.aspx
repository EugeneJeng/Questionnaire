<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Statistics.aspx.cs" Inherits="Questionnaire.FrontDesk.Statistics" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../Content/bootstrap.css" rel="stylesheet" />
    <script language="javascript" type="text/javascript" src="../Scripts/bootstrap.js"></script>
    <!--[if IE]><script language="javascript" type="text/javascript" src="excanvas.js"></script><![endif]-->
    <script language="javascript" type="text/javascript" src="../Scripts/jquery.min.js"></script>
    <script language="javascript" type="text/javascript" src="../Scripts/jqPlot/jquery.jqplot.min.js"></script>
    <link href="../Scripts/jqPlot/jquery.jqplot.min.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="row">
                <div class="col-md-4">
                    <a href="../StartPage.aspx">首頁</a>
                </div>
                <div class="col-md-8">
                    <h2><asp:Label ID="labTitle" runat="server"></asp:Label></h2>
                    <h3><asp:Label ID="labContent" runat="server"></asp:Label></h3>
                    <asp:PlaceHolder ID="phStatistics" runat="server">
 <%--                       <div id ="chartQ1">

                        </div>--%>
                    </asp:PlaceHolder>
                </div>
            </div>
        </div>
    </form>
    <script>
        //$(document).ready(function () {
        //    var item = $('id=line1').get();
        //    var line1 = item.value;
        //    var plot1 = $.jqplot('id=chartQ1', [line1], {
        //        title: 'Q1',
        //        seriesDefaults: { renderer: $.jqplot.PieRenderer, rendererOptions: { sliceMargin: 8 } },
        //        legend: { show: true }
        //    });
        //})
        
        /*var line1 = [['網管(12票-7.19%)', 12], ['採購(37票-22.16%)', 37], ['網咖主(17票-10.18%)', 17], ['系統人員(17票-10.18%)', 17], ['其他(0票-%)', 0]];*/

    </script>
</body>
</html>
