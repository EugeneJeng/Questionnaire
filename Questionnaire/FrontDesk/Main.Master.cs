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
    public partial class Main : System.Web.UI.MasterPage
    {
        private QuestionnaireManager _qmgr = new QuestionnaireManager();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string txtQueID = Request.QueryString["ID"];
                Guid queID;
                if (!Guid.TryParse(txtQueID, out queID))
                {
                    AlertMessage("查無此問卷");
                    Response.Redirect("/FrontDesk/List.aspx");
                    return;
                }
                QuestionnaireModel model = _qmgr.GetQuestionnaireByQueID(queID);
                List<QuestionnaireModel> list = new List<QuestionnaireModel>();
                list.Add(model);
                repContent.DataSource = list;
                repContent.DataBind();
            }
        }
        protected void AlertMessage(string errorMsg)
        {
            //ClientScript.RegisterStartupScript(
            //    this.GetType(),
            //    "alert",
            //    "alert('" + errorMsg + "');",
            //    true
            //);
        }
    }
}