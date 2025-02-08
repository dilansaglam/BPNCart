namespace BPNCart.Domain.Entities;
public class Cart
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public List<Product>? Products { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now; //set?
    public DateTime UpdatedDate { get; set; }
}
