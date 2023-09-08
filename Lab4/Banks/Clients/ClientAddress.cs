using Banks.Tools;

namespace Banks.Clients;

public class ClientAddress
{
    private const int MinimumValueOfHouseOrFlatNumber = 1;
    public ClientAddress(string? city, string? street, int house, int flat)
    {
        if (string.IsNullOrEmpty(city))
            throw new BanksException("Enter city!");
        if (string.IsNullOrEmpty(street))
            throw new BanksException("Enter street!");
        if (house < MinimumValueOfHouseOrFlatNumber)
            throw new BanksException("Invalid value of house number!");
        if (flat < MinimumValueOfHouseOrFlatNumber)
            throw new BanksException("Invalid value of flat number!");
        City = city;
        Street = street;
        House = house;
        Flat = flat;
    }

    public string? City { get; }
    public string? Street { get; }
    public int House { get; }
    public int Flat { get; }
    public string GetAdress => City + ", " + Street + ", " + Convert.ToString(House) + ", " + Convert.ToString(Flat);
}