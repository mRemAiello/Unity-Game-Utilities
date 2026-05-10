# Singleton

This folder contains utility classes for implementing the **Singleton** pattern and managing Scriptable Object databases.

## `Singleton<T>`
Abstract class that guarantees the existence of a single instance per type. If a second object is created, it is automatically destroyed in `Awake`.

```cs
public class EventManager : Singleton<EventManager>
{
    // Manager methods and fields
}
```

`Instance` provides access to the single instance, while `InstanceExists` indicates whether it is already present. You can override `OnPostAwake` and `OnPostDestroy` to run initialization or cleanup code.

## `PersistentSingleton<T>`
Inherits from `Singleton<T>` and adds persistence across scenes through `DontDestroyOnLoad`.

```cs
public class AudioManager : PersistentSingleton<AudioManager>
{
    // Remains active even after a scene change
}
```

## `GenericDataManager<T1, T2>`
Generic manager designed to load assets that inherit from `UniqueID`. `T1` is the type of the manager itself, while `T2` is the type of asset being handled.

At startup (in the editor), it automatically loads all assets found in the specified path and stores them in `Items`.
Several search methods are available:

- `SearchAssetByID(string id)`
- `TrySearchAssetByID(string id, out T2 result)`
- `SearchAsset<T>()` to retrieve the first asset of a given type
- `HasAsset<T>()` to check whether an asset of type `T` exists

Implementation example:

```cs
public class CurrencyManager : GenericDataManager<CurrencyManager, CurrencyData>, ISaveable
{
    // Custom currency management
}
```

Place the subclass in a prefab and set the path where the assets to load are located. The instance will be accessible through `CurrencyManager.Instance`.
