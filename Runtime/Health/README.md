# Vital Systems

MonoBehaviour wrappers for `RuntimeVital` attributes that provide Unity-specific logic and gameplay integration for health, mana, stamina, shields, and other vital systems.

---

## Overview

The vital systems are designed to wrap `RuntimeVital` instances from the [Attribute System](../README.md) with MonoBehaviour-specific logic including:

- **Event integration** with the existing `GameEventAsset` system
- **Auto-discovery** of `RuntimeClass` components
- **Game logic** like damage, healing, regeneration, invulnerability
- **Inspector configuration** for designers via TriInspector attributes

This architecture separates data (AttributeData, RuntimeVital) from gameplay logic (VitalSystem MonoBehaviours), promoting reusability and flexibility.

---

## Base Class

### `BaseVitalSystem`

Abstract MonoBehaviour that provides the foundation for all vital systems.

**Key Features:**
- Auto-discovers `RuntimeClass` on the same GameObject or parent (configurable)
- Retrieves the corresponding `RuntimeVital` at runtime based on `AttributeData` reference
- Validates that the AttributeData is marked as `IsVital`
- Exposes common properties: `CurrentValue`, `MaxValue`, `MinValue`, `Percentage`, `IsDepleted`, `IsAtMax`
- Supports common operations: `AddValue()`, `SubtractValue()`, `SetToMin()`, `SetToMax()`
- Integrates with `GameEventAsset` system for optional event broadcasting

**Inspector Fields:**
- **Vital Setup** group:
  - `RuntimeClass` — Reference to the RuntimeClass component (auto-discovered if null)
  - `AttributeData` — The AttributeData asset representing this vital (e.g., Health, Mana)
  - `Auto Discover RuntimeClass` — Automatically find RuntimeClass on GameObject or parents
- **Events** group:
  - `On Value Changed` — `FloatEventAsset` invoked when value changes (passes new value)
  - `On Depleted` — `VoidEventAsset` invoked when vital reaches minimum value
  - `On Max Reached` — `VoidEventAsset` invoked when vital reaches maximum value

**Virtual Hooks** (override in subclasses):
- `OnVitalInitialized()` — Called after successful vital initialization
- `OnValueChanged(oldValue, newValue)` — Called whenever the value changes
- `OnDepleted()` — Called when reaching minimum value
- `OnMaxReached()` — Called when reaching maximum value

---

## Concrete Systems

### `VitalSystem`

Generic vital system for attributes like **health, shields, barriers, runes**, etc. Provides damage and restoration mechanics without automatic regeneration.

**Additional Features:**
- **Damage/Restore API**: `TakeDamage(amount, source)`, `Restore(amount, source)`, `Destroy()`
- **Invulnerability System**: `SetInvulnerable(enabled, duration)` with timer support
- **Source Tracking**: All damage/restore operations can track the source object

**Additional Inspector Fields:**
- **Vital Events** group:
  - `On Damaged` — `GameEventAsset<float, object>` invoked when damage is taken (amount, source)
  - `On Restored` — `GameEventAsset<float, object>` invoked when vital is restored (amount, source)
  - `On Destroyed` — `VoidEventAsset` invoked when vital is depleted/destroyed
- **Settings** group:
  - `Invulnerable` — Whether the system is currently invulnerable
  - `Invulnerability Duration` — Duration of invulnerability in seconds (0 = permanent)

**Public API:**
```csharp
// Damage and restoration
vitalSystem.TakeDamage(10f, damageSource);
vitalSystem.Restore(5f, healingItem);
vitalSystem.Destroy(); // Instantly depletes to minimum

// Invulnerability
vitalSystem.SetInvulnerable(true, 2f); // Invulnerable for 2 seconds
vitalSystem.SetInvulnerable(true, 0f); // Invulnerable until explicitly disabled
vitalSystem.SetInvulnerable(false); // Ends either mode; repeated calls do not emit another end notification
bool invuln = vitalSystem.IsInvulnerable;
```

`IsInvulnerable` reports the active runtime state independently of the temporary duration remaining. Positive durations expire automatically, while zero or negative durations remain active until `SetInvulnerable(false)` is called. Calling `SetInvulnerable(true, duration)` again refreshes the duration without emitting another start notification.

**Virtual Hooks:**
- `OnDamaged(amount, source)` — Called when damage is successfully applied
- `OnRestored(amount, source)` — Called when restoration is successful
- `OnDestroyed()` — Called when vital is destroyed (reaches minimum)
- `OnInvulnerabilityStarted()` — Called when invulnerability begins
- `OnInvulnerabilityEnded()` — Called once when invulnerability expires or is manually disabled

**Use Cases:**
- Player health systems
- Enemy health bars
- Shield/armor systems that absorb damage
- Destructible barriers or structures
- Resource pools that don't auto-regenerate

---

### `RegeneratingVitalSystem`

Generic regenerating vital system for attributes like **mana, stamina, energy, spirit**, etc. Supports automatic regeneration with configurable rate, delay, and consumption mechanics.

