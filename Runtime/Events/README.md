# Event System

This module provides a lightweight "Game Events" implementation based on `ScriptableObject` assets.
Each event asset stores runtime listeners and can be invoked directly from code.

## Scriptable event assets

You can create event assets from the `Game Utils/Events` menu (via `GameUtilsMenuConstants.EVENT_NAME`).
The default assets included in this folder are:

- `VoidEventAsset` — event without parameters
- `FloatEventAsset`, `IntEventAsset`
- `Vector2EventAsset`, `Vector3EventAsset`
- `QuaternionEventAsset`, `TransformEventAsset`, `GameObjectEventAsset`

Specialized event assets (for example `ModalWindowButtonEventAsset`) live in the feature folders that own their data types.

## Base classes and behavior

### `GameEventAssetBase`

`GameEventAssetBase` stores runtime listener metadata (`EventTuple`) for debug inspection.
It also exposes a `Log Enabled` toggle (via `ILoggable`) and a `ResetData()` method that clears the debug listener list (not the active callbacks).

### `GameEventAsset<T>`

`GameEventAsset<T>` extends the base class with the following API:

- `AddListener(Action<T>)`
- `RemoveListener(Action<T>)`
- `RemoveAllListeners()`
- `Invoke(T param)`

When `Invoke` is called, the asset:

- Logs (if `Log Enabled` is enabled).
- Stores the call in `_callHistory` and updates `CurrentValue`.
- Invokes all registered listeners.

### `GameEventAsset<T1, T2>`

`GameEventAsset<T1, T2>` extends the same pattern to events with two parameters:

- `AddListener(Action<T1, T2>)`
- `RemoveListener(Action<T1, T2>)`
- `RemoveAllListeners()`
- `Invoke(T1 param1, T2 param2)`

The package also provides multi-parameter generic variants with the same API shape:

- `GameEventAsset<T1, T2, T3>`
- `GameEventAsset<T1, T2, T3, T4>`
- `GameEventAsset<T1, T2, T3, T4, T5>`
- `GameEventAsset<T1, T2, T3, T4, T5, T6>`

Each variant stores call history for debug purposes, tracks the latest values in inspector properties, and invokes all registered listeners in order.

### `VoidEventAsset`

`VoidEventAsset` mirrors the same listener API without parameters:

- `AddListener(Action)`
- `RemoveListener(Action)`
- `RemoveAllListeners()`
- `Invoke()`

## Runtime management

`GameEventDataManager` inherits from `GenericDataManager` and resets all managed events on `Awake` and `OnDestroy`.
This clears the debug listener metadata and call history, but active listeners remain unless `RemoveAllListeners()` is called.

## Example usage

```cs
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private FloatEventAsset _damageEvent;

    private void OnEnable()
    {
        _damageEvent.AddListener(OnDamage);
    }

    private void OnDisable()
    {
        _damageEvent.RemoveListener(OnDamage);
    }

    private void OnDamage(float value)
    {
        Debug.Log($"Player damaged for {value}");
    }

    private void DealDamage()
    {
        _damageEvent.Invoke(10f);
    }
}
```
