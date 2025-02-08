namespace BPNCart.Domain.Entities;
public class Product
{
    public required string ProductBarcode { get; set; } // id?
    public int Quantity { get; set; }
    public double Price { get; set; }
}
