# Unity Game Utilities

A lightweight, modular and easy to use **Unity utility package** providing reusable runtime systems, editor tooling, and experimental gameplay modules editor utilities (events, save system, extensions, projectiles, currency, and more) to accelerate game development workflows. 

## Why this exists

After working on several Unity projects, the same issues kept resurfacing:

- Rebuilding core runtime systems from scratch
- Tightly coupled logic that made refactoring difficult
- Unity abstractions that introduced overhead without providing real value
- Systems that worked early in development but didn’t scale well over time

This framework was created as a practical response to those problems.

The goal is to keep systems **explicit**, **modular**, and **easy to extend**, **remove**, or **replace** as project requirements evolve.

Rather than enforcing a rigid architecture, it focuses on providing a lightweight and flexible foundation for building runtime systems that remain maintainable as projects grow.

## Requirements

To use this package you need the following assets:

### Unity Packages

* [Addressables (2.7.6+)](https://docs.unity3d.com/Packages/com.unity.addressables@2.1/manual/index.html)
* [New Input System](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.10/manual/index.html)

### Third-party

* [Tri-Inspector](https://github.com/codewriter-packages/Tri-Inspector)
* [Quick Save](https://www.claytoninds.com/quick-save)
* [DOTween](http://dotween.demigiant.com/)

## Included Systems

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

#### Status Effects
Implements status effect data and runtime application/removal logic.

#### Tag Manager
Contains custom tag management utilities for gameplay categorization.

#### Task
Provides asynchronous task helpers and coroutine-friendly runtime workflows.

#### Timed Events
Contains scheduled event utilities for delayed or periodic runtime execution.

#### Tracker
Implements variable and value tracking utilities for runtime monitoring.

#### UI
Contains reusable UI runtime components, including modal window integrations.

#### Utils (Runtime and Editor)
Provides additional helper utilities used across multiple runtime modules. Includes small editor-side helper tools to streamline repetitive tasks

#### AutoBundles
Provides automatic Addressables bundle generation tools and related editor automation.

#### Dictionary
Contains inspector drawers and editor utilities for SerializableDictionary types.

#### Windows
Contains custom Unity Editor windows for project-specific workflows.

#### Health
Prototype health and damage systems for future runtime integration.

#### Lights
Experimental lighting and day/night cycle features.

#### Pool
Work-in-progress object pooling systems and related runtime patterns.

#### Redeem
Prototype reward/redeem code features and supporting systems.

## Intended Audience

This framework is designed for:

- Indie developers and solo creators
- Gameplay programmers
- Technical programmers who work closely with runtime systems

It is not intended to replace existing Unity packages or enforce a rigid architecture.

Instead, think of it as a **runtime toolkit** — a flexible foundation you can build upon, customize, or use selectively depending on the needs of your project.