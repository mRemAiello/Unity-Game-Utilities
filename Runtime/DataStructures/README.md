# DataStructures

This folder contains the base **ScriptableObject** data structures used to describe item data in Unity projects.

## ItemIdentifierData
`ItemIdentifierData` is an abstract `ScriptableObject` that manages a unique ID:
- **ID** is auto-generated through `GUID.Generate()` in `OnValidate` and can be regenerated from the inspector button.
- Equality is based on the ID value.

## ItemTagsData
`ItemTagsData` extends `ItemIdentifierData` and implements `ITaggable` by exposing:
- **Tags** stored in a `List<GameTag>` (read-only via `IReadOnlyList`).

## ItemLocalizedData
`ItemLocalizedData` extends `ItemTagsData` and adds localization fields:
- **Internal Name**: internal string identifier.
- **Name** and **Description**: `LocalizedString` values resolved through Unity Localization.

## ItemVisualData
`ItemVisualData` extends `ItemLocalizedData` and manages visual data and features:
- **AssetReferenceIcon**: Addressables reference to the item sprite.
- **Icon**: `Task<Sprite>` loaded via `AssetLoader.LoadAssetAsync`.
- **ItemColor**: color associated with the item.
- **ItemFeatureData**: serialized list (using `SerializeReference`) accessible via `TryGetFeature<TFeature>`.

## ItemFeatureData
Serializable abstract base class for additional item feature data. Extend it with custom feature types and place them inside `ItemVisualData`.

## ItemAssetManager
`ItemAssetManager<TManager, TAsset>` is an abstract manager that extends `GenericDataManager` and provides fast lookups by **Internal Name** for assets derived from `ItemVisualData`.

Main methods:
- `bool TrySearchByInternalName(string internalName, out TAsset result)` – returns the asset if the internal name exists.
- `TAsset SearchByInternalName(string internalName)` – same as above, but logs an error when missing.

The manager instance automatically loads all assets from the configured path and builds the lookup dictionary in `OnPostAwake`.
