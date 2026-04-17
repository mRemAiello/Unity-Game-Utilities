# Skill Trees

A data-driven, multi-level skill tree system inspired by Path of Exile 2 and Diablo 4. The architecture separates **data** (ScriptableObjects) from **runtime** (MonoBehaviours) so you can design skill logic independently of the UI layout.

## Architecture Overview

```
SkillTrees/
├── Data/
│   ├── SkillNodeData          Abstract SO – defines a single skill (cost, max level, effects)
│   ├── SkillEffectData         Abstract SO – a single effect applied/removed per level
│   └── SkillNodeState          Enum – Locked, Available, Unlocked, Maxed
├── Events/
│   ├── ClickSkillEventAsset            GameEventAsset<RuntimeSkillNode>
│   └── ChangeSkillStateEventAsset      GameEventAsset<RuntimeSkillNode, SkillNodeState, int>
├── Interfaces/
│   ├── ISkillContext           Service-locator for capabilities
│   ├── ISkillPointHandler      HasEnough / Spend / Refund
│   └── ISkillStateProvider     IsUnlocked / GetLevel / GetUnlockedSkillIDs
├── UI/
│   ├── RuntimeSkillNode        MonoBehaviour placed on each node in the UI
│   ├── LineSkillTreeLink       Straight line between two nodes (LineRenderer)
│   └── CurvedSkillTreeLink     Catmull-Rom curved line between nodes (LineRenderer)
└── BaseSkillContext            Concrete ISkillContext backed by a Type → object dictionary
```

## Core Classes

### `SkillNodeData` (abstract ScriptableObject)

Defines a skill node's **data and rules**. Inherits from `ItemVisualData`, so it already has an icon, color, localized name/description, and an ID.

| Field | Type | Description |
|---|---|---|
| `_currency` | `CurrencyData` | Which currency is spent to level up this node |
| `_costPerLevel` | `int` | Cost multiplier — actual cost = `costPerLevel × level` |
| `_maxLevel` | `int` | Maximum level the node can reach (1 = single unlock, 5 = five ranks) |
| `_effects` | `List<SkillEffectData>` | Effects applied on level up / removed on level down |

Key methods:

```csharp
// Returns true if the node can be leveled up (checks max level + currency)
bool CanLevelUp(int currentLevel);

// Returns the currency cost for a specific level
int GetCostForLevel(int level);  // costPerLevel * level

// Called when the node gains or loses a level
void OnLevelUp(ISkillContext context, int newLevel);
void OnLevelDown(ISkillContext context, int newLevel);
```

Create concrete subclasses for different node types (passive, keystone, notable, etc.):

```csharp
[CreateAssetMenu(menuName = "GameUtils/Skills/Passive Node")]
public class PassiveSkillNodeData : SkillNodeData
{
    // Add type-specific fields or override CanLevelUp for custom rules
}
```

### `SkillEffectData` (abstract ScriptableObject)

Represents a single effect that scales with level. Subclass it for stat boosts, event triggers, ability unlocks, etc.

```csharp
public abstract class SkillEffectData : ItemIdentifierData
{
    public abstract void Apply(ISkillContext context, int level);
    public abstract void Remove(ISkillContext context, int level);
}
```

Example implementation:

```csharp
[CreateAssetMenu(menuName = "GameUtils/Skills/Effects/Stat Modifier")]
public class StatModifierSkillEffect : SkillEffectData
{
    [SerializeField] private AttributeData _attribute;
    [SerializeField] private float _valuePerLevel = 5f;

    public override void Apply(ISkillContext context, int level)
    {
        // Add _valuePerLevel * level to the attribute
    }

    public override void Remove(ISkillContext context, int level)
    {
        // Remove the modifier
    }
}
```

### `SkillNodeState` (enum)

```
Locked     → Prerequisites not met, node is inactive
Available  → At least one prerequisite is unlocked/maxed, node can be activated
Unlocked   → Node is active, can be leveled up further
Maxed      → Node has reached its maximum level
```

### `RuntimeSkillNode` (MonoBehaviour)

The component you place on each node in the scene/UI. Holds **runtime state** and **prerequisite references** (other `RuntimeSkillNode` instances).

| Field | Description |
|---|---|
| `_data` | Reference to the `SkillNodeData` SO |
| `_prerequisiteNodes` | List of `RuntimeSkillNode` that must be unlocked first |
| `_levelUpAction` | `InputActionReference` — left click to level up / unlock |
| `_levelDownAction` | `InputActionReference` — right click to level down |
| `_onLevelUpRequest` | `ClickSkillEventAsset` — fired when the player requests a level up |
| `_onLevelDownRequest` | `ClickSkillEventAsset` — fired when the player requests a level down |
| `_onStateChanged` | `ChangeSkillStateEventAsset` — fired on state/level change (node, state, level) |

Key methods:

