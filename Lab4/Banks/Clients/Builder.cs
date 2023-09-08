namespace Banks.Clients;
public abstract class Builder
{
    public abstract void SetFullName(ClientName? clientName);
    public abstract void SetId(int clientId);
    public abstract void SetFullAddress(ClientAddress? clientAddress);
    public abstract void SetPassport(ClientPassport? clientPassport);
    public abstract Client GetClient();
}