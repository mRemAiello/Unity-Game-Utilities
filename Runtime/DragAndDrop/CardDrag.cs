using DG.Tweening;
using TriInspector;
using UnityEngine;

namespace GameUtils
{
    [DeclareBoxGroup("References")]
    [DeclareBoxGroup("Animations")]
    [DeclareBoxGroup("Debug")]
    public class CardDrag : MonoBehaviour, IDraggable, ILoggable
    {
        [SerializeField, Group("Animations")] protected Ease _riseEase = Ease.Linear;
        [SerializeField, Range(0.0f, 5.0f), Group("Animations")] protected float _riseDuration = 0.2f;
        [SerializeField, Group("Animations")] protected Ease _dropEase = Ease.Linear;
        [SerializeField, Range(0.0f, 5.0f), Group("Animations")] protected float _dropDuration = 0.2f;
        [SerializeField, Group("Animations")] protected Ease _invalidDropEase = Ease.Linear;
        [SerializeField, Range(0.0f, 5.0f), Group("Animations")] protected float _invalidDropDuration = 0.2f;

        //
        [SerializeField, Group("Debug")] private bool _logEnabled = false;
        [SerializeField, Group("Debug"), ReadOnly] protected Vector3 _dragOriginPosition;
        [SerializeField, Group("Debug"), ReadOnly] protected bool _isDraggable = true;

        //
        public bool LogEnabled => _logEnabled;
        public virtual bool IsDraggable => _isDraggable;
        public bool Dragging { get; set; }

        //
        private void OnEnable()
        {
            _dragOriginPosition = transform.position;
        }

        public virtual void OnBeginDrag(Vector3 position, float height)
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

        public virtual void OnDrag(Vector3 deltaPosition, float height, IDroppable droppable)
        {
            transform.position += deltaPosition;
            transform.position = new Vector3(transform.position.x, transform.position.y, height);

            //
            OnPostDrag(deltaPosition, droppable);
        }

        public virtual void OnEndDrag(Vector3 position, IDroppable droppable)
        {
            if (droppable is { IsDroppable: true } && droppable.AcceptDrop(this) == true)
            {
                OnValidDrop(position, droppable);
            }
            else
            {
                OnInvalidDrop(position);
            }

            //
            OnPostEndDrag(position, droppable);
        }

        protected virtual void OnValidDrop(Vector3 position, IDroppable droppable)
        {
            this.Log($"Dropping on {position}");

            // Animate drop
            Tween tween = transform.DOMove(position, _dropDuration).From(transform.position);
            tween.SetEase(_dropEase);
            tween.OnComplete(() => { droppable.OnDrop(this); });
            tween.SetTarget(this);
        }

        protected virtual void OnInvalidDrop(Vector3 position)
        {
            this.Log($"Invalid drop on {position}");

            //
            _isDraggable = false;

            //
            Tween tween = transform.DOMove(_dragOriginPosition, _invalidDropDuration);
            tween.SetEase(_invalidDropEase);
            tween.OnComplete(() => { _isDraggable = true; });
            tween.SetTarget(this);
        }

        //
        public virtual void OnPointerEnter(Vector3 position) { }
        public virtual void OnPointerExit(Vector3 position) { }

        //
        protected virtual void OnPostBeginDrag(Vector3 position) { }
        protected virtual void OnPostDrag(Vector3 deltaPosition, IDroppable droppable) { }
        protected virtual void OnPostEndDrag(Vector3 position, IDroppable droppable) { }
    }
}