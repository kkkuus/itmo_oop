using Banks.Tools;

namespace Banks.Clients;

public class ClientPassport
{
    private const int MinimumValueOfSeries = 1000;
    private const int MaximumValueOfSeries = 9999;
    private const int MinimumValueOfNumber = 100000;
    private const int MaximumValueOfNumber = 999999;
    public ClientPassport(int series, int number)
    {
        if (series < MinimumValueOfSeries || series > MaximumValueOfSeries)
            throw new BanksException("Incorrect series of passport!");
        if (number < MinimumValueOfNumber || number > MaximumValueOfNumber)
            throw new BanksException("Incorrect number of passport!");
        SeriesOfPassport = series;
        NumberOfPassport = number;
    }

    public int SeriesOfPassport { get; }
    public int NumberOfPassport { get; }
}