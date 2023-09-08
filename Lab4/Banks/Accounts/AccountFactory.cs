using Banks.Banks;
using Banks.Clients;
using Banks.Tools;

namespace Banks.Accounts;

public class AccountFactory
{
    private const int MinimumId = 0;
    private Account? account;
    private AccountCreator? _accountCreator;
    public Account CreateAccount(Bank bank, Client client, int id, int period, double amount, string type)
    {
        if (bank == null)
            throw new BanksException("Incorrect value of bank!");
        if (client == null)
            throw new BanksException("Incorrect value of client!");
        if (id <= MinimumId)
            throw new BanksException("Incorrect value of id");
        if (string.IsNullOrWhiteSpace(type))
            throw new BanksException("You didn't enter account type!");
        switch (type)
        {
            case "debit":
                _accountCreator = new DebitAccountCreator(bank, client, id);
                account = _accountCreator.Create();
                break;
            case "deposit":
                _accountCreator = new DepositAccountCreator(bank, client, id, period, amount);
                account = _accountCreator.Create();
                break;
            case "credit":
                _accountCreator = new CreditAccountCreator(bank, client, id);
                account = _accountCreator.Create();
                break;
            default:
                throw new BanksException("Incorrect value of account type!");
        }

        return account;
    }
}