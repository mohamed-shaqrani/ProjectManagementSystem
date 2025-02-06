namespace ProjectManagementSystem.Api.Helpers;

public class ProjectParam
{
    private const int MaxPageSize = 30;
    private int _pageSize = 5;
    private int PageNumber { get; set; } = 1;

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
    }


    public string? Title { get; set; }
}
