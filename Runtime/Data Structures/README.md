# Data Structures

Questa cartella contiene alcune classi base utilizzate per gestire dati condivisi in progetti Unity.
Di seguito una panoramica dei singoli script.

## UniqueID
`UniqueID` è una classe astratta derivata da `ScriptableObject` che assegna automaticamente un identificativo univoco (_id) ad ogni asset. L'ID viene generato in fase di validazione tramite `GUID.Generate()` e può essere rigenerato tramite il pulsante dell'Inspector. La classe espone il campo `ID` e fornisce metodi di confronto per verificare l'uguaglianza di due oggetti `UniqueID`.

## ItemLocalizationData
Eredita da `UniqueID` e introduce campi dedicati alla localizzazione di un oggetto: un nome interno, un `LocalizedString` per il nome visibile e uno per la descrizione. I rispettivi getter restituiscono le stringhe localizzate tramite il sistema di Localizzazione di Unity.

## ItemAssetBase
Deriva da `ItemLocalizationData` e aggiunge le informazioni grafiche di base per un oggetto. Conserva un riferimento addressable ad un'icona (`AssetReferenceSprite`) e un colore associato. Espone sia il riferimento addressable, sia lo `Sprite` già caricato tramite l'`AssetLoader`, oltre al colore dell'oggetto.

## Rarity
`Rarity` è una semplice `ScriptableObject` che estende `ItemAssetBase`. Viene usata per definire la rarità di un oggetto e può essere creata dal menu "Create > <nome pacchetto> Rarity" grazie all'attributo `CreateAssetMenu`.
