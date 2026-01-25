using DG.Tweening;
using TriInspector;
using UnityEngine;

namespace GameUtils
{
    [DeclareBoxGroup("references")]
    [DeclareBoxGroup("animations")]
    [DeclareBoxGroup("debug")]
    public class CardDrag : MonoBehaviour, IDraggable, ILoggable
    {
        //
        [SerializeField, Group("animations")] private Ease _riseEase = Ease.Linear;
        [SerializeField, Range(0.0f, 5.0f), Group("animations")] private float _riseDuration = 0.2f;
        [SerializeField, Group("animations")] private Ease _dropEase = Ease.Linear;
        [SerializeField, Range(0.0f, 5.0f), Group("animations")] private float _dropDuration = 0.2f;
        [SerializeField, Group("animations")] private Ease _invalidDropEase = Ease.Linear;
        [SerializeField, Range(0.0f, 5.0f), Group("animations")] private float _invalidDropDuration = 0.2f;

        //
        [SerializeField, Group("debug")] private bool _logEnabled = false;
        [SerializeField, Group("debug"), ReadOnly] private Vector3 _dragOriginPosition;
        [SerializeField, Group("debug"), ReadOnly] protected bool _isDraggable = true;

        //
        public bool LogEnabled => _logEnabled;
        public bool IsDraggable => _isDraggable;
        public bool Dragging { get; set; }

        //
        private void OnEnable()
        {
            _dragOriginPosition = transform.position;
        }

        public void OnBeginDrag(Vector3 position)
        {
            _dragOriginPosition = transform.position;

            //
            _isDraggable = false;

            //
            Tween tween = transform.DOMoveY(position.y, _riseDuration).From(_dragOriginPosition.y);
            tween.SetEase(_riseEase);
            tween.OnComplete(() =>
            {
                _isDraggable = true;
            });
            tween.SetTarget(this);

            //
            OnPostBeginDrag(position);
        }

        public void OnDrag(Vector3 deltaPosition, IDroppable droppable)
        {
            deltaPosition.y = 0.0f;
            transform.position += deltaPosition;

            //
            OnPostDrag(deltaPosition, droppable);
        }

        public void OnEndDrag(Vector3 position, IDroppable droppable)
        {
            if (droppable is { IsDroppable: true } && droppable.AcceptDrop(this) == true)
            {
                transform.DOMoveY(position.y, _dropDuration).From(transform.position.y).SetEase(_dropEase).SetTarget(this);
            }
            else
            {
                _isDraggable = false;

                //
                Tween tween = transform.DOMove(_dragOriginPosition, _invalidDropDuration);
                tween.SetEase(_invalidDropEase);
                tween.OnComplete(() =>
                {
                    _isDraggable = true;
                });
                tween.SetTarget(this);
            }

            //
            OnPostEndDrag(position, droppable);
        }

        //
        public void OnPointerEnter(Vector3 position) { }
        public void OnPointerExit(Vector3 position) { }

        //
        protected virtual void OnPostBeginDrag(Vector3 position) { }
        protected virtual void OnPostDrag(Vector3 deltaPosition, IDroppable droppable) { }
        protected virtual void OnPostEndDrag(Vector3 position, IDroppable droppable) { }
    }
}