namespace ProjectManagementSystem.Api.Features.Common.OTPService
{
    public interface IOTPService
    {
        string GenerateOTP();

        void SaveOTP(UserTempData user, string otp);
        UserTempData GetTempUser(string otp);
        string GetOTP(string email);
        Task<bool> IsOTPExpiredAsync(string email);
        Task<bool> VerifyOTPAsync(string email, string otp);
    }
}
