using Questionnaire.Managers;
using Questionnaire.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Questionnaire.SystemAdmin
{
    public partial class DetailStatistics : System.Web.UI.Page
    {
        private QuestionManager _qmgr = new QuestionManager();
        private AnswerManager _amgr = new AnswerManager();
        private QuestionnaireManager _qnmgr = new QuestionnaireManager();
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpContext.Current.Session["QuestionList"] = null;
            string txtQueID = Request.QueryString["ID"];
            if (string.IsNullOrWhiteSpace(txtQueID) || !Guid.TryParse(txtQueID, out Guid queID))
            {
                Response.Redirect("List.aspx");
            }
            else
            {
                HttpContext.Current.Session["QueID"] = queID;
                LinkSet(queID);
                QuestionnaireModel que = _qnmgr.GetQuestionnaireByQueID(queID);
                List<QuestionModel> queList = _qmgr.GetQuestionList(queID);
                Label labTitle = new Label();
                labTitle.Text = $"{que.Title}<br>";
                labTitle.Font.Size = 24;
                phStatistics.Controls.Add(labTitle);
                Label labContent = new Label();
                labContent.Text = $"{que.QueContent}<br/><br/>";
                labContent.Font.Size = 16;
                phStatistics.Controls.Add(labContent);
                if (queList == null || queList.Count == 0)
                {
                    Label labNoData = new Label();
                    labNoData.Text = "尚無數據";
                    phStatistics.Controls.Add(labNoData);
                    return;
                }
                int queNumber = 1;                
                foreach (QuestionModel model in queList)
                {
                    Label label = new Label();
                    if (model.Necessary)
                    {
                        label.Text = $"{queNumber}. {model.QueTitle} (必填)<br/>";
                    }
                    else
                    {
                        label.Text = $"{queNumber}. {model.QueTitle}<br/>";
                    }                    
                    phStatistics.Controls.Add(label);
                    if (model.Type == QueType.文字)
                    {
                        Label labText = new Label();
                        labText.Text = "- <br/><br>";
                        phStatistics.Controls.Add(labText);
                        queNumber++;
                        continue;
                    }
                    List<Temp> list = AnsStatistics(model);
                    double totalCount = 0;
                    foreach (Temp t in list)
                    {
                        totalCount += t.ansCount;
                    }
                    foreach (Temp t in list)
                    {
                        Label label1 = new Label();
                        double percentage = t.ansCount / totalCount * 100;
                        if(percentage is double.NaN|| percentage < 0)
                        {
                            percentage = 0;
                        }
                        string txtPercent = percentage.ToString("0");
                        label1.Text = $"{t.selectName}{t.type} {txtPercent}% ({t.ansCount})<br/>";
                        phStatistics.Controls.Add(label1);
                    }
                    Label label2 = new Label();
                    label2.Text = "<br/>";
                    phStatistics.Controls.Add(label2);
                    queNumber++;
                }
            }
        }
        protected void LinkSet(Guid queID)
        {
            linkQue.HRef = $"Detail.aspx?ID={queID}";
            linkQueContent.HRef = $"DetailQue.aspx?ID={queID}";
            linkData.HRef = $"DetailAns.aspx?ID={queID}";
            //linkStatistics.HRef = $"DetailStatistics.aspx?ID={queID}";
        }
        protected List<Temp> AnsStatistics(QuestionModel model)
        {
            List<AnswerModel> ansList = _amgr.GetAnswerList(model.QueID);
            List<Temp> list = new List<Temp>();
            bool IsRadio = true;
            switch (model.Type)
            {
                case QueType.單選方塊:
                    IsRadio = true;
                    break;

                case QueType.複選方塊:
                    IsRadio = false;
                    break;
            }
            string selecttion = model.QueAns;
            string[] selArray = selecttion.Split(';');
            foreach (string sel in selArray)
            {
                Temp t = new Temp();
                t.selectName = sel;
                int count = 0;
                foreach (AnswerModel ansModel in ansList)
                {
                    if (IsRadio)
                    {
                        t.type = "單選";
                    }
                    else
                    {
                        t.type = "複選";
                    }
                    string ans = ansModel.Answer;
                    string[] ansArray = ans.Split(';');
                    for (int i = 0; i < ansArray.Length; i++)
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
            public string type { get; set; }
        }
    }
}