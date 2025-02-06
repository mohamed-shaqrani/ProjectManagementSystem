using FluentValidation;

namespace ProjectManagementSystem.Api.Features.UserManagement.GetUsers;

public class UserParam
{
    private const int MaxPageSize = 30;
    private int _pageSize = 5;
    public int PageNumber { get; set; } = 1;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
    }

    public string? Name { get; set; }

    public string? Email { get; set; }
}
public class UserParamValidator : AbstractValidator<UserParam>
{
    public UserParamValidator()
    {

    }
}
