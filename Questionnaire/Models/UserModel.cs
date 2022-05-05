using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Questionnaire.Models
{
    public class UserModel
    {
        public Guid UserID { get; set; }
        public Guid QueID { get; set; }
        public string UserName { get; set; }
        public string UserMail { get; set; }
        public string UserPhone { get; set; }
        public int UserAge { get; set; }
        public DateTime CreateDate { get; set; }
    }
}