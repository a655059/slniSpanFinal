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
    }
}
