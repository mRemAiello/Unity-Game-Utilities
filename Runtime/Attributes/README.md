# Sistema di Attributi

Questa cartella contiene tutti gli script e gli Scriptable Object utili per gestire un sistema di attributi e classi per i personaggi di gioco.

## Scriptable Object

### `AttributeData`
Rappresenta un singolo attributo (es. `Forza`, `Agilità` ecc.). È possibile crearne uno dal menu `Game Utils/Attributes/Attribute`.

Campi principali:
- `MinValue` e `MaxValue` – valori minimo e massimo consentiti.
- `IsVital` – se l'attributo rappresenta un valore vitale (ad esempio punti vita).
- `ClampType` – modalità con cui viene arrotondato il valore (`RawFloat`, `Floor`, `Round`, `Ceiling`, `Integer`).

### `ClassData`
È un asset che raccoglie una lista di coppie `AttributeData`/valore iniziale (`AttributeIntTuple`).
Si crea dal menu `Game Utils/Attributes/Class` e viene utilizzato per definire le statistiche di partenza di una classe di personaggi.

## Classi Runtime

### `AttributeDataManager`
Gestore generico degli `AttributeData`. Carica automaticamente tutti gli asset presenti nel progetto e ne fornisce l'accesso centralizzato.

### `RuntimeAttribute`
Oggetto serializzabile che rappresenta un attributo in gioco. Mantiene il valore base e quello corrente (modificato dai vari `Modifier`).
Metodi principali:
- `AddModifier`/`RemoveModifier` – aggiunge o rimuove un modificatore.
- `Refresh` – aggiorna la durata dei modificatori nel tempo.

### `RuntimeVital`
Estende `RuntimeAttribute` per gli attributi vitali (es. punti vita), tenendo traccia anche del valore attuale e del massimo temporaneo.

### `RuntimeClass`
`MonoBehaviour` che applica una `ClassData` ad un oggetto di scena. All'avvio inizializza la lista di `RuntimeAttribute` in base ai dati forniti e permette di recuperare i valori con `GetAttribute<T>()`.

## Modificatori

### `Modifier`
Classe base astratta. Contiene:
- `Source` – origine del modificatore.
- `Amount` – valore della modifica.
- `Duration` – tempo di vita; se maggiore di zero viene ridotto ad ogni `Refresh()`.
- `ModifierType` – positivo, negativo o neutro.

Ogni modificatore implementa `Order` e `ApplyModifier` per definire la priorità e l'effetto sul valore.

### `ModifierFixed`
Aggiunge o sottrae un valore fisso.

### `ModifierPercentage`
Modifica l'attributo di una percentuale rispetto al valore corrente.

## Esempio di Utilizzo

```cs
// Creazione di una classe
ClassData guerriero = CreateInstance<ClassData>();
// Aggiungi attributi nello stesso ordine in cui sono definiti nell'asset
// guerriero.Attributes.Add(new AttributeIntTuple(forzaData, 10));

// Assegnazione ad un GameObject
public class Player : MonoBehaviour
{
    public RuntimeClass runtimeClass;

    void Start()
    {
        runtimeClass.SetClass(guerriero);
    }
}
```

Questo README fornisce una panoramica sul funzionamento del sistema di attributi e delle classi. Gli script possono essere estesi a seconda delle necessità del progetto.
