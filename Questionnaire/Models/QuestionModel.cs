using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Questionnaire.Models
{
    public class QuestionModel
    {
        public int QuestionNumber { get; set; }
        public Guid QueID { get; set; }
        public Guid QuestionID { get; set; }
        public string QueTitle { get; set; }
        public string QueAns { get; set; }
        public QueType Type { get; set; }
        public bool Necessary { get; set; }
    }
    public enum QueType
    {
        文字 = 0,
        單選方塊 = 1,
        複選方塊 = 2
    }
}