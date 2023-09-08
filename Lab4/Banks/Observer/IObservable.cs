using Banks.Clients;
namespace Banks.Observer;

public interface IObservable
{
    void RegisterObserver(Client newClient);
    void RemoveObserver(Client oldClient);
    void NotifyObservers();
}