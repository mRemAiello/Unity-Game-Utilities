using System;
using UnityEngine;
using VInspector;

namespace GameUtils
{
    public class LoadingManager : Singleton<LoadingManager>
    {
        [Tab("References")]
        [SerializeField] private LoadingData _fullscreenLoading;
        [SerializeField] private LoadingData _popupLoading;

        [Tab("Events")]
        [SerializeField] private VoidGameEventAsset _onDisableLoading;
        [SerializeField] private VoidGameEventAsset _onEnableLoading;

        [Tab("Debug")]
        [SerializeField, ReadOnlyField] private LoadingType _currentLoadingType = LoadingType.Fullscreen;

        public void EnableLoading(string text = "", LoadingType type = LoadingType.Fullscreen, bool enableLoadingAnimation = true)
        {
            DisableLoading();
            
            //
            switch (type)
            {
                case LoadingType.Fullscreen:
                    _fullscreenLoading.Text.text = text;
                    _fullscreenLoading.Container.SetActive(true);
                    _fullscreenLoading.Animation.SetActive(enableLoadingAnimation);
                    _currentLoadingType = type;
                    break;

                case LoadingType.Popup:
                    _popupLoading.Text.text = text;
                    _popupLoading.Container.SetActive(true);
                    _popupLoading.Animation.SetActive(enableLoadingAnimation);
                    _currentLoadingType = type;
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

            //
            _onEnableLoading.Invoke();
        }

        public void DisableLoading()
        {
            switch (_currentLoadingType)
            {
                case LoadingType.Fullscreen:
                    _fullscreenLoading.Container.SetActive(false);
                    break;
                
                case LoadingType.Popup:
                    _popupLoading.Container.SetActive(false);
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(_currentLoadingType), _currentLoadingType, null);
            }

            //
            _onDisableLoading.Invoke();
        }
    }
}
