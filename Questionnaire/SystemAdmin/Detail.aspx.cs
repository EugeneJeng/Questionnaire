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
    public partial class Detail : System.Web.UI.Page
    {
        QuestionnaireManager _qmgr = new QuestionnaireManager();
        private bool _IsCreateMode = true;
        protected void Page_Load(object sender, EventArgs e)
        {
            Uri url = Request.UrlReferrer;
            string txtQueID = Request.QueryString["ID"];
            if(Guid.TryParse(txtQueID, out Guid queID))
            {
                _IsCreateMode = false;
                HttpContext.Current.Session["QueID"] = queID;
            }
            LinkSet(queID, _IsCreateMode);
            if (!_IsCreateMode&&!IsPostBack)
            {
                ShowData(queID);
            }
            //Response.Redirect(url.ToString());
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.RawUrl);
        }
        protected void LinkSet(Guid queID, bool IsCreateMode)
        {
            if (queID == null || IsCreateMode)
            {
                //linkQue.HRef = "Detail.aspx";
                //linkQueContent.HRef = "DetailQue.aspx";
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
        protected void ShowData(Guid queID)
        {
            QuestionnaireModel model = _qmgr.GetQuestionnaireByQueID(queID);
            txtQueName.Text = model.Title;
            txtQueContent.Text = model.QueContent;
            txtStartDate.Text = model.StartTime.ToString("yyyy/MM/dd");
            txtEndDate.Text = model.EndTime.ToString("yyyy/MM/dd");
            if (model.State == StateType.啟用中)
            {
                ckbEnable.Checked = true;
            }
            else
            {
                ckbEnable.Checked = false;
            }
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            int datetimeError = 0;
            string errMsg = string.Empty;
            QuestionnaireModel model = new QuestionnaireModel();
            if (_IsCreateMode)
            {
                model.QueID = Guid.NewGuid();
            }
            else
            {
                Guid queID = (Guid)HttpContext.Current.Session["QueID"];
                model.QueID = queID;
            }
            model.Title = this.txtQueName.Text.Trim();
            if (string.IsNullOrWhiteSpace(model.Title))
            {
                errMsg += "問題名稱為必填\\n";
            }
            model.QueContent = this.txtQueContent.Text.Trim();
            if (string.IsNullOrWhiteSpace(txtQueContent.Text.Trim()))
            {
                errMsg += "描述內容為必填\\n";
            }
            DateTime startDate, endDate;
            if (!DateTime.TryParse(this.txtStartDate.Text.Trim(), out startDate))
            {
                errMsg += "開始時間格不不正確 (yyyy/MM/dd)\\n";
                datetimeError++;
            }
            else
            {
                model.StartTime = startDate;
            }
            if (!DateTime.TryParse(this.txtEndDate.Text.Trim(), out endDate))
            {
                errMsg += "結束時間格不不正確 (yyyy/MM/dd)\\n";
                datetimeError++;
            }
            else
            {
                model.EndTime = endDate;
            }
            if (datetimeError == 0)
            {
                if (endDate < DateTime.Now)
                {
                    errMsg += "結束時間必須在今天以後\\n";
                }
                TimeSpan ts = endDate - startDate;
                TimeSpan minDays = TimeSpan.FromDays(1);
                if (ts<minDays)
                {
                    errMsg += "投票時間必須大於一天\\n";
                }
            }
            bool IsEnable = ckbEnable.Checked;
            if (IsEnable)
            {
                model.State = StateType.啟用中;
            }
            else
            {
                model.State = StateType.已關閉;
            }
            if (!string.IsNullOrWhiteSpace(errMsg))
            {
                ShowErrorMsg(errMsg);
            }
            else
            {
                if (_IsCreateMode)
                {
                    _qmgr.CreateQuestionnaire(model);
                    ShowMsg();
                    Response.Redirect("List.aspx");
                    //HttpContext.Current.Session["tempGuid"] = model.QueID;
                }
                else
                {
                    _qmgr.UpdateQuestionnaire(model);
                    ShowMsg();
                }
                //ShowMsg();
                //Response.Redirect("List.aspx");
            }
        }
        protected void ShowErrorMsg(string errMsg)
        {
            if (!string.IsNullOrWhiteSpace(errMsg))
            {
                string errorMsg = "alert('" + errMsg + "');";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), string.Empty, errorMsg, true);
            }                       
        }
        protected void ShowMsg()
        {
            if (_IsCreateMode)
            {
                string errorMsg = "alert('新增成功');";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), string.Empty, errorMsg, true);
                HttpContext.Current.Session["temp"] = "新增成功";
                //HiddenField hd1 = Form.FindControl("hd1") as HiddenField;
                //hd1.Value = "OK";
                //Guid temp = (Guid)HttpContext.Current.Session["tempGuid"];
                //HiddenField hd2 = Form.FindControl("hd2") as HiddenField;
                //hd2.Value = temp.ToString();
            }
            else
            {
                string errorMsg = "alert('修改成功');";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), string.Empty, errorMsg, true);
            }
        }
    }
}