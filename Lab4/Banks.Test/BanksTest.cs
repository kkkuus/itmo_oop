using Banks.Accounts;
using Banks.Banks;
using Banks.Clients;
using Banks.Tools;
using Banks.Transactions;
using Xunit;

namespace Banks.Test;

public class BanksTest
{
    private CentralBank _centralBank = CentralBank.GetInstance();

    [Fact]
    public void CreateBankAndClient()
    {
        BankConditions bankConditions = new BankConditions(10000, 3, 50, 500000, new List<double> { 1, 2, 3 }, new List<double> { 50000, 100000 });
        _centralBank.CreateBank("Tinkoff", bankConditions);
        Assert.Equal("Tinkoff", _centralBank.Banks[0].Name);
        Bank bank = _centralBank.Banks[0];
        ClientName clientName = new ClientName("Кусайкина", "Елизавета");
        ClientPassport clientPassport = new ClientPassport(1234, 123456);
        bank.CreateClient(clientName, null, clientPassport);
        Assert.Equal("Кусайкина Елизавета", _centralBank.Banks[0].Clients[0].Name.FullName);
        Assert.Null(_centralBank.Banks[0].Clients[0].Address);

        // Assert.False(centralBank.Banks[0].Clients[0].IsVarified());
        ClientAddress adress = new ClientAddress("Saint Petersburg", "Kronverkskiy prospect", 1, 2);
        _centralBank.Banks[0].Clients[0].AddAddress(adress);
        Assert.True(_centralBank.Banks[0].Clients[0].IsVarified());
        _centralBank.ClearBanks();
        _centralBank.ClearTransactions();
    }

    [Fact]
    public void CreateAccounts()
    {
        BankConditions bankConditions = new BankConditions(10000, 3, 50, 500000, new List<double> { 1, 2, 3 }, new List<double> { 50000, 100000 });
        _centralBank.CreateBank("Tinkoff", bankConditions);
        Bank bank = _centralBank.Banks[0];
        ClientName clientName = new ClientName("Кусайкина", "Елизавета");
        ClientPassport clientPassport = new ClientPassport(1234, 123456);
        bank.CreateClient(clientName, null, clientPassport);
        Account account = bank.CreateAccount(_centralBank.Banks[0].Clients[0], 12, 1000, "credit");

        // Account account = bank.CreateCreditAccount(_centralBank.Banks[0].Clients[0]);
        Assert.Contains(account, _centralBank.Banks[0].Accounts);
        Assert.Contains(account, _centralBank.Banks[0].Clients[0].Accounts);
        account = bank.CreateAccount(_centralBank.Banks[0].Clients[0], 12, 1000, "debit");

        // account = bank.CreateDebitAccount(_centralBank.Banks[0].Clients[0]);
        Assert.Contains(account, _centralBank.Banks[0].Accounts);
        Assert.Contains(account, _centralBank.Banks[0].Clients[0].Accounts);
        account = bank.CreateAccount(_centralBank.Banks[0].Clients[0], 12, 500000, "deposit");

        // account = bank.CreateDepositAccount(_centralBank.Banks[0].Clients[0], 12, 500000);
        Assert.Contains(account, _centralBank.Banks[0].Accounts);
        Assert.Contains(account, _centralBank.Banks[0].Clients[0].Accounts);
        _centralBank.ClearBanks();
        _centralBank.ClearTransactions();
    }

    [Fact]
    public void DifferentOperationstWithCreditAccount()
    {
        BankConditions bankConditions = new BankConditions(10000, 3, 50, 500000, new List<double> { 1, 2, 3 }, new List<double> { 50000, 100000 });
        _centralBank.CreateBank("Tinkoff", bankConditions);
        Bank bank = _centralBank.Banks[0];
        ClientName clientName = new ClientName("Кусайкина", "Елизавета");
        ClientPassport clientPassport = new ClientPassport(1234, 123456);
        bank.CreateClient(clientName, null, clientPassport);
        Account account = bank.CreateAccount(_centralBank.Banks[0].Clients[0], 12, 1000, "credit");

        // Account account = bank.CreateCreditAccount(_centralBank.Banks[0].Clients[0]);
        account.AddMoney(5000);
        Assert.Equal(505000, account.Balance());
        account.TakeOffMoney(10000);
        Assert.Equal(494950, account.Balance());
        Assert.Throws<BanksException>(() =>
        {
            account.TakeOffMoney(80000);
        });
        ClientAddress adress = new ClientAddress("Saint Petersburg", "Kronverkskiy prospect", 1, 2);
        _centralBank.Banks[0].Clients[0].AddAddress(adress);
        account.TakeOffMoney(80000);
        Assert.Equal(414900, account.Balance());
        _centralBank.ClearBanks();
        _centralBank.ClearTransactions();
    }

    [Fact]
    public void DifferentOperationstWithDebitAccount()
    {
        BankConditions bankConditions = new BankConditions(10000, 3, 50, 500000, new List<double> { 1, 2, 3 }, new List<double> { 50000, 100000 });
        _centralBank.CreateBank("Tinkoff", bankConditions);
        Bank bank = _centralBank.Banks[0];
        ClientName clientName = new ClientName("Кусайкина", "Елизавета");
        ClientPassport clientPassport = new ClientPassport(1234, 123456);
        bank.CreateClient(clientName, null, clientPassport);
        Account account = bank.CreateAccount(_centralBank.Banks[0].Clients[0], 12, 1000, "debit");

        // Account account = bank.CreateDebitAccount(_centralBank.Banks[0].Clients[0]);
        account.AddMoney(20000);
        Assert.Equal(20000, account.Balance());
        Assert.Throws<BanksException>(() =>
        {
            account.TakeOffMoney(80000);
        });
        ClientAddress adress = new ClientAddress("Saint Petersburg", "Kronverkskiy prospect", 1, 2);
        _centralBank.Banks[0].Clients[0].AddAddress(adress);
        account.TakeOffMoney(11000);
        Assert.Equal(9000, account.Balance());
        account.DaysPassed(30);
        double tempValue = (90 * 30 * (Convert.ToDouble(3) / Convert.ToDouble(365))) + 9000;
        Assert.Equal(tempValue, account.Balance());
        _centralBank.ClearBanks();
        _centralBank.ClearTransactions();
    }

