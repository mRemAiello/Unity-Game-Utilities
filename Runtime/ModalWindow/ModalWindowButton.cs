using TMPro;
using TriInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GameUtils
{
    [DeclareBoxGroup("references", Title = "References")]
    [DeclareBoxGroup("debug", Title = "Debug")]
    [DeclareBoxGroup("events", Title = "Events")]
    public class ModalWindowButton : MonoBehaviour
    {
        [SerializeField, Group("references")] private TextMeshProUGUI _text;
        [SerializeField, Group("references")] private Button _button;
        [SerializeField, Group("debug")] private ModalWindowButtonEventAsset _onClickEvent;
        [SerializeField, Group("debug"), ReadOnly] private UnityAction _onButtonClicked;

        //
        public void Init(string text, UnityAction onButtonClicked)
        {
            _text.text = text;
            _onButtonClicked = onButtonClicked;

            //
            _button.onClick.AddListener(Click);
        }

        private void Click()
        {
            _onButtonClicked?.Invoke();
            _onClickEvent?.Invoke(this);
        }
    }
}
