# DataStructures

Questa cartella contiene alcune **Scriptable Object** utilizzate per gestire dati comuni all'interno dei progetti Unity.

## UniqueID
`UniqueID` è una classe astratta che deriva da `ScriptableObject` e fornisce un identificatore univoco per ogni asset. L'ID viene generato automaticamente tramite `GUID.Generate()` al momento della convalida (`OnValidate`) se non è già presente. È possibile rigenerarlo manualmente grazie a un pulsante di `TriInspector`. L'uguaglianza tra istanze è basata sul confronto dell'ID.

## ItemLocalizationData
Estende `UniqueID` aggiungendo campi dedicati alla localizzazione:
- **Internal Name**: stringa utilizzata internamente.
- **Name** e **Description**: `LocalizedString` che recuperano il testo localizzato tramite il sistema di localizzazione di Unity.

## ItemAssetBase
Classe astratta che eredita da `ItemLocalizationData`. Gestisce gli aspetti grafici di un item, in particolare:
- **AssetReferenceIcon**: riferimento `Addressables` a una sprite.
- **Icon**: `Task<Sprite>` ottenuto tramite `AssetLoader.LoadAssetAsync`.
- **ItemColor**: colore associato all'item.

## ItemAssetManager
`ItemAssetManager<TManager, TAsset>` è una classe astratta che estende `GenericDataManager` e fornisce una ricerca veloce per **Internal Name** sugli asset che derivano da `ItemAssetBase`.

Metodi principali:
- `bool TrySearchByInternalName(string internalName, out TAsset result)` – restituisce l'asset se l'internal name esiste.
- `TAsset SearchByInternalName(string internalName)` – come sopra, ma logga un errore se l'asset non viene trovato.

L'istanza del manager carica automaticamente tutti gli asset dal percorso configurato e costruisce il dizionario in `OnPostAwake`.

## Rarity
`Rarity` è un semplice Scriptable Object (creabile tramite `CreateAssetMenu`) che non aggiunge nuovi dati ma permette di definire diverse rarità ereditando tutte le funzionalità offerte da `ItemAssetBase`.

### RarityManager
`RarityManager` è un'implementazione concreta di `ItemAssetManager` che carica tutte le `Rarity` presenti nella cartella configurata. Può essere aggiunto a un prefab/scena e permette di recuperare rapidamente una rarità per **Internal Name** o per **ID** (metodi ereditati da `GenericDataManager`).

Esempio d'uso:
```cs
if (RarityManager.Instance.TrySearchByInternalName("epic", out var rarity))
{
    Debug.Log($"Colore rarità: {rarity.ItemColor}");
}
```
