using Banks.Accounts;
using Banks.Banks;
using Banks.Observer;
using Banks.Tools;

namespace Banks.Clients;

public class Client : IObserver
{
    private const int MinimumValeuOfId = 0;
    private List<Account> _accounts = new List<Account>();
    private List<string> _updates = new List<string>();
    private bool _isSubscribe = false;
    public Client(ClientName name, int id, ClientAddress? address, ClientPassport? passport)
    {
        if (name == null)
            throw new BanksException("Enter your full name!");
        if (id < MinimumValeuOfId)
            throw new BanksException("Incorrect value of ID!");
        Name = name;
        Address = address;
        Passport = passport;
        Id = id;
    }

    public ClientName Name { get; }
    public int Id { get; }
    public ClientAddress? Address { get; private set; }
    public ClientPassport? Passport { get; private set; }
    public IReadOnlyList<Account> Accounts => _accounts;
    public IReadOnlyList<string> Updates => _updates;
    public bool IsVarified() => !(Passport == null) && !(Address == null);
    public bool IsSubscribe() => _isSubscribe;

    public void ChangeSubscribe(bool subscribeFlag) => _isSubscribe = subscribeFlag;

    public void AddAddress(ClientAddress newAddress)
    {
        if (newAddress == null)
            throw new BanksException("Incorrect value address!");
        Address = newAddress;
    }

    public void AddPassport(ClientPassport newPassport)
    {
        if (newPassport == null)
            throw new BanksException("Incorrect value of passport!");
        Passport = newPassport;
    }

    public void AddAccount(Account newAccount)
    {
        if (newAccount == null)
            throw new BanksException("There isn't account");
        _accounts.Add(newAccount);
    }

    public void Update(string notification)
    {
        if (string.IsNullOrWhiteSpace(notification))
            throw new BanksException("Incorrect value of new update!");
        _updates.Add(notification);
    }
}