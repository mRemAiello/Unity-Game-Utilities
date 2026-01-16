# Currency Management

This folder contains the scripts needed to implement a simple currency system.

## Core classes

### `CurrencyData`
`ScriptableObject` that represents a currency. It includes:
- `CurrencyType` to identify the category (normal, premium, special).
- `MaxAmount` which defines the maximum amount that can be owned.
- `IsConvertible` to enable conversions into other currencies.
- `CurrencyConversions` list of `CurrencyConversionRate` entries with exchange rates.

### `CurrencyConversionRate`
Serializable class that defines exchange ratios:
- Destination `CurrencyData`.
- `ConversionRate` multiplier used during conversion.

### `CurrencyType`
Enum with currency types (`Normal`, `Premium`, `Special`).

### `CurrencyManager`
Singleton that loads `CurrencyData` assets and stores values via `GameSaveManager`.
Useful methods:
- `AddCurrency` adds an amount.
- `TryRemoveCurrency` subtracts if there is enough currency.
- `TryExchangeCurrency` converts from one currency to another.
- `SetCurrencyAmount`, `GetCurrencyAmount`, and `HasEnoughCurrency` manage and validate values.

### `Collectable`
`MonoBehaviour` that, when collected via `Collect()`, increments the specified currency and can run extra logic in `OnPostCollect`.
Use the trigger/collision toggles to enable or disable automatic collection via `OnTriggerEnter` or `OnCollisionEnter`.

### `CurrencyChangeEvent` and `CurrencyChangeEventArgs`
Asset event that notifies currency changes. The arguments include the target `CurrencyData` and the new amount.

### `CurrencyUITracker`
Connects UI elements to the currency change event, showing icon and current value. The icon is loaded asynchronously
via Addressables; once completed, the sprite is assigned and the `AsyncOperationHandle` released.
If loading fails, a fallback sprite is used.

## Setup instructions
1. Create `CurrencyData` assets from **Game Utils/Currency/Currency**.
2. Add a prefab containing `CurrencyManager` to the scene.
3. Use `Collectable` objects to increment currencies.
4. Connect a `CurrencyUITracker` to UI components to display current values.

## Examples

### Subscribing to the event
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

### Changing currency values
```cs
// Add 10 units
CurrencyManager.Instance.AddCurrency(goldData, 10);

// Try to remove 5 units
CurrencyManager.Instance.TryRemoveCurrency(goldData, 5);

// Convert 100 units into gems
CurrencyManager.Instance.TryExchangeCurrency(goldData, gemData, 100);
```
