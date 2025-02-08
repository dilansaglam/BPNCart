namespace BPNCart.Domain.Responses.Base;
public class BaseResponse
{
    public BaseResponse() { }

    public BaseResponse(bool isSuccess, string? message)
    {
        Result = isSuccess;
        Message = message;
    }

    public bool Result { get; set; }
    public string? Message { get; set; }
}
