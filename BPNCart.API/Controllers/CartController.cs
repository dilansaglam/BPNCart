using BPNCart.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BPNCart.API.Controllers;

[Route("cart")]
[ApiController]
public class CartController : Controller
{
    [HttpPost("add")]
    public bool AddProduct(int userId, Product product) //async? 
    {
        return true; //
    }
}
