using BPNCart.Application.ExternalServices;

namespace BPNCart.Infrastructure.ExternalServices;
public class StockHttpClient : IStockHttpClient
{
    public int GetStockCount(string barcode)
    {
        return 5; // assume http get operation for stock endpoint here 
    }
}
