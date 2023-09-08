using Shops.Tools;

namespace Shops.Services;

public class ShopManager : IShopManager
{
    private const int MinimalValueOfId = 1;
    private static int _idShop = 1;
    private static int _idPerson = 1;
    private List<Shop> _shops = new List<Shop>();
    private List<Person> _persons = new List<Person>();
    public IReadOnlyList<Shop> Shops => _shops;
    public Shop AddShop(string shopName, string shopAdress)
    {
        Shop newShop = new Shop(shopName, _idShop, shopAdress);
        if (_shops.Contains(newShop))
            throw new ShopException("This shop has already been created");

        foreach (var shop in _shops)
        {
            if (shop.Adress == shopAdress)
                throw new ShopException("This address is already occupied");
        }

        _shops.Add(newShop);
        ++_idShop;
        return newShop;
    }

    public Person AddPerson(string personName, int money)
    {
        Person newPerson = new Person(personName, money, _idPerson);
        _persons.Add(newPerson);
        ++_idPerson;
        return newPerson;
    }

    public Shop GetShop(int shopId, string shopName)
    {
        if (shopId < MinimalValueOfId || string.IsNullOrWhiteSpace(shopName))
            throw new ShopException("Invalid data");
        var selectedShop = _shops
           .SingleOrDefault(shop => shop.Id == shopId);
        if (selectedShop == null)
            throw new ShopException("Shop not found");

        if (selectedShop.Name != shopName)
               throw new ShopException("Check if the ID and Name match correctly");

        return selectedShop;
    }

    public Shop? FindShop(int shopId, string shopName)
    {
        var selectedShop = _shops
            .SingleOrDefault(shop => shop.Id == shopId);
        if (selectedShop?.Name != shopName)
            return null;

        return selectedShop;
    }

    public Person GetPerson(int personId, string personName)
    {
        if (personId < MinimalValueOfId || string.IsNullOrWhiteSpace(personName))
            throw new ShopException("Invalid data");
        var selectedPerson = _persons
            .SingleOrDefault(person => person.Id == personId);
        if (selectedPerson == null)
            throw new ShopException("Shop not found");

        if (selectedPerson.Name != personName)
            throw new ShopException("Check if the ID and Name match correctly");

        return selectedPerson;
    }

    public Person? FindPerson(int personId, string personName)
    {
        var selectedPerson = _persons
            .SingleOrDefault(person => person.Id == personId);
        if (selectedPerson?.Name != personName)
            return null;

        return selectedPerson;
    }

    public void AddProductToShop(Shop shop, Product newProduct)
    {
        if (newProduct == null || shop == null)
            throw new ShopException("Invalid data");
        shop.AddProduct(newProduct);
    }

    public void BuyingProduct(Person person, Shop shop, Product product)
    {
        if (shop == null || person == null || product == null)
            throw new ShopException("Invalid data");
        if (shop.FindProduct(product) == null)
            throw new ShopException("There is no such product in the shop");
        if (person.Money < product.Price)
            throw new ShopException("Not enough money");
        shop.AddMoney(product.Price);
        person.WasteOfMoney(product.Price);
        shop.ReduceProduct(product);
    }

    public Product ChangeProductPrice(Shop shop, Product product, int newPrice)
    {
        if (shop == null || product == null)
            throw new ShopException("Invalid data");
        Product? tempProduct = shop.FindProduct(product);
        if (tempProduct == null)
            throw new ShopException("There is no such product");
        tempProduct.GhangePrice(newPrice);
        return tempProduct;
    }

    public int TotalCost(Shop shop, Dictionary<Product, int> products)
    {
        if (shop == null)
            throw new ShopException("Invalid data");
        Product? tempProduct;
        int cost = 0;
        foreach (var product in products)
        {
            tempProduct = shop.FindProduct(product.Key);
            if (tempProduct == null)
                return -1;
            if (tempProduct.Count < product.Value)
                return -1;
            cost += tempProduct.Price * product.Value;
        }

        return cost;
    }

    public void BuyingProducts(Person person, Shop shop, Dictionary<Product, int> products)
    {
        if (shop == null || person == null)
            throw new ShopException("Invalid data");
        int totalCost = TotalCost(shop, products);
        if (totalCost == -1)
            throw new ShopException("Not enough product");
        if (person.Money < totalCost)
            throw new ShopException("Not enough money");
        Product? tempProduct;
        foreach (var product in products)
        {
            tempProduct = shop.FindProduct(product.Key);
            tempProduct?.ReduceCount(product.Value);
        }

        person.WasteOfMoney(totalCost);
    }

    public Shop? TheCheapestShop(List<Shop> shops, Dictionary<Product, int> products)
    {
        int minimalCost = -1;
        Shop? shopWithMinimalCost = null;
        foreach (Shop shop in shops)
        {
            if (shop == null)
                throw new ShopException("There is no such shop");
            int costOfProducts = TotalCost(shop, products);
            if (minimalCost == -1 && costOfProducts > -1)
            {
                minimalCost = costOfProducts;
                shopWithMinimalCost = shop;
            }

            if (costOfProducts > -1 && minimalCost > costOfProducts)
            {
                minimalCost = costOfProducts;
                shopWithMinimalCost = shop;
            }
        }

        if (minimalCost == -1)
            throw new ShopException("Couldn't find needed shop");
        return shopWithMinimalCost;
    }
}