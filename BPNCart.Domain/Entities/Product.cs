using BPNCart.Domain.Entities.Base;

namespace BPNCart.Domain.Entities;
public class Product : BaseEntity
{
    public Product()
    {
        CreatedDate = DateTime.Now;
    }

    public required string Barcode { get; set; }
    public int Quantity { get; set; }
    public double Price { get; set; }
}
