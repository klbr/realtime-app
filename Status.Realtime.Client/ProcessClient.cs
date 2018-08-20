using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Status.Realtime.Domain;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Status.Realtime.Client
{
    internal class ProcessClient
    {
        private readonly IConfigurationRoot configuration;
        private readonly HubConnection connection;

        public ProcessClient(IConfigurationRoot configuration, HubConnection connection)
        {
            this.configuration = configuration;
            this.connection = connection;
        }

        public async Task GetStatus(CancellationToken token)
        {
            var processes = Process.GetProcesses().Select(x => x.ProcessName + ":" + x.Id).ToList();
            var processModel = new ProcessModel
            {
                Id = Environment.MachineName,
                Processes = processes
            };

            await connection.SendAsync("SendStatus", processModel);
        }

        public async Task ConnectAsync(CancellationToken token)
        {
            await Task.Delay(10000);

            connection.On("GetStatus", async () =>
            {
                await GetStatus(token);
            });

            await connection.StartAsync(cancellationToken: token);
        }
    }
}