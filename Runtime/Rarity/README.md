# Rarity

Questa cartella contiene gli asset relativi alle rarità degli item, basati su `ItemVisualData`.

## Rarity
`Rarity` è uno ScriptableObject creato tramite `CreateAssetMenu` e usa il menu `GameUtilsMenuConstants.CURRENCY_NAME + "Rarity"`. Non aggiunge nuovi campi rispetto a `ItemVisualData`, ma consente di definire colori, icone e feature per rappresentare rarità diverse.

## RarityManager
`RarityManager` è un `ItemAssetManager` concreto che carica tutte le rarità dalla cartella configurata e permette la ricerca per **Internal Name** o per **ID** (metodi ereditati da `GenericDataManager`).

Esempio d'uso:
```cs
if (RarityManager.Instance.TrySearchByInternalName("epic", out var rarity))
{
    Debug.Log($"Colore rarità: {rarity.ItemColor}");
}
```
