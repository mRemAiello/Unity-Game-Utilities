# Attribute System

ScriptableObjects and runtime components to define attributes (e.g. `Strength`, `Agility`, `Health`) and apply them to characters or game entities.

---

## Scriptable Objects

### `AttributeData`

Describes a single attribute with its validation rules.

| Field | Description |
|---|---|
| `MinValue` / `MaxValue` | Allowed value range. |
| `ClampType` | How the final value is rounded (see below). |
| `IsVital` | When enabled, the runtime instance will be a `RuntimeVital` instead of a plain `RuntimeAttribute`. |
| `RefreshCurrentValueOnChange` | *(Vital only)* When enabled, the current vital value is rescaled proportionally whenever the computed max changes. |

The `ComputeCurrentValue` and `ComputeCurrentMaxValue` methods are virtual, so you can subclass `AttributeData` to implement custom formulas that take `ClassData` context into account.

> **Create:** Right-click → *Create → Game Utils/Attributes/Attribute*

#### `AttributeClampType`

| Value | Behaviour |
|---|---|
| `RawFloat` | No rounding. |
| `Round` | `Mathf.Round` |
| `Floor` | `Mathf.Floor` |
| `Ceiling` | `Mathf.Ceil` |
| `Integer` | `Mathf.RoundToInt` |

---

### `ClassData`

An ordered list of `AttributeValuePair` (attribute + starting value) that represents the stat loadout of a character class.

Utility buttons:
- **`ApplyBlueprint`** – aligns the attribute list with a given `AttributeBlueprintData`, removing attributes not in the blueprint and reordering the rest.
- **`PopulateAttributes`** – adds any `AttributeData` asset found in the project that is not already in the list.

> **Create:** Right-click → *Create → Game Utils/Attributes/Class*

### `AttributeBlueprintData`

Defines an ordered template of `AttributeData` entries that can be reused across multiple `ClassData` assets to enforce a consistent attribute layout.

- **`PopulateAttributes`** button – auto-fills the blueprint with every `AttributeData` asset in the project.

> **Create:** Right-click → *Create → Game Utils/Attributes/Attribute Blueprint*

### `AttributeDataManager`

Singleton (`GenericDataManager<AttributeDataManager, AttributeData>`) that loads and centralises access to all `AttributeData` assets in the project. Runs at `DefaultExecutionOrder(-100)`.

---

## Runtime Classes

### `RuntimeAttribute`

Serializable object that holds the **base value** and **current value** of an attribute at runtime, together with a list of `Modifier` instances.

| Member | Description |
|---|---|
| `BaseValue` | The unmodified starting value. |
| `CurrentValue` | Value after modifiers have been applied and clamped. |
| `Modifiers` | Read-only list of active modifiers. |
| `AddModifier(modifier)` | Adds a modifier and recalculates. |
| `RemoveModifier(modifier, includePermanent)` | Removes a specific modifier. Permanent modifiers are skipped unless `includePermanent` is `true`. |
| `ClearModifiers(includePermanent)` | Removes all modifiers. |
| `GetModifier(source, amount, duration, type)` | Finds the first modifier matching the given parameters. |
| `GetModifiersBySource(source)` | Returns all modifiers from the given source. |
| `HasModifier(...)` | Returns `true` if a matching modifier exists. |
| `Refresh()` | Ticks modifier durations by `Time.deltaTime`, removes expired ones, and recalculates. |

**Virtual hooks** (override in subclasses to react to value changes):
- `OnChangeValue()` – called whenever the value changes.
- `OnMinValue()` – called when the value reaches `MinValue`.
- `OnMaxValue()` – called when the value reaches `MaxValue`.

### `RuntimeVital`

Extends `RuntimeAttribute` for vital-style stats (e.g. hit points, mana). Tracks a separate **current value** and a **computed max value** derived from base + modifiers.

