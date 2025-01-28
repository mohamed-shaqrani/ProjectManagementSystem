using ProjectManagementSystem.Api.Response;

namespace HotelManagement.Core.ViewModels.Response;
public static class ResponseMessage
{
    private static readonly Dictionary<SuccessCode, string> SuccessMessages = new()
    {
        // AUTH
        { SuccessCode.ChangePasswordUpdated, "Password changed successfully." },
        { SuccessCode.LoginCorrectly, "The user logged in successfully." },


        // USER
        { SuccessCode.UserCreated, "User created successfully." },
        { SuccessCode.UsersRetrieved, "Users retrieved successfully." },
        { SuccessCode.UserDetailsRetrieved, "User details retrieved successfully." },
        { SuccessCode.UserDeleted, "Course deleted successfully."  },
        { SuccessCode.AdminCreated, "Admin created successfully." },


    };

    private static readonly Dictionary<ErrorCode, string> ErrorMessages = new()
    {
         { ErrorCode.DataBaseError, "A database error occurred while processing the request. Please try again later or contact support if the issue persists." },

        { ErrorCode.ValidationError, "Validation failed: One or more fields contain invalid values. Please review the errors and try again." },

        // Auth
        { ErrorCode.IncorrectPassword, "The specified password is not correct." },


        // USER
        { ErrorCode.UserNotFound, "The specified user could not be found." },
        { ErrorCode.UserNameExist, "The specified username is already in use." },
        { ErrorCode.UserEmailExist, "The specified Email is already in use." },
        { ErrorCode.UserPhoneExist, "The specified Phone is already in use." },


    };


    public static string Success(SuccessCode code)
    {
        return SuccessMessages.TryGetValue(code, out string? message) ?
            message : "Operation completed successfully.";
    }

    public static string Failure(ErrorCode code)
    {
        return ErrorMessages.TryGetValue(code, out string? message) ?
            message : "An unexpected error occurred. Please contact support.";
    }
}
