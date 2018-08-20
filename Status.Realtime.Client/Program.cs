using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;

namespace Status.Realtime.Client
{
    internal class Program
    {
        private static string ServiceName => "Status.Monitor.Client";
        private static CancellationTokenSource cancelationTokenSource;
        private static ProcessClient processClient;
        private static HubConnection connection;

        public class Service : ServiceBase
        {
            public Service()
            {
                ServiceName = Program.ServiceName;
            }

            protected override void OnStart(string[] args)
            {
                Program.Start(args);
            }

            protected override void OnStop()
            {
                Program.Stop();
            }
        }

        private static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                  .SetBasePath(Directory.GetCurrentDirectory())
                  .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();

            connection = new HubConnectionBuilder()
                .WithUrl(configuration.GetSection("HubHost").Value)
                .Build();

            processClient = new ProcessClient(configuration, connection);
            cancelationTokenSource = new CancellationTokenSource();

            if (!Environment.UserInteractive)
            {
                // running as service
                using (var service = new Service())
                {
                    ServiceBase.Run(service);
                }
            }
            else
            {
                Start(args);

                Console.WriteLine("Press any key to stop...");
                Console.ReadKey(true);

                Stop();
            }
        }

        private static void Start(string[] args)
        {
            Task.Run(async () =>
            {
                await processClient.ConnectAsync(cancelationTokenSource.Token);
            });
        }

        private static void Stop()
        {
            cancelationTokenSource.Cancel();
        }
    }
}