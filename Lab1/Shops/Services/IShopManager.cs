namespace Shops.Services;

public interface IShopManager
{
    Shop AddShop(string shopName, string shopAdress);
    Person AddPerson(string personName, int money);
    Shop GetShop(int shopId, string shopName);
    Shop? FindShop(int shopId, string shopName);
    Person GetPerson(int personId, string personName);
    Person? FindPerson(int personId, string personName);
    void AddProductToShop(Shop shop, Product newProduct);
    public void BuyingProduct(Person person, Shop shop, Product product);
    Product ChangeProductPrice(Shop shop, Product product, int newPrice);
    Shop? TheCheapestShop(List<Shop> shops, Dictionary<Product, int> products);
    void BuyingProducts(Person person, Shop shop, Dictionary<Product, int> products);
}
