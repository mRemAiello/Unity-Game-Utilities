using System;
using TriInspector;
using UnityEngine;

namespace GameUtils
{
    [Serializable]
    public class EventTuple
    {
        [SerializeField, ShowIf(nameof(HasCallerGameObject))] private GameObject _callerGameObject;
        [SerializeField, ShowIf(nameof(HasCallerScriptable))] private ScriptableObject _callerScriptable;

        // Indica se il caller associato è un GameObject.
        private bool HasCallerGameObject => _callerGameObject != null;

        // Indica se il caller associato è una ScriptableObject.
        private bool HasCallerScriptable => _callerScriptable != null;

        // Espone il GameObject associato alla tupla evento.
        public GameObject CallerGameObject => _callerGameObject;

        // Espone la ScriptableObject associata alla tupla evento.
        public ScriptableObject CallerScriptable => _callerScriptable;

        // Crea una tupla evento con riferimento a GameObject o ScriptableObject.
        public EventTuple(GameObject callerGameObject, ScriptableObject callerScriptable)
        {
            // Memorizza i riferimenti serializzabili al caller.
            _callerGameObject = callerGameObject;
            _callerScriptable = callerScriptable;
        }
    }
}
