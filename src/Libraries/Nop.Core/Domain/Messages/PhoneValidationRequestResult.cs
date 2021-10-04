namespace Nop.Core.Domain.Messages
{
    public class PhoneValidationRequestResult
    {
        public string Id { get; set; }

        public string Type { get; set; }

        public string Phone { get; set; }

        public string From { get; set; }
    }
}