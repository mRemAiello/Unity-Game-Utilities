# Unity Game Utilities

A lightweight, modular and easy to use **Unity utility package** providing reusable runtime systems, editor tooling, and experimental gameplay modules editor utilities (events, save system, extensions, projectiles, currency, and more) to accelerate game development workflows. 

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

This section describes the main package areas and what each one contains.

### [Runtime](./Runtime)
Scripts intended for in-game usage and reusable gameplay systems.

#### Achievements
Contains achievement data and runtime logic to register and unlock achievements.

#### Attributes
Provides an attribute/stat system with support for RPG-style classes and progression.

#### Audio
Includes utilities and components for audio clip playback and runtime audio handling.

#### Commands
Implements command patterns and executable queues for decoupled gameplay actions.

#### Constant
Stores shared constants and global values used across runtime systems.

#### Currency
Contains in-game currency models, helpers, and related gameplay logic.

#### DataStructures
Provides reusable ScriptableObject-based data containers and shared data models.

#### Debug
Offers debugging helpers and logging-focused runtime utilities.

#### Dictionary
Contains serializable dictionary implementations and related helpers.

#### Events
Implements ScriptableObject-based event assets and listener tracking utilities for decoupled communication.

#### Extensions
Provides general-purpose extension methods to simplify common coding patterns.

#### Input
Contains Input System components and interfaces for runtime player interaction.

#### Math
Includes mathematical helpers and utility methods for gameplay calculations.

#### ModalWindow
Provides modal window UI flow management and modal button event handling.

#### Misc
Contains assorted generic utilities that do not belong to a specific runtime domain.

#### Movement
Includes movement-related scripts and helpers for entity locomotion.

#### Placement
Provides runtime systems and helpers for object placement workflows.

#### Projectiles
Contains 2D projectile behaviors, configuration, and management logic.

#### Random
Includes randomization utilities and helper methods for gameplay variability.

#### Renderer
Provides custom rendering-related helpers and runtime renderer components.

#### Save
Contains save/load systems and persistence utilities.

#### Singleton
Provides singleton and manager base classes for global runtime services.

#### States
Contains finite state machine utilities and state lifecycle abstractions.

#### StatusEffects
Implements status effect data and runtime application/removal logic.

#### TagManager
Contains custom tag management utilities for gameplay categorization.

#### Task
Provides asynchronous task helpers and coroutine-friendly runtime workflows.

#### TimedEvents
Contains scheduled event utilities for delayed or periodic runtime execution.

#### Tracker
Implements variable and value tracking utilities for runtime monitoring.

#### UI
Contains reusable UI runtime components, including modal window integrations.

#### Utils
Provides additional helper utilities used across multiple runtime modules.

### [Editor](./Editor)
Tools and windows designed for Unity Editor workflows and content authoring.

#### AutoBundles
Provides automatic Addressables bundle generation tools and related editor automation.

#### Dictionary
Contains inspector drawers and editor utilities for SerializableDictionary types.

#### Utils
Includes small editor-side helper tools to streamline repetitive tasks.

#### Windows
Contains custom Unity Editor windows for project-specific workflows.

### [WIP](./WIP)
Experimental modules under active development that are not part of the stable runtime API.

#### Health
Prototype health and damage systems for future runtime integration.

#### Lights
Experimental lighting and day/night cycle features.

#### Pool
Work-in-progress object pooling systems and related runtime patterns.

#### Redeem
Prototype reward/redeem code features and supporting systems.

Other useful resources:
- [CHANGELOG.md](./CHANGELOG.md)
- [LICENSE.md](./LICENSE.md)
- [package.json](./package.json)
