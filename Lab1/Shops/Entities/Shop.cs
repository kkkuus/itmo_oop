using Shops.Tools;

namespace Shops;

public class Shop
{
    private const int _minimalValueOfMoney = 0;
    private int _money;
    private List<Product> _products = new List<Product>();
    public Shop(string name, int id, string adress)
    {
        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(adress))
            throw new ShopException("Invalid value of name/adress");
        Name = name;
        Id = id;
        Adress = adress;
    }

    public IReadOnlyList<Product> Products => _products;

    public string Name { get; }
    public int Id { get; }
    public string Adress { get; }
    public void AddProduct(Product newProduct)
    {
        if (newProduct == null)
            throw new ShopException("Invalid value of newProduct");
        Product? tempProduct = _products.Find(item => item.Name == newProduct.Name);
        if (tempProduct != null)
            tempProduct.AddCount(newProduct.Count);
        else
            _products.Add(newProduct);
    }

    public void ReduceProduct(Product product)
    {
        if (product == null)
            throw new ShopException("Invalid data");
        int countOfReducedProduct = 1;
        Product? tempProduct = _products.Find(item => item.Name == product.Name);
        tempProduct?.ReduceCount(countOfReducedProduct);
    }

    public void AddMoney(int newMoney)
    {
        if (newMoney < _minimalValueOfMoney)
            throw new ShopException("Invalid value of newMoney");
        this._money += newMoney;
    }

    public Product? FindProduct(Product product)
    {
        return _products.Find(item => item.Name == product.Name);
    }
}