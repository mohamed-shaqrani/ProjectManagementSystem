using HotelManagement.Core.ViewModels.Response;

namespace ProjectManagementSystem.Api.Response;

public class SuccessResponseViewModel<T> : ResponseViewModel<T>
{
    public SuccessResponseViewModel(SuccessCode successCode, T? data = default)
    {
        IsSuccess = true;
        SuccessCode = successCode;
        Data = data;
        Message = ResponseMessage.Success(successCode);

    }
}
