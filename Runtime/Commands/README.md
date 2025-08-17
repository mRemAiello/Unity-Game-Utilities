# Sistema di Comandi

Questa cartella contiene gli script necessari per gestire una coda di comandi eseguibili in sequenza.

## Classi principali

### `Command`
Classe astratta derivata da `ScriptableObject`. È sufficiente crearne una sottoclasse e implementare il metodo `Execute` per definire la logica del comando.

### `CommandInput`
Struttura utilizzata per passare parametri al comando. Contiene:
- `GameObject Igniter` – l'oggetto che ha generato il comando.
- `object[] AdditionalSettings` – eventuali parametri aggiuntivi.

### `CommandTuple`
Semplice contenitore per una coppia `Command`/`CommandInput`. Viene utilizzato internamente dal `CommandManager`.

### `CommandManager`
`Singleton` incaricato di gestire la coda dei comandi. Espone i metodi principali:
- `AddToQueue(GameObject igniter, Command command, params object[] additionalSettings)` – aggiunge un comando alla coda e lo esegue se non è già in corso un altro comando.
- `CommandExecutionComplete()` – da richiamare alla fine dell'esecuzione di un comando per avviare il successivo.

Sono inoltre disponibili le proprietà `IsCommandPlaying` e `CurrentCommand` per verificare lo stato corrente.

## Esempio di utilizzo

```cs
public class DrawCardAction : Command
{
    public override void Execute(CommandInput input)
    {
        // Logica del comando
        Debug.Log($"Pesca carta da {input.Igniter}");

        // Notifica al manager che il comando è terminato
        CommandManager.Instance.CommandExecutionComplete();
    }
}
```

```cs
// Aggiunta di un comando alla coda
Command drawCard = ScriptableObject.CreateInstance<DrawCardAction>();
CommandManager.Instance.AddToQueue(gameObject, drawCard);
```