```csharp
// Update the node's visual state and level
void SetState(SkillNodeState newState, int level);

// Recalculate state from the context (checks ISkillStateProvider + prerequisites)
void RefreshState(ISkillContext context);

// Returns true if at least one prerequisite node is Unlocked or Maxed
bool ArePrerequisitesMet();

// Call from IPointerEnterHandler/IPointerExitHandler on a subclass
void SetHovered(bool hovered);
```

Input actions only fire events when the node is hovered. The manager listens to `_onLevelUpRequest` / `_onLevelDownRequest` to perform the actual unlock/level logic.

### `ISkillContext` / `BaseSkillContext`

A lightweight service-locator that allows skill logic to access external systems without hard dependencies.

```csharp
var context = new BaseSkillContext();
context.Add<ISkillStateProvider>(myStateProvider);
context.Add<ISkillPointHandler>(myPointHandler);

// Inside SkillNodeData or SkillEffectData:
if (context.TryGet<ISkillStateProvider>(out var provider))
{
    bool unlocked = provider.IsUnlocked(someSkillID);
    int level = provider.GetLevel(someSkillID);
}
```

### Visual Links

- **`LineSkillTreeLink`** — Draws a straight line between two `Transform` references using a `LineRenderer`. Works in edit mode.
- **`CurvedSkillTreeLink`** — Draws a Catmull-Rom spline through an array of control points. Works in edit mode.

Both update every frame and in `[ExecuteAlways]` mode, so you can position nodes in the Scene view and see the links update in real time.

## Setup Guide

### 1. Create Skill Data Assets

1. Create a subclass of `SkillNodeData` (e.g. `PassiveSkillNodeData`).
2. Add a `[CreateAssetMenu]` attribute.
3. Create SO assets in the Project window and configure currency, cost per level, max level, and effects.

### 2. Build the UI

1. Create a Canvas with your skill tree layout (nodes can be any UI element).
2. Add `RuntimeSkillNode` to each node GameObject.
3. Assign the `SkillNodeData` SO, prerequisite node references, and input actions in the Inspector.
4. Connect nodes visually with `LineSkillTreeLink` or `CurvedSkillTreeLink`.

### 3. Create a Manager

The manager (not yet included — implement per-project) should:

```csharp
// 1. Collect all RuntimeSkillNodes in the tree
// 2. Listen to the level up/down event assets
// 3. On level up request:
if (node.ArePrerequisitesMet() && node.Data.CanLevelUp(node.CurrentLevel))
{
    int cost = node.Data.GetCostForLevel(node.CurrentLevel + 1);
    CurrencyManager.Instance.TryRemoveCurrency(node.Data.Currency, cost);
    node.Data.OnLevelUp(context, node.CurrentLevel + 1);
    node.SetState(SkillNodeState.Unlocked, node.CurrentLevel + 1);
    // Refresh all nodes to update Available/Locked states
}

// 4. On level down request:
if (node.CurrentLevel > 0)
{
    int cost = node.Data.GetCostForLevel(node.CurrentLevel);
    CurrencyManager.Instance.AddCurrency(node.Data.Currency, cost);
    node.Data.OnLevelDown(context, node.CurrentLevel - 1);
    int newLevel = node.CurrentLevel - 1;
    var newState = newLevel == 0 ? SkillNodeState.Available : SkillNodeState.Unlocked;
    node.SetState(newState, newLevel);
}

// 5. Implement ISaveable to persist unlocked node IDs and levels via GameSaveManager
```

### 4. Subclass RuntimeSkillNode for Visuals

Override `ApplyVisualState()` and `Init()` to customize the look:

```csharp
public class MySkillNodeUI : RuntimeSkillNode, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _levelText;

    public override void Init()
    {
        // Load icon, set name, etc.
    }

    protected override void ApplyVisualState()
    {
        _icon.color = State switch
        {
            SkillNodeState.Locked    => Color.gray,
            SkillNodeState.Available => Color.white,
            SkillNodeState.Unlocked  => Color.yellow,
            SkillNodeState.Maxed     => Color.cyan,
            _ => Color.gray
        };
        _levelText.text = $"{CurrentLevel}/{Data.MaxLevel}";
    }

    public void OnPointerEnter(PointerEventData e) => SetHovered(true);
    public void OnPointerExit(PointerEventData e) => SetHovered(false);
}
```

## Multi-Level Cost Formula

Cost scales linearly with level by default:

| Level | Cost |
|---|---|
| 1 | `costPerLevel × 1` |
| 2 | `costPerLevel × 2` |
| 3 | `costPerLevel × 3` |

Override `CanLevelUp` or `GetCostForLevel` in a subclass for custom curves (exponential, flat, etc.).

## Dependencies

- **Currency System** — `CurrencyManager` + `CurrencyData` for spending/refunding skill points.
- **Save System** — `GameSaveManager` + `ISaveable` for persisting unlocked nodes.
- **Input System** — `InputActionReference` for level up/down input.
- **TriInspector** — Inspector attributes (`[Group]`, `[Required]`, `[ReadOnly]`).
- **Addressables** — Icon loading via `AssetReferenceSprite` (inherited from `ItemVisualData`).
