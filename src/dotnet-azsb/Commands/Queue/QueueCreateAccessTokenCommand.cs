using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Azure.ServiceBus.Tools.Commmands;
using McMaster.Extensions.CommandLineUtils;

namespace Azure.ServiceBus.Tools.Commands.Queue
{
    [Command("create", Description = "Create new queue sas token")]
    internal class QueueCreateAccessTokenCommand : CommandBase
    {
        public QueueCreateAccessTokenCommand(IConsole console) : base(console)
        {
        }

        [Required(ErrorMessage = "You must specify the url to Azure Service Bus queue.")]
        [Argument(0, Name = "url", Description = "Url to Azure Service Bus queue.")]
        [Url]
        public string QueueUrl { get; set; }

        [Option(CommandOptionType.SingleValue, Description = "Secure key name.", ShortName = "n")]
        public string KeyName { get; set; }

        [Option(CommandOptionType.SingleValue, Description = "Access key for the specified key name.", ShortName = "k")]
        public string AccessKey { get; set; }

        protected override async Task<int> OnExecute(CommandLineApplication app)
        {
            var token = GetSasToken(QueueUrl, KeyName, AccessKey, TimeSpan.FromDays(1));
            await WriteInformationAsync($"Token: <<{token}>>");
            return CommandExitCodes.Ok;
        }

        public static string GetSasToken(string resourceUri, string keyName, string key, TimeSpan ttl)
        {
            var expiry = GetExpiry(ttl);
            var stringToSign = $"{HttpUtility.UrlEncode(resourceUri)}\n{expiry}";
            var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key));
            var signature = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(stringToSign)));
            var sasToken = string.Format(CultureInfo.InvariantCulture, "SharedAccessSignature sr={0}&sig={1}&se={2}&skn={3}",
                HttpUtility.UrlEncode(resourceUri), HttpUtility.UrlEncode(signature), expiry, keyName);
            return sasToken;
        }

        private static string GetExpiry(TimeSpan ttl)
        {
            var expirySinceEpoch = DateTime.UtcNow - new DateTime(1970, 1, 1) + ttl;
            return Convert.ToString((int)expirySinceEpoch.TotalSeconds);
        }
    }
}
