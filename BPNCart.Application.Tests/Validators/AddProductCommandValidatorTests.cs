using Bogus;
using BPNCart.Application.Commands;
using BPNCart.Application.Validators;
using BPNCart.Domain.Entities;

namespace BPNCart.Application.Tests.Validators;
public class AddProductCommandValidatorTests
{
    private readonly AddProductCommandValidator _validator;

    private readonly Faker<AddProductCommand> _commandFaker;
    private readonly Faker<Product> _productFaker;

    public AddProductCommandValidatorTests()
    {
        _validator = new AddProductCommandValidator();

        _commandFaker = new Faker<AddProductCommand>();
        _productFaker = new Faker<Product>();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Should_HaveError_When_ProductBarcodeIsNullOrEmpty(string barcode)
    {
        //Arrange
        var product = _productFaker
            .RuleFor(p => p.Barcode, barcode)
            .RuleFor(p => p.Quantity, 1)
            .Generate();
        var command = _commandFaker
            .RuleFor(c => c.Product, product)
            .RuleFor(c => c.UserId, 123)
            .Generate();

        //Act
        var result = _validator.Validate(command);

        //Assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);
        Assert.Equal("Product.Barcode", result.Errors.First().PropertyName);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Should_HaveError_When_ProductQuantityIsLessThanOne(int quantity)
    {
        //Arrange
        var product = _productFaker
            .RuleFor(p => p.Barcode, "123")
            .RuleFor(p => p.Quantity, quantity)
            .Generate();
        var command = _commandFaker
            .RuleFor(c => c.Product, product)
            .RuleFor(c => c.UserId, 123)
            .Generate();

        //Act
        var result = _validator.Validate(command);

        //Assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);
        Assert.Equal("Product.Quantity", result.Errors.First().PropertyName);
    }

    [Fact]
    public void Should_BeValid()
    {
        //Arrange
        var product = _productFaker
            .RuleFor(p => p.Barcode, "123")
            .RuleFor(p => p.Quantity, 1)
            .RuleFor(p => p.Price, 10)
            .Generate();
        var command = _commandFaker
            .RuleFor(c => c.Product, product)
            .RuleFor(c => c.UserId, 123)
            .Generate();

        //Act
        var result = _validator.Validate(command);

        //Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }
}
