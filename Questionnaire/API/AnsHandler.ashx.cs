using Questionnaire.Managers;
using Questionnaire.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace Questionnaire.API
{
    /// <summary>
    /// AnsHandler 的摘要描述
    /// </summary>
    public class AnsHandler : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {
        private static QuestionManager _qmgr = new QuestionManager();
        public void ProcessRequest(HttpContext context)
        {
            if (string.Compare("POST", context.Request.HttpMethod, true) == 0 &&
                Guid.TryParse(context.Request.QueryString["ID"], out Guid queID))
            {
                string errorMsg;
                bool IsOK = DataConfirm(context, out errorMsg);
                if (!IsOK)
                {
                    HttpContext.Current.Session["ErrorMsg"] = errorMsg;
                    context.Response.ContentType = "text/plain";
                    context.Response.Write(errorMsg);                    
                    return;
                }
                else
                {
                    UserModel user = new UserModel()
                    {
                        UserID = Guid.NewGuid(),
                        UserName = context.Request.Form["Name"],
                        UserPhone = context.Request.Form["Mobile"],
                        UserMail = context.Request.Form["Email"],
                        UserAge = Convert.ToInt32(context.Request.Form["Age"]),
                        QueID = queID,
                        CreateDate = DateTime.Now
                    };
                    HttpContext.Current.Session["User"] = user;
                    string answerString = context.Request.Form["Answer"];
                    if (string.IsNullOrWhiteSpace(answerString))
                    {
                        context.Response.ContentType = "text/plain";
                        context.Response.Write("noAnswer");
                        return;
                    }

                    string[] ansArr = answerString.Trim().Split(' ');
                    List<AnswerModel> answerList = new List<AnswerModel>();
                    foreach (string item in ansArr)
                    {
                        string[] ansArray = item.Split(';');
                        Guid.TryParse(ansArray[1], out Guid ansID);
                        //QuestionModel question = _qmgr.GetQuestionByID(queID, ansID);
                        AnswerModel answer = new AnswerModel()
                        {
                            UserID = user.UserID,
                            QueID = queID,
                            AnswerID = ansID,
                            Answer = ansArray[3]
                        };
                        answerList.Add(answer);
                    }
                    List<QuestionModel> questionList = _qmgr.GetNeceQuestionList(queID);
                    foreach (QuestionModel qq in questionList)
                    {
                        int count = 0;
                        foreach(AnswerModel aa in answerList)
                        {
                            if (qq.QuestionID == aa.AnswerID)
                            {
                                if(qq.Type==QueType.文字 && !string.IsNullOrWhiteSpace(aa.Answer))
                                {
                                    count++;
                                }
                                if(qq.Type != QueType.文字)
                                {
                                    count++;
                                }
                            }
                        }
                        if (count == 0)
                        {
                            context.Response.ContentType = "text/plain";
                            context.Response.Write($"問題 {qq.QueTitle} 為必填");
                            return;
                        }
                    }
                    HttpContext.Current.Session["AnsList"] = answerList;
                    context.Response.ContentType = "text/plain";
                    context.Response.Write("success");
                }              
            }
        }
        public bool DataConfirm(HttpContext context, out string errorMsg)
        {
            bool IsOK = true;
            errorMsg = string.Empty;
            string userName = context.Request.Form["Name"];
            string userPhone = context.Request.Form["Mobile"];
            string userMail = context.Request.Form["Email"];
            string txtUserAge = context.Request.Form["Age"];
            if (string.IsNullOrWhiteSpace(userName))
            {
                errorMsg += "姓名為必填\\n";
                IsOK = false;
            }
            if (string.IsNullOrWhiteSpace(userPhone))
            {
                errorMsg += "電話為必填\\n";
                IsOK = false;
            }
            if (string.IsNullOrWhiteSpace(userMail))
            {
                errorMsg += "Email為必填\\n";
                IsOK = false;
            }
            if (string.IsNullOrWhiteSpace(txtUserAge))
            {
                errorMsg += "年齡為必填\\n";
                IsOK = false;
            }
            else
            {
                if(!int.TryParse(txtUserAge,out int userAge))
                {
                    errorMsg += "年齡請填寫數字\\n";
                    IsOK = false;
                }
                else
                {
                    if (userAge <= 0 || userAge > 100)
                    {
                        errorMsg += "年齡不符合\\n";
                        IsOK = false;
                    }
                }
            }
            return IsOK;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}