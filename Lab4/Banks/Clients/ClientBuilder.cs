using Banks.Clients;
using Banks.Tools;

namespace Banks.Clients;

public class ClientBuilder : Builder
{
    private const int MinimumId = 1;
    private ClientName _clientName = new ClientName("Surname", "Name");
    private int _clientId;
    private ClientAddress? _clientAddress;
    private ClientPassport? _clientPassport;
    public override void SetFullName(ClientName? clientName)
    {
        if (clientName == null)
            throw new BanksException("Invalid full name!");
        _clientName = clientName;
    }

    public override void SetId(int clientId)
    {
        if (clientId < MinimumId)
            throw new BanksException("Incorrect ID!");
        _clientId = clientId;
    }

    public override void SetFullAddress(ClientAddress? clientAddress)
    {
        _clientAddress = clientAddress;
    }

    public override void SetPassport(ClientPassport? clientPassport)
    {
        _clientPassport = clientPassport;
    }

    public override Client GetClient()
    {
        return new Client(_clientName, _clientId, _clientAddress, _clientPassport);
    }
}