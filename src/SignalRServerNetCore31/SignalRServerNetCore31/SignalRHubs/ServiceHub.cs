using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRServerNetCore31.SignalRHubs
{

    public class ServiceHub : Hub
    {
        private readonly static ConnectionMapping<string> _connections =
            new ConnectionMapping<string>();

        public override async Task OnConnectedAsync()
        {
            //string name = Context.User.Identity.Name;
            string name = "testUser";

            _connections.Add(name, Context.ConnectionId);

            await RegisterGroupAsync(name);
            await RegisterGroupAsync(Constants.GROUP_SIGNALR);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            //string name = Context.User.Identity.Name;
            string name = "testUser";

            _connections.Remove(name, Context.ConnectionId);

            await UnRegisterGroupAsync(name);
            await UnRegisterGroupAsync(Constants.GROUP_SIGNALR);
            await base.OnDisconnectedAsync(exception);
        }

        public async Task RegisterGroupAsync(string group)
        {
            await Task.Run(() =>
            {
                //string name = Context.User.Identity.Name;
                string name = "testUser";
                var conectionsUser = _connections.GetConnections(name).ToList();
                conectionsUser.ForEach(cnn =>
                {
                    Groups.AddToGroupAsync(cnn, group).Wait();
                });
            });
        }

        public async Task UnRegisterGroupAsync(string group)
        {
            await Task.Run(() =>
            {
                //string name = Context.User.Identity.Name;
                string name = "testUser";
                var conectionsUser = _connections.GetConnections(name).ToList();
                conectionsUser.ForEach(cnn =>
                {
                    Groups.RemoveFromGroupAsync(cnn, group);
                });
            });
        }

        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }

        public async Task SendMessageToUserGroup(string group, string message)
        {
            await Clients.Group(group).SendAsync("ReceiveMessage", message);
        }

        public async Task SendMessageToUser(string user, string message)
        {
            var conectionsUser = _connections.GetConnections(user).ToList();
            if (conectionsUser.Count > 0)
            {
                await Clients.Users(conectionsUser).SendAsync("ReceiveMessage", message);
            }
        }

        public async Task SendObjectToUser(string user)
        {
            var conectionsUser = _connections.GetConnections(user).ToList();
            if (conectionsUser.Count > 0)
            {
                await Clients.Clients(conectionsUser).SendAsync("ReceiveObjectMessage", new { id = 1, descricao = "teste de envio de objeto" });
            }
        }
    }
}
