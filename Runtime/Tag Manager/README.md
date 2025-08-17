# Tag Manager

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

