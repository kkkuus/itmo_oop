using Banks.Banks;
using Banks.Clients;
using Banks.Tools;
namespace Banks.Accounts;

public class DepositAccount : Account
{
    private const int CountOfDaysInAYear = 365;
    private const int CountOfDaysInAMonth = 30;
    private const int MinimumPeriodValue = 0;
    private const double MinimumMoneyAndPercentCount = 0;
    private const double MaximumPercent = 100;
    private double _money = 0;
    private double _accruedPercent;
    private int _period;
    private int _passedDays = 0;
    private double _lastTransaction;
    private int _id;

    public DepositAccount(Bank bank, Client client, int id, double summ, int period)
    {
        if (bank == null)
            throw new BanksException("Incorrect value of bank!");
        if (client == null)
            throw new BanksException("Incorrect value of client!");
        if (summ <= MinimumMoneyAndPercentCount)
            throw new BanksException("You haven't invested the money!");
        if (period < MinimumPeriodValue)
            throw new BanksException("Incorrect period value!");
        AccountBank = bank;
        AccountClient = client;
        _money = summ;
        _period = period * CountOfDaysInAMonth;
        _id = id;
        client.AddAccount(this);
        CalculatePrecent();
        CalculateAccruedPercent(Percent, summ);
    }

    public Bank AccountBank { get; }
    public Client AccountClient { get; }
    public double Percent { get; private set; }
    public int Period { get; }

    public override double Balance() => _money;
    public override int Id() => _id;

    public void CalculatePrecent()
    {
        if (_money > AccountBank.Conditions.DepositLimits[AccountBank.Conditions.DepositLimits.Count - 1])
        {
            Percent = AccountBank.Conditions.DepositPercents[AccountBank.Conditions.DepositPercents.Count - 1];
        }
        else
        {
            for (int i = 0; i < AccountBank.Conditions.DepositLimits.Count; ++i)
            {
                if (_money <= AccountBank.Conditions.DepositLimits[i])
                    Percent = AccountBank.Conditions.DepositPercents[i];
            }
        }
    }

    public void CalculateAccruedPercent(double percent, double summ)
    {
        if (percent <= MinimumMoneyAndPercentCount || summ <= MinimumMoneyAndPercentCount)
            throw new BanksException("Incorrect value of percent or summ!");
        _accruedPercent = (percent / (MaximumPercent * CountOfDaysInAYear)) * summ;
    }

    public void AccrueMonthlyPercent()
    {
        if (_period < MinimumPeriodValue)
            _accruedPercent = MinimumMoneyAndPercentCount;
        _money += _accruedPercent * CountOfDaysInAMonth;
    }

    public override void AddMoney(double newMoney)
    {
        if (newMoney <= MinimumMoneyAndPercentCount)
            throw new BanksException("You haven't added any money!");
        _money += newMoney;
    }

    public override void TakeOffMoney(double removeMoney)
    {
        if (_period >= MinimumPeriodValue)
            throw new BanksException("You cannot take money out of the deposit account!");
        if (removeMoney < MinimumMoneyAndPercentCount)
            throw new BanksException("Incorrect withdrawal amount!");
        if (_money < removeMoney)
            throw new BanksException("Not enough money in the account!");
        if (!AccountClient.IsVarified() && removeMoney > AccountBank.Conditions.LimitForDoubtfulAccount)
            throw new BanksException("You can't take off money");
        _money -= removeMoney;
    }

    public override void LastTransaction(double money)
    {
        _lastTransaction = money;
    }

    public override double GetLastTransaction()
    {
        return _lastTransaction;
    }

    public override void DaysPassed(int countDays)
    {
        _passedDays += countDays;
        while (_passedDays >= CountOfDaysInAMonth)
        {
            AccrueMonthlyPercent();
            _passedDays -= CountOfDaysInAMonth;
        }

        _period -= countDays;
    }

    public override double GetPercent() => Percent;
    public override Bank GetBank() => AccountBank;
}