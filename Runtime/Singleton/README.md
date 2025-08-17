# Singleton

Questa cartella contiene classi utili a implementare il pattern **Singleton** e a gestire database di Scriptable Object.

## `Singleton<T>`
Classe astratta che garantisce l'esistenza di una singola istanza per tipo. Se un secondo oggetto viene creato, viene distrutto automaticamente in `Awake`.

```cs
public class EventManager : Singleton<EventManager>
{
    // Metodi e campi del manager
}
```

`Instance` permette di accedere all'unica istanza, mentre `InstanceExists` indica se è già presente. È possibile sovrascrivere `OnPostAwake` e `OnPostDestroy` per eseguire codice di inizializzazione o pulizia.

## `PersistentSingleton<T>`
Deriva da `Singleton<T>` e aggiunge la persistenza tra le scene tramite `DontDestroyOnLoad`.

```cs
public class AudioManager : PersistentSingleton<AudioManager>
{
    // Rimane attivo anche dopo un cambio scena
}
```

## `GenericDataManager<T1, T2>`
Manager generico pensato per caricare asset che ereditano da `UniqueID`. `T1` è il tipo del manager stesso mentre `T2` è il tipo di asset gestito.

All'avvio (in editor) carica automaticamente tutti gli asset trovati nel percorso indicato e li conserva in `Items`.
Sono disponibili vari metodi per la ricerca:

- `SearchAssetByID(string id)`
- `TrySearchAssetByID(string id, out T2 result)`
- `SearchAsset<T>()` per recuperare il primo asset di un certo tipo
- `HasAsset<T>()` per verificare la presenza di un asset di tipo `T`

Esempio di implementazione:

```cs
public class CurrencyManager : GenericDataManager<CurrencyManager, CurrencyData>, ISaveable
{
    // Gestione personalizzata delle valute
}
```

Inserisci la sottoclasse in un prefab ed imposta il percorso dove si trovano gli asset da caricare. L'istanza sarà accessibile tramite `CurrencyManager.Instance`.
