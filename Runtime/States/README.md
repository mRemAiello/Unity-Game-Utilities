# State Machines

This folder contains two complementary state machine patterns:

- a **pure C# stack-based state machine** (`BaseStateMachine` + `IState`) for gameplay logic that does not require `MonoBehaviour` states;
- a **component-based Unity state machine** (`StateMachineMB<T>` + `StateMB<T>`) for workflows where each state is a `MonoBehaviour` attached to the same `GameObject`.

Use the C# variant when you want plain classes and full control over instantiation. Use the `MonoBehaviour` variant when state logic benefits from Unity lifecycle methods and serialized inspector data.

---

## Architecture overview

### `IState`
Defines the lifecycle contract for plain C# states:

- `OnInitialize()` — one-time initialization after state registration.
- `OnEnterState()` — called when the state becomes active.
- `OnExitState()` — called when the state is no longer active.
- `OnUpdate()` — called every machine update while active.
- `OnClear()` — cleanup when the machine is cleared.
- `OnNextState(IState next)` — hook to react to the next state before transition (available in interface, optional to use in your concrete flow).
- `IsInitialized` — flag exposed by the concrete state implementation.

### `IStateMachineHandler`
Represents the owner/host of the machine and exposes:

- `Name` — used by logs in `BaseStateMachine` to identify the source.

### `BaseStateMachine`
Core stack-driven implementation for `IState` objects:

- Registers states by type (`RegisterState`).
- Initializes all registered states (`Initialize`).
- Pushes and pops active states (`PushState`, `PopState`).
- Updates only the current top state (`Update`).
- Supports lifecycle extension points (`OnBeforeInitialize`, `OnInitialize`).

### `StateMB<T>`
Abstract base for `MonoBehaviour` states:

- Stores a typed reference to its state machine (`Fsm`).
- Offers state callbacks (`OnEnterState`, `OnExitState`, `OnUpdate`).
- Adds setup callbacks (`OnAwake`, `OnStart`) invoked by the machine.
- Includes optional debug logging with Tri-Inspector grouping.

### `StateMachineMB<T>`
Unity `MonoBehaviour` state machine:

- Auto-discovers all `StateMB<T>` components on the same `GameObject`.
- Registers them by runtime type during `Initialize()`.
- Runs state setup in `Awake()` / `Start()`.
- Pushes and pops states with stack semantics.
- Updates only the currently active state every frame.

---

## How to implement a state machine (pure C# workflow)

This is the recommended path when states are plain classes.

### 1) Create a machine handler

Implement `IStateMachineHandler` in the object that owns the machine:

```csharp
using GameUtils;

public class PlayerController : IStateMachineHandler
{
    public string Name => "Player";
}
```

### 2) Implement concrete states (`IState`)

Each state is a class that implements the full lifecycle:

```csharp
using GameUtils;
using UnityEngine;

public class IdleState : IState
{
    public bool IsInitialized { get; private set; }

    public void OnInitialize()
    {
        IsInitialized = true;
        Debug.Log("IdleState initialized");
    }

    public void OnEnterState() => Debug.Log("Enter IdleState");
    public void OnExitState() => Debug.Log("Exit IdleState");
    public void OnUpdate() { }
    public void OnClear() => Debug.Log("Clear IdleState");
    public void OnNextState(IState next) { }
}
```

> Tip: keep state responsibilities narrow (input, movement, combat, etc.) and delegate shared services through constructor injection when needed.

### 3) Create a concrete machine class

Derive from `BaseStateMachine`:

```csharp
using GameUtils;

public class PlayerStateMachine : BaseStateMachine
{
    public PlayerStateMachine(IStateMachineHandler handler) : base(handler) { }
}
```

You can override:

- `OnBeforeInitialize()` to build/register dependencies before state initialization.
- `OnInitialize()` for machine-level startup logic once all states are initialized.

### 4) Register states and initialize machine

