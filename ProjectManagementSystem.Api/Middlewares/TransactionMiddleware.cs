using Microsoft.EntityFrameworkCore.Storage;
using ProjectManagementSystem.Api.Data;

namespace ProjectManagementSystem.Api.Middlewares;

public class TransactionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly AppDbContext _appDbContext;
    private readonly ILogger<TransactionMiddleware> _logger;

    public TransactionMiddleware(RequestDelegate next, AppDbContext appDbContext, ILogger<TransactionMiddleware> logger)
    {
        _next = next;
        _appDbContext = appDbContext;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContent,AppDbContext appDb)
    {
        IDbContextTransaction transaction = null;
        
        try
        {
            Console.WriteLine("[DEBUG] TransactionMiddleware invoked");

            
            httpContent.Request.EnableBuffering();
            transaction = await _appDbContext.Database.BeginTransactionAsync();
            if (httpContent.Request.Body.CanSeek)
            {
                httpContent.Request.Body.Position = 0;
            }

             
            
            await _next.Invoke(httpContent);
            if (httpContent.Request.Body.CanSeek)
            {
                httpContent.Request.Body.Position = 0;
            }

            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            if (transaction is not null)
                await transaction.RollbackAsync();

            _logger.LogError(ex.Message, "An error occurred while processing the transaction.");
            throw;
        }
    }
}
