using System;
using System.Collections;
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
        [SerializeField] private VoidEventAsset _onLoadingStarted;
        [SerializeField] private FloatEventAsset _onLoadingProgress;
        [SerializeField] private VoidEventAsset _onLoadingCompleted;

        [Tab("Debug")]
        [SerializeField, ReadOnly] private LoadingType _currentLoadingType = LoadingType.Fullscreen;
        [SerializeField, ReadOnly] private bool _isLoading;

        public void StartLoading(Action<Action<float>> loadAction, LoadingScreenData data)
        {
            if (_isLoading)
                return;

            //
            StartCoroutine(LoadingCoroutine(loadAction, data));
        }

        private IEnumerator LoadingCoroutine(Action<Action<float>> loadAction, LoadingScreenData data)
        {
            //
            ShowLoadingScreen(data);

            //
            _onLoadingStarted?.Invoke();

            //
            _isLoading = true;

            // Start the loading action with a callback to update progress
            loadAction(progress =>
            {
                if (data.Type == LoadingType.Fullscreen)
                {
                    _fullscreenLoading.ProgressBar.value = progress;
                    _fullscreenLoading.ProgressText.text = (progress * 100f).ToString("F2") + "%";
                }
                else
                {
                    _fullscreenLoading.ProgressBar.value = progress;
                    _fullscreenLoading.ProgressText.text = (progress * 100f).ToString("F2") + "%";
                }

                //
                _onLoadingProgress?.Invoke(progress);

                //
                if (progress >= 1f)
                {
                    progress = 1f;
                    _isLoading = false;
                }
            });

            while (_isLoading)
            {
                yield return null;
            }

            //
            _onLoadingCompleted?.Invoke();

            //
            HideLoadingScreen();
        }

        private void ShowLoadingScreen(LoadingScreenData data)
        {
            if (data.Type == LoadingType.Fullscreen)
            {
                _fullscreenLoading.Text.text = data.Text;
                _fullscreenLoading.Container.SetActive(true);
                if (_fullscreenLoading.Animation != null)
                    _fullscreenLoading.Animation.SetActive(data.EnableLoadingAnimation);
                _currentLoadingType = data.Type;
            }
            else
            {
                _popupLoading.Text.text = data.Text;
                _popupLoading.Container.SetActive(true);
                if (_popupLoading.Animation != null)
                    _popupLoading.Animation.SetActive(data.EnableLoadingAnimation);
                _currentLoadingType = data.Type;
            }
        }

        private void HideLoadingScreen()
        {
            if (_currentLoadingType == LoadingType.Fullscreen)
            {
                _fullscreenLoading.Container.SetActive(false);
            }
            else
            {
                _popupLoading.Container.SetActive(false);
            }
        }
    }
}
