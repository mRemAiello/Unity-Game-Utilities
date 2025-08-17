# Event System

Questo modulo fornisce una semplice implementazione degli "Game Events" basata su `ScriptableObject`.
Gli eventi sono asset che mantengono una lista di listener e possono essere richiamati direttamente o tramite `EventManager`.

## ScriptableObject

Gli asset si creano dal menu `Game Utils/Events` e derivano tutti da `GameEventBaseAsset`.
Esistono vari tipi già pronti:

- `VoidEventAsset` – evento senza parametri
- `FloatEventAsset`, `IntEventAsset`
- `Vector2EventAsset`, `Vector3EventAsset`
- `QuaternionEventAsset`, `TransformEventAsset`, `GameObjectEventAsset`
- `SelectableEventAsset`, `ModalWindowButtonEventAsset`

Ogni asset espone l'opzione `Log Enabled` per stampare a console gli invocation tramite l'interfaccia `ILoggable`.

Gli asset di tipo generico (`GameEventAsset<T>`) espongono:

- `AddListener(UnityAction<T>)`
- `RemoveListener(UnityAction<T>)`
- `RemoveAllListeners()`
- `Invoke(T param)`

`VoidEventAsset` offre gli stessi metodi ma senza parametri.

## EventCollectionData

È un semplice contenitore di eventi accessibili tramite chiave stringa. Può essere utilizzato per organizzare e recuperare gli asset con `GetEventAsset(key)`.

## EventManager

Singleton che fornisce un punto di accesso globale agli eventi registrati nelle collezioni configurate. Le funzioni principali sono:

```cs
GameEventAsset<T> GetEventAssetByName<T>(string eventName);
void AddListenerToEventByName<T>(string eventName, UnityAction<T> call);
void RemoveListenerToEventByName<T>(string eventName, UnityAction<T> call);
void RemoveAllListerToEventByName<T>(string eventName);
void InvokeEventByName<T>(string eventName, T param);
```

## Esempio di utilizzo

```cs
// Creiamo un FloatEventAsset (menu: Game Utils/Events/Numeric/Float)
// e lo aggiungiamo a una EventCollectionData.

public class Player : MonoBehaviour
{
    [SerializeField] private EventCollectionData _events;
    [SerializeField] private string _damageEvent = "OnDamage";

    private void OnEnable()
    {
        EventManager.Instance.AddListenerToEventByName<float>(_damageEvent, OnDamage);
    }

    private void OnDisable()
    {
        EventManager.Instance.RemoveListenerToEventByName<float>(_damageEvent, OnDamage);
    }

    private void OnDamage(float value)
    {
        Debug.Log($"Player damaged for {value}");
    }
}

// Invocazione dell'evento
EventManager.Instance.InvokeEventByName<float>("OnDamage", 10f);
```
