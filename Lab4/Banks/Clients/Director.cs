using Banks.Tools;

namespace Banks.Clients;

public class Director
{
    private const int MinimumId = 1;
    public Director(ClientName clientName, int clientId, ClientAddress? clientAddress, ClientPassport? clientPassport)
    {
        if (clientName == null)
            throw new BanksException("Incorrect value of full name!");
        if (clientId < MinimumId)
            throw new BanksException("Incorrect value of Id");
        Name = clientName;
        Id = clientId;
        Address = clientAddress;
        Passport = clientPassport;
    }

    public ClientName Name { get; }
    public int Id { get; }
    public ClientAddress? Address { get; }
    public ClientPassport? Passport { get; }
    public Client Create(Builder builder)
    {
        builder.SetFullName(Name);
        builder.SetId(Id);
        builder.SetFullAddress(Address);
        builder.SetPassport(Passport);
        return builder.GetClient();
    }
}