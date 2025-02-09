using Bogus;
using BPNCart.Application.Commands;
using BPNCart.Application.ExternalServices;
using BPNCart.Application.Handlers;
using BPNCart.Domain.Entities;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Xunit.Sdk;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace BPNCart.Application.Tests.Handlers;
public class AddProductCommandHandlerTests
{
    private readonly AddProductCommandHandler _handler;

    private readonly Mock<IValidator<AddProductCommand>> _validatorMock;
    private readonly Mock<IStockHttpClient> _stockHttpClientMock;

    private readonly Faker<AddProductCommand> _commandFaker;
    private readonly Faker<Product> _productFaker;

    public AddProductCommandHandlerTests()
    {
        _validatorMock = new Mock<IValidator<AddProductCommand>>();
        _stockHttpClientMock = new Mock<IStockHttpClient>();

        _handler = new AddProductCommandHandler(_validatorMock.Object, _stockHttpClientMock.Object);

        _commandFaker = new Faker<AddProductCommand>();
        _productFaker = new Faker<Product>();
    }

    [Fact]
    public async void Should_ReturnFalse_When_RequestIsNotValid()
    {
        //Arrange
        var request = _commandFaker.Generate();
        
        var errorMessage = "Invalid value";
        var validationFailureList = new List<ValidationFailure> { new ("Property", errorMessage) };
        var validationResult = new ValidationResult(validationFailureList);
        _validatorMock
            .Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        //Act
        var result = await _handler.Handle(request, CancellationToken.None);

        //Assert
        Assert.False(result.Result);
        Assert.Equal($"Request is not valid. {errorMessage}", result.Message);
    }

    [Fact]
    public async void Should_ReturnFalse_When_StockUnavailable()
    {
        //Arrange
        var product = _productFaker
            .RuleFor(p => p.Barcode, "123")
            .RuleFor(p => p.Quantity, 6)
            .Generate();
        var request = _commandFaker.RuleFor(c => c.Product, product).Generate();

        var validationResult = new ValidationResult { Errors = [] };
        _validatorMock
            .Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        var stockCount = 3;
        _stockHttpClientMock.Setup(s => s.GetStockCount(It.IsAny<string>())).Returns(stockCount);

        //Act
        var result = await _handler.Handle(request, CancellationToken.None);

        //Assert
        Assert.False(result.Result);
        Assert.Equal($"Stock is not available. Available stock count: {stockCount}", result.Message);
    }

    //[Fact]
    //public void Should_IncreaseQuantity_When_SameItemAndStockAvailable()
    //{

    //}

    //[Fact]
    //public void Should_ReturnFalse_When_SameItemButStockUnavailable()
    //{

    //}

    //[Fact]
    //public void Should_AddItem()
    //{

    //}


}