**Additional Features:**
- **Consumption/Restore API**: `Consume(amount, source)`, `TryConsume(amount, source)`, `Restore(amount, source)`
- **Auto-Regeneration**: Configurable regen rate, delay after consumption, and behavior at max
- **Regeneration Control**: Manual `StartRegeneration()`, `StopRegeneration()` methods
- **State Tracking**: `IsRegenerating` property

**Additional Inspector Fields:**
- **Regeneration Events** group:
  - `On Consumed` — `GameEventAsset<float, object>` invoked when vital is consumed (amount, source)
  - `On Restored` — `GameEventAsset<float, object>` invoked when vital is restored (amount, source)
  - `On Regen Started` — `VoidEventAsset` invoked when regeneration begins
  - `On Regen Stopped` — `VoidEventAsset` invoked when regeneration stops
  - `On Depleted` — `VoidEventAsset` invoked when vital reaches zero
- **Regeneration Settings** group:
  - `Auto Regenerate` — Enable automatic regeneration over time
  - `Regen Rate` — Amount regenerated per second
  - `Regen Delay` — Delay in seconds before regeneration starts after consumption
  - `Regen While At Max` — Whether to continue regen coroutine when at maximum (normally false)

**Public API:**
```csharp
// Consumption and restoration
regenSystem.Consume(20f, ability);
bool success = regenSystem.TryConsume(30f, spell); // Returns false if insufficient
regenSystem.Restore(15f, potion);

// Manual regeneration control
regenSystem.StartRegeneration();
regenSystem.StopRegeneration();
bool isRegen = regenSystem.IsRegenerating;
```

**Virtual Hooks:**
- `OnConsumed(amount, source)` — Called when consumption is successful
- `OnRestored(amount, source)` — Called when restoration is successful
- `OnRegenStarted()` — Called when regeneration begins
- `OnRegenStopped()` — Called when regeneration stops
- `RegenerateCoroutine()` — Override to customize regeneration behavior

**Use Cases:**
- Mana/magic systems for spellcasting
- Stamina systems for sprinting/dodging
- Energy bars for abilities
- Spirit or focus resources
- Any resource that regenerates over time after use

---

## Integration with Attribute System

Both systems integrate seamlessly with the existing [Attribute System](../README.md):

1. **AttributeData** — Create an `AttributeData` asset with `IsVital = true` (e.g., "Health", "Mana")
2. **ClassData** — Add the attribute to a `ClassData` asset with starting values
3. **RuntimeClass** — Attach `RuntimeClass` MonoBehaviour to GameObject and assign the ClassData
4. **Vital System** — Attach `VitalSystem` or `RegeneratingVitalSystem` to the same GameObject or child
5. **Configure** — Set the `AttributeData` reference and optionally configure event assets

The vital system will automatically retrieve the `RuntimeVital` instance at runtime and wrap it with gameplay logic.

---

## Event System Integration

All vital systems integrate with the existing [Event System](../../Events/README.md) using `GameEventAsset` ScriptableObjects:

**Advantages:**
- **Decoupling**: Events can be shared across multiple GameObjects
- **Designer-friendly**: Configure event routing in the Inspector
- **Debugging**: Event assets show call history and current values
- **Flexibility**: Optional events (leave field null if not needed)

**Event Types Used:**
- `VoidEventAsset` — Events without parameters (OnDepleted, OnDestroyed, OnRegenStarted)
- `FloatEventAsset` — Events with single float parameter (OnValueChanged)
- `GameEventAsset<float, object>` — Events with amount and source (OnDamaged, OnRestored, OnConsumed)

**Example Setup:**
1. Create event assets: Right-click → Create → Game Utils/Events → Float Event (or appropriate type)
2. Assign event assets to vital system fields in Inspector
3. Other systems can listen to these events:
```csharp
[SerializeField] private FloatEventAsset _playerHealthChanged;

void Start()
{
    _playerHealthChanged?.AddListener(this, OnPlayerHealthChanged);
}

void OnPlayerHealthChanged(float newHealth)
{
    Debug.Log($"Player health: {newHealth}");
}
```

---

## Usage Examples

### Example 1: Player Health System

```csharp
// 1. Create AttributeData asset: "Health" with IsVital=true, MinValue=0, MaxValue=100
// 2. Add to ClassData: "Player" class with Health starting at 100
// 3. Setup GameObject:

GameObject player = new GameObject("Player");
RuntimeClass runtimeClass = player.AddComponent<RuntimeClass>();
runtimeClass.SetClass(playerClassData); // Assign your ClassData

VitalSystem healthSystem = player.AddComponent<VitalSystem>();
// healthSystem will auto-discover runtimeClass and retrieve Health RuntimeVital

// 4. Usage in gameplay:
healthSystem.TakeDamage(25f, enemy);
healthSystem.Restore(10f, healthPotion);
healthSystem.SetInvulnerable(true, 2f); // Invulnerable for 2 seconds

if (healthSystem.IsDepleted)
{
    Debug.Log("Player died!");
}
```

