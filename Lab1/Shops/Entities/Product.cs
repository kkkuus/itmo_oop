using Shops.Tools;

namespace Shops;

public class Product
{
    private const int _minimalValueOfPrice = 0;
    private const int _minimalValueOfCount = 0;
    public Product(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ShopException("Invalid name");
        Name = name;
    }

    public Product(string name, int price, int count)
    {
        if (string.IsNullOrWhiteSpace(name) || price < _minimalValueOfPrice || count < _minimalValueOfCount)
            throw new ShopException("Invalid data");
        Name = name;
        Price = price;
        Count = count;
    }

    public string Name { get; }
    public int Price { get; private set; }
    public int Count { get; private set; }

    public void AddCount(int newCount)
    {
        if (newCount < _minimalValueOfCount)
            throw new ShopException("Invalid value of newCount");
        this.Count += newCount;
    }

    public void ReduceCount(int count)
    {
        if (count < _minimalValueOfCount)
            throw new ShopException("Invalid value of count");
        this.Count -= count;
    }

    public void GhangePrice(int newPrice)
    {
        if (newPrice < _minimalValueOfPrice)
            throw new ShopException("Invalid value of newPrice");
        this.Price = newPrice;
    }
}