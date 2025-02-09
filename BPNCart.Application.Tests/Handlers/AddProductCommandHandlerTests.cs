using Bogus;
using BPNCart.Application.Commands;
using BPNCart.Application.ExternalServices;
using BPNCart.Application.Handlers;
using BPNCart.Application.Persistence.Repositories;
using BPNCart.Domain.Entities;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace BPNCart.Application.Tests.Handlers;
public class AddProductCommandHandlerTests
{
    private readonly AddProductCommandHandler _handler;

    private readonly Mock<IValidator<AddProductCommand>> _validatorMock;
    private readonly Mock<IStockHttpClient> _stockHttpClientMock;
    private readonly Mock<ICartRepository> _cartRepositoryMock;

    private readonly Faker<AddProductCommand> _commandFaker;
    private readonly Faker<Product> _productFaker;
    private readonly Faker<Cart> _cartFaker;

    public AddProductCommandHandlerTests()
    {
        _validatorMock = new Mock<IValidator<AddProductCommand>>();
        _stockHttpClientMock = new Mock<IStockHttpClient>();
        _cartRepositoryMock = new Mock<ICartRepository>();

        _handler = new AddProductCommandHandler(_validatorMock.Object, _stockHttpClientMock.Object, _cartRepositoryMock.Object);

        _commandFaker = new Faker<AddProductCommand>();
        _productFaker = new Faker<Product>();
        _cartFaker = new Faker<Cart>();
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

    [Fact]
    public async void Should_IncreaseQuantity_When_SameItemExists()
    {
        //Arrange
        var product = _productFaker
            .RuleFor(p => p.Barcode, "123")
            .RuleFor(p => p.Quantity, 1)
            .Generate();
        var request = _commandFaker.RuleFor(c => c.Product, product).Generate();

        var validationResult = new ValidationResult { Errors = [] };
        _validatorMock
            .Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        var stockCount = 5;
        _stockHttpClientMock.Setup(s => s.GetStockCount(It.IsAny<string>())).Returns(stockCount);

        var cart = _cartFaker.RuleFor(c => c.Products, [product]).Generate();
        _cartRepositoryMock.Setup(c => c.DoesProductExistAsync(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(true);
        _cartRepositoryMock.Setup(c => c.UpdateProductQuantityAsync(It.IsAny<int>(), It.IsAny<Product>())).ReturnsAsync(true);

        //Act
        var result = await _handler.Handle(request, CancellationToken.None);

        //Assert
        Assert.True(result.Result);
        _cartRepositoryMock.Verify(c => c.UpdateProductQuantityAsync(It.IsAny<int>(), product), Times.Once());
    }



    [Fact]
    public async void Should_Add()
    {
        //Arrange
        var product = _productFaker
            .RuleFor(p => p.Barcode, "123")
            .RuleFor(p => p.Quantity, 1)
            .Generate();
        var request = _commandFaker.RuleFor(c => c.Product, product).Generate();

        var validationResult = new ValidationResult { Errors = [] };
        _validatorMock
            .Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        var stockCount = 5;
        _stockHttpClientMock.Setup(s => s.GetStockCount(It.IsAny<string>())).Returns(stockCount);

        var cart = _cartFaker.RuleFor(c => c.Products, [product]).Generate();
        _cartRepositoryMock.Setup(c => c.DoesProductExistAsync(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(false);
        _cartRepositoryMock.Setup(c => c.AddProductAsync(It.IsAny<int>(), It.IsAny<Product>())).ReturnsAsync(true);

        //Act
        var result = await _handler.Handle(request, CancellationToken.None);

        //Assert
        Assert.True(result.Result);
        _cartRepositoryMock.Verify(c => c.AddProductAsync(It.IsAny<int>(), product), Times.Once());
    }


}