### Example 2: Enemy Mana System

```csharp
// 1. Create AttributeData asset: "Mana" with IsVital=true, MinValue=0, MaxValue=50
// 2. Add to ClassData: "Wizard" class with Mana starting at 50
// 3. Setup GameObject:

GameObject wizard = new GameObject("Wizard");
RuntimeClass runtimeClass = wizard.AddComponent<RuntimeClass>();
runtimeClass.SetClass(wizardClassData);

RegeneratingVitalSystem manaSystem = wizard.AddComponent<RegeneratingVitalSystem>();
// Configure in Inspector:
// - Auto Regenerate = true
// - Regen Rate = 5 (regenerates 5 mana per second)
// - Regen Delay = 2 (starts regenerating 2 seconds after last consumption)

// 4. Usage in spellcasting:
if (manaSystem.TryConsume(30f, fireball))
{
    CastFireball();
}
else
{
    Debug.Log("Not enough mana!");
}

// Mana will automatically start regenerating after 2 seconds
```

### Example 3: Custom Subclass with Special Logic

```csharp
public class PlayerHealthSystem : VitalSystem
{
    [SerializeField] private AudioClip _damageSound;
    [SerializeField] private ParticleSystem _deathEffect;

    protected override void OnDamaged(float amount, object source)
    {
        base.OnDamaged(amount, source);
        // Play damage sound and visual feedback
        AudioSource.PlayClipAtPoint(_damageSound, transform.position);
        // Trigger damage animation, screen shake, etc.
    }

    protected override void OnDestroyed()
    {
        base.OnDestroyed();
        // Spawn death particles
        Instantiate(_deathEffect, transform.position, Quaternion.identity);
        // Trigger ragdoll, respawn logic, game over screen, etc.
        StartCoroutine(HandleDeath());
    }

    private IEnumerator HandleDeath()
    {
        yield return new WaitForSeconds(2f);
        // Respawn or load game over scene
    }
}
```

---

## Best Practices

1. **Use Event Assets for Cross-System Communication**
   - Create reusable event assets (e.g., "Player Health Changed") that multiple systems can listen to
   - Keep event assets organized in dedicated folders

2. **Leverage Auto-Discovery**
   - Let vital systems auto-discover RuntimeClass for cleaner inspector setup
   - Only manually assign RuntimeClass when using non-standard hierarchies

3. **Validate AttributeData**
   - Always mark AttributeData as `IsVital = true` when using with vital systems
   - Systems will log warnings if this is not set correctly

4. **Extend with Subclasses**
   - Create custom subclasses for game-specific behavior (death animations, respawn logic, etc.)
   - Override virtual hooks rather than modifying core system classes

5. **Consider Modifiers for Temporary Effects**
   - Use the existing `Modifier` system for temporary buffs/debuffs to max health, regen rate, etc.
   - Modifiers integrate seamlessly with RuntimeVital and will update vital systems automatically

6. **Source Tracking**
   - Pass meaningful source objects to damage/consume methods for analytics and gameplay logic
   - Sources can be GameObjects, Components, ScriptableObjects, or any object reference

7. **Performance**
   - Regenerating systems use coroutines for smooth regeneration
   - Disable `Auto Regenerate` when not needed to save performance
   - Event invocations are optional (null checks prevent unnecessary overhead)

---

## Future Extensions

Potential extensions built on this system:

- **StaminaSystem**: Subclass of RegeneratingVitalSystem with sprint/dodge integration
- **ShieldSystem**: Subclass of VitalSystem that absorbs damage before health
- **Damage Types**: Extend VitalSystem to support typed damage (physical, magic, fire, etc.)
- **Multi-Vital Coordination**: Systems that coordinate multiple vitals (e.g., shield absorbs before health)
- **Death Handling**: Specialized health systems with respawn, ragdoll, and game over logic
- **Resource Costs**: Ability systems that consume multiple vital types simultaneously

---

## Related Systems

- [Attribute System](../README.md) — Foundation for all attributes and vitals
- [Event System](../../Events/README.md) — ScriptableObject-based event architecture
- [Modifiers](../Modifiers/) — Temporary and permanent stat modifiers

---

## Troubleshooting

**"Vital not initialized" error:**
- Ensure the GameObject has a `RuntimeClass` component
- Verify the `AttributeData` is added to the `ClassData` used by RuntimeClass
- Check that `AttributeData.IsVital = true`

**Auto-discovery not working:**
- Ensure RuntimeClass is on the same GameObject or a parent
- Try manually assigning the RuntimeClass reference
- Check that RuntimeClass has been initialized (runs in Start)

**Events not firing:**
- Verify event asset references are assigned in Inspector
- Check that listeners are added with `AddListener(this, method)`
- Ensure event assets are not null

**Regeneration not starting:**
- Verify `Auto Regenerate = true` in Inspector
- Check that vital is not already at maximum
- Ensure `Regen Delay` has elapsed since last consumption
