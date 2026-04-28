# Save System

This module provides a simplified layer on top of **Quick Save** to store typed
data across multiple save slots.

Main components:
- `GameSaveManager`: persistent singleton responsible for save files and slot management.
- `ISaveable`: interface for scene components that expose save/load behavior.
- `BaseSettingData<T>` and `SettingBinder<T, TUI>`: optional helpers to keep setting assets and UI controls in sync.

## Overview

The save flow is context-based:
- A **context** groups keys (for example `"PlayerInventory"`, `"Deck"`, `"VideoSettings"`).
- A **key** identifies one value inside that context (for example `"Money"`, `"Card1"`, `"MasterVolume"`).
- The active **slot** decides which save file is currently used.

This makes it easy to avoid key collisions and keep save data organized per system.

## GameSaveManager API

`GameSaveManager` is a persistent singleton. It creates the save file automatically if needed
and keeps a debug dictionary of registered keys.

Common methods:
- `SetActiveSaveSlot(int slot)`: switch active slot.
- `Save<T>(string context, string key, T value)`: store a typed value.
- `Load<T>(string context, string key, T defaultValue)`: load a typed value or fallback.
- `TryLoad<T>(string context, string key, out T result, T defaultValue)`: load safely and return `true` only if the key exists.
- `Exists<T>(string context, string key)`: check whether a key is present.
- `RemoveKey<T>(string context, string key)`: remove a specific key.
- `Clear()`: clear all keys in the active slot.
- `SaveAll()` and `LoadAll()`: call `Save()` and `Load()` on every `ISaveable` component found in the scene.

## ISaveable

`ISaveable` exposes:
- `string SaveContext { get; }`
- `void Save()`
- `void Load()`

Implement it on components that should participate in save/load cycles. This keeps each
component's data under a stable context and allows `GameSaveManager.SaveAll()` / `LoadAll()`
to process all saveable scene objects automatically.

```cs
public class PlayerInventory : MonoBehaviour, ISaveable
{
    public string SaveContext => "PlayerInventory";

    public void Save()
    {
        GameSaveManager.Instance.Save(this, "Money", 100);
    }

    public void Load()
    {
        int money = GameSaveManager.Instance.Load<int>(this, "Money", 0);
    }
}
```

## Quick Start

1. Add a `GameObject` with `GameSaveManager` to your scene.
2. Implement `ISaveable` on components that need persistence.
3. Save and load values through the manager using context + key.

```cs
// Save through ISaveable context
GameSaveManager.Instance.Save(this, "Money", 100);

// Save with an explicit custom context
GameSaveManager.Instance.Save("Deck", "Card1", "123x1123");

// Load
int money = GameSaveManager.Instance.Load<int>(this, "Money", 0);
string card1 = GameSaveManager.Instance.Load<string>("Deck", "Card1", "");
string card2 = GameSaveManager.Instance.Load<string>("Deck", "Card2", "");
string card3 = GameSaveManager.Instance.Load<string>("Deck", "Card3", "");
```

## Auto Save/Load for Scene Objects

Use `SaveAll()` and `LoadAll()` when you want a centralized save/load pass:
- Good for checkpoints, scene transitions, profile changes, and quit flows.
- Ensures all active `ISaveable` components are processed with a single call.

## Extending the Manager

You can derive from `GameSaveManager` to add custom behavior (for example encryption,
analytics, or extra logging) by overriding save-related methods.

```cs
public class EncryptedSaveManager : GameSaveManager
{
    protected override void Save<T>(string context, string key, T value)
    {
        // Apply custom logic before writing data.
        base.Save(context, key, value);
    }
}
```

## Settings Data and UI Binding

UI binders can subscribe to `BaseSettingData<T>.OnValueChanged` to react whenever a value changes.

```cs
mySetting.OnValueChanged += value => mySlider.value = value;
```

The event is raised both when:
- the value changes via `SetValue`, and
- the value is restored via `Load`.

### Configure a Setting with UI

1. Create a `SettingData` asset.
2. Create a binder class deriving from `SettingBinder<T, TUI>`.
3. Assign references in the Inspector (`Data` and `UI Component`).

Example: bind a `Slider` to a `float` setting.

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

At runtime, the setting asset and the UI control stay synchronized.

## Best Practices

- Keep `SaveContext` stable over time to avoid migration issues.
- Use clear key names (`"MasterVolume"` instead of generic names like `"Value1"`).
- Prefer `TryLoad` when the presence of a key is optional.
- Group save/load calls by game flow (checkpoint, scene exit, profile switch).
- Use slot switching intentionally (`SetActiveSaveSlot`) before reading or writing data.