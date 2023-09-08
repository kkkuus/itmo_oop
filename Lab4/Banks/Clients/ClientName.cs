using Banks.Tools;

namespace Banks.Clients;

public class ClientName
{
    public ClientName(string surname, string name)
    {
        if (string.IsNullOrWhiteSpace(surname))
            throw new BanksException("Enter your surname!");
        if (string.IsNullOrWhiteSpace(name))
            throw new BanksException("Enter your name!");
        Surname = surname;
        Name = name;
    }

    public string Surname { get; }
    public string Name { get; }
    public string FullName => Surname + " " + Name;
}