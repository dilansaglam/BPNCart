using BPNCart.Domain.Entities.Base;

namespace BPNCart.Domain.Entities;
public class Cart : BaseEntity
{
    public Cart()
    {
        CreatedDate = DateTime.Now;
        Products = []; //
    }

    public int UserId { get; set; }
    public List<Product> Products { get; set; }
}
