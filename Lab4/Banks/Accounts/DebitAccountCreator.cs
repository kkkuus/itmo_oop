using Banks.Banks;
using Banks.Clients;
using Banks.Tools;
namespace Banks.Accounts;

public class DebitAccountCreator : AccountCreator
{
    private Bank _bank;
    private Client _client;
    private int _id;
    public DebitAccountCreator(Bank bank, Client client, int id)
    {
        if (bank == null)
            throw new BanksException("Incorrect value of bank!");
        if (client == null)
            throw new BanksException("Incorrect value of client!");
        _bank = bank;
        _client = client;
        _id = id;
    }

    public override Account Create()
    {
        return new DebitAccount(_bank, _client, _id);
    }
}