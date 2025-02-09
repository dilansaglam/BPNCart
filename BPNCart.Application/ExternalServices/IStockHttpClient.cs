namespace BPNCart.Application.ExternalServices;
public interface IStockHttpClient
{
    int GetStockCount(string barcode);
}
