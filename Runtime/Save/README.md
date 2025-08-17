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
string card2 = GameSaveManager.Instance.Load<string>("Deck", "Card2", "");
string card3 = GameSaveManager.Instance.Load<string>("Deck", "Card3", "");
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

### Aggiornamenti delle impostazioni

I binder dell'interfaccia utente possono sottoscriversi all'evento `OnValueChanged`
esposto da `BaseSettingData<T>` per ricevere notifiche quando il valore cambia.

```cs
mySetting.OnValueChanged += value => mySlider.value = value;
```

L'evento viene invocato sia quando il valore viene modificato tramite `SetValue`
sia quando viene caricato con `Load`.

### Configurare una impostazione con la UI

1. **Creare uno `SettingData`**
   - Nel _Project Window_ fare clic destro e selezionare **Create > Game Utils > Settings > [Tipo]**.
   - Rinominare l'asset e impostare il **Default Value** nell'Inspector.

2. **Aggiungere il binder alla UI**
   - Creare uno script che derivi da `SettingBinder<T, TUI>` per il componente di interfaccia che si vuole sincronizzare.
   - Esempio per collegare uno `Slider` a un `float`:

```cs
using UnityEngine.UI;

public class SliderSettingBinder : SettingBinder<float, Slider>
{
    protected override void AddUIListener() =>
        _uiComponent.onValueChanged.AddListener(_ => OnUIValueChanged());

    protected override void RemoveUIListener() =>
        _uiComponent.onValueChanged.RemoveListener(_ => OnUIValueChanged());

    protected override void SetUIValue(float value) => _uiComponent.value = value;
    protected override float GetUIValue() => _uiComponent.value;
}
```

3. **Configurare i riferimenti nel Inspector**
   - Aggiungere il binder al GameObject che contiene il componente UI.
   - Trascinare l'asset `SettingData` nel campo **Data**.
   - Assegnare il componente UI corrispondente al campo **UI Component**.
   - In Play Mode il valore del `SettingData` e della UI rimarranno sincronizzati.