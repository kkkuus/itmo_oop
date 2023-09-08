using Banks.Banks;
using Banks.Clients;
using Banks.Tools;

namespace Banks.Accounts;

public class DepositAccountCreator : AccountCreator
{
    private const int MinimumPeriod = 0;
    private const double MinimumAmount = 0;
    private Bank _bank;
    private Client _client;
    private int _period;
    private double _amount;
    private int _id;
    public DepositAccountCreator(Bank bank, Client client, int id, int period, double amount)
    {
        if (bank == null)
            throw new BanksException("Incorrect value of bank!");
        if (client == null)
            throw new BanksException("Incorrect value of client!");
        if (period <= MinimumPeriod)
            throw new BanksException("Incorrect value of period!");
        if (amount <= MinimumAmount)
            throw new BanksException("Incorrect value of amount!");
        _bank = bank;
        _client = client;
        _period = period;
        _amount = amount;
        _id = id;
    }

    public override Account Create()
    {
        return new DepositAccount(_bank, _client, _id, _amount, _period);
    }
}