# Runtime/DragAndDrop

This folder contains a drag-and-drop workflow designed for card-like scene objects using the Unity Input System.

## Available classes and interfaces

- `DragAndDropManager`
  - Central singleton manager that listens to pointer movement and click input.
  - Detects draggable objects (`IDraggable`) through `_dragMask`.
  - Detects drop targets (`IDroppable`) during dragging through `_dropMask`.
  - Converts pointer movement into world-space offset (`_dragSpeed`) and applies a vertical start offset (`_height`) when dragging begins.
- `CardDrag`
  - Base `IDraggable` implementation for draggable objects with DOTween-based animations.
  - Handles:
    - lift animation at drag start (`_riseDuration`, `_riseEase`),
    - valid drop animation (`_dropDuration`, `_dropEase`),
    - return-to-origin animation on invalid drop (`_invalidDropDuration`, `_invalidDropEase`).
  - Exposes virtual hooks (`OnPostBeginDrag`, `OnPostDrag`, `OnPostEndDrag`) so you can extend behavior without rewriting the core flow.
- `CardTilter`
  - Adds a pitch/roll tilt effect based on object movement to provide visual feedback while dragging.
- `IDraggable`
  - Contract for draggable objects (`IsDraggable`, `Dragging`, pointer and drag callbacks).
- `IDroppable`
  - Contract for drop targets (`IsDroppable`, `AcceptDrop`, `OnDrop`).

## Quick setup

1. Add `DragAndDropManager` to your scene (single instance).
2. Configure the manager fields:
   - `_pointerPositionAction`: pointer `Vector2` input action,
   - `_clickAction`: click/press input action,
   - `_camera`: camera used for raycasts and screen-to-world conversions,
   - `_dragMask`: layer mask for draggable objects,
   - `_dropMask`: layer mask for droppable targets,
   - `_cardSize`: sampled card area used to detect a drop target under the dragged object.
3. For draggable objects:
   - add a collider,
   - add `CardDrag` (or a custom component implementing `IDraggable`),
   - assign the GameObject to a layer included in `_dragMask`.
4. For drop targets:
   - add a collider,
   - implement `IDroppable` on a target component,
   - assign the GameObject to a layer included in `_dropMask`.

## Runtime flow

1. The manager detects a hovered `IDraggable` and updates hover callbacks (`OnPointerEnter`/`OnPointerExit`).
2. On click, drag starts (`OnBeginDrag`) and the cursor is hidden (if configured).
3. While moving, the manager calls `OnDrag(deltaPosition, droppable)` on the current draggable.
4. On release, the manager calls `OnEndDrag(position, droppable)`.
5. If `droppable.AcceptDrop(drag)` returns `true`, the target can finalize its drop logic through `OnDrop` (for example in your `CardDrag` extension or target component).

## Minimal droppable target example

```csharp
using UnityEngine;

namespace GameUtils
{
    public class ExampleDropZone : MonoBehaviour, IDroppable
    {
        public bool IsDroppable => true;

        public bool AcceptDrop(IDraggable drag)
        {
            return drag != null;
        }

        public void OnDrop(IDraggable drag)
        {
            Debug.Log($"Dropped: {drag}");
        }
    }
}
```

## Practical notes

- Ensure the `Input System` package is enabled and `InputActionReference` fields are assigned.
- If you use `CardDrag`, verify that DOTween is installed in the project.
- `_hitsCount` on the manager controls the internal raycast buffer size.
