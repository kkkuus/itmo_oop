using Banks.Banks;
using Banks.Clients;
using Banks.Tools;

namespace Banks.Accounts;

public class CreditAccount : Account
{
    private const int CreditPercent = 0;
    private const double MinimumValueOfCommissionOrMoney = 0;
    private double _money;
    private double _lastTransaction;
    private double _creditLimit;
    private double _commission;
    private double _limitForDoubtfulAccount;
    private int _id;
    public CreditAccount(Bank bank, Client client, int id)
    {
        if (bank == null)
            throw new BanksException("Incorrect value of bank!");
        if (client == null)
            throw new BanksException("Incorrect value of client");
        AccountBank = bank;
        AccountClient = client;
        _creditLimit = bank.Conditions.CreditLimit;
        _money += _creditLimit;
        _commission = bank.Conditions.CreditCommission;
        _limitForDoubtfulAccount = bank.Conditions.LimitForDoubtfulAccount;
        client.AddAccount(this);
        _id = id;
    }

    public Bank AccountBank { get; }
    public Client AccountClient { get; }
    public double Commission => _commission;
    public double Limit => _creditLimit;
    public override double Balance() => _money;
    public override Bank GetBank() => AccountBank;
    public override int Id() => _id;

    public override void AddMoney(double newMoney)
    {
        if (newMoney <= MinimumValueOfCommissionOrMoney)
            throw new BanksException("You haven't added money!");
        _money += newMoney;
    }

    public override void TakeOffMoney(double removeMoney)
    {
        if (removeMoney <= MinimumValueOfCommissionOrMoney)
            throw new BanksException("Incorrect withdrawal amount!");
        if (!AccountClient.IsVarified() && removeMoney > _limitForDoubtfulAccount)
            throw new BanksException("Your account isn't varified!");
        if (removeMoney + _commission > _money)
            throw new BanksException("You have exceeded the limit!");
        _money -= removeMoney;
        if (_money < _creditLimit)
            _money -= _commission;
    }

    public override void DaysPassed(int countDays)
    {
    }

    public override double GetPercent() => CreditPercent;

    public override void LastTransaction(double money)
    {
        _lastTransaction = money;
    }

    public override double GetLastTransaction()
    {
        return _lastTransaction;
    }
}