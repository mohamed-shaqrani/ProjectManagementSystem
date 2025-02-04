namespace ProjectManagementSystem.Api.Response;

public enum ErrorCode
{
    None = 00,
    ValidationError = 01,
    DataBaseError = 02,

    InternalServerError = 500,


    // USER
    UserNotFound = 600,
    UserNameExist = 601,
    UserEmailExist = 602,
    UserEmailNotExist = 603,
    UserPhoneExist = 604,
    InvalidCode = 605,
    UserAlreadyDeactivated = 606,
    // Project
    ProjectExist = 700,
    ProjectNotExist = 701,


    // Task
    TaskNotExist = 800,

    ProjectDoesNotExist = 801,

    ProjectHasTasks = 802,
    //
    // AUTH
    ChangePasswordError = 900,
    IncorrectPassword = 901,
}
