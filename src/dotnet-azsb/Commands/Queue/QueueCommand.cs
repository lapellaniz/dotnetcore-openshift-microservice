using System.Threading.Tasks;
using Azure.ServiceBus.Tools.Commmands;
using McMaster.Extensions.CommandLineUtils;

namespace Azure.ServiceBus.Tools.Commands.Queue
{
    [Command("queue", Description = "Manage queue")]
    [Subcommand("sas", typeof(QueueAccessTokenCommand))]
    internal class QueueCommand : CommandBase
    {
        public QueueCommand(IConsole console) : base(console)
        {
        }

        protected override Task<int> OnExecute(CommandLineApplication app)
        {
            return OnProgramErrorAsync(app);
        }
    }
}