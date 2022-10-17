using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace prjiSpanFinal.Hubs
{
    public class ChatHub : Hub
    {
        public static List<string> ConnIDList = new List<string>();

        public async Task SendMessage(string selfUser, string message, string sendToUser)
        {
            await Clients.All.SendAsync("ReceiveMessage", selfUser, message, sendToUser);
        }
    }
}
