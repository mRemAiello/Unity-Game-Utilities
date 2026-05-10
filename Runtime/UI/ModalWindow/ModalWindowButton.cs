using TMPro;
using TriInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GameUtils
{
    [DeclareBoxGroup("References")]
    [DeclareBoxGroup("Debug")]
    [DeclareBoxGroup("Events")]
    public class ModalWindowButton : MonoBehaviour
    {
        [SerializeField, Group("References")] private TextMeshProUGUI _text;
        [SerializeField, Group("References")] private Button _button;
        [SerializeField, Group("Debug")] private ModalWindowButtonEventAsset _onClickEvent;
        [SerializeField, Group("Debug"), ReadOnly] private UnityAction _onButtonClicked;

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
