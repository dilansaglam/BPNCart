using Bogus;
using BPNCart.Application.Commands;
using BPNCart.Application.Handlers;
using BPNCart.Domain.Entities;
using FluentValidation;
using FluentValidation.Results;
using Moq;

namespace BPNCart.Application.Tests.Handlers;
public class AddProductCommandHandlerTests
{
    private readonly AddProductCommandHandler _handler;

    private readonly Mock<IValidator<AddProductCommand>> _validatorMock;

    private readonly Faker<AddProductCommand> _commandFaker;
    private readonly Faker<Product> _productFaker;

    public AddProductCommandHandlerTests()
    {
        _validatorMock = new Mock<IValidator<AddProductCommand>>();

        _handler = new AddProductCommandHandler(_validatorMock.Object);

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
        var validationResult = new FluentValidation.Results.ValidationResult(validationFailureList);

        _validatorMock
            .Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        //Act
        var result = await _handler.Handle(request, CancellationToken.None);

        //Assert
        Assert.False(result.Result);
        Assert.Equal($"Request is not valid. {errorMessage}", result.Message);
    }

    //[Fact]
    //public void Should_ReturnFalse_When_StockUnavailable()
    //{

    //}

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
