# Gestione della Valuta

Questa cartella contiene gli script utili per implementare un semplice sistema di valuta.

## Classi principali

### `CurrencyData`
`ScriptableObject` che rappresenta una valuta. Contiene:
- `CurrencyType` per identificare la categoria (normale, premium, special).
- `MaxAmount` che indica il quantitativo massimo possedibile.
- `IsConvertible` abilita la conversione in altre valute.
- `CurrencyConversions` elenco di `CurrencyConversionRate` con i tassi di cambio.

### `CurrencyConversionRate`
Classe serializzabile che definisce i rapporti di cambio:
- `CurrencyData` destinazione.
- `ConversionRate` moltiplicatore usato nella conversione.

### `CurrencyType`
Enum con i tipi di valuta (`Normal`, `Premium`, `Special`).

### `CurrencyManager`
Singleton che carica gli asset `CurrencyData` e mantiene i valori tramite `GameSaveManager`.
Metodi utili:
- `AddCurrency` aggiunge una quantità.
- `TryRemoveCurrency` sottrae se l'importo è sufficiente.
- `TryExchangeCurrency` converte da una valuta a un'altra.
- `SetCurrencyAmount`, `GetCurrencyAmount` e `HasEnoughCurrency` per gestire e verificare i valori.

### `Collectable`
`MonoBehaviour` che, quando raccolto tramite `Collect()`, incrementa la valuta specificata e può eseguire logica aggiuntiva in `OnPostCollect`.

### `CurrencyChangeEvent` e `CurrencyChangeEventArgs`
Evento asset che notifica le variazioni di una valuta. Gli argomenti includono la `CurrencyData` interessata e il nuovo importo.

### `CurrencyUITracker`
Collega gli elementi dell'interfaccia all'evento di cambio valuta mostrando icona e valore corrente. L'icona viene caricata
in modo asincrono tramite Addressables; al completamento lo sprite viene assegnato e l'`AsyncOperationHandle` rilasciato.
Se il caricamento fallisce viene utilizzata una sprite di fallback.

## Istruzioni di setup
1. Crea gli asset `CurrencyData` dal menu **Game Utils/Currency/Currency**.
2. Inserisci in scena un prefab contenente `CurrencyManager`.
3. Utilizza oggetti `Collectable` per incrementare le valute.
4. Collega un `CurrencyUITracker` ai componenti UI per visualizzare i valori correnti.

## Esempi

### Sottoscrizione all'evento
```cs
public class CurrencyLogger : MonoBehaviour
{
    [SerializeField] private CurrencyChangeEvent onChangeEvent;

    void Awake()
    {
        onChangeEvent.AddListener(OnCurrencyChanged);
    }

    void OnCurrencyChanged(CurrencyChangeEventArgs args)
    {
        Debug.Log($"{args.CurrencyData.ID} => {args.Amount}");
    }
}
```

### Modifica della valuta
```cs
// Aggiunge 10 unità
CurrencyManager.Instance.AddCurrency(goldData, 10);

// Prova a rimuovere 5 unità
CurrencyManager.Instance.TryRemoveCurrency(goldData, 5);

// Converte 100 unità in gemme
CurrencyManager.Instance.TryExchangeCurrency(goldData, gemData, 100);
```
