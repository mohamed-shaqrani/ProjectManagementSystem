using FluentValidation;

namespace ProjectManagementSystem.Api.Features.TasksManagement.Tasks.GetTask;

public class TaskParam
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

    public string? ProjectTitle { get; set; }

    public string? UserName { get; set; }
}
public class TaskParamValidator : AbstractValidator<TaskParam>
{
    public TaskParamValidator()
    {

    }
}
