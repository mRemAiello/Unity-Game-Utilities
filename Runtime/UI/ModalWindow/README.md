# Modal Window UI

## Overview
The modal window utilities provide a small, reusable UI flow for showing prompts with configurable buttons. The workflow is:

1. Add a concrete modal window component (such as `ModalWindowSimple`) to your canvas.
2. Configure the header/body `TextMeshProUGUI` references and button root in the inspector.
3. Use `ModalWindowBase` APIs to set text, create buttons, and show/close the modal.

## Classes

### `ModalWindowBase`
`ModalWindowBase` is the abstract foundation for all modal windows. It handles text assignment, button creation, visibility, and lifecycle hooks.

**Key members**
- `bool Visible { get; set; }` — Concrete classes implement how visibility is stored or applied.
- `bool Ignorable` — Indicates whether the modal can be dismissed without user input.
- `SetHeaderText(string headerText)` — Assigns the header text.
- `SetBodyText(string text)` — Assigns the body/question text.
- `AddButton(GameObject buttonPrefab, string text, ModalWindowButtonEventAsset buttonEvent, ModalButtonType type)` — Instantiates a new button and wires it to the event asset.
- `Show()` / `Close()` — Toggles visibility and clears generated buttons on close.
- `ClearButtons()` — Removes all runtime-created buttons.

### `ModalWindowSimple`
`ModalWindowSimple` is a minimal concrete implementation that stores `Visible` as a serialized boolean. Use it when you want to control visibility via custom UI logic or animations in the scene.

### `ModalWindowButton`
`ModalWindowButton` is the runtime component that represents each modal action button. It exposes the `Type` (normal/danger/success) and invokes a `ModalWindowButtonEventAsset` when clicked.

### `ModalButtonType`
An enum used to tag buttons:
- `Normal`
- `Danger`
- `Success`

### `ModalWindowButtonEventAsset`
A `ScriptableObject` event (`GameEventAsset<ModalWindowButton>`) raised when a modal button is clicked. It allows you to react to modal actions from any listener without direct references.

## Usage examples

### Show a modal with two buttons
```csharp
using UnityEngine;

namespace GameUtils
{
    public class ModalWindowExample : MonoBehaviour
    {
        [SerializeField] private ModalWindowSimple _modal;
        [SerializeField] private GameObject _buttonPrefab;
        [SerializeField] private ModalWindowButtonEventAsset _buttonEvent;

        private void Start()
        {
            // Update the modal content before showing it.
            _modal.SetHeaderText("Quit game?");
            _modal.SetBodyText("Are you sure you want to exit the game?");

            // Add buttons with different styles.
            _modal.AddButton(_buttonPrefab, "Cancel", _buttonEvent, ModalButtonType.Normal);
            _modal.AddButton(_buttonPrefab, "Quit", _buttonEvent, ModalButtonType.Danger);

            // Show the modal window.
            _modal.Show();
        }
    }
}
```

### Listen to button clicks
```csharp
using UnityEngine;

namespace GameUtils
{
    public class ModalWindowListener : MonoBehaviour
    {
        [SerializeField] private ModalWindowButtonEventAsset _buttonEvent;

        private void OnEnable()
        {
            // Register to receive modal button clicks.
            _buttonEvent.AddListener(OnModalButtonClicked);
        }

        private void OnDisable()
        {
            // Unregister to avoid dangling listeners.
            _buttonEvent.RemoveListener(OnModalButtonClicked);
        }

        private void OnModalButtonClicked(ModalWindowButton button)
        {
            // React to the button type.
            if (button.Type == ModalButtonType.Danger)
            {
                // Handle a danger action.
                Debug.Log("Danger action selected.");
            }
        }
    }
}
```
