using System;
using System.Collections.Generic;

#nullable disable

namespace prjiSpanFinal.Models
{
    public partial class Faqtype
    {
        public Faqtype()
        {
            Faqs = new HashSet<Faq>();
        }

        public int FaqtypeId { get; set; }
        public string FaqtypeName { get; set; }

        public virtual ICollection<Faq> Faqs { get; set; }
    }
}
