using System;
using System.Collections.Generic;
using System.Text;

namespace Azure.ServiceBus.Tools.Commands
{
    internal static class CommandExitCodes
    {
        internal static readonly int Exception = 2;
        internal static readonly int Error = 1;
        internal static readonly int Ok = 0;
    }
}
