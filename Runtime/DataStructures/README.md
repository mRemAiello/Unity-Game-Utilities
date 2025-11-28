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

## Rarity
`Rarity` è un semplice Scriptable Object (creabile tramite `CreateAssetMenu`) che non aggiunge nuovi dati ma permette di definire diverse rarità ereditando tutte le funzionalità offerte da `ItemAssetBase`.
