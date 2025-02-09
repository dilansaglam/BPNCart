using BPNCart.Application.Commands;
using BPNCart.Application.ExternalServices;
using BPNCart.Application.Persistence.Repositories;
using BPNCart.Domain.Responses.Base;
using FluentValidation;
using MediatR;

namespace BPNCart.Application.Handlers;
public class AddProductCommandHandler(
    IValidator<AddProductCommand> validator, 
    IStockHttpClient stockHttpClient,
    ICartRepository cartRepository) : IRequestHandler<AddProductCommand, BaseResponse>
{
    private readonly IValidator<AddProductCommand> _validator = validator;
    private readonly IStockHttpClient _stockHttpClient = stockHttpClient;
    private readonly ICartRepository _cartRepository = cartRepository;

    public async Task<BaseResponse> Handle(AddProductCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return new BaseResponse { Result = false, Message = $"Request is not valid. {validationResult}" };

        var stockCount = _stockHttpClient.GetStockCount(request.Product.Barcode);
        if (stockCount < request.Product.Quantity)
            return new BaseResponse { Result = false, Message = $"Stock is not available. Available stock count: {stockCount}" };

        bool dbResult = false;

        if (await _cartRepository.DoesProductExistAsync(request.UserId, request.Product.Barcode))
            dbResult = await _cartRepository.UpdateProductQuantityAsync(request.UserId, request.Product);
        else
            dbResult = await _cartRepository.AddProductAsync(request.UserId, request.Product);

        if (dbResult) 
            return new BaseResponse { Result = true };
            
        return new BaseResponse { Result = false , Message = "Cart not found."};
    }
}
