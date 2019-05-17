using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataModels.Chat;
using Microsoft.AspNetCore.SignalR;

namespace Server
{
    public partial class ImHub
    {

        public async Task SendMessage(MessageModel model)
        {
            await Clients.All.SendAsync("Message", DateTime.Now, $"{model.UserName}: {model.Message}");


        }

    }
}
