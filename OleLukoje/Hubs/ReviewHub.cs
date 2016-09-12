using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OleLukoje.Hubs
{
    public class ReviewHub : Hub
    {
        public void Send()
        {
            Clients.Others.newReview();
        }
    }
}