    [Fact]
    public void DifferentOperationstWithDepositAccount()
    {
        BankConditions bankConditions = new BankConditions(10000, 3, 50, 500000, new List<double> { 1, 2, 3 }, new List<double> { 50000, 100000 });
        _centralBank.CreateBank("Tinkoff", bankConditions);
        Bank bank = _centralBank.Banks[0];
        ClientName clientName = new ClientName("Кусайкина", "Елизавета");
        ClientPassport clientPassport = new ClientPassport(1234, 123456);
        bank.CreateClient(clientName, null, clientPassport);
        Account account = bank.CreateAccount(_centralBank.Banks[0].Clients[0], 12, 70000, "deposit");

        // Account account = bank.CreateDepositAccount(_centralBank.Banks[0].Clients[0], 12, 70000);
        Assert.Equal(2, account.GetPercent());
        Assert.Throws<BanksException>(() =>
        {
            account.TakeOffMoney(1000);
        });
        account.DaysPassed(30);
        double tempValue = (700 * 30 * (Convert.ToDouble(2) / Convert.ToDouble(365))) + 70000;
        Assert.Equal(tempValue, account.Balance());
        account.DaysPassed(332);
        account.TakeOffMoney(1000);
        tempValue = Math.Round(((700 * 30 * (Convert.ToDouble(2) / Convert.ToDouble(365))) * 12) + 70000 - 1000, 10);
        Assert.Equal(tempValue, account.Balance());
        _centralBank.ClearBanks();
        _centralBank.ClearTransactions();
    }

    [Fact]
    public void TransactionsMoney()
    {
        BankConditions bankConditions = new BankConditions(10000, 3, 50, 500000, new List<double> { 1, 2, 3 }, new List<double> { 50000, 100000 });
        _centralBank.CreateBank("Tinkoff", bankConditions);
        Bank bank1 = _centralBank.Banks[0];
        ClientName clientName = new ClientName("Kusaikina", "Elizaveta");
        ClientPassport clientPassport = new ClientPassport(1234, 123456);
        bank1.CreateClient(clientName, null, clientPassport);
        Account account1 = bank1.CreateAccount(_centralBank.Banks[0].Clients[0], 12, 1000, "debit");

        // Account account1 = bank1.CreateDebitAccount(_centralBank.Banks[0].Clients[0]);
        bankConditions = new BankConditions(5000, 3, 100, 100000, new List<double> { 2, 3, 5 }, new List<double> { 100000, 500000 });
        _centralBank.CreateBank("Sberbank", bankConditions);
        Bank bank2 = _centralBank.Banks[1];
        clientName = new ClientName("Kusaikina", "Elizaveta");
        bank2.CreateClient(clientName, null, null);
        Account account2 = bank2.CreateAccount(_centralBank.Banks[0].Clients[0], 12, 1000, "debit");

        // Account account2 = bank2.CreateDebitAccount(_centralBank.Banks[0].Clients[0]);
        bank1.DepositCash(account1, 5000);
        Assert.Equal(5000, account1.Balance());
        bank1.WithdrawalCash(account1, 1000);
        Assert.Equal(4000, account1.Balance());
        _centralBank.Transfer(account1, account2, 1000);
        Assert.Equal(3000, account1.Balance());
        Assert.Equal(1000, account2.Balance());
        Assert.NotEmpty(_centralBank.Transactions);
        Assert.NotEmpty(bank1.Transactions);
        TransactionMoney tempTransaction = _centralBank.Transactions[0];
        _centralBank.CancelTransaction(tempTransaction, account2);
        Assert.Equal(4000, account1.Balance());
        Assert.Equal(0, account2.Balance());
        Assert.Empty(_centralBank.Transactions);
        _centralBank.ClearBanks();
        _centralBank.ClearTransactions();
    }

    [Fact]
    public void ChangeConditionsAndNotifyingClients()
    {
        BankConditions bankConditions = new BankConditions(10000, 3, 50, 500000, new List<double> { 1, 2, 3 }, new List<double> { 50000, 100000 });
        _centralBank.CreateBank("Tinkoff", bankConditions);
        Bank bank = _centralBank.Banks[0];
        ClientName clientName = new ClientName("Кусайкина", "Елизавета");
        ClientPassport clientPassport = new ClientPassport(1234, 123456);
        bank.CreateClient(clientName, null, clientPassport);
        Client client = bank.Clients[0];
        Assert.False(client.IsSubscribe());
        client.ChangeSubscribe(true);
        Assert.True(client.IsSubscribe());
        bank.ChangeCreditConditions(100, 100000);
        Assert.Equal(100, bank.Conditions.CreditCommission);
        Assert.Equal(100000, bank.Conditions.CreditLimit);
        Assert.NotEmpty(client.Updates);
        _centralBank.ClearBanks();
        _centralBank.ClearTransactions();
    }
}