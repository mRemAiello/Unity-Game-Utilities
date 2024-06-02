using System.Collections.Generic;
using UnityEngine;

namespace GameUtils
{
    public class GameEventBaseAsset : ScriptableObject
    {
        public List<EventTuple> Listeners
        {
            get;
            set;
        }

        private void OnValidate()
        {
            Listeners = new List<EventTuple>();
        }
    }
}