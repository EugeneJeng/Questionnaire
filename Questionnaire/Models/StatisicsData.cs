using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Questionnaire.Models
{
    public class StatisicsData
    {
        public string Question { get; set; }
        public List<string> Selections { get; set; }
        public List<int> Percentage { get; set; }
        public List<int> Counts { get; set; }
    }
}