
using Microsoft.Extensions.Caching.Memory;

namespace ProjectManagementSystem.Api.Features.Common.OTPService
{
    public class OTPService : IOTPService
    {
        private readonly IMemoryCache _memoryCache;

        public OTPService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        public string GenerateOTP()
        {
            var random = new Random();
            return random.Next(100000, 999999).ToString();
        }
        public void SaveOTP(string email, string otp)
        {
            _memoryCache.Set(email, otp, TimeSpan.FromMinutes(10));
        }

        public string GetOTP(string email)
        {
            var otp = _memoryCache.Get<string>(email);
            return otp;
        }

        public Task<bool> IsOTPExpiredAsync(string email)
        {
            var otp = _memoryCache.Get<string>(email);
            return Task.FromResult(string.IsNullOrEmpty(otp));
        }

        public Task<bool> VerifyOTPAsync(string email, string otp)
        {
            var storedOtp = _memoryCache.Get<string>(email);
            return Task.FromResult(storedOtp == otp);
        }
    }
}
