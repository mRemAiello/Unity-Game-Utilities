# Timed Events

Questa cartella contiene strumenti per schedulare eventi che scattano dopo un certo tempo.

## TimedEventsManager

`TimedEventsManager` permette di:

- registrare eventi a runtime passando un ritardo e un callback;
- rimuovere un evento precedentemente schedulato;
- configurare dall'Inspector una lista di eventi da eseguire dopo N secondi.

### Uso rapido

```csharp
using GameUtils;

var handle = TimedEventsManager.Instance.AddEvent(2.5f, () => Debug.Log("Evento scattato"));
TimedEventsManager.Instance.RemoveEvent(handle);
```

### Inspector

Nell'Inspector è possibile aggiungere elementi alla lista **Initial Events** specificando:

- `Delay In Seconds`
- `Use Unscaled Time`
- `On Elapsed` (UnityEvent)
```