```csharp
var handler = new PlayerController();
var fsm = new PlayerStateMachine(handler);

fsm.RegisterState(new IdleState());
fsm.RegisterState(new WalkState());
fsm.RegisterState(new AttackState());

fsm.Initialize();
```

### 5) Set the initial state and update continuously

```csharp
fsm.PushState<IdleState>();

// game loop
fsm.Update();
```

### 6) Manage transitions explicitly

- `PushState<T>()`: exits current state (unless silent), pushes next, enters next.
- `PopState()`: exits current, removes it, re-enters previous (unless silent).
- `IsCurrent<T>()`: runtime check for active state type.

Example:

```csharp
if (wantsToRun && fsm.IsCurrent<IdleState>())
    fsm.PushState<WalkState>();

if (wantsToStop && fsm.IsCurrent<WalkState>())
    fsm.PopState();
```

### 7) Cleanup

Call `Clear()` when the machine is no longer needed to forward cleanup to all registered states and reset internal collections.

---

## How to implement a state machine (`MonoBehaviour` workflow)

Use this approach when each state should be a Unity component with serialized fields.

### 1) Create a concrete machine component

```csharp
using GameUtils;
using UnityEngine;

public class EnemyStateMachine : StateMachineMB<EnemyStateMachine>
{
}
```

Attach this component to a `GameObject`.

### 2) Create concrete state components

```csharp
using GameUtils;
using UnityEngine;

public class EnemyIdleState : StateMB<EnemyStateMachine>
{
    public override void OnEnterState()
    {
        base.OnEnterState();
        // enter logic
    }

    public override void OnUpdate()
    {
        // update logic
    }
}
```

Attach each state component to the **same GameObject** as the machine.

### 3) Let initialization run automatically

At runtime, `StateMachineMB<T>`:

- collects all `StateMB<T>` on the object,
- injects machine reference into each state,
- calls `OnAwake()` for all states in `Awake()`,
- calls `OnStart()` for all states in `Start()`.

### 4) Push your initial state

You can push the first state from the machine itself (for example in `OnInitialize`) or from another coordinator script:

```csharp
enemyStateMachine.PushState<EnemyIdleState>();
```

---

## Transition behavior details

Both implementations are **stack-based**, so transitions are not "replace current" by default.

- Pushing a state layers it on top of previous state.
- Popping returns to the previous state.
- Silent transitions (`isSilent = true`) skip enter/exit callbacks for the underlying state during that transition step.

This is useful for temporary overlays such as pause, stun, dialogue, or modal interaction states.

---

## Common pitfalls and recommendations

- Register each state type only once in `BaseStateMachine` (`Dictionary<Type, IState>` is used internally).
- Ensure `Handler` is not null when using `BaseStateMachine` logs (`Handler.Name` is referenced).
- In `StateMachineMB<T>`, keep all state components on the same GameObject, otherwise they will not be auto-registered.
- Prefer `PushState<T>()` and `IsCurrent<T>()` for type-safe transitions.
- Keep transition conditions outside state classes when you need a centralized transition policy.

---

## Minimal end-to-end example (pure C#)

```csharp
using GameUtils;

public sealed class PlayerHandler : IStateMachineHandler
{
    public string Name => "Player";
}

public sealed class PlayerFsm : BaseStateMachine
{
    public PlayerFsm(IStateMachineHandler handler) : base(handler) { }
}

public class PlayerBootstrap
{
    private readonly PlayerFsm _fsm;

    public PlayerBootstrap()
    {
        _fsm = new PlayerFsm(new PlayerHandler());
        _fsm.RegisterState(new IdleState());
        _fsm.RegisterState(new WalkState());
        _fsm.Initialize();
        _fsm.PushState<IdleState>();
    }

    public void Tick() => _fsm.Update();
}
```

This setup gives you deterministic lifecycle control, explicit transitions, and reusable state modules.
