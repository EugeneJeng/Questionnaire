using Questionnaire.Managers;
using Questionnaire.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Questionnaire.FrontDesk
{
    public partial class ConfirmPage : System.Web.UI.Page
    {
        private QuestionnaireManager _qnmr = new QuestionnaireManager();
        private AnswerManager _amr = new AnswerManager();
        private UserManager _umr = new UserManager();
        private QuestionManager _qmr = new QuestionManager();
        private Guid _queID;
        private List<AnswerModel> _answerList;
        private UserModel _user;
        protected void Page_Load(object sender, EventArgs e)
        {
            _answerList = HttpContext.Current.Session["AnsList"] as List<AnswerModel>;
            string txtQueID = Request.QueryString["ID"];
            if (Guid.TryParse(txtQueID, out _queID) && _answerList != null)
            {
                QuestionnaireModel questionnaire = _qnmr.GetQuestionnaireByQueID(_queID);
                this.hfID.Value = _queID.ToString();
                this.ltlTitle.Text = questionnaire.Title;
                this.ltlContent.Text = questionnaire.QueContent;

                _user = HttpContext.Current.Session["User"] as UserModel;
                this.txtName.Text = _user.UserName;
                this.txtMobile.Text = _user.UserPhone;
                this.txtMail.Text = _user.UserMail;
                this.txtAge.Text = _user.UserAge.ToString();

                List<QuestionModel> questionList = _qmr.GetQuestionList(_queID);
                int count = 1;
                foreach (QuestionModel question in questionList)
                {
                    string q = $"<br/>{count}. {question.QueTitle}";
                    if (question.Necessary)
                        q += " (必填)";
                    Literal ltlQuestion = new Literal();
                    ltlQuestion.Text = q + "<br/>";
                    this.phAnsList.Controls.Add(ltlQuestion);

                    switch (question.Type)
                    {
                        case QueType.文字:
                            CreateTextBox(question, count);
                            break;
                        case QueType.單選方塊:
                            CreateRadioButtonList(question, count);
                            break;
                        case QueType.複選方塊:
                            CreateCheckBoxLsit(question, count);
                            break;                        
                    }
                    count++;
                }
            }
            else
            {
                Response.Redirect("List.aspx");
            }            
        }
        private void CreateTextBox(QuestionModel que, int number)
        {
            AnswerModel ans = _answerList.Find(x => x.AnswerID == que.QuestionID);
            TextBox textBox = new TextBox();
            textBox.ID = $"Question{number};{que.QuestionID};{que.Necessary}";
            textBox.Enabled = false;
            textBox.Text = ans.Answer;
            this.phAnsList.Controls.Add(textBox);
        }
        private void CreateRadioButtonList(QuestionModel que, int number)
        {
            AnswerModel ans = _answerList.Find(x => x.AnswerID == que.QuestionID);
            RadioButtonList rbList = new RadioButtonList();
            rbList.ID = $"Question{number};{que.QuestionID};{que.Necessary}";
            rbList.Enabled = false;
            this.phAnsList.Controls.Add(rbList);
            string[] array = que.QueAns.Split(';');
            if (ans != null)
            {
                if (int.TryParse(ans.Answer, out int txtAns))
                {
                    ans.Answer = array[txtAns];
                }
                int i = 0;
                foreach (string option in array)
                {
                    ListItem listItem = new ListItem(option, i.ToString());
                    if (ans != null)
                    {
                        if (string.Compare(ans.Answer, option) == 0)
                        {
                            listItem.Selected = true;
                        }
                    }
                    i++;
                    rbList.Items.Add(listItem);
                }
            }
            else
            {
                int i = 0;
                foreach (string option in array)
                {
                    ListItem listItem = new ListItem(option, i.ToString());
                    rbList.Items.Add(listItem);
                    i++;
                }
            }
            
        }
        private void CreateCheckBoxLsit(QuestionModel que, int number)
        {
            CheckBoxList checkBoxList = new CheckBoxList();
            List<AnswerModel> ansList = _answerList.FindAll(x => x.AnswerID == que.QuestionID);            
            checkBoxList.ID = $"Question{number};{que.QuestionID};{que.Necessary}";
            checkBoxList.Enabled = false;
            this.phAnsList.Controls.Add(checkBoxList);
            string[] array = que.QueAns.Split(';');
            int i = 0;
            foreach(string option in array)
            {
                ListItem item = new ListItem(array[i], i.ToString());
                if (ansList != null)
                {
                    foreach(AnswerModel ans in ansList)
                    {
                        if (int.TryParse(ans.Answer, out int txtAns))
                        {
                            ans.Answer = array[txtAns];
                        }
                        if (option == ans.Answer)
                        {
                            item.Selected = true;
                        }
                    }
                }
                i++;
                checkBoxList.Items.Add(item);
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect($"Form.aspx?ID={_queID}");
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            _umr.CreateUser(_user);
            foreach (AnswerModel answer in _answerList)
            {
                _amr.CreateAnswer(answer);
            }
            HttpContext.Current.Session.RemoveAll();
            Response.Redirect($"Statistics.aspx?ID={_queID}");
        }
    }
}