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
    public partial class NewQue : System.Web.UI.Page
    {
        QuestionnaireManager _qmgr = new QuestionnaireManager();
        protected void Page_Load(object sender, EventArgs e)
        {
            LinkSet();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.RawUrl);
        }
        protected void LinkSet()
        {
            //linkQue.HRef = "NewQue.aspx";
            linkQueContent.HRef = "DetailQue.aspx";
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            string errMsg = string.Empty;
            QuestionnaireModel model = new QuestionnaireModel();
            model.QueID = Guid.NewGuid();
            model.Title = txtQueName.Text.Trim();
            if (string.IsNullOrWhiteSpace(model.Title)){
                errMsg += "問題名稱為必填<br/>";
            }
            model.QueContent = txtQueContent.Text.Trim();
            if (string.IsNullOrWhiteSpace(txtQueContent.Text.Trim()))
            {
                errMsg += "描述內容為必填<br/>";
            }
            DateTime startDate, endDate;
            if(!DateTime.TryParse(txtStartDate.Text.Trim(),out startDate)){
                errMsg += "開始時間格不不正確\"yyyy/MM/dd\"<br>";
            }
            if(!DateTime.TryParse(txtEndDate.Text.Trim(),out endDate))
            {
                errMsg += "結束時間格不不正確\"yyyy/MM/dd\"<br>";
            }
            bool IsEnable = ckbEnable.Checked;
            if (errMsg != null)
            {
                ShowErrorMsg(errMsg);
            }
            else
            {
                _qmgr.CreateQuestionnaire(model);
            }
        }
        protected void ShowErrorMsg(string errMsg)
        {

        }
    }
}