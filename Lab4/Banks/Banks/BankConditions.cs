using Banks.Tools;

namespace Banks.Banks;

public class BankConditions
{
    private const double MinimumValueForBankConditions = 0;
    private double _limitForDoubtfulAccount;
    private double _debitPercent;
    private double _creditCommission;
    private double _creditLimit;
    private List<double> _depositPercents = new List<double>();
    private List<double> _depositLimits = new List<double>();

    public BankConditions(double limitForDoubtfulAccount, double debitPercent, double creditCommission, double creditLimit, List<double> depositPercents, List<double> depositLimits)
    {
        if (limitForDoubtfulAccount < MinimumValueForBankConditions)
            throw new BanksException("Incorrect value of limit for doubtful account!");
        if (debitPercent <= MinimumValueForBankConditions)
            throw new BanksException("Incorrect value of debit percent!");
        if (creditCommission <= MinimumValueForBankConditions)
            throw new BanksException("Incorrect value of credit commission!");
        if (creditLimit <= MinimumValueForBankConditions)
            throw new BanksException("Incorrect value of credit limit!");
        if (depositPercents.Count <= MinimumValueForBankConditions)
            throw new BanksException("Incorrect value of count of deposit percents!");
        if (depositLimits.Count != depositPercents.Count - 1)
            throw new BanksException("Incorrect value of number of deposit limits!");
        _limitForDoubtfulAccount = limitForDoubtfulAccount;
        _debitPercent = debitPercent;
        _creditCommission = creditCommission;
        _creditLimit = creditLimit;
        _depositPercents = depositPercents;
        _depositLimits = depositLimits;
    }

    public double LimitForDoubtfulAccount => _limitForDoubtfulAccount;
    public double DebitPercent => _debitPercent;
    public double CreditCommission => _creditCommission;
    public double CreditLimit => _creditLimit;
    public IReadOnlyList<double> DepositPercents => _depositPercents;
    public IReadOnlyList<double> DepositLimits => _depositLimits;

    public void ChangeDebitConditions(double newDebitPersent)
    {
        if (newDebitPersent < MinimumValueForBankConditions)
            throw new BanksException("Incorrect new percent rate!");
        _debitPercent = newDebitPersent;
    }

    public void ChangeCreditConditions(double newCreditCommission, double newCreditLimit)
    {
        if (newCreditCommission < MinimumValueForBankConditions)
            throw new BanksException("Incorrect new credit commission");
        if (newCreditLimit < MinimumValueForBankConditions)
            throw new BanksException("Incorrect new credit limit");
        _creditLimit = newCreditLimit;
        _creditCommission = newCreditCommission;
    }

    public void ChangeDepositConditions(List<double> newDepositPercents, List<double> newDepositLimits)
    {
        if (newDepositPercents.Count < MinimumValueForBankConditions)
            throw new BanksException("Incorrect count of percents");
        if (newDepositLimits.Count != newDepositPercents.Count - 1)
            throw new BanksException("Incorrect count of limits");
        _depositLimits = newDepositLimits;
        _depositPercents = newDepositPercents;
    }
}