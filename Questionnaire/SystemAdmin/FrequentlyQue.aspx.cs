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
    public partial class FrequentlyQue : System.Web.UI.Page
    {
        private static CommonlyManager _clmgr = new CommonlyManager();
        private static bool _IsCreateMode = true;
        private static List<CommonlyModel> _commList = new List<CommonlyModel>();
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpContext.Current.Session["QuestionList"] = null;
            dpQue.SelectedValue = "1";
            dpQue.Enabled = false;
            string txtCommID = Request.QueryString["CommID"];
            if(string.IsNullOrWhiteSpace(txtCommID) || !Guid.TryParse(txtCommID,out Guid commID))
            {
                _IsCreateMode = true;
            }
            else
            {
                _IsCreateMode = false;
                CommonlyModel model = _clmgr.GetCommonlyModel(commID);
                ShowData(model);
            }
            List<CommonlyModel> list = _clmgr.GetAllCommonly();
            SetRep(list);
        }
        protected void SetRep(List<CommonlyModel> list)
        {
            if (!IsPostBack)
            {
                repDown.DataSource = list;
                repDown.DataBind();
                int i = 1;
                foreach (RepeaterItem item in this.repDown.Items)
                {
                    Label labQueNumber = item.FindControl("labQueNumber") as Label;
                    labQueNumber.Text = i.ToString();
                    i++;
                }
            }            
        }
        protected void ShowData(CommonlyModel model)
        {
            if (!IsPostBack)
            {
                txtxQueTitle.Text = model.QueTitle;
                txtAns.Text = model.QueAns;
                dpQueType.SelectedIndex = (int)model.Type;
            }            
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (_IsCreateMode)
            {
                CreateMode();
            }
            else
            {
                EditMode();
            }
        }
        protected void CreateMode()
        {
            CommonlyModel model = new CommonlyModel();
            bool IsOK = ContentConfirm();
            if (IsOK)
            {
                model.QuestionID = Guid.NewGuid();
                model.QueTitle = txtxQueTitle.Text.Trim();
                model.QueAns = txtAns.Text.Trim();
                model.Type = (QueType)Convert.ToInt32(dpQueType.SelectedIndex);
                _clmgr.CreateCommonly(model);
            }
            Response.Redirect("FrequentlyQue.aspx");
        }
        protected void EditMode()
        {
            CommonlyModel model = new CommonlyModel();
            Guid commID;
            string txtCommID = Request.QueryString["CommID"];
            if (!Guid.TryParse(txtCommID, out commID))
            {
                Response.Redirect(Request.RawUrl);
            }
            bool IsOK = ContentConfirm();
            if (IsOK)
            {
                model.QuestionID = commID;
                model.QueTitle = this.txtxQueTitle.Text.Trim();
                model.QueAns = this.txtAns.Text.Trim();
                model.Type = (QueType)Convert.ToInt32(this.dpQueType.SelectedIndex);
                _commList.Add(model);
                HttpContext.Current.Session["CommList"] = _commList;
            }
            Response.Redirect("FrequentlyQue.aspx");
        }
        protected bool ContentConfirm()
        {
            bool IsOK = true;
            string errorMsg = string.Empty;
            if (string.IsNullOrWhiteSpace(txtxQueTitle.Text.Trim()))
            {
                errorMsg += "問題不可為空白\\n";
                IsOK = false;
            }
            if (string.IsNullOrWhiteSpace(txtAns.Text.Trim()))
            {
                errorMsg += "回答不可為空白\\n";
                IsOK = false;
            }
            ErrorMag(errorMsg);

            return IsOK;
        }
        protected void ErrorMag(string errorMsg)
        {
            if (!string.IsNullOrWhiteSpace(errorMsg))
            {
                errorMsg = "alert('" + errorMsg + "');";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), string.Empty, errorMsg, true);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            foreach (RepeaterItem item in this.repDown.Items)
            {
                HiddenField hf2 = item.FindControl("hf2") as HiddenField;
                CheckBox ckbDelet = item.FindControl("ckbDelet") as CheckBox;
                if (ckbDelet.Checked && Guid.TryParse(hf2.Value, out Guid questionID))
                {
                    _clmgr.DeleteCommonly(questionID);
                }
            }
            Response.Redirect($"FrequentlyQue.aspx");
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect($"FrequentlyQue.aspx");
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            List<CommonlyModel> list = (List<CommonlyModel>)HttpContext.Current.Session["CommList"];
            if (list == null || list.Count == 0)
            {
                Response.Redirect($"FrequentlyQue.aspx");
            }
            else
            {
                foreach (CommonlyModel model in list)
                {
                    _clmgr.UpdateCommonly(model);
                }
            }
            Response.Redirect($"FrequentlyQue.aspx");
        }
    }
}