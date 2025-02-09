using BPNCart.Domain.Entities;

namespace BPNCart.Application.Persistence.Repositories;
public interface ICartRepository
{
    Task<Cart> GetCart(int userId);
    Task<bool> DoesProductExistAsync(int userId, string barcode);
    Task AddProductAsync(int userId, Product product);
    Task UpdateProductQuantityAsync(int userId, Product product);
}
