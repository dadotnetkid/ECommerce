using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Models;
using Models.Repository;

namespace Web.Hubs
{
    [HubName("NotifierHub")]
    public class NotifierHub : Hub
    {
        //public void NotifySmsServer(string contactNumber, string message)
        //{
        //    Clients.All.Notify(new Messages{ ContactNumber = contactNumber, Message = message });
        //}


    }
}