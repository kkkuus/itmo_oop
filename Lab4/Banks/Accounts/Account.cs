using Banks.Banks;
namespace Banks.Accounts;
public abstract class Account
{
    public abstract void AddMoney(double newMoney);
    public abstract void TakeOffMoney(double removeMoney);
    public abstract void LastTransaction(double money);
    public abstract double GetLastTransaction();
    public abstract void DaysPassed(int countDays);
    public abstract double Balance();
    public abstract double GetPercent();
    public abstract Bank GetBank();
    public abstract int Id();
}