using Banks.Accounts;
using Banks.Banks;
using Banks.Clients;
using Banks.Tools;

namespace Banks;

internal static class Program
{
    public static void Main(string[] args)
    {
        CentralBank centralBank = CentralBank.GetInstance();
        List<Bank> banks = new List<Bank>();
        List<Client> clients = new List<Client>();
        List<Account> accounts = new List<Account>();
        BankConditions tempBankConditions;
        Bank tempBank;
        Client tempClient;
        Account tempAccount;
        string tempBankName;
        double tempDebitPercent;
        double tempCreditCommission;
        double tempCreditLimit;
        double tempLimitForDoubtfulAccount;
        int tempCountOfDepositPercent;
        string tempAddressCity;
        string tempAddressStreet;
        int tempAddressHouse;
        int tempAddressFlat;
        int tempPassportSeries;
        int tempPassportNumber;
        List<double> tempDepositPercents = new List<double>();
        List<double> tempDepositLimits = new List<double>();
        Console.WriteLine("Press 1 to get a information about commands!");
        int command = 0;
        while (command != 2)
        {
            command = Convert.ToInt32(Console.ReadLine());
            switch (command)
            {
                case 1:
                    Console.WriteLine("1 - get information about commands");
                    Console.WriteLine("2 - exit");
                    Console.WriteLine("3 - create bank");
                    Console.WriteLine("4 - create client");
                    Console.WriteLine("5 - create account");
                    Console.WriteLine("6 - withdrawal money");
                    Console.WriteLine("7 - deposit money");
                    Console.WriteLine("8 - transfer money");
                    Console.WriteLine("9 - check balance");
                    Console.WriteLine("10 - add address");
                    Console.WriteLine("11 - add passport");
                    Console.WriteLine("12 - passed days");
                    Console.WriteLine("13 - change subscribe");
                    Console.WriteLine("14 - change conditions");
                    break;
                case 2:
                    Console.WriteLine("Goodbye!");
                    break;
                case 3:
                    Console.WriteLine("Enter bank name:");
                    tempBankName = Console.ReadLine() !;
                    Console.WriteLine("1) Let's do debit account");
                    Console.WriteLine("  -Enter debit precent: ");
                    tempDebitPercent = Convert.ToDouble(Console.ReadLine());
                    Console.WriteLine("2) Let's do credit account");
                    Console.WriteLine("   -Enter credit limit: ");
                    tempCreditLimit = Convert.ToDouble(Console.ReadLine());
                    Console.WriteLine("   -Enter credit limit: ");
                    tempCreditCommission = Convert.ToDouble(Console.ReadLine());
                    Console.WriteLine("3) Let's do deposit account");
                    tempDepositLimits.Clear();
                    tempDepositPercents.Clear();
                    Console.WriteLine("  -Enter how mane percent values for a deposit account?");
                    tempCountOfDepositPercent = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("  -Enter boundary conditions of the sum: ");
                    double tempDepositObject;
                    for (int i = 0; i < tempCountOfDepositPercent - 1; ++i)
                    {
                        tempDepositObject = Convert.ToDouble(Console.ReadLine());
                        tempDepositLimits.Add(tempDepositObject);
                    }

                    Console.WriteLine("  -Enter percents for deposit account: ");
                    for (int i = 0; i < tempCountOfDepositPercent; ++i)
                    {
                        tempDepositObject = Convert.ToDouble(Console.ReadLine());
                        tempDepositPercents.Add(tempDepositObject);
                    }

                    Console.WriteLine("You have entered the values for the accounts!");
                    Console.WriteLine("Enter limit for doubtful client: ");
                    tempLimitForDoubtfulAccount = Convert.ToDouble(Console.ReadLine());
                    tempBankConditions = new BankConditions(tempLimitForDoubtfulAccount, tempDebitPercent, tempCreditCommission, tempCreditLimit, tempDepositPercents, tempDepositLimits);
                    centralBank.CreateBank(tempBankName, tempBankConditions);
                    Console.WriteLine($"BANK {centralBank.Banks[centralBank.Banks.Count - 1].Name} CREATED!");
                    break;
                case 4:
                    Console.WriteLine("Which bank?");
                    tempBankName = Console.ReadLine() !;
                    tempBank = centralBank.Banks.FirstOrDefault(bank => bank.Name == tempBankName) ?? throw new BanksException("There isn't a bank!");
                    Console.WriteLine("  -Enter surname: ");
                    string? tempSurname = Console.ReadLine();
                    while (string.IsNullOrWhiteSpace(tempSurname))
                    {
                        Console.WriteLine("  -Enter correct surname!: ");
                        tempSurname = Console.ReadLine() !;
                    }

                    Console.WriteLine("  -Enter name: ");
                    string? tempName = Console.ReadLine();
                    while (string.IsNullOrWhiteSpace(tempName))
                    {
                        Console.WriteLine("  -Enter correct name!: ");
                        tempName = Console.ReadLine() !;
                    }

                    ClientName tempClientName = new ClientName(tempSurname, tempName);
                    ClientAddress? tempClientAddress = null;
                    ClientPassport? tempClientPassport = null;
                    Console.WriteLine("Do you want enter your address?");
                    if (Console.ReadLine() == "yes")
                    {
                        Console.WriteLine("  -1)Enter city: ");
                        tempAddressCity = Console.ReadLine() !;
                        Console.WriteLine("  -2)Enter street: ");
                        tempAddressStreet = Console.ReadLine() !;
                        Console.WriteLine("  -3)Enter house number: ");
                        tempAddressHouse = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("  -4)Enter flat number: ");
                        tempAddressFlat = Convert.ToInt32(Console.ReadLine());
                        tempClientAddress = new ClientAddress(tempAddressCity, tempAddressStreet, tempAddressHouse, tempAddressFlat);
                    }

                    Console.WriteLine("Do you want enter your passport?");
                    if (Console.ReadLine() == "yes")
                    {
                        Console.WriteLine("  -1)Enter passport series: ");
                        tempPassportSeries = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("  -1)Enter passport number: ");
                        tempPassportNumber = Convert.ToInt32(Console.ReadLine());
                        tempClientPassport = new ClientPassport(tempPassportSeries, tempPassportNumber);
                    }

                    tempClient = tempBank.CreateClient(tempClientName, tempClientAddress, tempClientPassport);
                    clients.Add(tempClient);
                    Console.WriteLine($"Client {tempBank.Clients[tempBank.Clients.Count - 1].Name.FullName} created!");
                    break;
                case 5:
                    Console.WriteLine("In which bank?");
                    tempBankName = Console.ReadLine() !;
                    tempBank = centralBank.Banks.FirstOrDefault(bank => bank.Name == tempBankName) !;
                    Console.WriteLine("For which client? (enter client ID)");
                    int tempClientId = Convert.ToInt32(Console.ReadLine());
                    tempClient = tempBank.Clients.FirstOrDefault(client => client.Id == tempClientId) !;
                    Console.WriteLine("What type of account do you want to creat?  debit/credit/deposit");
                    string tempTypeAccount = Console.ReadLine() !;
                    switch (tempTypeAccount)
                    {
                        case "debit":
                            tempAccount = tempBank.CreateAccount(tempClient, 72, 0, "debit");
                            accounts.Add(tempAccount);
                            break;
                        case "credit":
                            tempAccount = tempBank.CreateAccount(tempClient, 72, 0, "credit");
                            accounts.Add(tempAccount);
                            break;
                        case "deposit":
                            Console.WriteLine("For what period to create an account? (enter count of month)");
                            int tempPeriod = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("How much do you want to invest?");
                            double tempSumm = Convert.ToDouble(Console.ReadLine());
                            tempAccount = tempBank.CreateAccount(tempClient, tempPeriod, tempSumm, "deposit");
                            accounts.Add(tempAccount);
                            break;
                    }

                    Console.WriteLine("Account was created!");
                    break;
                case 6:
                    Console.WriteLine("In which bank?");
                    tempBankName = Console.ReadLine() !;
                    tempBank = centralBank.Banks.FirstOrDefault(bank => bank.Name == tempBankName) !;
                    Console.WriteLine("From which client? (enter client ID)");
                    tempClientId = Convert.ToInt32(Console.ReadLine());
                    tempClient = tempBank.Clients.FirstOrDefault(client => client.Id == tempClientId) !;
                    Console.WriteLine("From which account?  (enter account ID)");
                    int tempAccountId = Convert.ToInt32(Console.ReadLine()) !;
                    tempAccount = tempBank.Accounts.FirstOrDefault(account => account.Id() == tempAccountId) !;
                    Console.WriteLine("How much money do you want to withdraw?");
                    tempBank.WithdrawalCash(tempAccount, Convert.ToDouble(Console.ReadLine()));
                    Console.WriteLine("Transaction ended!");
                    break;
                case 7:
                    Console.WriteLine("In which bank?");
                    tempBankName = Console.ReadLine() !;
                    tempBank = centralBank.Banks.FirstOrDefault(bank => bank.Name == tempBankName) !;
                    Console.WriteLine("From which client? (enter client ID)");
                    tempClientId = Convert.ToInt32(Console.ReadLine());
                    tempClient = tempBank.Clients.FirstOrDefault(client => client.Id == tempClientId) !;
                    Console.WriteLine("From which account?  (enter account ID)");
                    tempAccountId = Convert.ToInt32(Console.ReadLine()) !;
                    tempAccount = tempBank.Accounts.FirstOrDefault(account => account.Id() == tempAccountId) !;
                    Console.WriteLine("How much money do you want to invest?");
                    tempBank.DepositCash(tempAccount, Convert.ToDouble(Console.ReadLine()));
                    Console.WriteLine("Transaction ended!");
                    break;
                case 8:
                    Console.WriteLine("In which bank?");
                    tempBankName = Console.ReadLine() !;
                    tempBank = centralBank.Banks.FirstOrDefault(bank => bank.Name == tempBankName) !;
                    Console.WriteLine("From which client? (enter client ID)");
                    tempClientId = Convert.ToInt32(Console.ReadLine());
                    tempClient = tempBank.Clients.FirstOrDefault(client => client.Id == tempClientId) !;
                    Console.WriteLine("From which account?  (enter account ID)");
                    tempAccountId = Convert.ToInt32(Console.ReadLine()) !;
                    tempAccount = tempBank.Accounts.FirstOrDefault(account => account.Id() == tempAccountId) !;
                    Console.WriteLine("For which bank?");
                    string tempBankName2 = Console.ReadLine() !;
                    Bank tempBank2 = centralBank.Banks.FirstOrDefault(bank => bank.Name == tempBankName2) !;
                    Console.WriteLine("For which client? (enter client ID)");
                    int tempClientId2 = Convert.ToInt32(Console.ReadLine());
                    Client tempClient2 = tempBank2.Clients.FirstOrDefault(client => client.Id == tempClientId2) !;
                    Console.WriteLine("For which account?  (enter account ID)");
                    int tempAccountId2 = Convert.ToInt32(Console.ReadLine()) !;
                    Account tempAccount2 = tempBank2.Accounts.FirstOrDefault(account => account.Id() == tempAccountId2) !;
                    Console.WriteLine("How much money do you want to transfer?");
                    centralBank.Transfer(tempAccount, tempAccount2, Convert.ToDouble(Console.ReadLine()));
                    Console.WriteLine("Transaction ended!");
                    break;
                case 9:
                    Console.WriteLine("In which bank?");
                    tempBankName = Console.ReadLine() !;
                    tempBank = centralBank.Banks.FirstOrDefault(bank => bank.Name == tempBankName) !;
                    Console.WriteLine("From which client? (enter client ID)");
                    tempClientId = Convert.ToInt32(Console.ReadLine());
                    tempClient = tempBank.Clients.FirstOrDefault(client => client.Id == tempClientId) !;
                    Console.WriteLine("From which account?  (enter account ID)");
                    tempAccountId = Convert.ToInt32(Console.ReadLine()) !;
                    tempAccount = tempBank.Accounts.FirstOrDefault(account => account.Id() == tempAccountId) !;
                    Console.WriteLine($"BALANCE: {tempAccount.Balance()}");
                    break;
                case 10:
                    Console.WriteLine("In which bank?");
                    tempBankName = Console.ReadLine() !;
                    tempBank = centralBank.Banks.FirstOrDefault(bank => bank.Name == tempBankName) !;
                    Console.WriteLine("In which client? (enter client ID)");
                    tempClientId = Convert.ToInt32(Console.ReadLine());
                    tempClient = tempBank.Clients.FirstOrDefault(client => client.Id == tempClientId) !;
                    Console.WriteLine("  -1)Enter city: ");
                    tempAddressCity = Console.ReadLine() !;
                    Console.WriteLine("  -2)Enter street: ");
                    tempAddressStreet = Console.ReadLine() !;
                    Console.WriteLine("  -3)Enter house number: ");
                    tempAddressHouse = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("  -4)Enter flat number: ");
                    tempAddressFlat = Convert.ToInt32(Console.ReadLine());
                    tempClientAddress = new ClientAddress(tempAddressCity, tempAddressStreet, tempAddressHouse, tempAddressFlat);
                    tempClient.AddAddress(tempClientAddress);
                    Console.WriteLine("Adress was added!");
                    break;
                case 11:
                    Console.WriteLine("In which bank?");
                    tempBankName = Console.ReadLine() !;
                    tempBank = centralBank.Banks.FirstOrDefault(bank => bank.Name == tempBankName) !;
                    Console.WriteLine("In which client? (enter client ID)");
                    tempClientId = Convert.ToInt32(Console.ReadLine());
                    tempClient = tempBank.Clients.FirstOrDefault(client => client.Id == tempClientId) !;
                    Console.WriteLine("  -1)Enter passport series: ");
                    tempPassportSeries = Convert.ToInt32(Console.ReadLine()) !;
                    Console.WriteLine("  -2)Enter passport number: ");
                    tempPassportNumber = Convert.ToInt32(Console.ReadLine()) !;
                    tempClientPassport = new ClientPassport(tempPassportSeries, tempPassportNumber);
                    tempClient.AddPassport(tempClientPassport);
                    Console.WriteLine("Passport was added!");
                    break;
                case 12:
                    Console.WriteLine("How many days have passed?");
                    centralBank.RewindDays(Convert.ToInt32(Console.ReadLine()));
                    Console.WriteLine("Days have passed!");
                    break;
                case 13:
                    Console.WriteLine("In which bank?");
                    tempBankName = Console.ReadLine() !;
                    tempBank = centralBank.Banks.FirstOrDefault(bank => bank.Name == tempBankName) !;
                    Console.WriteLine("For which client? (enter client ID)");
                    tempClientId = Convert.ToInt32(Console.ReadLine());
                    tempClient = tempBank.Clients.FirstOrDefault(client => client.Id == tempClientId) !;
                    Console.WriteLine("You want to subscribe?");
                    if (Console.ReadLine() == "yes")
                        tempClient.ChangeSubscribe(true);
                    else
                        tempClient.ChangeSubscribe(false);
                    Console.WriteLine("Subscribe was changed!");
                    break;
                case 14:
                    Console.WriteLine("In which bank?");
                    tempBankName = Console.ReadLine() !;
                    tempBank = centralBank.Banks.FirstOrDefault(bank => bank.Name == tempBankName) !;
                    Console.WriteLine("Which conditions you want change?  debit/deposit/cretid");
                    string tempAnswer = Console.ReadLine() !;
                    if (tempAnswer == "debit")
                    {
                        Console.WriteLine(" -Enter new percent: ");
                        tempBank.ChangeDebitConditions(Convert.ToDouble(Console.ReadLine()));
                        Console.WriteLine("Conditions was changed");
                    }

                    if (tempAnswer == "credit")
                    {
                        Console.WriteLine(" -Enter new commission and limit: ");
                        tempBank.ChangeCreditConditions(Convert.ToDouble(Console.ReadLine()), Convert.ToDouble(Console.ReadLine()));
                        Console.WriteLine("Conditions was changed");
                    }

                    if (tempAnswer == "deposit")
                    {
                        tempDepositPercents.Clear();
                        tempDepositLimits.Clear();
                        Console.WriteLine("  -Enter how mane percent values for a deposit account?");
                        tempCountOfDepositPercent = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine(" -Enter boundary conditions of the sum: ");
                        for (int i = 0; i < tempCountOfDepositPercent - 1; ++i)
                        {
                            tempDepositObject = Convert.ToDouble(Console.ReadLine());
                            tempDepositLimits.Add(tempDepositObject);
                        }

                        Console.WriteLine("  -Enter percents for deposit account: ");
                        for (int i = 0; i < tempCountOfDepositPercent; ++i)
                        {
                            tempDepositObject = Convert.ToDouble(Console.ReadLine());
                            tempDepositPercents.Add(tempDepositObject);
                        }

                        tempBank.ChangeDepositConditions(tempDepositPercents, tempDepositLimits);
                        Console.WriteLine("Conditions was changed");
                    }

                    break;
            }
        }
    }
}