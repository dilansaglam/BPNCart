using BPNCart.Application.Commands;
using BPNCart.Domain.Responses.Base;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BPNCart.API.Controllers;

[Route("cart")]
[ApiController]
public class CartController(IMediator mediator) : Controller
{
    private readonly IMediator _mediator = mediator;

    [HttpPost("add")]
    public async Task<BaseResponse> AddProduct(AddProductCommand request)
    {
        return await _mediator.Send(request);
    }
}
