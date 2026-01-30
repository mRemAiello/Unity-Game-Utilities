# AutoBundles

AutoBundles provides an editor-only workflow that keeps Addressables groups in sync with folders under `Assets/`.
It is meant to speed up repetitive Addressables setup by discovering asset folders, creating groups, and keeping
labels aligned with asset types.

## How the system works

1. **Folder discovery**
   - The AutoBundles asset scans the top-level `Assets/` folders and their immediate subfolders.
   - It skips folders listed in the exclusion list (for example `Editor`, `Resources`, or `Scripts`).
2. **AutoBundleData creation**
   - Each discovered folder becomes an `AutoBundleData` entry with a generated group name.
   - Missing entries are added and missing folders are removed from the list.
3. **Group synchronization**
   - For every `AutoBundleData`, a matching Addressables group is created (if needed).
   - Files with excluded extensions are removed from Addressables entries.
   - Every asset in the folder (and its subfolders) is added to the group.
4. **Label assignment**
   - Each Addressables entry receives the labels listed in `AutoBundleData` plus a label matching the asset type name.

## Setup and usage

1. Create an AutoBundles asset from the Unity menu (the item is under the GameUtils Addressables menu).
2. Select the asset and click **Populate Bundle Datas From Assets** to generate folder entries.
3. Adjust labels or exclusions in the inspector if needed.
4. Click **Sync Addressable Groups** to refresh Addressables groups, entries, and labels.

## Class reference

### `AutoBundles`

`AutoBundles` is a `ScriptableObject` that drives the system.

Key responsibilities:
- Collects folder paths from `Assets/` while honoring excluded folders.
- Creates and maintains `AutoBundleData` entries.
- Generates Addressables groups and keeps them synced with the filesystem.
- Removes Addressables entries that match excluded file extensions.
- Assigns labels based on `AutoBundleData` plus the asset type name.

### `AutoBundleData`

`AutoBundleData` stores the data required for a single Addressables group:
- `FolderName`: the asset folder path (e.g. `Assets/MyFolder`).
- `GroupName`: the generated Addressables group name.
- `Labels`: the list of custom labels to assign.

### `AutoBundleArgs`

`AutoBundleArgs` is a small data container for folder traversal arguments.
It holds the current path, depth limits, and the lists used during folder collection.

## Notes

- The system relies on Unity Addressables and Tri-Inspector.
- Excluded folders and extensions can be tuned directly in the AutoBundles asset.
- The generated group name strips `Assets/` and whitespace and removes path separators.
