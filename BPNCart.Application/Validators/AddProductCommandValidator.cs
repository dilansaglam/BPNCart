using BPNCart.Application.Commands;
using FluentValidation;

namespace BPNCart.Application.Validators;
public class AddProductCommandValidator : AbstractValidator<AddProductCommand>
{
    public AddProductCommandValidator()
    {
        RuleFor(a => a.Product.Barcode).NotNull().NotEmpty();
        RuleFor(a => a.Product.Quantity).GreaterThanOrEqualTo(1);
    }
}
