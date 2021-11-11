using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoginandR.Hubs
{
    /// <summary>
    /// DemoChat class derived from hub
    /// </summary>
    [HubName("chat")]
    public class DemoChat : Hub
    {
        /// <summary>
        /// Function used to connect all clients
        /// </summary>
        public void Hello()
        {
            Clients.All.hello();
        }

        /// <summary>
        /// Function used to connect caller
        /// </summary>
        /// <param name="name">account name</param>
        public void Connect(string name)
        {
            Clients.Caller.connect(name);
        }

        /// <summary>
        /// Function used to send message
        /// </summary>
        /// <param name="name">account name</param>
        /// <param name="message">message content</param>
        public void Message(string name, string message)
        {
            Clients.All.message(name, message);
        }
    }
}