| Member | Description |
|---|---|
| `CurrentMaxValue` | The derived maximum after modifiers. |
| `SetCurrentValue(value)` | Sets the current vital value, clamped between `MinValue` and `CurrentMaxValue`. |

When `RefreshCurrentValueOnChange` is enabled on the `AttributeData`, the current value is rescaled proportionally to the new max whenever modifiers cause the max to change.

Override `OnCurrentValueChange()` to react to changes in the vital value.

### `RuntimeClass`

`MonoBehaviour` that instantiates and manages runtime attributes on a GameObject.

| Inspector Field | Description |
|---|---|
| `_startWithClass` | Automatically calls `SetClass` in `Start`. |
| `_classData` | The `ClassData` asset to instantiate. |
| `_refreshClassOnUpdate` | Calls `RefreshAttributes()` every frame (ticks modifier durations). |

#### Key methods

| Method | Description |
|---|---|
| `SetClass(classData)` | Instantiates `RuntimeAttribute` / `RuntimeVital` for every entry in the class data. |
| `RefreshAttributes()` | Calls `Refresh()` on every attribute. |
| `GetAttribute<T>()` | Finds an attribute by `AttributeData` subtype. |
| `GetAttribute<TData, TRuntime>()` | Finds an attribute by data type and runtime type. |
| `GetAttribute(string id)` | Finds an attribute by ID. |
| `GetAttribute(AttributeData)` | Finds an attribute by reference. |
| `TryGetAttribute(...)` | Try-pattern variants of the above. |
| `GetVital<TData>()` / `TryGetVital<TData>()` | Shorthand for `GetAttribute<TData, RuntimeVital>()`. |
| `GetVital(string id)` / `GetVital(AttributeData)` | Gets a vital by ID or reference. |
| `IncreaseValue(data, amount)` | Increases a vital's current value by `amount`. |
| `DecreaseValue(data, amount)` | Decreases a vital's current value by `amount`. |
| `AddModifier(data/id, modifier)` | Adds a modifier to the specified attribute. |
| `RemoveModifier(data/id, modifier)` | Removes a modifier from the specified attribute. |
| `ClearModifiers(data/id)` | Removes all non-permanent modifiers from the specified attribute. |

---

## Modifiers

### `Modifier` (abstract)

Base class for temporary or permanent adjustments to an attribute.

| Field | Description |
|---|---|
| `Source` | Arbitrary reference to the modifier's origin (e.g. a buff object). |
| `Amount` | Numeric value used by the modifier. |
| `Duration` | Remaining time in seconds. Ticked down by `Refresh()`. |
| `ModifierType` | `Positive`, `Negative`, or `Neutral`. |
| `IsPermanent` | When `true`, the modifier never expires and is protected from casual removal. |
| `Order` | Application priority (lower values are applied first). |
| `ApplyModifier(value)` | Transforms the incoming value. |

### Built-in implementations

| Class | Order | Behaviour |
|---|---|---|
| `ModifierFixed` | 1 | Adds a flat amount: `value + Amount`. |
| `ModifierPercentage` | 2 | Applies a percentage: `value * (1 + Amount / 100)`. |

Because fixed modifiers run before percentage modifiers, a +10 flat bonus on a base of 50 followed by a +20% modifier results in `(50 + 10) * 1.2 = 72`.

---

## UI Components

### `BaseTrackerUI` (abstract)

Base `MonoBehaviour` for attribute display. Validates references and calls `RefreshUI()` every `Update`. Provides `FormatRuntimeAttribute()` to produce formatted strings (including vital current/max and optional modifier details).

### `AttributeTrackerUI`

Tracks a **single attribute** on a `RuntimeClass` and writes its formatted value to a `TextMeshProUGUI` label.

| Field | Description |
|---|---|
| `_targetRuntimeClass` | The `RuntimeClass` component to read from. |
| `_targetAttributeData` | Which `AttributeData` to display. |
| `_attributeTextLabel` | The TMP label to update. |
| `_showModifiers` | Appends modifier details to the label. |

