using Banks.Accounts;
using Banks.Clients;
using Banks.Observer;
using Banks.Tools;
using Banks.Transactions;

namespace Banks.Banks;

public class Bank : IObservable
{
    private const int MinimumId = 1;
    private const int MinimumValueOfPeriodOrAmount = 0;
    private int _clientId = 1;
    private int _accountId = 0;
    private List<Client> _clients = new List<Client>();
    private List<Account> _accounts = new List<Account>();
    private List<TransactionMoney> _transactions = new List<TransactionMoney>();
    private AccountFactory _accountFactory = new AccountFactory();
    public Bank(string bankName, int bankId, BankConditions bankConditions, CentralBank centralBank)
    {
        if (string.IsNullOrWhiteSpace(bankName))
            throw new BanksException("Invalid bank name!");
        if (bankConditions == null)
            throw new BanksException("Incorrect value of banks conditions!");
        if (bankId < MinimumId)
            throw new BanksException("Incorrect value of ID!");
        Name = bankName;
        Conditions = bankConditions;
        CentralBank = centralBank;
    }

    public string Name { get; }
    public BankConditions Conditions { get; }
    public CentralBank CentralBank { get; }
    public IReadOnlyList<Client> Clients => _clients;
    public IReadOnlyList<Account> Accounts => _accounts;
    public IReadOnlyList<TransactionMoney> Transactions => _transactions;

    public Client CreateClient(ClientName clientName, ClientAddress? clientAddress, ClientPassport? clientPassport)
    {
        Director director = new Director(clientName, _clientId, clientAddress, clientPassport);
        Builder builder = new ClientBuilder();
        Client client = director.Create(builder);
        _clients.Add(client);
        ++_clientId;
        return client;
    }

    public void AddClient(Client newClient)
    {
        if (newClient == null)
            throw new BanksException("There isn't a client!");
        _clients.Add(newClient);
    }

    public void AddAccount(Account newAccount)
    {
        if (newAccount == null)
            throw new BanksException("Incorrect value of account!");
        _accounts.Add(newAccount);
    }

    public Account CreateAccount(Client client, int period, double amount, string type)
    {
        if (client == null)
            throw new BanksException("Incorrect value of client!");
        if (string.IsNullOrWhiteSpace(type))
            throw new BanksException("You didn't enter the account type!");
        if (type != "debit" && type != "deposit" && type != "credit")
            throw new BanksException("Incorrect value of account!");
        ++_accountId;
        Account account = _accountFactory.CreateAccount(this, client, _accountId, period, amount, type);
        _accounts.Add(account);
        return account;
    }

    public void RegisterObserver(Client newClient)
    {
        if (newClient == null)
            throw new BanksException("Incorrect value of new client!");
        newClient.ChangeSubscribe(true);
    }

    public void RemoveObserver(Client oldClient)
    {
        if (oldClient == null)
            throw new BanksException("Incorrect value of client!");
        oldClient.ChangeSubscribe(false);
    }

    public void NotifyObservers()
    {
        foreach (var client in _clients)
        {
            if (client.IsSubscribe())
                client.Update("Banks' conditions changed! You can watch difference in official website");
        }
    }

    public void ChangeDebitConditions(double newPercent)
    {
        if (newPercent <= MinimumValueOfPeriodOrAmount)
            throw new BanksException("Incorrect value of new percent!");
        Conditions.ChangeDebitConditions(newPercent);
        NotifyObservers();
    }

    public void ChangeCreditConditions(double newCommission, double newLimit)
    {
        if (newCommission <= MinimumValueOfPeriodOrAmount || newLimit <= MinimumValueOfPeriodOrAmount)
            throw new BanksException("Incorrect value of new credit conditions!");
        Conditions.ChangeCreditConditions(newCommission, newLimit);
        NotifyObservers();
    }

    public void ChangeDepositConditions(List<double> newPercents, List<double> newLimits)
    {
        if (newPercents.Count < MinimumValueOfPeriodOrAmount || newLimits.Count < MinimumValueOfPeriodOrAmount)
            throw new BanksException("Incorrect value of new deposit conditions!");
        Conditions.ChangeDepositConditions(newPercents, newLimits);
        NotifyObservers();
    }

    public void DepositCash(Account account, double money)
    {
        if (account == null || !_accounts.Contains(account))
            throw new BanksException("Incorrect value of client!");
        if (money < MinimumValueOfPeriodOrAmount)
            throw new BanksException("Incorrect value of money!");
        TransactionMoney transaction = new TransactionMoney(money, account);
        transaction.DepositCash();
        _transactions.Add(transaction);
    }

    public void WithdrawalCash(Account account, double money)
    {
        if (account == null || !_accounts.Contains(account))
            throw new BanksException("Incorrect value of client!");
        if (money < MinimumValueOfPeriodOrAmount)
            throw new BanksException("Incorrect value of money!");
        TransactionMoney transaction = new TransactionMoney(money, account);
        transaction.WithdrawalCash();
        _transactions.Add(transaction);
    }
}