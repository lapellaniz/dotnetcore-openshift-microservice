using System;
using System.Threading.Tasks;
using Azure.ServiceBus.Tools.Commands;
using McMaster.Extensions.CommandLineUtils;

namespace Azure.ServiceBus.Tools
{
    internal static class Program
    {
        public static Task<int> Main(string[] args)
        {
            try
            {
                return CommandLineApplication.ExecuteAsync<AzureServiceBusCommand>(args);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Error.WriteLine($"Unexpected error: {ex}");
                Console.ResetColor();
                return Task.FromResult(CommandExitCodes.Exception);
            }
        }
    }
}
