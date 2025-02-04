using FluentValidation;

namespace ProjectManagementSystem.Api.Features.UserManagement.GetUsers;

public class UserParam
{
    private const int MaxPageSize = 30;
    private int _pageSize = 5;
    private int _pageNumber = 10;
    public int PageNumber
    {
        get => _pageNumber;
        set => _pageNumber = value < 1 ? 1 : value;
    }
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
