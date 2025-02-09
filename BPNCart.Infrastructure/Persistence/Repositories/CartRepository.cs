using BPNCart.Application.Persistence.Repositories;
using BPNCart.Domain.Entities;
using BPNCart.Infrastructure.Persistence.DbContexts;
using MongoDB.Driver;

namespace BPNCart.Infrastructure.Persistence.Repositories;
public class CartRepository(MongoDbContext dbContext) : ICartRepository
{
    private readonly MongoDbContext _dbContext = dbContext;
    private readonly IMongoCollection<Cart> _cartCollection = dbContext.GetCollection<Cart>("cart");

    public async Task<Cart> GetCart(int userId)
    {
        return await _cartCollection.Find(c => c.UserId == userId).FirstOrDefaultAsync();
    }

    public async Task<bool> DoesProductExistAsync(int userId, string barcode)
    {
        var cart = await _cartCollection.Find(c => c.UserId == userId).FirstOrDefaultAsync();

        if (cart == null)
            return false;

        var existingProduct = cart.Products.FirstOrDefault(c => c.Barcode == barcode);

        if (existingProduct == null)
            return false;
        else
            return true;
    }

    public async Task<bool> AddProductAsync(int userId, Product product)
    {
        var cart = await _cartCollection.Find(c => c.UserId == userId).FirstOrDefaultAsync();

        if (cart == null)
            return false;

        cart.Products.Add(product);

        var update = Builders<Cart>.Update.Set(c => c.Products, cart.Products);
        await _cartCollection.UpdateOneAsync(c => c.UserId ==  userId, update);
        return true;
    }

    public async Task<bool> UpdateProductQuantityAsync(int userId, Product product)
    {
        var cart = await _cartCollection.Find(c => c.UserId == userId).FirstOrDefaultAsync();

        if (cart == null)
            return false;

        var existingProduct = cart.Products.FirstOrDefault(c => c.Barcode == product.Barcode);
        if (existingProduct != null) 
            existingProduct.Quantity += product.Quantity; //

        var update = Builders<Cart>.Update.Set(c => c.Products, cart.Products);
        await _cartCollection.UpdateOneAsync(c => c.UserId == userId, update);
        return true;
    }
}
