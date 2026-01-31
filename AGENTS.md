# AGENTS.md

## Repository scope
Utility package for Unity 2022.3 (`com.gedemy.gameutils`). Runtime scripts live in `Runtime/`, editor tools in `Editor/`, and experimental features in `WIP/`.

## Main structure
- `Runtime/` — gameplay-usable code (namespace `GameUtils`).
- `Editor/` — editor tools and windows (namespace `UnityEditor.GameUtils`).
- `WIP/` — in-development features; keep separate from stable runtime.
- `*.asmdef` — respect the Runtime/Editor separation.

## Code conventions
- **Namespace**:
  - Runtime: `GameUtils`
  - Editor: `UnityEditor.GameUtils`
- **Internal naming convention**: always follow this project's conventions (namespace, file/class names, serialized fields, style).
- **File/class names**: 1 main public class per file, file name = class name.
- **Serialized fields**: `private` with `_` prefix and `[SerializeField]`.
- **Unity API**: use `CreateAssetMenu`/`MenuItem` with `GameUtilsMenuConstants` for consistent menu entries.
- **Style**: PascalCase for classes/methods/properties, camelCase for parameters, underscore for private fields.
- **ScriptableObject**: prefer SOs for shared data/configs; keep menu paths consistent.

## Agent requirements
- Always include `<summary>` in final responses.
- Always comment the code you write.
- Class variables must not be commented.
- Every change should update the CHANGELOG.
- Every time a change is recorded in CHANGELOG.md, include the version found in package.json.

## Assets and meta
- Do not remove or rename `.meta` files manually; Unity manages them.
- Every new asset/script must have a corresponding `.meta`.

## WIP notes
- Features in `WIP/` are experimental: avoid reverse dependencies from stable `Runtime/`.
