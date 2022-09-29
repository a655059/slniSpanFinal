using System;
using System.Collections.Generic;

#nullable disable

namespace prjiSpanFinal.Models
{
    public partial class Faq
    {
        public int Faqid { get; set; }
        public string Answer { get; set; }
        public string Question { get; set; }
        public int FaqtypeId { get; set; }

        public virtual Faqtype Faqtype { get; set; }
    }
}
