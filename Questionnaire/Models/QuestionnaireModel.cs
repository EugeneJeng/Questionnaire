using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Questionnaire.Models
{
    public class QuestionnaireModel
    {
        public int Number { get; set; }
        public string Title { get; set; }
        public Guid QueID { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime CreateDate { get; set; }
        public string QueContent { get; set; }
        public StateType State { get; set; }
    }
    public enum StateType
    {
        已關閉=0,
        啟用中=1
    }
}