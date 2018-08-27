using System.Threading.Tasks;
using Azure.ServiceBus.Tools.Commmands;
using McMaster.Extensions.CommandLineUtils;

namespace Azure.ServiceBus.Tools.Commands.Queue
{
    [Command("sas", Description = "Manage queue sas token")]
    [Subcommand("create", typeof(QueueCreateAccessTokenCommand))]
    internal class QueueAccessTokenCommand : CommandBase
    {
        public QueueAccessTokenCommand(IConsole console) : base(console)
        {
        }

        protected override Task<int> OnExecute(CommandLineApplication app)
        {
            return OnProgramErrorAsync(app);
        }
    }
}