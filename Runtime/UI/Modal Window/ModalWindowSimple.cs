using System.Collections.Generic;
using System.Linq;
using TriInspector;
using UnityEngine;

namespace GameUtils
{
    public class ModalWindowSimple : ModalWindowBase
    {
        [SerializeField, ReadOnly, Group("debug")] private bool _visible;

        //
        public override bool Visible
        {
            get => _visible;
            set => _visible = value;
        }

        //
        protected override void OnPostAwake()
        {
            base.OnPostAwake();

            //
            _visible = false;
        }

        protected override void OnBeforeShow()
        {

        }

        protected override void OnPostClose()
        {

        }
    }
}