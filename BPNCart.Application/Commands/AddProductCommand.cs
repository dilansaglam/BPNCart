﻿using BPNCart.Domain.Entities;
using BPNCart.Domain.Responses.Base;
using MediatR;

namespace BPNCart.Application.Commands;
public class AddProductCommand : IRequest<BaseResponse>
{
    public required int UserId { get; set; }
    public required Product Product { get; set; }
}
