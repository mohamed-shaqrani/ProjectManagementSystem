using FluentValidation;

namespace ProjectManagementSystem.Api.Features.TasksManagement.Tasks.GetTask;

public class TaskParam
{
    private const int MaxPageSize = 30;
    private int _pageSize = 5;
    public int PageNumber { get; set; } = 1;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
    }

    public string? TaskTitle { get; set; }
    public string? ProjectTitle { get; set; }

    public string? UserName { get; set; }
}
public class TaskParamValidator : AbstractValidator<TaskParam>
{
    public TaskParamValidator()
    {

    }
}
