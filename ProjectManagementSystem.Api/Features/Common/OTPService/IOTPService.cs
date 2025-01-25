namespace ProjectManagementSystem.Api.Features.Common.OTPService
{
    public interface IOTPService
    {
        string GenerateOTP();
        void SaveOTP(string email, string otp);
        string GetOTP(string email);
        Task<bool> IsOTPExpiredAsync(string email);
        Task<bool> VerifyOTPAsync(string email, string otp);
    }
}
