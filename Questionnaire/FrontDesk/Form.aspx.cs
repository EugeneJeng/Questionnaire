using Questionnaire.Managers;
using Questionnaire.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Questionnaire.FrontDesk
{
    public partial class Form : System.Web.UI.Page
    {
        private QuestionManager _qmr = new QuestionManager();
        private QuestionnaireManager _qnmr = new QuestionnaireManager();
        private List<AnswerModel> _ansList;
        private bool _IsCreate = true;
        protected void Page_Load(object sender, EventArgs e)
        {
            string txtQueID = Request.QueryString["ID"];
            if (!Guid.TryParse(txtQueID, out Guid queID))
            {
                Response.Redirect("List.aspx");
            }
            string errorMsg = HttpContext.Current.Session["ErrorMsg"] as string;
            if (!string.IsNullOrWhiteSpace(errorMsg))
            {
                ErrorMag(errorMsg);
            }
            hfID.Value = txtQueID;
            _ansList = HttpContext.Current.Session["AnsList"] as List<AnswerModel>;
            if (_ansList != null)
            {
                _IsCreate = false;
            }
            HttpContext.Current.Session["QueID"] = queID;
            QuestionnaireModel que = _qnmr.GetQuestionnaireByQueID(queID);
            bool IsEnable = IsEnableOrNot(que);
            if (!IsEnable)
            {
                Control btn = form1.FindControl("btnSubmit");
                btn.Visible = false;
            }
            List<QuestionModel> questionList = _qmr.GetQuestionList(queID);
            if (_IsCreate)
            {
                SetQuestions(questionList, IsEnable);
            }
            else
            {
                GetQuestions(questionList);
            }
            if (!IsEnable)
            {
                btnSubmit.Visible = false;
            }
        }
        protected bool IsEnableOrNot(QuestionnaireModel model)
        {
            bool IsEnable = true;
            if (DateTime.Now < model.StartTime)
            {
                IsEnable = false;
            }
            if (DateTime.Now > model.EndTime)
            {
                IsEnable = false;
            }
            if (model.State == StateType.已關閉)
            {
                IsEnable = false;
            }
            return IsEnable;
        }
        protected void SetQuestions(List<QuestionModel> questionList, bool IsEnable)
        {
            int count = 1;
            foreach (QuestionModel que in questionList)
            {
                Label label = new Label();
                string title = string.Empty;
                if (que.Necessary)
                {
                    title = $"{count}. {que.QueTitle} (必填)";
                }
                else
                {
                    title = $"{count}. {que.QueTitle}";
                }
                label.Text = title;
                this.phQuestion.Controls.Add(label);
                switch (que.Type)
                {
                    case QueType.文字:
                        CreateTextBox(que, IsEnable, count);
                        break;
                    case QueType.單選方塊:
                        CreateRadioButtonList(que, IsEnable, count);
                        break;
                    case QueType.複選方塊:
                        CreateCheckBoxLsit(que, IsEnable, count);
                        break;
                }
                count++;
            }
        }
        protected void GetQuestions(List<QuestionModel> questionList)
        {
            int count = 1;
            foreach (QuestionModel que in questionList)
            {
                Label label = new Label();
                string title = $"{count.ToString()}. {que.QueTitle}";
                if (que.Necessary)
                {
                    title += " (必填)";
                }
                label.Text = title;
                this.phQuestion.Controls.Add(label);
                switch (que.Type)
                {
                    case QueType.文字:
                        EditTextBox(que, count);
                        break;
                    case QueType.單選方塊:
                        EditRadioButtonList(que, count);
                        break;
                    case QueType.複選方塊:
                        EditCheckBoxLsit(que, count);
                        break;
                }
                count++;
            }
            UserModel user = HttpContext.Current.Session["User"] as UserModel;
            this.txtName.Text = user.UserName;
            this.txtMobile.Text = user.UserPhone;
            this.txtMail.Text = user.UserMail;
            this.txtAge.Text = user.UserAge.ToString();
        }
        protected void CreateTextBox(QuestionModel que, bool IsEnable, int number)
        {
            Label label = new Label();
            label.Text = "<br/>";
            this.phQuestion.Controls.Add(label);
            TextBox textBox = new TextBox();
            textBox.ID = $"Question{number};{que.QuestionID};{que.Necessary}";
            if (!IsEnable)
            {
                textBox.Enabled = false;
            }
            //textBox.TextMode = TextBoxMode.MultiLine;
            this.phQuestion.Controls.Add(textBox);
            Label label2 = new Label();
            label2.Text = "<br/><br/>";
            this.phQuestion.Controls.Add(label2);
        }
        protected void EditTextBox(QuestionModel que, int number)
        {
            AnswerModel ans = _ansList.Find(x => x.AnswerID == que.QuestionID);
            Label label = new Label();
            label.Text = "<br/>";
            this.phQuestion.Controls.Add(label);
            TextBox textBox = new TextBox();
            textBox.ID = $"Question{number};{que.QuestionID};{que.Necessary}";
            textBox.Text = ans.Answer;
            this.phQuestion.Controls.Add(textBox);
            Label label2 = new Label();
            label2.Text = "<br/><br/>";
            this.phQuestion.Controls.Add(label2);
        }
        protected void CreateCheckBoxLsit(QuestionModel que, bool IsEnable, int number)
        {
            CheckBoxList ckList = new CheckBoxList();
            ckList.ID = $"Question{number};{que.QuestionID};{que.Necessary}";
            if (!IsEnable)
            {
                ckList.Enabled = false;
            }
            string[] array = que.QueAns.Split(';');
            foreach (string option in array)
            {
                ListItem listItem = new ListItem();
                listItem.Text = option;
                ckList.Items.Add(listItem);
            }
            this.phQuestion.Controls.Add(ckList);
            Label label = new Label();
            label.Text = "<br/>";
            this.phQuestion.Controls.Add(label);
        }
        protected void EditCheckBoxLsit(QuestionModel que, int number)
        {
            List<AnswerModel> ckbList = _ansList.FindAll(x => x.AnswerID == que.QuestionID);
            CheckBoxList checkBoxList = new CheckBoxList();
            checkBoxList.ID = $"Question{number};{que.QuestionID};{que.Necessary}";
            this.phQuestion.Controls.Add(checkBoxList);
            string[] arrQue = que.QueAns.Split(';');
            int i = 0;
            foreach (string option in arrQue)
            {
                ListItem item = new ListItem(arrQue[i], i.ToString());
                if (ckbList != null)
                {
                    foreach (AnswerModel ans in ckbList)
                    {
                        if (option == ans.Answer)
                        {
                            item.Selected = true;
                        }
                    }
                }
                i++;
                checkBoxList.Items.Add(item);
            }
            Label label = new Label();
            label.Text = "<br/>";
            this.phQuestion.Controls.Add(label);
        }
        protected void CreateRadioButtonList(QuestionModel que, bool IsEnable, int number)
        {
            RadioButtonList rbList = new RadioButtonList();
            rbList.ID = $"Question{number};{que.QuestionID};{que.Necessary}";
            if (!IsEnable)
            {
                rbList.Enabled = false;
            }
            string[] array = que.QueAns.Split(';');
            foreach (string option in array)
            {
                ListItem listItem = new ListItem();
                listItem.Text = option;
                rbList.Items.Add(listItem);
            }
            this.phQuestion.Controls.Add(rbList);
            Label label = new Label();
            label.Text = "<br/>";
            this.phQuestion.Controls.Add(label);
        }
        protected void EditRadioButtonList(QuestionModel que, int number)
        {
            AnswerModel rdb = _ansList.Find(x => x.AnswerID == que.QuestionID);
            RadioButtonList radioButtonList = new RadioButtonList();
            radioButtonList.ID = $"Question{number};{que.QuestionID};{que.Necessary}";
            this.phQuestion.Controls.Add(radioButtonList);
            string[] arrQue = que.QueAns.Split(';');
            int i = 0;
            foreach(string selection in arrQue)
            {
                ListItem item = new ListItem(arrQue[i], i.ToString());
                if (rdb != null)
                {
                    if (selection == rdb.Answer)
                    {
                        item.Selected = true;
                    }
                }                
                i++;
                radioButtonList.Items.Add(item);
            }
            Label label = new Label();
            label.Text = "<br/>";
            this.phQuestion.Controls.Add(label);
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Guid queID = (Guid)HttpContext.Current.Session["QueID"];
            Response.Redirect($"Form2.aspx?ID={queID.ToString()}");
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            UserInfoConfirm();
        }
        protected void UserInfoConfirm()
        {
            if (_IsCreate)
            {
                Guid queID = (Guid)HttpContext.Current.Session["QueID"];
                UserModel user = new UserModel();
                string errorMsg = string.Empty;
                int errorCount = 0;
                user.QueID = queID;
                user.UserID = Guid.NewGuid();
                user.CreateDate = DateTime.Now;
                if (!string.IsNullOrWhiteSpace(txtName.Text.Trim()))
                {
                    user.UserName = txtName.Text.Trim();
                }
                else
                {
                    errorMsg += "姓名為必填\\n";
                    errorCount++;
                }
                if (!string.IsNullOrWhiteSpace(txtMobile.Text.Trim()))
                {
                    user.UserPhone = txtMobile.Text.Trim();
                }
                else
                {
                    errorMsg += "電話為必填\\n";
                    errorCount++;
                }
                if (!string.IsNullOrWhiteSpace(txtMail.Text.Trim()))
                {
                    user.UserMail = txtMail.Text.Trim();
                }
                else
                {
                    errorMsg += "Email 為必填\\n";
                    errorCount++;
                }
                string ageText = txtAge.Text.Trim();
                if (int.TryParse(ageText, out int age))
                {
                    user.UserAge = age;
                }
                else
                {
                    errorMsg += "年齡不符合規範\\n";
                    errorCount++;
                }
                //ControlCollection controls = phQuestion.Controls;
                //List<AnswerModel> ansList = new List<AnswerModel>();
                //foreach (Control item in controls)
                //{
                //    AnswerModel ans = new AnswerModel();
                //    if (item is TextBox)
                //    {
                //        string id = item.ID;
                //        string[] array = id.Split(';');
                //        if (string.IsNullOrWhiteSpace(((TextBox)item).Text.Trim()) && string.Compare(array[1], "true", true) == 0)
                //        {
                //            errorMsg += $"{array[0]}為必填\\n";
                //            errorCount++;
                //        }
                //        else
                //        {
                //            Guid.TryParse(array[2], out Guid ansID);
                //            ans.AnswerID = ansID;
                //            ans.QueID = queID;
                //            ans.UserID = user.UserID;
                //            ans.Answer = ((TextBox)item).Text.Trim();
                //            ansList.Add(ans);
                //        }
                //    }
                //    if (item is CheckBoxList)
                //    {
                //        string id = item.ID;
                //        string[] array = id.Split(';');
                //        if (string.IsNullOrWhiteSpace(((CheckBoxList)item).SelectedValue) && string.Compare(array[1], "true", true) == 0)
                //        {
                //            errorMsg += $"{array[0]}為必填\\n";
                //            errorCount++;
                //        }
                //        else
                //        {
                //            Guid.TryParse(array[2], out Guid ansID);
                //            ans.AnswerID = ansID;
                //            ans.QueID = queID;
                //            ans.UserID = user.UserID;
                //            ans.Answer = ((CheckBoxList)item).SelectedValue;
                //            ansList.Add(ans);
                //        }
                //    }
                //    if (item is RadioButtonList)
                //    {
                //        string id = item.ID;
                //        string[] array = id.Split(';');
                //        if (string.IsNullOrWhiteSpace(((RadioButtonList)item).SelectedValue) && string.Compare(array[1], "true", true) == 0)
                //        {
                //            errorMsg += $"{array[0]}為必填\\n";
                //            errorCount++;
                //        }
                //        else
                //        {
                //            Guid.TryParse(array[2], out Guid ansID);
                //            ans.AnswerID = ansID;
                //            ans.QueID = queID;
                //            ans.UserID = user.UserID;
                //            ans.Answer = ((RadioButtonList)item).SelectedValue;
                //            ansList.Add(ans);
                //        }
                //    }
                //}
                if (errorCount != 0)
                {
                    ErrorMag(errorMsg);
                }
            }
        }
        public void ErrorMag(string errorMsg)
        {
            if (!string.IsNullOrWhiteSpace(errorMsg))
            {
                errorMsg = "alert('" + errorMsg + "');";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), string.Empty, errorMsg, true);
            }
        }
    }
}