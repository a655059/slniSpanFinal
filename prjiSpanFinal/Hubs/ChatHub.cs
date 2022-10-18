using Microsoft.AspNetCore.SignalR;
using prjiSpanFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace prjiSpanFinal.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string sendFrom, string message, string sendTo)
        {
            
            iSpanProjectContext dbcontext = new iSpanProjectContext();
            ChatLog chat = new ChatLog() { SendFrom = Convert.ToInt32(sendFrom), Msg = message, SendTo = Convert.ToInt32(sendTo), HaveRead = false };
            dbcontext.ChatLogs.Add(chat);
            dbcontext.SaveChanges();
            await Clients.All.SendAsync("ReceiveMessage", sendFrom, message, sendTo, chat.ChatLogId);
        }
        public async Task HaveReadMessage(string id)
        {
            iSpanProjectContext dbcontext = new iSpanProjectContext();
            var q = dbcontext.ChatLogs.Where(i => i.ChatLogId == Convert.ToInt32(id)).FirstOrDefault();
            q.HaveRead = true;
            dbcontext.SaveChanges();
        }
        public async Task ReadMessage(string sid, string scid)
        {
            var cid = Convert.ToInt32(scid);
            var id = Convert.ToInt32(sid);
            iSpanProjectContext dbcontext = new iSpanProjectContext();
            var q = dbcontext.ChatLogs.Where(i => (i.SendFrom == cid && i.SendTo == id)).ToList();
            foreach(var item in q)
            {
                item.HaveRead = true;
            }
            dbcontext.SaveChanges();
        }
    }
}
