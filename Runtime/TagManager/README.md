# TagManager

Questa cartella contiene un semplice sistema di gestione dei tag utilizzabile nei progetti Unity.

## `GameTag`
`GameTag` è uno `ScriptableObject` derivato da `ItemAssetBase` e rappresenta un identificatore di tag. Può essere creato dal menu **Game Utils/Tag/Tag** e sfrutta l'`ID` univoco fornito dalla classe `UniqueID`.

## `TagManager`
`TagManager` è una classe serializzabile che mantiene un dizionario di tag e informazioni (`SerializedDictionary<string, TagInfo>`), dove `TagInfo` contiene il nome e il valore intero associato al tag. Permette di assegnare e leggere rapidamente lo stato dei tag.

### Metodi principali
- `SetTagValue(GameTag tag, int value)` / `SetTagValue(GameTag tag, bool value)` – imposta il valore associato al tag e ne registra anche il nome.
- `int GetTagValue(GameTag tag)` – restituisce il valore numerico del tag (0 se assente).
- `bool HasTag(GameTag tag)` – verifica se un tag è presente e con valore maggiore di zero.
- `Dictionary<string, TagInfo> GetMap()` – ottiene l'intero dizionario di tag e relative informazioni.
- `TryGetValue(string tag, out TagInfo info)` – recupera le informazioni del tag tramite la stringa `ID`.
- `Clear()` – rimuove tutte le associazioni registrate.
- `HasAny(params GameTag[] tags)` – verifica se almeno uno dei tag specificati è presente.
- `HasAll(params GameTag[] tags)` – verifica se tutti i tag specificati sono presenti.
- `List<GameTag> Intersection(params GameTag[] tags)` – restituisce l'intersezione tra i tag presenti e quelli passati.
- `List<GameTag> Union(params GameTag[] tags)` – restituisce l'unione tra i tag presenti e quelli passati.

Sono inoltre disponibili versioni statiche degli stessi metodi che accettano un'interfaccia `ITaggable` e consentono di effettuare queste operazioni direttamente sulle liste di tag.

`ITaggable` espone inoltre dei metodi di utilità per interrogare i tag direttamente sugli oggetti che implementano l'interfaccia:
- `GetTags()` – restituisce la lista dei tag (mai `null`).
- `HasAnyTag(...)` – verifica se almeno un tag è presente (overload per `GameTag`, `List<GameTag>` e stringhe `ID`).
- `HasAllTag(...)` – verifica se tutti i tag sono presenti (overload per `GameTag`, `List<GameTag>` e stringhe `ID`).

## `GameTagManager`
`GameTagManager` eredita da `GenericDataManager` e carica automaticamente tutti gli asset `GameTag` presenti nel percorso configurato. In `OnPostAwake` costruisce due dizionari per ricerche rapide:
- per **ID** (`TryGetTag(string id, out GameTag tag)` e `GetAll()`),
- per tipo concreto del tag (`TryGetTag<T>()` / `GetTag<T>()`).

Questo permette di ottenere rapidamente un tag senza doverlo referenziare manualmente nel codice o nelle scene.

Esempio d'uso:
```cs
void Awake()
{
    // Recupero per ID
    if (GameTagManager.Instance.TryGetTag("poison", out var poisonTag))
    {
        Debug.Log(poisonTag.InternalName);
    }

    // Recupero per tipo concreto
    var fireTag = GameTagManager.Instance.GetTag<FireTag>();
}
```

### Esempio di utilizzo
```cs
public class Example : MonoBehaviour
{
    [SerializeField] private GameTag jumpTag;
    private TagManager tags = new TagManager();

    void Start()
    {
        tags.SetTagValue(jumpTag, true);
        if (tags.HasTag(jumpTag))
        {
            Debug.Log("Può saltare!");
        }
    }
}
```
