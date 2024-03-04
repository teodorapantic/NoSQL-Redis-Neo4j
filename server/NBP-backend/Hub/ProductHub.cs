using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace NBP_backend
{
    public class ProductHub : Hub
    {

        public async Task SetProductOnSale(string notification)
        {
            // var subscribedUsers = await _subscribedUsers.Get(productId);
            // await Clients.Users(subscribedUsers).SendAsync("ProductOnSale", productId);
            await Clients.All.SendAsync("ProductNotification", notification);
            //await Clients.Group(6).SendAsync("MessageReceived", JsonConvert.SerializeObject(poruka), idObjave);
        }
    }
}
