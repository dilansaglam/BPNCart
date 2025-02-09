using BPNCart.Application.Commands;
using BPNCart.Application.ExternalServices;
using BPNCart.Domain.Responses.Base;
using FluentValidation;
using MediatR;

namespace BPNCart.Application.Handlers;
public class AddProductCommandHandler(
    IValidator<AddProductCommand> validator, 
    IStockHttpClient stockHttpClient) : IRequestHandler<AddProductCommand, BaseResponse>
{
    private readonly IValidator<AddProductCommand> _validator = validator;
    private readonly IStockHttpClient _stockHttpClient = stockHttpClient;

    public async Task<BaseResponse> Handle(AddProductCommand request, CancellationToken cancellationToken)
    {
        var result = await _validator.ValidateAsync(request, cancellationToken);
        if (!result.IsValid)
            return new BaseResponse { Result = false, Message = $"Request is not valid. {result}" };

        var stockCount = _stockHttpClient.GetStockCount(request.Product.Barcode);  //check stock or product first?
        if (stockCount < request.Product.Quantity)
            return new BaseResponse { Result = false, Message = $"Stock is not available. Available stock count: {stockCount}" };

        return new BaseResponse { Result = true };
    }
}
