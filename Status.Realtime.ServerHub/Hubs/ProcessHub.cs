using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Status.Realtime.Domain;
using Status.Realtime.ServerHub.Processes;
using System;
using System.Threading.Tasks;

namespace Status.Realtime.ServerHub.Hubs
{
    public class ProcessHub : Hub
    {
        public Task SendStatus(ProcessModel process)
        {
            var manager = ProcessesManager.Create();

            process.ConnectionId = Context.ConnectionId;
            manager.RegisterProcess(process);

            var allProcesses = manager.GetAll();
            var data = JsonConvert.SerializeObject(allProcesses);

            return Clients.All.SendAsync("OnReceiveStatus", data.Replace(" ", ""));
        }

        public override Task OnConnectedAsync()
        {
            Console.WriteLine($"Connected: {Context.ConnectionId}");
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            Console.WriteLine($"Disconnected: {Context.ConnectionId}");
            return base.OnDisconnectedAsync(exception);
        }
    }
}