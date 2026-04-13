# Unity Game Utilities

A lightweight, modular and easy to use **Unity utility package** providing reusable runtime systems, editor tooling, and experimental gameplay modules (events, save system, extensions, projectiles, currency, and more) to accelerate game development workflows.

## Why this exists

After working on several Unity projects, the same issues kept resurfacing:

- Rebuilding core runtime systems from scratch
- Tightly coupled logic that made refactoring difficult
- Unity abstractions that introduced overhead without providing real value
- Systems that worked early in development but didn't scale well over time

This framework was created as a practical response to those problems.

The goal is to keep systems **explicit**, **modular**, and **easy to extend**, **remove**, or **replace** as project requirements evolve.

Rather than enforcing a rigid architecture, it focuses on providing a lightweight and flexible foundation for building runtime systems that remain maintainable as projects grow.

## Requirements

To use this package you need the following assets:

### Unity Packages

* [Addressables (2.7.6+)](https://docs.unity3d.com/Packages/com.unity.addressables@2.1/manual/index.html)
* [New Input System](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.10/manual/index.html)

### Third-party

* [Tri-Inspector](https://github.com/codewriter-packages/Tri-Inspector)
* [Quick Save](https://www.claytoninds.com/quick-save)
* [DOTween](http://dotween.demigiant.com/)

---

## Included Systems

### Runtime

#### [Achievements](Runtime/Achievements/README.md)
Defines achievement data via `ScriptableObject` assets and manages them at runtime through a central `AchievementManager`. Supports two types: **Simple** (immediate condition check) and **Progress** (gradual value accumulation). The manager listens for game events, updates `RuntimeAchievement` instances, and broadcasts unlock/cancel notifications. A built-in `AchievementVisualizer` component can display on-screen notifications when a milestone is reached.

#### [Attributes](Runtime/Attributes/README.md)
A complete attribute/stat system built on `ScriptableObject` assets. Define named attributes (e.g. `Strength`, `Health`) with value ranges, rounding rules, and optional vital semantics. Attach them to characters via `ClassData` blueprints. At runtime, `RuntimeAttribute` instances support timed and permanent **modifiers** (add, remove, refresh), and `RuntimeVital` adds a current/max split with proportional rescaling. A `GenericDataManager` subclass centralizes lookups across the project.

#### Audio
Utilities and components for audio clip playback and runtime audio handling.

#### [Commands](Runtime/Commands/README.md)
Implements a sequential command queue using `ScriptableObject`-based commands. Create commands by subclassing `Command` and implementing `Execute`. The `CommandManager` singleton handles queuing — a new command starts immediately if the queue is idle, or waits until the current one calls `CommandExecutionComplete()`. Useful for card games, turn-based systems, or any gameplay flow that requires serialized, decoupled actions.

#### Constant
Stores shared constants and global values used across runtime systems.

#### [Currency](Runtime/Currency/README.md)
A multi-currency system built on `ScriptableObject` assets. Each `CurrencyData` defines a currency type (Normal, Premium, Special), a maximum amount, and optional conversion rates to other currencies. The `CurrencyManager` singleton persists values via the Save system and exposes methods to add, remove, and convert currency. A `Collectable` component handles pick-up logic, while `CurrencyUITracker` binds a UI element to a currency and loads its icon asynchronously via Addressables.

#### [DataStructures](Runtime/DataStructures/README.md)
A hierarchy of `ScriptableObject` base classes for item data. `ItemIdentifierData` provides a stable GUID-based ID. `ItemTagsData` adds tag support via `ITaggable`. `ItemLocalizedData` layers in Unity Localization fields (name and description). `ItemVisualData` adds an Addressables icon reference, a color, and a list of serializable `ItemFeatureData` entries for arbitrary per-item metadata. The companion `ItemAssetManager` enables fast O(1) lookups by internal name.

#### Debug
Debugging helpers and logging-focused runtime utilities.

#### Dictionary
Serializable dictionary implementations and related helpers.

#### [Drag & Drop](Runtime/DragAndDrop/README.md)
A drag-and-drop workflow for card-like scene objects using the Unity Input System. The `DragAndDropManager` singleton detects draggable objects (`IDraggable`) and drop targets (`IDroppable`) through configurable layer masks and raycasts, then routes pointer events accordingly. `CardDrag` provides a ready-made `IDraggable` implementation with DOTween-based lift, drop, and return-to-origin animations, plus virtual hooks for custom behavior. `CardTilter` adds a pitch/roll tilt effect for visual feedback during dragging.

#### [Events](Runtime/Events/README.md)
A lightweight `ScriptableObject`-based event system for decoupled communication. Event assets (`VoidEventAsset`, `FloatEventAsset`, `GameObjectEventAsset`, etc.) store runtime listeners and can be invoked directly from code or the inspector. Generic base classes support up to six typed parameters. Events keep an inspector-visible call history and current value for debugging, and a `GameEventDataManager` flushes debug state on scene transitions without removing active subscriptions.

#### [Extensions](Runtime/Extensions/README.md)
General-purpose extension methods covering `List<T>` (random pick, pop, shuffle, insert before/after, etc.), `Array`, `Camera` (background color channels), `Color`/`Color32` (per-channel fluent setters, grayscale), `Dictionary`, `Vector2`/`Vector3`, `Transform`, and more. Designed to reduce boilerplate in everyday Unity code.

#### [Input](Runtime/Input/README.md)
Runtime utilities for pointer and mouse input using the new Input System. `ClickInputManager` raycasts from the camera on click and delivers the world-space hit point to any `IClickable` on the hit collider. `UIMouseInputProvider` wraps EventSystems callbacks (click, drag, drop, hover) into `Action<PointerEventData>` delegates accessible through the `IMouseInput` interface, including a `DragDirection` enum for estimated swipe direction.

#### [Math](Runtime/Math/README.md)
Lightweight math helpers: `FloatMinMax` and `IntMinMax` are serializable range types with a `GetValue()` method that returns a random value within the range. `MathUtility` provides `Normalize` and `SmoothTime` static helpers. `Point` is a simple integer 2D coordinate with value equality.

#### Misc
Assorted generic utilities that do not belong to a specific runtime domain.

#### [Modal Window](Runtime/ModalWindow/README.md)
A reusable UI layer for confirmation dialogs and prompts. `ModalWindowBase` handles header/body text assignment, runtime button creation (`AddButton`), animator-driven Show/Close lifecycle, and button cleanup. `ModalWindowSimple` is a ready-to-use concrete implementation. Each button is backed by `ModalWindowButton`, which fires both a direct `UnityAction` callback and an optional `ModalWindowButtonEventAsset` for listener-based decoupling.

#### Movement
Movement-related scripts and helpers for entity locomotion.

#### Placement
Runtime systems and helpers for object placement workflows.

#### [Projectiles](Runtime/Projectiles/README.md)
A 2D projectile system with curve-driven trajectories. `Projectile2D` moves toward a target position by sampling three `AnimationCurve` assets every frame: **ProjSpeedCurve** (velocity over time), **TrajectoryCurve** (arc height), and **AxisCorrectionCurve** (secondary-axis correction). On arrival the `OnHit` event fires, enabling pooling via `IPoolable` or triggering effects. Add `ProjectileVisual2D` for a shadow and rotation-following visual. Use the `Shooter` component for turnkey periodic spawning and initialization.

#### Random
Randomization utilities and helper methods for gameplay variability.

#### [Rarity](Runtime/Rarity/README.md)
Defines item rarity tiers as `ScriptableObject` assets derived from `ItemVisualData`, carrying colors, icons, and feature data. `RarityManager` loads all rarity assets at startup and provides fast lookup by internal name or ID, making it straightforward to color-code or style items based on their rarity tier.

#### Renderer
Custom rendering-related helpers and runtime renderer components.

#### [Save](Runtime/Save/README.md)
A typed persistence layer built on top of **Quick Save**. `GameSaveManager` (persistent singleton) exposes `Save<T>` / `Load<T>` / `TryLoad<T>` with a `(context, key)` address scheme and supports multiple save slots via `SetActiveSaveSlot`. Components implement `ISaveable` (a `SaveContext` string plus `Save()` / `Load()` methods) so that `SaveAll()` / `LoadAll()` can serialize the entire scene in one call. The manager auto-creates the save file if missing and maintains a debug key registry.

#### [Singleton](Runtime/Singleton/README.md)
Base classes for the singleton pattern: `Singleton<T>` ensures a single instance and auto-destroys duplicates; `PersistentSingleton<T>` adds `DontDestroyOnLoad` for cross-scene services. `GenericDataManager<TManager, TAsset>` extends the pattern into a data-loading manager that auto-collects all assets of a given type from a configured path and exposes lookup by ID or concrete type.

#### [States](Runtime/States/README.md)
Two complementary state machine patterns. The **pure C# variant** (`BaseStateMachine` + `IState`) uses a stack to push/pop states, suitable for gameplay logic in plain classes. The **MonoBehaviour variant** (`StateMachineMB<T>` + `StateMB<T>`) auto-discovers states as sibling components and integrates with Unity's `Awake`/`Start` lifecycle. Both variants call `OnEnterState`, `OnExitState`, and `OnUpdate` on the active state, and support optional debug logging.

#### [Status Effects](Runtime/StatusEffects/README.md)
A data-driven status effect system. Define effects by subclassing the abstract `StatusEffectData` `ScriptableObject` and implementing `ApplyEffect`, `UpdateEffect`, and `EndEffect`. Three stacking modes are supported: **None**, **Duration** (up to a configurable max), and **Intensity**. `StatusEffectManager` tracks active `RuntimeStatusEffect` instances per entity, handles tick-based duration reduction, and supports tag-based immunity. `StatusEffectDataManager` provides fast lookup by ID, visible-effect filtering, and tag-to-effect mapping.

#### [Tag Manager](Runtime/TagManager/README.md)
A typed tag system using `GameTag` `ScriptableObject` assets with stable GUID-based IDs. `TagManager` stores integer tag values in a serializable dictionary and exposes has/set/get methods plus set operations (`HasAny`, `HasAll`, `Intersection`, `Union`). `ITaggable` standardizes the interface so `TagManager` static helpers work uniformly across any taggable object. `GameTagManager` centralizes asset loading and enables lookup by ID or concrete type without inspector references.

#### [Task](Runtime/Task/README.md)
A minimal framework for time-measured, sequentially-executed tasks. Implement `ITask` (with `Execute(ref object context, ref object data)` and a `ShortName`) and register instances in `TaskManager`. Attach typed `TaskContext` and `TaskData` subclasses to carry shared state and per-task inputs. `TaskManager.ShowElapsedTimeForTasks()` logs execution times for profiling.

#### [Timed Events](Runtime/TimedEvents/README.md)
Schedules callbacks to fire after a configurable delay. `TimedEventsManager` (singleton) supports adding events at runtime via `AddEvent(delay, callback)` and canceling them with the returned handle. Delays can use scaled or unscaled time. A list of **Initial Events** can also be configured directly in the inspector for scene-level timed triggers.

#### [Tracker](Runtime/Tracker/README.md)
A polling-based value-change tracker. Add nodes via `AddNodeForValueType(() => source.Value, callback)` — each node stores the last-seen value and invokes its callback when the value differs. Call `Refresh()` each `LateUpdate` to check for changes, and `ForceInvoke()` to push the initial state immediately. Useful for binding gameplay state to UI without event boilerplate.

#### UI
Reusable UI runtime components, including modal window integrations.

#### Utils (Runtime and Editor)
Additional helper utilities used across multiple runtime modules. Includes small editor-side helper tools to streamline repetitive tasks.

---

### Editor

#### [AutoBundles](Editor/AutoBundles/README.md)
Automates Addressables group management from a `ScriptableObject` asset. Scans top-level `Assets/` subfolders, creates an `AutoBundleData` entry per folder, then synchronizes Addressables groups, entries, and labels in one click (**Sync Addressable Groups**). Entries receive both the labels listed in `AutoBundleData` and an automatic label matching the asset type name. Exclusion lists for folders and file extensions keep generated groups clean.

#### Dictionary
Inspector drawers and editor utilities for `SerializableDictionary` types.

#### Windows
Custom Unity Editor windows for project-specific workflows.

---

### Experimental / Work in Progress

#### Health
Prototype health and damage systems for future runtime integration.

#### Lights
Experimental lighting and day/night cycle features.

#### Pool
Work-in-progress object pooling systems and related runtime patterns.

#### Redeem
Prototype reward/redeem code features and supporting systems.

---

## Intended Audience

This framework is designed for:

- Indie developers and solo creators
- Gameplay programmers
- Technical programmers who work closely with runtime systems

It is not intended to replace existing Unity packages or enforce a rigid architecture.

Instead, think of it as a **runtime toolkit** — a flexible foundation you can build upon, customize, or use selectively depending on the needs of your project.