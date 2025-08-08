# State Machines

Questa cartella contiene una semplice implementazione di macchine a stati.

## Panoramica

- **BaseStateMachine** – gestisce stati C# che implementano `IState`. Mantiene uno stack, permette di registrare, inizializzare e cambiare stato.
- **IState** – interfaccia che definisce il ciclo di vita di uno stato (`OnInitialize`, `OnEnterState`, `OnExitState`, `OnUpdate`, `OnClear`).
- **IStateMachineHandler** – interfaccia per l'oggetto che possiede la macchina a stati, fornisce il `Name` usato nei log.
- **StateMB<T>** – base per uno stato come componente `MonoBehaviour`, con metodi di ciclo di vita (`OnAwake`, `OnStart`, `OnEnterState`, `OnExitState`).
- **StateMachineMB<T>** – versione `MonoBehaviour` della macchina a stati che registra automaticamente tutti gli `StateMB<T>` presenti sullo stesso `GameObject`.

## Esempio d'uso

```csharp
// Handler della macchina a stati
class Player : IStateMachineHandler
{
    public string Name => "Player";
    public readonly PlayerStateMachine Fsm;

    public Player()
    {
        Fsm = new PlayerStateMachine(this);
        Fsm.RegisterState(new Idle());
        Fsm.RegisterState(new Walk());
        Fsm.Initialize();

        // stato iniziale
        Fsm.PushState<Idle>();
    }

    public void Update() => Fsm.Update();
}

// Stati concreti
class Idle : IState
{
    public bool IsInitialized { get; private set; }
    public void OnInitialize() => IsInitialized = true;
    public void OnEnterState()  { /* logica di ingresso */ }
    public void OnExitState()   { /* logica di uscita */ }
    public void OnUpdate()      { }
    public void OnClear()       { }
    public void OnNextState(IState next) { }
}

class Walk : IState
{
    // Implementazione analoga a Idle
}

// Macchina a stati concreta
class PlayerStateMachine : BaseStateMachine
{
    public PlayerStateMachine(IStateMachineHandler handler) : base(handler) { }
}

// Utilizzo
var player = new Player();
player.Fsm.PushState<Walk>(); // passa da Idle a Walk
player.Fsm.PopState();        // torna allo stato precedente (Idle)
```

Questo esempio mostra come registrare stati, inizializzarli e gestire le transizioni tramite `PushState` e `PopState`.
