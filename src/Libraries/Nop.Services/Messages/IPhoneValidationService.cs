using System.Threading.Tasks;
using Nop.Core.Domain.Messages;

namespace Nop.Services.Messages
{
    public interface IPhoneValidationService
    {
        Task<PhoneValidationRequestResult> SendValidationRequestAsync(string phone);
        
        Task<PhoneCodeVerificationRequestResult> SendCodeVerificationRequestAsync(string requestId, string code);
    }
}