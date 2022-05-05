using Questionnaire.Managers;
using Questionnaire.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;

namespace Questionnaire.FrontDesk
{
    public partial class Statistics : System.Web.UI.Page
    {
        private QuestionManager _qmgr = new QuestionManager();
        private AnswerManager _amgr = new AnswerManager();
        private QuestionnaireManager _qnmgr = new QuestionnaireManager();
        private readonly Color[] _colors = new Color[] { Color.Peru, Color.PowderBlue, Color.RosyBrown,
            Color.Salmon, Color.Sienna, Color.SlateBlue };
        protected void Page_Load(object sender, EventArgs e)
        {
            string txtQueID = Request.QueryString["ID"];
            if (string.IsNullOrWhiteSpace(txtQueID) || !Guid.TryParse(txtQueID, out Guid queID))
            {
                Response.Redirect("List.aspx");
            }
            else
            {
                QuestionnaireModel que = _qnmgr.GetQuestionnaireByQueID(queID);
                List<QuestionModel> queList = _qmgr.GetQuestionList(queID);
                if (queList == null)
                {
                    Response.Redirect("List.aspx");
                }
                int queNumber = 1;
                Label labTitle = new Label();
                labTitle.Text = $"{que.Title}<br>";
                labTitle.Font.Size = 40;
                phStatistics.Controls.Add(labTitle);
                Label labContent = new Label();
                labContent.Text = $"{que.QueContent}<br/><br/>";
                labContent.Font.Size = 26;
                phStatistics.Controls.Add(labContent);

                List<StatisicsData> staData = new List<StatisicsData>();
                foreach (QuestionModel model in queList)
                {
                    if(model.Type == QueType.文字)
                    {
                        continue;
                    }
                    Label label = new Label();
                    label.Text = $"{queNumber}. {model.QueTitle}<br/>";
                    label.Font.Size = 18;
                    phStatistics.Controls.Add(label);
                    List<Temp> list = AnsStatistics(model);
                    double totalCount = 0;
                    //List<double> count = new List<double>();
                    //List<string> selection = new List<string>();
                    foreach (Temp t in list)
                    {
                        totalCount += t.ansCount;
                        //count.Add(t.ansCount);
                        //selection.Add(t.selectName);
                    }
                    StatisicsData sd = new StatisicsData();
                    sd.Question = model.QueTitle;
                    List<string> selNames = new List<string>();
                    List<int> selPercentages = new List<int>();
                    List<int> selCounts = new List<int>();
                    foreach (Temp t in list)
                    {
                        Label label1 = new Label();
                        double percentage = t.ansCount / totalCount * 100;
                        if(percentage is double.NaN || percentage < 0)
                        {
                            percentage = 0;
                        }
                        string txtPercent = percentage.ToString("0");
                        label1.Text = $"{t.selectName} : {txtPercent} %  ,  總比數 : {t.ansCount}<br/>";
                        phStatistics.Controls.Add(label1);
                        selNames.Add(t.selectName);
                        selPercentages.Add((int)percentage);
                        selCounts.Add((int)t.ansCount);
                    }
                    sd.Selections = selNames;
                    sd.Percentage = selPercentages;
                    sd.Counts = selCounts;
                    staData.Add(sd);
                    Label label2 = new Label();
                    label2.Text = "<br/>";
                    phStatistics.Controls.Add(label2);
                    //Chart chart = DrawData(selection, count);
                    //phStatistics.Controls.Add(chart);
                    queNumber++;
                }
                DrawData(staData);
            }            
        }
        protected void DrawData(List<StatisicsData> staData)
        {
            int count = 0;
            foreach(StatisicsData data in staData)
            {
                count++;
                List<string> xValues = data.Selections;
                List<int> yValues = data.Counts;

                //  ChartAreas, Series, Legends 基本設定
                Chart chart1 = new Chart();
                Series series = new Series();
                chart1.ChartAreas.Add("ChartArea1");
                chart1.Legends.Add("Legend1");
                chart1.Series.Add("Series1");

                //  設定 Chart
                chart1.Width = 300;
                chart1.Height = 200;
                Title title = new Title();
                title.Text = data.Question;
                title.Alignment = ContentAlignment.MiddleCenter;
                title.Font = new System.Drawing.Font("Trebuchet MS", 14F, FontStyle.Bold);
                chart1.Titles.Add(title);

                //  設定 ChartArea
                chart1.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;
                chart1.ChartAreas[0].AxisX.Interval = 1;

                //  設定 被景色
                chart1.Legends["Legend1"].BackColor = Color.FromArgb(235, 235, 235);

                //  斜線背景
                chart1.Legends["Legend1"].BackHatchStyle = ChartHatchStyle.DarkDownwardDiagonal;
                chart1.Legends["Legend1"].BorderWidth = 1;
                chart1.Legends["Legend1"].BorderColor = Color.FromArgb(200, 200, 200);

                //  設定 Series1
                chart1.Series["Series1"].ChartType = SeriesChartType.Pie;
                chart1.Series["Series1"].Points.DataBindXY(xValues, yValues);
                chart1.Series["Series1"].LegendText = "#VALX: [#PERCENT{P1}]";  //  X軸 + 百分比

                //  字體設定
                chart1.Series["Series1"].Font = new System.Drawing.Font("Trebuchet MS", 10);
                chart1.Series["Series1"].Points.FindMaxByValue().LabelForeColor = Color.Red;
                chart1.Series["Series1"].BorderColor = Color.FromArgb(255, 101, 101, 101);
                chart1.Series["Series1"]["PieLabelStyle"] = "Inside";
                chart1.Series["Series1"]["PieDrawingStyle"] = "Default";

                //  亂數產生區塊顏色
                Random rnd = new Random();
                foreach(DataPoint point in chart1.Series["Series1"].Points)
                {
                    //  pie 顏色
                    point.Color = Color.FromArgb(150, rnd.Next(0, 255), rnd.Next(0, 255), rnd.Next(0, 255));
                }
                string folder = "~/temp";
                string folderPath = System.Web.Hosting.HostingEnvironment.MapPath(folder);
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                chart1.SaveImage($"{folderPath}/{count}.Png", ChartImageFormat.Png);
            }
            for(int i = 1; i <= count; i++)
            {
                System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
                img.ImageUrl = $"/temp/{i}.Png";
                phStatistics.Controls.Add(img);
            }
        }
        protected List<Temp> AnsStatistics(QuestionModel model)
        {
            List<AnswerModel> ansList = _amgr.GetAnswerList(model.QueID);
            List<Temp> list = new List<Temp>();
            string selecttion = model.QueAns;
            string[] selArray = selecttion.Split(';');
            foreach(string sel in selArray)
            {
                Temp t = new Temp();
                t.selectName = sel;
                int count = 0;
                foreach(AnswerModel ansModel in ansList)
                {
                    string ans = ansModel.Answer;
                    string[] ansArray = ans.Split(';');
                    for(int i = 0; i < ansArray.Length; i++)
                    {
                        if (sel == ansArray[i])
                        {
                            count++;
                        }
                    }
                }
                t.ansCount = count;
                list.Add(t);
            }
            return list;
        }
        public class Temp
        {
            public string selectName { get; set; }
            public double ansCount { get; set; }
        }
    }
}