using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Azure.ServiceBus.Tools.Commmands;
using McMaster.Extensions.CommandLineUtils;
using Azure.ServiceBus.Tools.Commands.Queue;

namespace Azure.ServiceBus.Tools.Commands
{
    [Command("dotnet-azsb")]
    [VersionOptionFromMember("--version", MemberName = nameof(GetVersion))]
    [Subcommand("queue", typeof(QueueCommand))]
    internal class AzureServiceBusCommand : CommandBase
    {
        public AzureServiceBusCommand(IConsole console) : base(console)
        {
        }

        protected override Task<int> OnExecute(CommandLineApplication app)
        {
            return OnProgramErrorAsync(app);
        }

        private static string GetVersion()
            => typeof(AzureServiceBusCommand).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
    }
}
