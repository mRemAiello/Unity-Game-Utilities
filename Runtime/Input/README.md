# Input

## Overview
This folder contains runtime utilities for mouse/tap input. The classes allow you to:
- Cast raycasts from clicks using the new Input System.
- Define a contract for clickable objects.
- Handle UI/drag events with callbacks and drag direction.

## ClickInputManager
`ClickInputManager` listens to a pointer position and a click action (Input System) and casts a ray from `Camera.main`. If the hit collider implements `IClickable`, it invokes `OnClick` with the hit point in world space.

**Primary usage**
- Assign the `InputActionReference` assets for position and click in the Inspector.
- Set the raycast distance.
- Implement `IClickable` on the objects that should react to clicks.

```csharp
using GameUtils;
using UnityEngine;

public class ClickableTarget : MonoBehaviour, IClickable
{
    // Reacts to the click received from ClickInputManager.
    public void OnClick(Vector3 hitPoint)
    {
        Debug.Log($"Clicked at {hitPoint}");
    }
}
```

## IClickable
Interface for raycast-clickable objects. Implement `OnClick(Vector3 hitPoint)` to receive the hit point.

## IMouseInput
Interface that aggregates `EventSystems` handlers for click, drag, drop, and hover. It exposes assignable actions and useful properties:
- `MousePosition` (current mouse position).
- `DragDirection` (estimated drag direction).

## UIMouseInputProvider
`UIMouseInputProvider` implements `IMouseInput` on a `MonoBehaviour` with a collider. It requires a `PhysicsRaycaster` on the `MainCamera` and translates `EventSystems` callbacks into configurable `Action<PointerEventData>`.

**Primary usage**
- Ensure the main camera has a `PhysicsRaycaster`.
- Add `UIMouseInputProvider` and a collider to the 3D/World UI GameObject.
- Subscribe to the actions through the `IMouseInput` reference.

```csharp
using GameUtils;
using UnityEngine;

public class MouseInputConsumer : MonoBehaviour
{
    void Awake()
    {
        // Retrieves the provider and subscribes to the required callbacks.
        IMouseInput provider = GetComponent<IMouseInput>();
        if (provider == null)
        {
            return;
        }

        provider.OnPointerClick += eventData =>
        {
            Debug.Log("Pointer click received.");
        };

        provider.OnBeginDrag += eventData =>
        {
            Debug.Log($"Drag started in direction {provider.DragDirection}");
        };
    }
}
```

## DragDirection
Enum that describes the estimated drag direction: `None`, `Down`, `Left`, `Top`, `Right`.
