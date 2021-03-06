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
    public partial class DetailQue : System.Web.UI.Page
    {
        private static bool _IsCreateMode = true;
        private static QuestionManager _qtmgr = new QuestionManager();
        private static List<QuestionModel> _QuestionList = new List<QuestionModel>();
        private static CommonlyManager _cmgr = new CommonlyManager();
        protected void Page_Load(object sender, EventArgs e)
        {
            bool IsComm = false;
            string txtIsComm = HttpContext.Current.Session["IsComm"] as string;
            if (string.Compare(txtIsComm, "true") == 0)
            {
                IsComm = true;
            }
            Uri url = Request.UrlReferrer;
            string txtqueID = Request.QueryString["ID"];
            if (!Guid.TryParse(txtqueID, out Guid queID))
            {
                Response.Redirect("List.aspx");
            }
            else
            {
                HttpContext.Current.Session["QueID"] = queID;
                hf3.Value = queID.ToString();
                SetRep(queID);
            }
            LinkSet(txtqueID);
            List<QuestionModel> list = (List<QuestionModel>)HttpContext.Current.Session["QuestionList"];
            if (list == null)
            {
                labWorring.Text = "";
            }
            else
            {
                labWorring.Text = "有修改內容尚未儲存";
            }
            string txtQuestionID = Request.QueryString["Ques"];
            string txtCommID = Request.QueryString["CommID"];
            if (!string.IsNullOrWhiteSpace(txtQuestionID))
            {
                if(Guid.TryParse(txtQuestionID,out Guid questionID))
                {
                    _IsCreateMode = false;
                    ShowData(queID, questionID);
                }
            }
            else if (Guid.TryParse(txtCommID, out Guid commID))
            {
                _IsCreateMode = true;
                ShowCommData(commID);
            }
            else
            {
                _IsCreateMode = true;
            }
        }
        protected void ShowCommData(Guid questionID)
        {
            if (!IsPostBack)
            {
                CommonlyModel model = _cmgr.GetCommonlyModel(questionID);
                txtxQueTitle.Text = model.QueTitle;
                txtAns.Text = model.QueAns;
                ckbNecessaryFirst.Checked = true;
                dpQueType.SelectedIndex = (int)model.Type;
            }
        }
        protected void ShowData(Guid queID, Guid questionID)
        {
            if (!IsPostBack)
            {
                List<QuestionModel> list = (List<QuestionModel>)HttpContext.Current.Session["QuestionList"];
                if (list == null)
                {
                    QuestionModel model = _qtmgr.GetQuestionByID(queID, questionID);
                    txtxQueTitle.Text = model.QueTitle;
                    txtAns.Text = model.QueAns;
                    ckbNecessaryFirst.Checked = model.Necessary;
                    dpQueType.SelectedIndex = (int)model.Type;
                }
                else
                {
                    foreach (QuestionModel q in list)
                    {
                        if (q.QuestionID == questionID)
                        {
                            txtxQueTitle.Text = q.QueTitle;
                            txtAns.Text = q.QueAns;
                            ckbNecessaryFirst.Checked = q.Necessary;
                            dpQueType.SelectedIndex = (int)q.Type;
                            break;
                        }
                        else
                        {
                            QuestionModel model = _qtmgr.GetQuestionByID(queID, questionID);
                            txtxQueTitle.Text = model.QueTitle;
                            txtAns.Text = model.QueAns;
                            ckbNecessaryFirst.Checked = model.Necessary;
                            dpQueType.SelectedIndex = (int)model.Type;
                        }
                    }
                }             
            }            
        }

        protected void LinkSet(string queID)
        {
            if (string.IsNullOrWhiteSpace(queID))
            {
                linkQue.HRef = "NewQue.aspx";
                //linkQueContent.HRef = "DetailQue.aspx";
                //linkData.HRef = "DetailAns.aspx";
                //linkStatistics.HRef = "DetailStatistics.aspx";
            }
            else
            {
                linkQue.HRef = $"Detail.aspx?ID={queID}";
                //linkQueContent.HRef = $"DetailQue.aspx?ID={queID}";
                linkData.HRef = $"DetailAns.aspx?ID={queID}";
                linkStatistics.HRef = $"DetailStatistics.aspx?ID={queID}";
            }
        }
        protected void SetRep(Guid queID)
        {
            if (!IsPostBack)
            {
                HttpContext.Current.Session["QueID"] = queID;
                if (dpQue.SelectedIndex == 0)
                {
                    List<QuestionModel> list = _qtmgr.GetQuestionList(queID);
                    repDown.DataSource = list;
                    repDown.DataBind();
                    int i = 1;
                    foreach (RepeaterItem item in this.repDown.Items)
                    {
                        Label labQueNumber = (Label)item.FindControl("labQueNumber");
                        labQueNumber.Text = i.ToString();
                        i++;
                    }
                }
                else
                {
                    List<QuestionModel> list = _qtmgr.GetQuestionList(queID);
                    repDown.DataSource = list;
                    repDown.DataBind();

                }              
            }            
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            string txtqueID = Request.QueryString["ID"];
            if (!Guid.TryParse(txtqueID, out Guid queID))
            {
                Response.Redirect("List.aspx");
            }        
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
            QuestionModel model = new QuestionModel();
            Guid queID;
            string txtqueID = Request.QueryString["ID"];
            if (!Guid.TryParse(txtqueID, out queID))
            {
                Response.Redirect(Request.RawUrl);
            }
            bool IsOK = ContentConfirm();
            if (IsOK)
            {
                model.QueID = queID;
                model.QuestionID = Guid.NewGuid();
                model.Necessary = ckbNecessaryFirst.Checked;
                model.QueTitle = txtxQueTitle.Text.Trim();
                model.QueAns = txtAns.Text.Trim();
                model.Type = (QueType)Convert.ToInt32(dpQueType.SelectedIndex);
                _qtmgr.CreateQuestion(model);
                SetRep(queID);
                Response.Redirect($"DetailQue.aspx?ID={queID}");
            }
            //Response.Redirect($"DetailQue.aspx?ID={queID}");
        }
        protected void EditMode()
        {
            QuestionModel model = new QuestionModel();
            Guid queID, questionID;
            string txtqueID = Request.QueryString["ID"];
            if (!Guid.TryParse(txtqueID, out queID))
            {
                Response.Redirect(Request.RawUrl);
            }
            string txtQuestionID = Request.QueryString["Ques"];
            if (!Guid.TryParse(txtQuestionID, out questionID))
            {
                Response.Redirect(Request.RawUrl);
            }
            bool IsOK = ContentConfirm();
            if (IsOK)
            {
                model.QueID = queID;
                model.QuestionID = questionID;
                model.Necessary = this.ckbNecessaryFirst.Checked;
                model.QueTitle = this.txtxQueTitle.Text.Trim();
                model.QueAns = this.txtAns.Text.Trim();
                model.Type = (QueType)Convert.ToInt32(this.dpQueType.SelectedIndex);
                _QuestionList.Add(model);
                HttpContext.Current.Session["QuestionList"] = _QuestionList;
                Response.Redirect($"DetailQue.aspx?ID={queID}");
            }            
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
            if (dpQueType.SelectedValue != "0")
            {
                if (string.IsNullOrWhiteSpace(txtAns.Text.Trim()))
                {
                    errorMsg += "回答不可為空白\\n";
                    IsOK = false;
                }
                else
                {
                    string txtSelect = txtAns.Text.Trim();
                    string[] txtSelectArray = txtSelect.Split(';');
                    foreach(string sel in txtSelectArray)
                    {
                        string select = sel.Trim();
                        if (string.IsNullOrWhiteSpace(select))
                        {
                            errorMsg += "回答選項不可為空白\\n";
                            IsOK = false;
                            break;
                        }
                    }
                }
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
            Response.Write(errorMsg);
        }
        protected void ShowMsg(string msg)
        {
            if (!string.IsNullOrWhiteSpace(msg))
            {
                msg = "alert('" + msg + "');";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), string.Empty, msg, true);
            }
            Response.Write(msg);
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            if (_IsCreateMode)
            {
                CreateMode();
            }
            List<QuestionModel> list = (List<QuestionModel>)HttpContext.Current.Session["QuestionList"];
            Guid queID = (Guid)HttpContext.Current.Session["QueID"];
            if (list==null || list.Count == 0)
            {
                string title = txtxQueTitle.Text.Trim();
                string selection = txtAns.Text.Trim();
                if(!string.IsNullOrWhiteSpace(title) || !string.IsNullOrWhiteSpace(selection))
                {
                    string msg = "如欲修改問題請先按加入後再送出";
                    ShowMsg(msg);
                }
                else
                {
                    Response.Redirect($"DetailQue.aspx?ID={queID}");
                }                
            }
            else
            {
                foreach(QuestionModel model in list)
                {
                    _qtmgr.UpdateQuestion(model);
                }
                HttpContext.Current.Session["QuestionList"] = null;
                labWorring.Text = "";
                Response.Redirect($"DetailQue.aspx?ID={queID}");                
            }            
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Guid queID = (Guid)HttpContext.Current.Session["QueID"];
            Response.Redirect($"DetailQue.aspx?ID={queID}");
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            Guid id = (Guid)HttpContext.Current.Session["QueID"];
            foreach (RepeaterItem item in this.repDown.Items)
            {
                HiddenField hf1 = item.FindControl("hf1") as HiddenField;
                HiddenField hf2 = item.FindControl("hf2") as HiddenField;
                CheckBox ckbDelet = item.FindControl("ckbDelet") as CheckBox;
                if(ckbDelet.Checked && Guid.TryParse(hf1.Value, out Guid queID) && Guid.TryParse(hf2.Value, out Guid questionID))
                {
                    _qtmgr.DeleteQuestion(queID, questionID);
                }
            }
            Response.Redirect($"DetailQue.aspx?ID={id.ToString()}");          
        }
        protected void dpQue_SelectedIndexChanged(object sender, EventArgs e)
        {
            Guid txtQueId = (Guid)HttpContext.Current.Session["QueID"];
            if (dpQue.SelectedIndex == 0)
            {
                HttpContext.Current.Session["IsComm"] = "false";
                string txtQueID = Request.QueryString["ID"];
                if (string.IsNullOrWhiteSpace(txtQueID) || !Guid.TryParse(txtQueID, out Guid queID))
                {
                    Response.Redirect("List.aspx");
                }
                else
                {
                    List<QuestionModel> list = _qtmgr.GetQuestionList(queID);
                    repDown.Visible = true;
                    repDown.DataSource = list;
                    repDown.DataBind();
                    repCommonly.Visible = false;
                }
            }
            else
            {
                HttpContext.Current.Session["IsComm"] = "true";
                List<CommonlyModel> list = _cmgr.GetAllCommonly();
                repCommonly.Visible = true;
                repCommonly.DataSource = list;
                repCommonly.DataBind();
                repDown.Visible = false;
                int i = 1;
                foreach(RepeaterItem item in this.repCommonly.Items)
                {
                    Label labQueNumber = (Label)item.FindControl("labQueNumber");
                    labQueNumber.Text = i.ToString();
                    i++;
                }
            }
        }
    }
}