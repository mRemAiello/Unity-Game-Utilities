# DataStructures

Questa cartella contiene le **ScriptableObject** di base usate per descrivere dati di oggetti (item) nei progetti Unity.

## ItemIdentifierData
`ItemIdentifierData` è una classe astratta che estende `ScriptableObject` e implementa `ITaggable`. Gestisce:
- **ID** univoco generato automaticamente (tramite `GUID.Generate()` in `OnValidate`) con un pulsante di rigenerazione.
- **Tags** tramite una lista di `GameTag`.

L'uguaglianza tra istanze è basata sul confronto dell'ID.

## ItemLocalizedData
`ItemLocalizedData` estende `ItemIdentifierData` aggiungendo campi dedicati alla localizzazione:
- **Internal Name**: stringa utilizzata internamente.
- **Name** e **Description**: `LocalizedString` che recuperano il testo localizzato tramite Unity Localization.

## ItemVisualData
`ItemVisualData` estende `ItemLocalizedData` e gestisce gli aspetti grafici e le feature di un item:
- **AssetReferenceIcon**: riferimento `Addressables` a una sprite.
- **Icon**: `Task<Sprite>` ottenuto tramite `AssetLoader.LoadAssetAsync`.
- **ItemColor**: colore associato all'item.
- **ItemFeatureData**: lista serializzata tramite `SerializeReference`, con accesso tipizzato via `TryGetFeature<TFeature>`.

## ItemFeatureData
Classe astratta serializzabile per descrivere dati aggiuntivi di un item. È pensata per essere estesa da feature specifiche e inserita in `ItemVisualData`.

## ItemAssetManager
`ItemAssetManager<TManager, TAsset>` è una classe astratta che estende `GenericDataManager` e fornisce una ricerca veloce per **Internal Name** sugli asset che derivano da `ItemVisualData`.

Metodi principali:
- `bool TrySearchByInternalName(string internalName, out TAsset result)` – restituisce l'asset se l'internal name esiste.
- `TAsset SearchByInternalName(string internalName)` – come sopra, ma logga un errore se l'asset non viene trovato.

L'istanza del manager carica automaticamente tutti gli asset dal percorso configurato e costruisce il dizionario in `OnPostAwake`.
