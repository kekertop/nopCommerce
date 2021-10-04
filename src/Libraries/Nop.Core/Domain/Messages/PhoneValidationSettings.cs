using Nop.Core.Configuration;

namespace Nop.Core.Domain.Messages
{
    public class PhoneValidationSettings : ISettings
    {
        public string DefaultValidationType { get; set; }

        public string DefaultFromAccount { get; set; }

        public string DefaultMessageBody { get; set; }

        public string Token { get; set; }
    }
}