using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Nop.Core.Domain.Messages
{
    public class PhoneCodeVerificationRequestResult
    {
        public string Phone { get; set; }

        public bool Validated { get; set; }

        [JsonConverter(typeof(UnixDateTimeConverter))]
        [JsonProperty("validation_date")]
        public DateTime? ValidationDate { get; set; }
    }
}