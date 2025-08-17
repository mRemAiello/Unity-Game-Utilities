# Sistema di Status Effect

Questa cartella raccoglie gli script utili per applicare e gestire effetti di stato sugli oggetti di gioco.
Ogni effetto viene descritto da uno `StatusEffectData` e gestito a runtime tramite `StatusEffectManager`.

## Classi principali

### `StatusEffectData`
`ScriptableObject` astratto da cui derivare i vari effetti. Espone:
- **IsVisible**: se l'effetto deve essere mostrato in UI.
- **Order**: priorità di visualizzazione.
- **StackType**: modalità di accumulo (`None`, `Duration` o `Intensity`).
- **MaxDuration**: durata massima se lo stack avviene sulla durata.

È necessario implementare tre metodi:
- `ApplyEffect(RuntimeStatusEffect effect)` – logica all'applicazione.
- `UpdateEffect(RuntimeStatusEffect effect)` – eseguito ad ogni aggiornamento.
- `EndEffect(RuntimeStatusEffect effect)` – chiamato alla fine o rimozione.

### `RuntimeStatusEffect`
Struttura serializzabile che mantiene le informazioni di un effetto applicato:
`ID`, `Source`, `Target`, riferimento allo `StatusEffectData`, durata e intensità attuali.

### `StatusEffectManager`
`MonoBehaviour` che gestisce la lista di effetti attivi. Fornisce:
- `ApplyEffect(GameObject source, GameObject target, StatusEffectData data, int amount)` – aggiunge o incrementa un effetto.
- `UpdateEffect()` – aggiorna tutti gli effetti in lista e riduce la durata se necessario.
- `RemoveEffect(RuntimeStatusEffect effect, bool launchEndEvent = false)` – rimuove un singolo effetto.
- `RemoveAllEffects(string id, bool launchEndEvent = false)` – rimuove tutti gli effetti con un determinato ID.
- Metodi di utilità come `FindEffect`, `FindEffects` e `HasEffect`.

Il manager espone anche tre `StatusEffectEventAsset` opzionali per ricevere notifiche all'applicazione, aggiornamento ed esaurimento di un effetto.

### `StatusEffectEventAsset`
Evento basato su `GameEventAsset<RuntimeStatusEffect>` utilizzabile per collegare logiche esterne (ad esempio UI o feedback sonori).

### `StatusEffectStackType`
Enum che definisce il tipo di accumulo dell'effetto:
- `None` – nessuno stack.
- `Duration` – accumula la durata fino a `MaxDuration`.
- `Intensity` – incrementa l'intensità.

### `StatusEffectType`
Enum per classificare l'effetto (`Positive`, `Negative`, `Neutral`).

## Esempio di utilizzo
```cs
public class PlayerEffects : MonoBehaviour
{
    [SerializeField] private StatusEffectManager manager;
    [SerializeField] private StatusEffectData poisonData;

    void ApplyPoison()
    {
        manager.ApplyEffect(gameObject, gameObject, poisonData, 5); // 5 turni
    }

    void Update()
    {
        manager.UpdateEffect();
    }
}
```