### `ClassTrackerUI`

Tracks **all attributes** of a `RuntimeClass` and writes them to a single `TextMeshProUGUI` label.

---

## Recommended Workflow

1. Create `AttributeData` assets for each stat (set limits, clamp type, and vital flag).
2. *(Optional)* Create an `AttributeBlueprintData` and click **Populate** to auto-fill it. Use it to keep multiple classes in sync.
3. Create one or more `ClassData` assets and populate their `AttributeValuePair` lists with starting values (or **Apply Blueprint**).
4. Add a `RuntimeClass` component to a GameObject, assign the `ClassData`, and enable `_startWithClass`.
5. At runtime, use `AddModifier` / `RemoveModifier` to apply buffs and debuffs. Enable `_refreshClassOnUpdate` or call `RefreshAttributes()` manually to tick durations.
6. For vital attributes, use `IncreaseValue` / `DecreaseValue` or `SetCurrentValue` to modify the current value within the computed bounds.
7. *(Optional)* Add `AttributeTrackerUI` or `ClassTrackerUI` components to display attribute values on screen.

---

## Code Example

```csharp
using UnityEngine;
using GameUtils;

// Example custom AttributeData subclasses (create these as ScriptableObject assets).
// Right-click → Create → Game Utils/Attributes/Attribute
// [CreateAssetMenu(menuName = "Game Utils/Attributes/Strength")]
// public class StrengthData : AttributeData { }
//
// [CreateAssetMenu(menuName = "Game Utils/Attributes/Health")]
// public class HealthData : AttributeData { }  // Set IsVital = true in the inspector

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private RuntimeClass _runtimeClass;

    // Cached references
    private RuntimeAttribute _strength;
    private RuntimeVital _health;

    void Start()
    {
        // Retrieve attributes by data type.
        // StrengthData / HealthData are your custom AttributeData subclasses.
        _strength = _runtimeClass.GetAttribute<StrengthData>();
        _health = _runtimeClass.GetVital<HealthData>();

        Debug.Log($"Strength: {_strength.CurrentValue}");
        Debug.Log($"Health: {_health.CurrentValue}/{_health.CurrentMaxValue}");
    }

    /// <summary>
    /// Apply a temporary strength buff (+15 flat for 10 seconds).
    /// </summary>
    public void ApplyStrengthBuff()
    {
        var buff = new ModifierFixed(
            source: this,
            amount: 15f,
            duration: 10f,
            isPermanent: false
        );
        _strength.AddModifier(buff);
        Debug.Log($"Strength after buff: {_strength.CurrentValue}");
    }

    /// <summary>
    /// Apply a permanent +20% health bonus (e.g. from an equipped item).
    /// </summary>
    public void EquipHealthAmulet()
    {
        var bonus = new ModifierPercentage(
            source: this,
            amount: 20f,
            duration: 0f,
            isPermanent: true
        );
        _health.AddModifier(bonus);
        Debug.Log($"Health max after amulet: {_health.CurrentMaxValue}");
    }

    /// <summary>
    /// Deal damage to this character.
    /// </summary>
    public void TakeDamage(float damage)
    {
        _health.SetCurrentValue(_health.CurrentValue - damage);
        Debug.Log($"Health: {_health.CurrentValue}/{_health.CurrentMaxValue}");

        if (_health.CurrentValue <= _health.MinValue)
        {
            Debug.Log("Character is dead!");
        }
    }

    /// <summary>
    /// Heal this character.
    /// </summary>
    public void Heal(float amount)
    {
        _health.SetCurrentValue(_health.CurrentValue + amount);
    }

    /// <summary>
    /// Remove all non-permanent modifiers from strength.
    /// </summary>
    public void ClearTemporaryBuffs()
    {
        _strength.ClearModifiers(includePermanent: false);
    }
}
```
