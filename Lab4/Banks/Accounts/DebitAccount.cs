using Banks.Banks;
using Banks.Clients;
using Banks.Tools;

namespace Banks.Accounts;

public class DebitAccount : Account
{
    private const int CountOfDaysInYear = 365;
    private const int MaximumValueOfPercent = 100;
    private const int CountOfDaysInMonth = 30;
    private const int MinimumMoneyAndPercentCount = 0;
    private double _money = 0;
    private double _percent;
    private double _accruedBalance = 0;
    private double _lastTransaction;
    private double _limitForDoubtfulAccount;
    private int _passedDays = 0;
    private int _id;

    public DebitAccount(Bank bank, Client client, int id)
    {
        if (bank == null)
            throw new BanksException("Incorrect value of bank!");
        if (client == null)
            throw new BanksException("Incorrect value of client!");
        AccountClient = client;
        AccountBank = bank;
        _percent = bank.Conditions.DebitPercent;
        _limitForDoubtfulAccount = bank.Conditions.LimitForDoubtfulAccount;
        _id = id;
        client.AddAccount(this);
    }

    public Client AccountClient { get; }
    public Bank AccountBank { get; }
    public double Percent => _percent;
    public override double Balance() => _money;
    public override int Id() => _id;
    public override void AddMoney(double newMoney)
    {
        if (newMoney < MinimumMoneyAndPercentCount)
            throw new BanksException("You haven't added any money!");
        _money += newMoney;
    }

    public override void TakeOffMoney(double removeMoney)
    {
        if (removeMoney <= MinimumMoneyAndPercentCount)
            throw new BanksException("Incorrect withdrawal amount!");
        if (_money < removeMoney)
            throw new BanksException("Not enough money in the account!");
        if (!AccountClient.IsVarified() && removeMoney > _limitForDoubtfulAccount)
            throw new BanksException("Your account isn't varified!");
        _money -= removeMoney;
    }

    public override double GetLastTransaction()
    {
        return _lastTransaction;
    }

    public override void LastTransaction(double money)
    {
        _lastTransaction = money;
    }

    public void ChangeThePercentRate(double newPercentRate)
    {
        if (newPercentRate <= MinimumMoneyAndPercentCount)
            throw new BanksException("Incorrect value of percent rate");
        _percent = newPercentRate;
    }

    public void CalculateTheDailyPercentage(int countDays)
    {
        _accruedBalance += (_percent / (MaximumValueOfPercent * CountOfDaysInYear)) * _money * countDays;
    }

    public void ChargeMonthlyPercent()
    {
        _money += _accruedBalance;
        _accruedBalance = MinimumMoneyAndPercentCount;
    }

    public override void DaysPassed(int countDays)
    {
        CalculateTheDailyPercentage(countDays);
        _passedDays += countDays;
        if (_passedDays >= CountOfDaysInMonth)
        {
            ChargeMonthlyPercent();
            _passedDays -= CountOfDaysInMonth;
        }
    }

    public override double GetPercent() => _percent;
    public override Bank GetBank() => AccountBank;
}