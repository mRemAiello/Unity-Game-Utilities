using System.Collections.Generic;
using TriInspector;
using UnityEngine;

namespace GameUtils
{
    [DeclareBoxGroup("debug", Title = "Debug")]
    [DeclareBoxGroup("log", Title = "Log")]
    public class GameEventAssetBase : ScriptableObject, ILoggable
    {
        [SerializeField, Group("log"), PropertyOrder(0)] private bool _logEnabled = false;
        
        //
        [SerializeField, PropertyOrder(2), Group("debug"), TableList(AlwaysExpanded = true, HideAddButton = true, HideRemoveButton = true), ReadOnly] 
        private List<EventTuple> _listeners = new();
        [SerializeField, HideInInspector] private int _listenersDataVersion;

        //
        public bool LogEnabled => _logEnabled;
        public IReadOnlyList<EventTuple> Listeners => _listeners;
        protected List<EventTuple> MutableListeners => _listeners;

        // Costruisce i dati del listener con riferimento a GameObject o ScriptableObject.
        protected EventTuple BuildListenerTuple(object target)
        {
            // Recupera eventuali riferimenti al GameObject o alla ScriptableObject del listener.
            var callerGameObject = GetListenerGameObject(target);
            var callerScriptable = GetListenerScriptableObject(target);

            // Ritorna la tupla con i riferimenti al caller.
            return new EventTuple(callerGameObject, callerScriptable);
        }

        // Estrae il GameObject associato al target del listener, se presente.
        protected static GameObject GetListenerGameObject(object target)
        {
            // Ritorna il GameObject del componente, se il listener è un Component.
            if (target is Component component)
            {
                return component.gameObject;
            }

            // Ritorna il GameObject direttamente se il listener è un GameObject.
            if (target is GameObject gameObject)
            {
                return gameObject;
            }

            return null;
        }

        // Estrae la ScriptableObject associata al target del listener, se presente.
        protected static ScriptableObject GetListenerScriptableObject(object target)
        {
            // Ritorna la ScriptableObject se il listener è un asset ScriptableObject.
            if (target is ScriptableObject scriptableObject)
            {
                return scriptableObject;
            }

            return null;
        }

        //
        private void OnValidate()
        {
            _listeners ??= new List<EventTuple>();

            // Ripulisce una volta i dati legacy dei listener dopo il cambio del campo serializzato.
            if (_listenersDataVersion == 0)
            {
                _listeners.Clear();
                _listenersDataVersion = 1;
            }
        }
    }
}
