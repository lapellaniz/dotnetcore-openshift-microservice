using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Azure.ServiceBus.Tools.Commands;
using McMaster.Extensions.CommandLineUtils;

namespace Azure.ServiceBus.Tools.Commmands
{
    [HelpOption("--help")]
    internal abstract class CommandBase
    {
        private readonly IConsole _console;
        protected CommandBase(IConsole console)
        {
            _console = console;
        }

        protected virtual Task<int> OnExecute(CommandLineApplication app)
        {
            return Task.FromResult(CommandExitCodes.Ok);
        }

        protected virtual Task<int> OnProgramErrorAsync(CommandLineApplication app)
        {
            return Task.FromResult(HandleProgramError(app));
        }

        protected virtual int OnValidationError(CommandLineApplication app, ValidationResult error)
        {
            WriteError(error.ErrorMessage);
            return HandleProgramError(app);
        }

        private int HandleProgramError(CommandLineApplication app)
        {
            app.ShowHelp();
            return CommandExitCodes.Error;
        }

        protected void WriteError(string message)
        {
            _console.ForegroundColor = ConsoleColor.Red;
            _console.Error.WriteLine(message);
            _console.ResetColor();
        }

        protected void WriteLine(string message)
        {
            _console.Out.WriteLine(message);
        }

        protected async Task WriteLineAsync(string message)
        {
            await _console.Out.WriteLineAsync(message);
        }

        protected void WriteInformation(string message)
        {
            _console.ForegroundColor = ConsoleColor.Green;
            _console.Out.WriteLine(message);
            _console.ResetColor();
        }

        protected async Task WriteInformationAsync(string message)
        {
            _console.ForegroundColor = ConsoleColor.Green;
            await _console.Out.WriteLineAsync(message);
            _console.ResetColor();
        }
    }
}
