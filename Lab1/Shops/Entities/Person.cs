using Shops.Tools;

namespace Shops;

public class Person
{
    private const int MinimalValueOfMoney = 0;
    public Person(string name, int money, int id)
    {
        if (string.IsNullOrWhiteSpace(name) || money < MinimalValueOfMoney)
            throw new ShopException("Invalid data");
        Name = name;
        Money = money;
        Id = id;
    }

    public string Name { get; }
    public int Money { get; private set; }
    public int Id { get; }

    public void WasteOfMoney(int amountSpent)
    {
        if (amountSpent < MinimalValueOfMoney)
            throw new ShopException("Invalid value of amountSpent");
        this.Money -= amountSpent;
    }

    public void AddMoney(int newMoney)
    {
        if (newMoney < MinimalValueOfMoney)
            throw new ShopException("Invalid value of newMoney");
        this.Money += newMoney;
    }
}