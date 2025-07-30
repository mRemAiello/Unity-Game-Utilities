# Sistema di Salvataggio

Questo modulo fornisce un layer semplificato sopra la libreria **Quick Save** e
permette di memorizzare dati tipizzati su slot multipli. Le classi principali
sono `GameSaveManager` e l'interfaccia `ISaveable`.

## GameSaveManager
`GameSaveManager` è un `Singleton` persistente che gestisce i file di salvataggio.
Le funzioni più utili sono:

- `SetActiveSaveSlot(int slot)` per cambiare lo slot corrente.
- `Save<T>(string context, string key, T value)` e `Load<T>(string context, string key, T defaultValue)` per scrivere e leggere valori generici.
- `TryLoad<T>(string context, string key, out T result, T defaultValue)` che restituisce `true` solo se il dato è presente.
- `Exists<T>(string context, string key)` e `RemoveKey<T>(string context, string key)` per verificare ed eliminare singole voci.
- `Clear()` che rimuove tutte le chiavi dallo slot attivo.

Il manager crea automaticamente il file di salvataggio se assente e mantiene un
dizionario di debug con tutte le chiavi registrate.

## ISaveable
L'interfaccia `ISaveable` espone la proprietà `SaveContext`. Implementandola sui
propri componenti è possibile chiamare i metodi di `GameSaveManager` passando
direttamente l'istanza e mantenere ordinati i dati sotto un contesto univoco.

```cs
public class PlayerInventory : MonoBehaviour, ISaveable
{
    public string SaveContext => "PlayerInventory";
}
```

## Utilizzo
1. Inserisci in scena un `GameObject` con `GameSaveManager`.
2. Implementa `ISaveable` nei componenti che devono salvare dati.
3. Usa i metodi `Save` e `Load` specificando la chiave desiderata.

```cs
// Salvataggio rapido tramite ISaveable
GameSaveManager.Instance.Save(this, "Money", 100);

// Salvataggio con contesto personalizzato
GameSaveManager.Instance.Save("Deck", "Card1", "123x1123");

// Caricamento
int money = GameSaveManager.Instance.Load<int>(this, "Money", 0);
string card1 = GameSaveManager.Instance.Load<string>("Deck", "Card1", "");
```

## Estensione
È possibile derivare da `GameSaveManager` per aggiungere funzionalità extra
(ad esempio crittografia o log personalizzati) sovrascrivendo i metodi di
salvataggio.

```cs
public class EncryptedSaveManager : GameSaveManager
{
    protected override void Save<T>(string context, string key, T amount)
    {
        // Logica di crittografia prima del salvataggio
        base.Save(context, key, amount);
    }
}
```
