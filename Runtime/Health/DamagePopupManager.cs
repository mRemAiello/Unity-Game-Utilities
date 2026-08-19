using DG.Tweening;
using TMPro;
using TriInspector;
using UnityEngine;

namespace GameUtils
{
    public class DamagePopupManager : Singleton<DamagePopupManager>
    {
        [SerializeField, Group("Debug"), PropertyOrder(100)] private int _arcCounter = 0;

        //
        public void ShowPopup(DamagePopupData data, string text, Vector3 position)
        {
            GameObject popup = Instantiate(data.DamagePopupPrefab);
            if (!popup.TryGetComponent<TextMeshPro>(out var tmPro))
            {
                this.Log("tmPro is null");
                Destroy(popup);
                return;
            }

            tmPro.text = text;
            tmPro.color = data.TextColor;
            tmPro.fontSize = data.TextSize;
            popup.transform.position = position;

            // Animate the popup to move upwards and fade out
            float startX = position.x;
            int direction = (_arcCounter++ % 2 == 0) ? 1 : -1;

            Sequence sequence = DOTween.Sequence();
            sequence.Append(
                popup.transform.DOMove(
                    position + (data.Duration * data.Speed * Vector3.up),
                    data.Duration
                )
            );
            sequence.Join(
                DOVirtual.Float(0f, 1f, data.Duration, t =>
                {
                    var pos = popup.transform.position;
                    pos.x = startX + direction * data.ArcWidth * Mathf.Sin(t * Mathf.PI);
                    popup.transform.position = pos;
                })
            );
            sequence.Insert(data.FadeStartTime, tmPro.DOFade(0f, data.FadeDuration));
            sequence.SetEase(Ease.Linear);
            sequence.OnComplete(() => Destroy(popup));
        }
    }
}