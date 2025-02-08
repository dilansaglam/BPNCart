using BPNCart.Application.Commands;
using BPNCart.Domain.Responses.Base;
using FluentValidation;
using MediatR;

namespace BPNCart.Application.Handlers;
public class AddProductCommandHandler(IValidator<AddProductCommand> validator) : IRequestHandler<AddProductCommand, BaseResponse>
{
    private readonly IValidator<AddProductCommand> _validator = validator;

    public async Task<BaseResponse> Handle(AddProductCommand request, CancellationToken cancellationToken) //cancellationToken?
    {
        var result = await _validator.ValidateAsync(request, cancellationToken);
        if (!result.IsValid)
            return new BaseResponse { Result = false, Message = $"Request is not valid. {result}" };

        return new BaseResponse { Result = true };
    }
}
