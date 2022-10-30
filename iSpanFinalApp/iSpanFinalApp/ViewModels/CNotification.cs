using System;
using System.Collections.Generic;


namespace iSpanFinalApp.ViewModels
{
    public class CNotification
    {
        public int NotificationId { get; set; }
        public string Text { get; set; }
        public DateTime Time { get; set; }
        public string TextContent { get; set; }
        public string NotiTime
        {
            get
            {
                return Time.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }
    }
}
