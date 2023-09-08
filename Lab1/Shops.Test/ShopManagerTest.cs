using Shops.Services;
using Shops.Tools;
using Xunit;

namespace Shops.Test;

public class ShopManagerTest
{
    private ShopManager _shopManager = new ShopManager();

    [Fact]
    public void DeliveryOfProductsToTheShopAndTheirBuying()
    {
        Shop newShop = _shopManager.AddShop("Pyaterochka", "Paramparampam street, 5");
        Assert.Contains(newShop, _shopManager.Shops);
        Product productOne = new Product("Bread", 30, 100);
        Product productTwo = new Product("Milk", 74, 80);
        List<Product> products = new List<Product>() { productOne, productTwo };
        foreach (var product in products)
            _shopManager.AddProductToShop(newShop, product);

        Assert.Contains(productOne, newShop.Products);
        Assert.Contains(productTwo, newShop.Products);
        Person person = _shopManager.AddPerson("Liza", 390);
        _shopManager.BuyingProduct(person, newShop, productOne);
        Assert.Equal(360, person.Money);
        Assert.Equal(99, productOne.Count);
    }

    [Fact]
    public void ChangeProductPrice()
    {
        Shop newShop = _shopManager.AddShop("Pyaterochka", "Paramparampam street, 5");
        Product productOne = new Product("Bread", 30, 100);
        _shopManager.AddProductToShop(newShop, productOne);
        _shopManager.ChangeProductPrice(newShop, productOne, 40);
        Assert.Equal(40, productOne.Price);
    }

    [Fact]
    public void SearchForTheCheapestShop()
    {
        Shop shopOne = _shopManager.AddShop("Pyaterochka", "Paramparampam street, 5");
        Shop shopTwo = _shopManager.AddShop("Lenta", "Pampam prospect, 12");
        Person person = _shopManager.AddPerson("Liza", 400);
        Product productOne = new Product("Bread", 30, 100);
        Product productTwo = new Product("Milk", 70, 19);
        Product productThree = new Product("Chocolate", 50, 50);
        Product productFour = new Product("Cheese", 110, 30);
        List<Product> products = new List<Product>() { productOne, productTwo, productThree };
        foreach (var product in products)
            _shopManager.AddProductToShop(shopOne, product);

        productOne = new Product("Bread", 40, 50);
        productTwo = new Product("Milk", 70, 3);
        products = new List<Product>() { productOne, productTwo, productFour };
        foreach (var product in products)
            _shopManager.AddProductToShop(shopTwo, product);

        List<Shop> shops = new List<Shop>() { shopOne, shopTwo };
        Product neededProductOne = new Product("Bread");
        Product neededProductTwo = new Product("Cheese");
        Dictionary<Product, int> neededProducts = new Dictionary<Product, int>()
            { { neededProductOne, 3 }, { neededProductTwo, 1 } };
        Shop? neededShop = _shopManager.TheCheapestShop(shops, neededProducts);
        Assert.Equal(shopTwo, neededShop);

        Assert.Throws<ShopException>(() =>
        {
            neededProductOne = new Product("Tea");
            neededProductTwo = new Product("Milk");
            neededProducts = new Dictionary<Product, int>()
                { { neededProductOne, 4 }, { neededProductTwo, 2 } };
            neededShop = _shopManager.TheCheapestShop(shops, neededProducts);
        });
    }

    [Fact]
    public void PurchaseOfABatchOfProducts()
    {
        Shop shop = _shopManager.AddShop("Pyaterochka", "Paramparampam street, 5");
        Person person = _shopManager.AddPerson("Liza", 400);
        Product productOne = new Product("Bread", 30, 100);
        Product productTwo = new Product("Milk", 70, 19);
        Product productThree = new Product("Chocolate", 50, 50);
        Product productFour = new Product("Cheese", 110, 30);
        List<Product> products = new List<Product>() { productOne, productTwo, productThree, productFour };
        foreach (var product in products)
            _shopManager.AddProductToShop(shop, product);

        Product neededProductOne = new Product("Bread");
        Product neededProductTwo = new Product("Cheese");
        Dictionary<Product, int> neededProducts = new Dictionary<Product, int>()
            { { neededProductOne, 3 }, { neededProductTwo, 1 } };
        _shopManager.BuyingProducts(person, shop, neededProducts);
        Assert.Equal(200, person.Money);
        Assert.Equal(97, productOne.Count);
        Assert.Equal(29, productFour.Count);

        Assert.Throws<ShopException>(() =>
        {
            neededProductOne = new Product("Bread");
            neededProductTwo = new Product("Tea");
            neededProducts = new Dictionary<Product, int>()
                { { neededProductOne, 4 }, { neededProductTwo, 2 } };
            _shopManager.BuyingProducts(person, shop, neededProducts);
        });
    }
}