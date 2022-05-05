using Questionnaire.Managers;
using Questionnaire.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Questionnaire.SystemAdmin
{
    public partial class DetailAns : System.Web.UI.Page
    {
        private static bool _IsCreateMode = true;
        private static UserManager _umr = new UserManager();
        private static AnswerManager _amgr = new AnswerManager();
        private static QuestionManager _qmgr = new QuestionManager();
        protected void Page_Load(object sender, EventArgs e)
        {
            Uri url = Request.UrlReferrer;
            string txtQueID = Request.QueryString["ID"];
            if (Guid.TryParse(txtQueID, out Guid queID))
            {
                _IsCreateMode = false;
                HttpContext.Current.Session["QueID"] = queID;
            }
            LinkSet(queID, _IsCreateMode);
            List<UserModel> list = _umr.GetUserList(queID);
            HttpContext.Current.Session["UserList"] = list;
            SetRep(list);

        }
        protected void SetRep(List<UserModel> list)
        {
            rptList.DataSource = list;
            rptList.DataBind();
            int count = list.Count();
            foreach (RepeaterItem item in this.rptList.Items)
            {
                Label lblNumber = item.FindControl("lblNumber") as Label;
                lblNumber.Text = count.ToString();
                count--;
            }
        }
        protected void LinkSet(Guid queID, bool IsCreateMode)
        {
            if (queID == null || IsCreateMode)
            {
                linkQue.HRef = "Detail.aspx";
                linkQueContent.HRef = "DetailQue.aspx";
                //linkData.HRef = "DetailAns.aspx";
                //linkStatistics.HRef = "DetailStatistics.aspx";
            }
            else
            {
                linkQue.HRef = $"Detail.aspx?ID={queID}";
                linkQueContent.HRef = $"DetailQue.aspx?ID={queID}";
                linkData.HRef = $"DetailAns.aspx?ID={queID}";
                linkStatistics.HRef = $"DetailStatistics.aspx?ID={queID}";
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            List<UserModel> list = (List<UserModel>)HttpContext.Current.Session["UserList"];
            string fileName = $"{DateTime.Now.ToString("yyyyMMdd_HH:mm")}.csv";
            StringBuilder sb = new StringBuilder();
            foreach(UserModel model in list)
            {
                sb.Append($"姓名,{model.UserName}, ,手機,{String.Format("{0:0000000}", model.UserPhone)}\n");
                sb.Append($"Email,{model.UserMail}, ,年齡,{model.UserAge}\n");
                sb.Append($" , , , 填寫時間,{model.CreateDate.ToString("yyyy/MM/dd HH:mm")}\n");
                sb.Append("\n");
                Guid queID = (Guid)HttpContext.Current.Session["QueID"];
                List<QuestionModel> questionList = _qmgr.GetQuestionList(queID);
                int count = 1;
                foreach(QuestionModel question in questionList)
                {
                    sb.Append($" {count}. ,{question.QueTitle}\n");
                    List<AnswerModel> ansList = _amgr.GetAnswerList(question.QuestionID, model.UserID);
                    foreach(AnswerModel ans in ansList)
                    {
                        sb.Append($" ,{ans.Answer}\n");
                    }
                    count++;
                    sb.Append("\n");
                }
                sb.Append("\n\n");
            }
            Response.AddHeader("Content-disposition", "attachment; filename=\"" + fileName + "" + "\"");
            //  設定回傳媒體型別(MIME)
            Response.ContentType = "text/csv";
            //  設定主體內容編碼
            Response.ContentEncoding = Encoding.UTF8;
            //  建立StreamWriter，取得Response的OutputStream並設定編碼為UTF8
            StreamWriter sw = new StreamWriter(Response.OutputStream, Encoding.UTF8);
            //寫入資料
            sw.Write(sb.ToString());
            //  關閉StreamWriter
            sw.Close();
            //  釋放StreamWriter資源
            sw.Dispose();
            //  送出Response
            Response.End();
        }
    }
}