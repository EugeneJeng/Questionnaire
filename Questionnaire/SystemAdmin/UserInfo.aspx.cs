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
    public partial class UserInfo : System.Web.UI.Page
    {
        private UserManager _umr = new UserManager();
        private QuestionnaireManager _qnmr = new QuestionnaireManager();
        private QuestionManager _qmr = new QuestionManager();
        private AnswerManager _amr = new AnswerManager();
        protected void Page_Load(object sender, EventArgs e)
        {
            string txtQueID = Request.QueryString["ID"];
            if (Guid.TryParse(txtQueID, out Guid queID))
            {
                HttpContext.Current.Session["QueID"] = queID;
            }
            else
            {
                Response.Redirect("List.aspx");
            }
            LinkSet(queID);
            string txtUserID = Request.QueryString["UserID"];
            if (Guid.TryParse(txtUserID, out Guid userID))
            {
                HttpContext.Current.Session["UserID"] = userID;
            }
            else
            {
                Response.Redirect($"DetailAns.aspx?ID={txtQueID}");
            }
            UserModel user = _umr.GetUser(queID, userID);
            QuestionnaireModel que = _qnmr.GetQuestionnaireByQueID(queID);
            List<QuestionModel> questionList = _qmr.GetQuestionList(queID);       
            SetUserInfo(que, user);
            SetAnsList(questionList, user);
        }
        protected void LinkSet(Guid queID)
        {
            if (queID == null)
            {
                Response.Redirect("List.aspx");
            }
            linkQue.HRef = $"Detail.aspx?ID={queID}";
            linkQueContent.HRef = $"DetailQue.aspx?ID={queID}";
            linkData.HRef = $"DetailAns.aspx?ID={queID}";
            linkStatistics.HRef = $"DetailStatistics.aspx?ID={queID}";
        }
        protected void SetUserInfo(QuestionnaireModel que, UserModel user)
        {
            ltlTitle.Text = que.Title;
            ltlContent.Text = que.QueContent;
            txtUserName.Text = user.UserName;
            txtUserPhone.Text = user.UserPhone;
            txtUserMail.Text = user.UserMail;
            txtUserAge.Text = user.UserAge.ToString();
            labCreateDate.Text = user.CreateDate.ToString("yyyy/MM/dd HH:mm");
        }
        protected void SetAnsList(List<QuestionModel> questionList, UserModel user)
        {
            int queNumber = 1;
            foreach (QuestionModel model in questionList)
            {
                AnswerModel ans = _amr.GetAnswer(model.QuestionID, user.UserID);
                if (ans.Answer == null)
                {
                    continue;
                }
                string txtAnsID = ans.AnswerID.ToString();
                string[] txtAnsIDArray = txtAnsID.Split('-');
                bool dataOK = false;
                int num = 0;                
                foreach (string txt in txtAnsIDArray)
                {
                    if (!int.TryParse(txt, out int guidNum))
                    {
                        dataOK = true;
                        break;
                    }
                    num += guidNum;
                    if (num > 0)
                    {
                        dataOK = true;
                        break;
                    }
                }
                if (!dataOK)
                {
                    break;
                }
                Label labQue = new Label();
                string question = $"{queNumber}. {model.QueTitle}";
                if (model.Necessary)
                {
                    question += " (必填欄位)";
                }
                labQue.Text = question + "<br/>";
                phList.Controls.Add(labQue);
                queNumber++;
                switch (model.Type)
                {
                    case QueType.文字:
                        CreateTextBox(model, user);
                        break;
                    case QueType.單選方塊:
                        CreateRadioButton(model, user);
                        break;
                    case QueType.複選方塊:
                        CreateCheckBox(model, user);
                        break;
                } 
            }
        }
        protected void CreateTextBox(QuestionModel que, UserModel user)
        {
            AnswerModel ans = _amr.GetAnswer(que.QuestionID, user.UserID);
            TextBox textBox = new TextBox();
            textBox.ID = $"Question{que.QuestionNumber}";
            textBox.Enabled = false;
            textBox.Text = ans.Answer;
            this.phList.Controls.Add(textBox);
            Label label = new Label();
            label.Text = "<br/><br/>";
            this.phList.Controls.Add(label);
        }
        protected void CreateRadioButton(QuestionModel que, UserModel user)
        {
            AnswerModel ans = _amr.GetAnswer(que.QuestionID, user.UserID);
            RadioButtonList rbList = new RadioButtonList();
            rbList.ID = $"Question{que.QuestionNumber}";
            rbList.Enabled = false;
            string[] array = que.QueAns.Split(';');
            string[] ansArray = ans.Answer.Split(';');
            foreach (string option in array)
            {
                ListItem listItem = new ListItem();
                listItem.Text = option;
                foreach(string item in ansArray)
                {
                    if (option == item)
                    {
                        listItem.Selected = true;
                    }
                }
                rbList.Items.Add(listItem);
            }
            this.phList.Controls.Add(rbList);
            Label label = new Label();
            label.Text = "<br/>";
            this.phList.Controls.Add(label);
        }
        protected void CreateCheckBox(QuestionModel que, UserModel user)
        {
            List<AnswerModel> ansList = _amr.GetAnswerList(que.QuestionID, user.UserID);
            CheckBoxList ckList = new CheckBoxList();
            ckList.ID = $"Question{que.QuestionNumber}";
            ckList.Enabled = false;
            string[] array = que.QueAns.Split(';');
            foreach (string option in array)
            {
                ListItem listItem = new ListItem();
                listItem.Text = option;
                foreach (AnswerModel item in ansList)
                {
                    if (option == item.Answer)
                    {
                        listItem.Selected = true;
                    }
                }
                ckList.Items.Add(listItem);
            }
            this.phList.Controls.Add(ckList);
            Label label = new Label();
            label.Text = "<br/>";
            this.phList.Controls.Add(label);
        }        
    }
}