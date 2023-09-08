using Banks.Accounts;
using Banks.Tools;
using Banks.Transactions;

namespace Banks.Banks;

public class CentralBank
{
    private const double MinimumValueOfMoney = 0;
    private const int MinimumValueOfDaysCount = 0;
    private static CentralBank? _instance;
    private int _bankId = 1;
    private List<Bank> _banks = new List<Bank>();
    private List<TransactionMoney> _transactions = new List<TransactionMoney>();
    private List<Account> _accountsTo = new List<Account>();
    private CentralBank()
    {
    }

    public IReadOnlyList<Bank> Banks => _banks;
    public IReadOnlyList<TransactionMoney> Transactions => _transactions;
    public static CentralBank GetInstance()
    {
        if (_instance == null)
            _instance = new CentralBank();
        return _instance;
    }

    public void ClearBanks() => _banks.Clear();

    public void ClearTransactions()
    {
        _transactions.Clear();
        _accountsTo.Clear();
    }

    public Bank CreateBank(string newBankName, BankConditions bankConditions)
    {
        if (bankConditions == null)
            throw new BanksException("Bank conditions is null");
        if (string.IsNullOrWhiteSpace(newBankName))
            throw new BanksException("Invalid value of bank name!");
        Bank tmpBank = new Bank(newBankName, _bankId, bankConditions, this);
        ++_bankId;
        _banks.Add(tmpBank);
        return tmpBank;
    }

    public void AddBank(Bank newBank)
    {
        if (newBank == null)
            throw new BanksException("There isn't a bank!");
        _banks.Add(newBank);
    }

    public void Transfer(Account accountFrom, Account accountTo, double money)
    {
        if (accountFrom == null || accountTo == null)
            throw new BanksException("Incorrect value of accounts!");
        if (money <= MinimumValueOfMoney)
            throw new BanksException("Incorrect value of money!");
        if (!_banks.Contains(accountFrom.GetBank()) || !_banks.Contains(accountTo.GetBank()))
            throw new BanksException("There aren't this banks!");
        TransactionMoney transaction = new TransactionMoney(money, accountFrom);
        transaction.TransferCash(accountTo);
        _transactions.Add(transaction);
        _accountsTo.Add(accountTo);
    }

    public void CancelTransaction(TransactionMoney transactionMoney, Account accountTo)
    {
        if (!_transactions.Contains(transactionMoney))
            throw new BanksException("Incorrect value of transaction!");
        int tempIndex = _transactions.IndexOf(transactionMoney);
        Transfer(accountTo, transactionMoney.Account, transactionMoney.Money);
        _transactions.Remove(transactionMoney);
        _accountsTo.RemoveAt(tempIndex);
        _transactions.RemoveAt(_transactions.Count - 1);
        _accountsTo.RemoveAt(_accountsTo.Count - 1);
    }

    public void RewindDays(int countOfDays)
    {
        if (countOfDays <= MinimumValueOfDaysCount)
            throw new BanksException("Incorrect value of count of days!");
        foreach (var bank in _banks)
        {
            foreach (var account in bank.Accounts)
            {
                account.DaysPassed(countOfDays);
            }
        }
    }
}