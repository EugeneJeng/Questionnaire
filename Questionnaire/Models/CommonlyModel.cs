using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Questionnaire.Models
{
    public class CommonlyModel
    {
        public Guid QuestionID { get; set; }
        public QueType Type { get; set; }
        public string QueTitle { get; set; }
        public string QueAns { get; set; }
    }
}