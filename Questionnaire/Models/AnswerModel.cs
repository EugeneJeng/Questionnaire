using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Questionnaire.Models
{
    public class AnswerModel
    {
        public Guid AnswerID { get; set; }
        public Guid QueID { get; set; }
        public Guid UserID { get; set; }
        public string Answer { get; set; }
    }
}