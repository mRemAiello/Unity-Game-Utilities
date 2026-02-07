# Unity Game Utilities

This package contains a set of utility scripts for Unity projects.

## Requirements

To use this package you need the following assets:

### Unity Packages

* [Addressables (2.7.6+)](https://docs.unity3d.com/Packages/com.unity.addressables@2.1/manual/index.html)
* [New Input System](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.10/manual/index.html)

### Third-party

* [Tri-Inspector](https://github.com/codewriter-packages/Tri-Inspector)
* [Quick Save](https://www.claytoninds.com/quick-save)
* [DOTween](http://dotween.demigiant.com/)

## Packages

This section covers the contents of the package.

### [Runtime](./Runtime)
Contains scripts intended for in-game usage.
Main subfolders:
- [Achievements](./Runtime/Achievements): achievement management.
- [Attributes](./Runtime/Attributes): attribute system and RPG-style classes.
- [Audio](./Runtime/Audio): audio clip playback.
- [Commands](./Runtime/Commands): executable command queue.
- [Constant](./Runtime/Constant): global values and constants.
- [Currency](./Runtime/Currency): in-game currency logic.
- [DataStructures](./Runtime/DataStructures): common ScriptableObjects.
- [Debug](./Runtime/Debug): debugging and logging utilities.
- [Dictionary](./Runtime/Dictionary): serializable dictionaries.
- [Events](./Runtime/Events): ScriptableObject-based game events with listener tracking, optional logging, and per-type assets.
- [Extensions](./Runtime/Extensions): assorted extension methods.
- [Input](./Runtime/Input): input system components.
- [Math](./Runtime/Math): helper math functions.
- [ModalWindow](./Runtime/ModalWindow): modal window UI flow and button events.
- [Misc](./Runtime/Misc): generic utilities.
- [Movement](./Runtime/Movement): movement scripts.
- [Placement](./Runtime/Placement): object placement.
- [Projectiles](./Runtime/Projectiles): 2D projectile management.
- [Random](./Runtime/Random): randomization tools.
- [Renderer](./Runtime/Renderer): custom renderers.
- [Save](./Runtime/Save): save/load management.
- [Singleton](./Runtime/Singleton): base classes for singletons and managers.
- [States](./Runtime/States): simple state machines.
- [StatusEffects](./Runtime/StatusEffects): status effect system.
- [TagManager](./Runtime/TagManager): custom tags.
- [Task](./Runtime/Task): async task management.
- [TimedEvents](./Runtime/TimedEvents): scheduled event management.
- [Tracker](./Runtime/Tracker): variable tracking.
- [UI](./Runtime/UI): UI components, including modal window button events.
- [Utils](./Runtime/Utils): additional utilities.

### [Editor](./Editor)
Tools and windows for the Unity editor.
- [AutoBundles](./Editor/AutoBundles): automatic bundle generation ([details](./Editor/AutoBundles/README.md)).
- [Dictionary](./Editor/Dictionary): drawers for SerializableDictionary.
- [Utils](./Editor/Utils): small support tools.
- [Windows](./Editor/Windows): custom editor windows.

### [WIP](./WIP)
Experimental features under development.
- [Health](./WIP/Health): health and damage system.
- [Lights](./WIP/Lights): lighting and day/night cycle.
- [Pool](./WIP/Pool): object pooling.
- [Redeem](./WIP/Redeem): reward codes.

Other useful resources:
- [CHANGELOG.md](./CHANGELOG.md)
- [LICENSE.md](./LICENSE.md)
- [package.json](./package.json)
