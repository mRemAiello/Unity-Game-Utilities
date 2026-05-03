# Save System

This module provides a simplified layer on top of **Quick Save** to store typed
data across multiple save slots.

Main components:
- `GameSaveManager`: persistent singleton responsible for save files and slot management.
- `ISaveable`: interface for scene components that expose state capture/restore behavior.
- `PersistentID`: component that generates and maintains unique, stable IDs for GameObjects.
- `BaseSettingData<T>` and typed variants: helpers to manage persistent settings with optional UI synchronization.

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
- `string SaveContext { get; }`: The context group for this component's save data
- `object CaptureState()`: Serialize the component's current state into a serializable object
- `void RestoreState(object state)`: Deserialize and apply the saved state

Implement it on components that should participate in save/load cycles via `SaveAll()` / `LoadAll()`. 
This keeps each component's data under a stable context.

```cs
public class PlayerInventory : MonoBehaviour, ISaveable
{
    [System.Serializable]
    public class InventoryState
    {
        public int money;
        public int itemCount;
    }

    public int Money { get; set; }
    public int ItemCount { get; set; }

    public string SaveContext => "PlayerInventory";

    public object CaptureState()
    {
        return new InventoryState
        {
            money = Money,
            itemCount = ItemCount
        };
    }

    public void RestoreState(object state)
    {
        if (state is InventoryState data)
        {
            Money = data.money;
            ItemCount = data.itemCount;
        }
    }
}
```

## Quick Start

1. Add a `GameObject` with `GameSaveManager` to your scene (or it will auto-create as a singleton).
2. Implement `ISaveable` on components that need persistence via `SaveAll()` / `LoadAll()`.
3. Or save/load individual values through the manager using context + key.

**Option A: Using ISaveable for automatic save/load**

```cs
// In a scene object
public class PlayerStats : MonoBehaviour, ISaveable
{
    public string SaveContext => "PlayerStats";
    
    public object CaptureState() => new { health = 100, level = 5 };
    public void RestoreState(object state) { /* restore from state */ }
}

// Then save/load all ISaveable components in the scene:
GameSaveManager.Instance.SaveAll();
GameSaveManager.Instance.LoadAll();
```

**Option B: Manual key-value storage**

```cs
// Save individual values
GameSaveManager.Instance.Save("Deck", "Card1", "123x1123");
GameSaveManager.Instance.Save("PlayerStats", "Money", 500);

// Load with fallback
int money = GameSaveManager.Instance.Load<int>("PlayerStats", "Money", 0);
string card1 = GameSaveManager.Instance.Load<string>("Deck", "Card1", "");

// Check if a key exists
if (GameSaveManager.Instance.Exists<string>("Deck", "Card1"))
{
    // Key exists, safe to load
}
```

**Switching save slots**

```cs
// Save slot 0
GameSaveManager.Instance.SetActiveSaveSlot(0);
GameSaveManager.Instance.SaveAll();

// Switch to slot 1 and load different data
GameSaveManager.Instance.SetActiveSaveSlot(1);
GameSaveManager.Instance.LoadAll();
```

## Auto Save/Load for Scene Objects

Use `SaveAll()` and `LoadAll()` when you want a centralized save/load pass:
- Good for checkpoints, scene transitions, profile changes, and quit flows.
- Ensures all active `ISaveable` components are processed with a single call.
- Automatically discovers and processes all `ISaveable` components in the active scene.

### Automatic Save Feature

Enable automatic periodic saves via Inspector settings:
- `Auto Save Enabled`: Toggles the feature on/off
- `Save Interval`: Time in seconds between auto-save calls (default: 5 seconds)

Use these methods:
- `StartAutoSave()`: Begin periodic saves (if not already running)
- `StopAutoSave()`: Stop periodic saves

```cs
// Auto save runs in background every 5 seconds
GameSaveManager.Instance.StartAutoSave();

// Stop when you're done
GameSaveManager.Instance.StopAutoSave();
```

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

`BaseSettingData<T>` and its typed variants provide a clean way to manage persistent settings with optional UI synchronization.

Available built-in types:
- `BoolSettingData`: For boolean settings (toggles, checkboxes)
- `IntSettingData`: For integer settings (player level, difficulty, etc.)
- `FloatSettingData`: For floating-point settings (volume, brightness, sensitivity)
- `StringSettingData`: For text settings (player name, language preference)

### Setting Features

Each setting provides:
- `OnValueChanged` event: Fired whenever the value changes via `SetValue()` or `Load()`
- `GetValue()`: Retrieve the current value (loads from save if available)
- `SetValue(T newValue)`: Update and persist the value
- `Load()`: Refresh from disk (useful after switching save slots)

UI binders can subscribe to `OnValueChanged` to react whenever a value changes:

```cs
mySetting.OnValueChanged += value => mySlider.value = value;
```

The event is raised both when:
- the value changes via `SetValue()`, and
- the value is restored via `Load()`.

### Configure a Setting with UI

1. Create a `[SettingType]SettingData` asset (right-click in Project → Create → GameUtils → [BoolSetting|IntSetting|FloatSetting|StringSetting] Data).
2. Create a binder class deriving from `SettingBinder<T, TUI>` for your UI component.
3. Assign the Setting asset and UI Component in the Inspector.

Example: bind a `Slider` to a `float` setting.

```cs
using UnityEngine.UI;
using GameUtils;

public class SliderSettingBinder : SettingBinder<float, Slider>
{
    protected override void AddUIListener() =>
        _uiComponent.onValueChanged.AddListener(_ => OnUIValueChanged());

    protected override void RemoveUIListener() =>
        _uiComponent.onValueChanged.RemoveListener(_ => OnUIValueChanged());

    protected override void SetUIValue(float value) => 
        _uiComponent.value = value;

    protected override float GetUIValue() => 
        _uiComponent.value;
}
```

At runtime, the setting asset and the UI control stay synchronized: changing the slider updates the setting, and loading a new save slot updates the slider.

## PersistentID

`PersistentID` is a component that assigns a unique, stable identifier to GameObjects. It's automatically generated in the editor and persists across sessions.

**Features:**
- Auto-generated unique ID when the component is first added
- Ensures uniqueness across scene instances
- Editor-only validation (does not affect runtime performance)
- Useful for tracking specific GameObjects across saves (e.g., specific enemies, NPCs, or interactable objects)

### Setup

Simply add the `PersistentID` component to any GameObject that needs a stable identifier:

```cs
public class SpecialEnemy : MonoBehaviour
{
    private PersistentID _persistentID;

    private void Awake()
    {
        _persistentID = GetComponent<PersistentID>();
    }

    public string GetStableID() => _persistentID.ID;
}
```

The ID is generated automatically and visible in the Inspector. No manual configuration needed.

## Best Practices

- Keep `SaveContext` stable over time to avoid migration issues.
- Use clear key names (`"MasterVolume"` instead of generic names like `"Value1"`).
- Prefer `TryLoad` when the presence of a key is optional.
- Group save/load calls by game flow (checkpoint, scene exit, profile switch).
- Use slot switching intentionally (`SetActiveSaveSlot`) before reading or writing data.
- Use `SaveAll()` / `LoadAll()` for complete scene-wide persistence rather than individual calls.
- For settings with UI, always use `BaseSettingData<T>` subclasses to ensure consistency between data and UI.
- Test save slot switching to ensure data properly isolates between slots.
- Add `PersistentID` to GameObjects that need unique identification across sessions.