using Banks.Accounts;
using Banks.Tools;

namespace Banks.Transactions;

public class TransactionMoney
{
    private const double MinimumValueOfMoney = 0;
    public TransactionMoney(double money, Account account)
    {
        if (money <= MinimumValueOfMoney)
            throw new BanksException("Incorrect value of money!");
        if (account == null)
            throw new BanksException("Incorrect value of account!");
        Money = money;
        Account = account;
    }

    public double Money { get; }
    public Account Account { get; }

    public void WithdrawalCash()
    {
        Account.TakeOffMoney(Money);
    }

    public void DepositCash()
    {
        Account.AddMoney(Money);
    }

    public void TransferCash(Account accountTo)
    {
        if (accountTo == null)
            throw new BanksException("Incorrect value of account!");
        Account.TakeOffMoney(Money);
        accountTo.AddMoney(Money);
    }
}