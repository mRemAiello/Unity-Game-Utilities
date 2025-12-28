# Sistema di Attributi

Questa cartella raccoglie Scriptable Object e componenti runtime per definire attributi (es. `Forza`, `Agilità`, `Salute`) e applicarli a personaggi o entità di gioco.

## Scriptable Object

### `AttributeData`
Asset che descrive un singolo attributo e le sue regole di validità (`MinValue`, `MaxValue`, `ClampType`). Se `IsVital` è attivo l'attributo userà la variante runtime vitale.

> Creazione: **Game Utils/Attributes/Attribute**.

### `ClassData`
Contiene una lista ordinata di `AttributeValuePair` (attributo + valore iniziale) che rappresentano la “classe” o configurazione di base di un personaggio.

> Creazione: **Game Utils/Attributes/Class**.

### `AttributeDataManager`
Eredita da `GenericDataManager` e centralizza il caricamento di tutti gli `AttributeData` presenti nel progetto, così da poterli recuperare in modo coerente da codice o altri sistemi.

## Classi runtime

### `RuntimeAttribute`
Oggetto serializzabile che mantiene valore base e corrente di un attributo. Gestisce una lista di `Modifier`, applicati in base a `Order`, e li fa scadere quando `Duration` si azzera tramite `Refresh()`. Il valore finale viene clamped secondo `MinValue`/`MaxValue` e la strategia `ClampType`.

Metodi utili:
- `AddModifier` / `RemoveModifier` / `ClearModifiers` per gestire i modificatori.
- `Refresh()` per aggiornare la durata dei modificatori e ricalcolare il valore.

### `RuntimeVital`
Estende `RuntimeAttribute` per gli attributi vitali (es. punti vita). Tiene traccia del valore vitale corrente e di un massimo temporaneo derivato dal valore base; `SetCurrentValue` permette di modificarlo in sicurezza mantenendolo entro i limiti calcolati.

### `RuntimeClass`
`MonoBehaviour` che istanzia gli attributi definiti in una `ClassData` su un GameObject. Può creare automaticamente la classe in `Start` (`_startWithClass`) e opzionalmente richiamare `Refresh()` ogni frame (`_refreshClassOnUpdate`). Fornisce metodi `GetAttribute`/`TryGetAttribute` per recuperare gli attributi per tipo, ID o riferimento al `AttributeData` originale.

## Modificatori

### `Modifier`
Classe base astratta per variazioni temporanee o permanenti di un attributo. Espone `Source`, `Amount`, `Duration`, `ModifierType` (positivo/negativo/neutro) e richiede di definire `Order` e `ApplyModifier` per stabilire priorità ed effetto.

### Implementazioni incluse
- `ModifierFixed` – aggiunge o sottrae un valore fisso (`Order = 1`), con supporto al flag `IsPermanent`.
- `ModifierPercentage` – applica una variazione percentuale (`Order = 2`), con supporto al flag `IsPermanent`.

## Flusso di utilizzo consigliato
1. Crea gli asset `AttributeData` necessari (specificando limiti, clamp e se sono vitali).
2. Definisci una o più `ClassData` popolando l'elenco di `AttributeValuePair` con i valori iniziali.
3. Inserisci un componente `RuntimeClass` su un GameObject, assegna la `ClassData` e, se serve, abilita `_startWithClass` per inizializzare automaticamente.
4. Gestisci i modificatori a runtime usando `AddModifier`/`RemoveModifier` e aggiorna la classe con `RefreshAttributes()` (oppure abilita `_refreshClassOnUpdate`).
5. Per gli attributi vitali usa `SetCurrentValue` per variare il valore corrente mantenendo i limiti calcolati.

Seguendo questi passaggi puoi modellare classi di gioco flessibili, applicare bonus/malus temporanei e monitorare in modo centralizzato gli attributi dei tuoi personaggi.
