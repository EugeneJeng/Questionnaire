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
    public partial class List : System.Web.UI.Page
    {
        private QuestionnaireManager _qmgr = new QuestionnaireManager();
        private QuestionManager _qtmr = new QuestionManager();
        private string _url = null;
        public string Url
        {
            get
            {
                if (_url == null)
                {
                    return Request.Url.LocalPath;
                }
                else
                {
                    return _url;
                }
            }
            set
            {
                _url = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //  後台
            if (!IsPostBack)
            {
                string createSucc = HttpContext.Current.Session["temp"] as string;
                HttpContext.Current.Session["QuestionList"] = null;
                if (!string.IsNullOrWhiteSpace(createSucc))
                {
                    ShowMsg();
                }
                string txtPageIndex = Request.QueryString["Index"];
                string txtKeyword = Request.QueryString["keyword"];
                string txtSatrtDate = Request.QueryString["StartDate"];
                string txtEndDate = Request.QueryString["EndDate"];
                if (string.IsNullOrWhiteSpace(txtPageIndex))
                {
                    txtPageIndex = "1";
                }
                int PageIndex = Convert.ToInt32(txtPageIndex);
                if(txtKeyword!=null||txtSatrtDate!=null|| txtEndDate != null)
                {
                    DateTime startDate, endDate;
                    DateTime? startTime, endTime;
                    if (!DateTime.TryParse(txtSatrtDate, out startDate))
                    {
                        startTime = null;
                    }
                    else
                    {
                        startTime = startDate;
                    }
                    if (!DateTime.TryParse(txtEndDate, out endDate))
                    {
                        endTime = null;
                    }
                    else
                    {
                        endTime = endDate;
                    }
                    List<QuestionnaireModel> list = _qmgr.GetQuestionnaireList(txtKeyword, startTime, endTime, PageIndex);
                    if (list == null)
                    {
                        return;
                    }
                    Pages(list);
                    ShowList(list);
                }
                else
                {
                    List<QuestionnaireModel> list = _qmgr.GetQuestionnaireList(PageIndex);
                    List<QuestionnaireModel> allList = _qmgr.GetQuestionnaireList();
                    if (allList == null)
                    {
                        return;
                    }
                    Pages(allList);
                    ShowList(list);
                }                         
            }
        }
        protected void ShowList(List<QuestionnaireModel> list)
        {
            List<QuestionnaireModel> allList = _qmgr.GetQuestionnaireList();
            int count = allList.Count;
            string txtPageIndex = Request.QueryString["Index"];
            if(!string.IsNullOrWhiteSpace(txtPageIndex) && string.Compare(txtPageIndex,"1") == 1)
            {
                int page = Convert.ToInt32(txtPageIndex);
                count = count - (page-1) * 10;
            }
            repList.DataSource = list;            
            repList.DataBind();
            
            foreach (RepeaterItem item in this.repList.Items)
            {
                Label labCount = item.FindControl("labCount") as Label;
                labCount.Text = $"{count}";
                count--;
            }

        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            //string url = Request.Url.LocalPath;
            string url = "List.aspx";
            string keyword = txtSearch.Text.Trim();
            int count = 0;
            string errMsg = string.Empty;
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                url += $"?keyword={keyword}";
                count++;
            }
            string txtStartDate = txtStartTime.Text.Trim();
            string txtEndDate = txtEndTime.Text.Trim();
            DateTime startDate, endDate;
            DateTime? startTime, endTime;

            if (!string.IsNullOrWhiteSpace(txtStartDate))
            {
                if (!DateTime.TryParse(txtStartDate, out startDate))
                {
                    startTime = null;
                    errMsg += "起始時間格式不符，請以 yyyy/MM/dd 來填寫\\n";
                }
                else if (count > 0)
                {
                    startTime = startDate;
                    url += $"&StartDate={startDate.ToString("yyyy/MM/dd")}";
                    count++;
                }
                else
                {
                    startTime = startDate;
                    url += $"?StartDate={startDate.ToString("yyyy/MM/dd")}";
                    count++;
                }
            }

            if (!string.IsNullOrWhiteSpace(txtEndDate))
            {
                if (!DateTime.TryParse(txtEndDate, out endDate))
                {
                    endTime = null;
                    errMsg += "結束時間格式不符，請以 yyyy/MM/dd 來填寫\\n";
                }
                else if (count > 0)
                {
                    endTime = endDate;
                    url += $"&EndDate={endDate.ToString("yyyy/MM/dd")}";
                }
                else
                {
                    endTime = endDate;
                    url += $"?EndDate={endDate.ToString("yyyy/MM/dd")}";
                }
            }

            if (string.IsNullOrWhiteSpace(errMsg))
            {
                Response.Redirect(url);
            }
            else
            {
                ShowErrorMsg(errMsg);
            }
        }
        protected void Pages(List<QuestionnaireModel> list)
        {
            int pageCount = (list.Count / 10);
            if ((list.Count % 10) > 0)
            {
                pageCount += 1;
            }
            string url = Url;
            string txtPageIndex = Request.QueryString["Index"];
            if (string.IsNullOrWhiteSpace(txtPageIndex))
            {
                txtPageIndex = "1";
            }
            int PageIndex = Convert.ToInt32(txtPageIndex);

            linkFirstPage.HRef = url + "?Index=1";
            linkPrevPage.HRef = url + "?Index=" + (PageIndex - 1);
            linkNextPage.HRef = url + "?Index=" + (PageIndex + 1);
            linkButtomPage.HRef = url + "?Index=" + pageCount;

            linkPage1.HRef = url + "?Index=" + (PageIndex - 2);
            linkPage1.InnerText = (PageIndex - 2).ToString();
            if (PageIndex <= 2)
            {
                this.linkPage1.Visible = false;
            }

            linkPage2.HRef = url + "?Index=" + (PageIndex - 1);
            linkPage2.InnerText = (PageIndex - 1).ToString();
            if (PageIndex <= 1)
            {
                this.linkPage2.Visible = false;

            }

            linkPage3.HRef = "";
            linkPage3.InnerText = PageIndex.ToString();

            linkPage4.HRef = url + "?Index=" + (PageIndex + 1);
            linkPage4.InnerText = (PageIndex + 1).ToString();
            //if ((PageIndex + 1) > pageCount)
            //{
            //    this.linkPage4.Visible = false;

            //}
            if ((PageIndex) > pageCount)
            {
                this.linkPage4.Visible = false;

            }

            linkPage5.HRef = url + "?Index=" + (PageIndex + 2);
            linkPage5.InnerText = (PageIndex + 2).ToString();
            if ((PageIndex + 2) > pageCount)
            {
                this.linkPage5.Visible = false;

            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            foreach(RepeaterItem item in this.repList.Items)
            {
                HiddenField hfID = item.FindControl("hfID") as HiddenField;
                CheckBox ckbDel = item.FindControl("ckbDel") as CheckBox;
                if(ckbDel.Checked && Guid.TryParse(hfID.Value, out Guid queID))
                {
                    _qmgr.DeleteQuestionnaireByQueID(queID);
                    _qtmr.DeleteQuestion(queID);
                }
            }
            Response.Redirect(Request.RawUrl.ToString());
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            Response.Redirect("Detail.aspx");
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
            string msg = "alert('新增成功');";
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), string.Empty, msg, true);
            HttpContext.Current.Session["temp"] = null;
        }
    }
}