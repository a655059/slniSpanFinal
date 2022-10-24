using System;
using System.Collections.Generic;

#nullable disable

namespace prjiSpanFinal.Models
{
    public partial class IconType
    {
        public IconType()
        {
            Notifications = new HashSet<Notification>();
        }

        public int IconTypeId { get; set; }
        public string IconTypeName { get; set; }
        public byte[] IconPic { get; set; }

        public virtual ICollection<Notification> Notifications { get; set; }
    }
}
